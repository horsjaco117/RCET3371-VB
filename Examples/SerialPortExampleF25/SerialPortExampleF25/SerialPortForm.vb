Imports System.IO.Ports
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class SerialPortForm
    ' --- Class-Level Declarations ---
    Private WithEvents RingTimer As New System.Windows.Forms.Timer()

    Private RingCounterStep As Integer = 1 'For the ring counter steps
    Private TrailingByte As Byte? = Nothing '
    Private LatestADCFrame As Byte() = Nothing
    Private LastGoodADCValue As Integer = 0 'Last known ADC Value
    ' ---------------------------------------

    Sub Connect() 'Serial configuration data
        If Not SerialPort1.IsOpen Then
            SerialPort1.BaudRate = 9600
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = "COM5"
            Try
                SerialPort1.Open()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Sub SendCommand(ByVal caseIndex As Integer)
        Dim byteToSend(2) As Byte
        Dim dataValue As Integer = (caseIndex - 1) * 8
        Dim hexValue = byteToSend(1).ToString("X2") 'Servo data

        If Not SerialPort1.IsOpen Then
            UpdateLogBox("Command skipped: COM port is closed.")
            Return
        End If

        If caseIndex < 1 OrElse caseIndex > 32 Then
            Return
        End If

        ' Byte 0: Handshake Identifier
        byteToSend(0) = &H24
        ' Byte 1: Servo Data (XX)
        byteToSend(1) = CByte(dataValue)
        ' Byte 2: ADC Request identifier
        byteToSend(2) = &H25

        SerialPort1.Write(byteToSend, 0, 3) 'Checks for three bytes of data

        'Writes the commands sent for user to see
        UpdateLogBox($"Sent command: 24 {hexValue} then 25 request (Case {caseIndex})")
    End Sub

    Private Sub UpdateLogBox(ByVal text As String)
        If Me.TransmissionToPicTextBox.InvokeRequired Then
            Me.Invoke(Sub() UpdateLogBox(text))
        Else
            Me.TransmissionToPicTextBox.AppendText(text & Environment.NewLine)
            Me.TransmissionToPicTextBox.ScrollToCaret()
        End If
    End Sub

    Sub Output_High()
        SendCommand(32)
    End Sub

    Sub Output_Low()
        SendCommand(1)
    End Sub

    ' --- Ring Counter Logic - Console Output Removed ---
    Sub RingCounter()
        If RingTimer.Enabled Then
            RingTimer.Stop()
            SendCommand(1)
            ' Console.WriteLine removed
            UpdateLogBox("Ring Counter Stopped.")
        Else
            RingCounterStep = 1
            RingTimer.Start()
            ' Console.WriteLine removed
            UpdateLogBox("Ring Counter Started.")
        End If
    End Sub

    Private Sub RingTimer_Tick(sender As Object, e As EventArgs) Handles RingTimer.Tick
        If RingCounterStep > 32 Then
            RingCounterStep = 1
        End If
        SendCommand(RingCounterStep)
        RingCounterStep += 1
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | SerialPort1_DataReceived - Console Output Removed |
    ' ----------------------------------------------------------------------------------
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        CheckForIllegalCrossThreadCalls = False
        Dim bytesToRead As Integer = SerialPort1.BytesToRead



        If bytesToRead = 0 Then Return

        Dim buffer(bytesToRead - 1) As Byte
        SerialPort1.Read(buffer, 0, bytesToRead)

        Me.Invoke(Sub() UpdateAllDataTextBox(buffer))

        Dim combinedData As New List(Of Byte)()
        If TrailingByte.HasValue Then
            combinedData.Add(TrailingByte.Value)
            TrailingByte = Nothing
        End If
        combinedData.AddRange(buffer)

        Dim currentData As Byte() = combinedData.ToArray()
        Dim servoData As New List(Of Byte)()

        Dim i As Integer = 0
        Do While i < currentData.Length
            ' Check for a 3-byte ADC frame
            If i + 2 < currentData.Length AndAlso currentData(i) = &H25 Then
                Dim headerByte As Byte = currentData(i)
                Dim highByte As Byte = currentData(i + 1)
                Dim lowByte As Byte = currentData(i + 2)

                LatestADCFrame = New Byte() {headerByte, highByte, lowByte}
                i += 3
                ' Check for the 2-byte Servo frame
            ElseIf i + 1 < currentData.Length AndAlso currentData(i) = &H0 Then
                servoData.Add(currentData(i))
                servoData.Add(currentData(i + 1))
                i += 2
                ' Handle non-frame or trailing bytes
            ElseIf i < currentData.Length Then
                If i = currentData.Length - 1 Then
                    TrailingByte = currentData(i)
                    i += 1
                Else
                    i += 1
                End If
            End If
        Loop

        ' ---- Update UI (thread-safe) ----

        'retain data for VBRecieve textbox until new data recieved

        'VBRecieveServoTextBox.Text = ConvertBytesToHexString(servoData.ToArray())
        If servoData.Count >= 2 Then

            VBRecieveServoTextBox.Text = ConvertBytesToHexString(servoData.ToArray())

        Else
            'Retain
        End If



        Me.Invoke(Sub()
                      If LatestADCFrame IsNot Nothing AndAlso LatestADCFrame.Length = 3 Then

                          Dim headerByte As Byte = LatestADCFrame(0)
                          Dim highByte As Byte = LatestADCFrame(1)
                          Dim lowByte As Byte = LatestADCFrame(2)

                          Dim currentAdc10BitValue As Integer = 0

                          ' --- 1. Filter Check ---
                          If lowByte = &H24 Then
                              currentAdc10BitValue = LastGoodADCValue
                              ' Console.WriteLine removed
                          Else
                              ' --- 2. Calculation (using your corrected logic) ---
                              currentAdc10BitValue = (highByte * 4) Or (lowByte >> 6)
                              LastGoodADCValue = currentAdc10BitValue
                          End If

                          ' --- 3. Format and Display ---
                          Dim displayString As String = $"DEC: {currentAdc10BitValue} / RAW: {ConvertBytesToHexString(LatestADCFrame)}"

                          UpdateTextBox(VBRecieveTextBox, displayString)
                          LatestADCFrame = Nothing
                      End If
                  End Sub)

        ' Console.WriteLine removed
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | ADCTimer_Tick - Console Output Removed |
    ' ----------------------------------------------------------------------------------
    Private Sub ADCTimer_Tick(sender As Object, e As EventArgs) Handles ADCTimer.Tick
        If LatestADCFrame IsNot Nothing AndAlso LatestADCFrame.Length = 3 Then

            Dim headerByte As Byte = LatestADCFrame(0)
            Dim highByte As Byte = LatestADCFrame(1)
            Dim lowByte As Byte = LatestADCFrame(2)
            Dim currentAdc10BitValue As Integer = 0


            ' --- 1. Filter Check and Value Calculation ---
            If lowByte = &H24 Then
                currentAdc10BitValue = LastGoodADCValue
            Else
                currentAdc10BitValue = (highByte * 4) Or (lowByte >> 6)
                LastGoodADCValue = currentAdc10BitValue
            End If

            'For the voltage display
            Dim adcVoltage As Double = (CDbl(currentAdc10BitValue) / 1023.0) * 5.0
            Dim Temperature As Double = (CDbl(currentAdc10BitValue) / 1023.0) * 500
            Dim voltageString As String = $"{adcVoltage.ToString("F2")} Volts" ' "F2" = Fixed point, 2 decimal places
            Dim temperatureString As String = $"{Temperature.ToString("F2")} Degrees"

            UpdateTextBox(TemperatureTextBox, temperatureString)
            UpdateTextBox(VoltageTextBox, voltageString)

            ' Update the ADC text box to show both the raw value and the voltage.
            Dim displayString As String = $"DEC: {currentAdc10BitValue} ({voltageString} V) / RAW: {ConvertBytesToHexString(LatestADCFrame)}"
            UpdateTextBox(VBRecieveTextBox, displayString)
            LatestADCFrame = Nothing
        End If
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | Helper Functions and Event Handlers - Console Output Removed |
    ' ----------------------------------------------------------------------------------
    Sub SendBytes(ByVal bytesToSend As Byte())
        If SerialPort1.IsOpen Then
            Try
                ' Send the array of bytes. The length is determined by the array size.
                SerialPort1.Write(bytesToSend, 0, bytesToSend.Length)

                ' Log the sent bytes
                Dim hexString As String = ConvertBytesToHexString(bytesToSend)
                UpdateLogBox($"Sent bytes: {hexString}")
            Catch ex As Exception
                UpdateLogBox($"ERROR sending bytes: {ex.Message}")
            End Try
        Else
            UpdateLogBox("Command skipped: COM port is closed.")
        End If
    End Sub
    Private Sub UpdateAllDataTextBox(ByVal data As Byte())
        ' Must use Invoke because this is called from the SerialPort thread
        If Me.AllDataTextBox.InvokeRequired Then
            Me.Invoke(Sub() UpdateAllDataTextBox(data))
        Else
            ' Convert the raw byte array into a hex string (e.g., "21 FF C0 24 08...")
            Dim hexString As String = ConvertBytesToHexString(data)

            ' Append the new data to the text box
            Me.AllDataTextBox.Text = hexString
            Me.AllDataTextBox.ScrollToCaret()
        End If
    End Sub
    Private Function ConvertHexStringToByteArray(ByVal hexString As String) As Byte()
        Dim hexValues As String() = hexString.Trim().Split(" "c)
        Dim byteCount As Integer = hexValues.Count(Function(s) Not String.IsNullOrWhiteSpace(s))
        If byteCount = 0 Then Return New Byte() {}
        Dim bytes As Byte() = New Byte(byteCount - 1) {}
        Dim byteIndex As Integer = 0
        For i As Integer = 0 To hexValues.Length - 1
            Dim hex As String = hexValues(i).Trim().Replace(",", "")
            If String.IsNullOrWhiteSpace(hex) Then Continue For
            Try
                bytes(byteIndex) = Convert.ToByte(hex, 16)
                byteIndex += 1
            Catch ex As Exception
                Throw New FormatException($"Invalid hexadecimal value found: '{hexValues(i)}'", ex)
            End Try
        Next
        Return bytes
    End Function

    Private Function ConvertBytesToHexString(ByVal data As Byte()) As String
        Dim sb As New System.Text.StringBuilder()
        For Each b As Byte In data
            sb.Append(b.ToString("X2") & " ")
        Next
        Return sb.ToString().TrimEnd()
    End Function

    Private Sub UpdateTextBox(tb As TextBox, text As String)
        tb.Text = text & Environment.NewLine
        tb.ScrollToCaret()
    End Sub


    Private Sub SendDataButton_Click(sender As Object, e As EventArgs) Handles SendDataButton.Click
        If Not SerialPort1.IsOpen Then

            UpdateLogBox("ERROR: COM port is closed. Cannot send data.")
            Return
        End If
        Dim hexInput As String = InputTextBox.Text ' Assuming InputTextBox is the control name
        If String.IsNullOrWhiteSpace(hexInput) Then

            Return
        End If
        Try
            Dim dataToSend As Byte() = ConvertHexStringToByteArray(hexInput)
            SerialPort1.Write(dataToSend, 0, dataToSend.Length)
            UpdateLogBox($"Sent Bytes: {hexInput.Trim()}")
        Catch ex As FormatException
            UpdateLogBox($"ERROR: Invalid Hex Format. {ex.Message}")
        Catch ex As Exception
            UpdateLogBox($"ERROR: Serial Write Failed. {ex.Message}")
        End Try
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        If RingTimer.Enabled Then
            RingTimer.Stop()
        End If
        SendCommand(TrackBar1.Value)
    End Sub

    Private Sub RingCounterButton_Click(sender As Object, e As EventArgs) Handles RingCounterButton.Click
        RingCounter()
    End Sub

    Private Sub SerialPortForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If
    End Sub

    Private Sub HighOutputButton_Click(sender As Object, e As EventArgs) Handles HighOutputButton.Click
        Output_High()
    End Sub

    Private Sub LowOutputButton_Click(sender As Object, e As EventArgs) Handles LowOutputButton.Click
        Output_Low()
    End Sub

    Private Sub ClearADCButton_Click(sender As Object, e As EventArgs) Handles ClearADCButton.Click
        VBRecieveTextBox.Clear()
    End Sub

    Private Sub ClearServoButton_Click(sender As Object, e As EventArgs) Handles ClearServoButton.Click
        VBRecieveServoTextBox.Clear()
    End Sub

    Private Sub ADCContinuousButton_Click(sender As Object, e As EventArgs) Handles ADCContinuousButton.Click
        ' 24 21 enables the continuous ADC command
        Dim command() As Byte = {&H24, &H21}
        SendBytes(command)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ADCOnceButton.Click
        ' Create a 2-byte array with the desired sequence
        Dim command() As Byte = {&H24, &H25}

        ' Call the updated function to send both bytes
        SendBytes(command)
    End Sub

    Private Sub SerialPortForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RingTimer.Interval = 100 'Sets ringtimer to 100 milliseconds
        RingTimer.Enabled = False 'Enables COM port
        Connect() 'Connects to the COM port
    End Sub

End Class