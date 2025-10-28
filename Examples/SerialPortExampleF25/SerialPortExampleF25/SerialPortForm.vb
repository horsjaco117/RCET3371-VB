Imports System.IO.Ports
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class SerialPortForm
    ' --- Class-Level Declarations ---
    Private WithEvents RingTimer As New System.Windows.Forms.Timer()

    Private RingCounterStep As Integer = 1
    Private TrailingByte As Byte? = Nothing
    Private LatestADCFrame As Byte() = Nothing
    Private LastGoodADCValue As Integer = 0
    ' ---------------------------------------

    Sub Connect()
        If Not SerialPort1.IsOpen Then
            SerialPort1.BaudRate = 9600
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = "COM5"
            Try
                SerialPort1.Open()
                ' Console.WriteLine removed
            Catch ex As Exception
                ' Console.WriteLine removed
                ' UpdateLogBox (if desired) is the user-facing alternative
            End Try
        Else
            ' Console.WriteLine removed
        End If
    End Sub

    Private Sub SerialPortForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RingTimer.Interval = 100
        RingTimer.Enabled = False

        Connect()
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | SendCommand - Console Output Removed |
    ' ----------------------------------------------------------------------------------
    Sub SendCommand(ByVal caseIndex As Integer)
        If Not SerialPort1.IsOpen Then
            ' Console.WriteLine removed
            UpdateLogBox("Command skipped: COM port is closed.")
            Return
        End If

        If caseIndex < 1 OrElse caseIndex > 32 Then
            ' Console.WriteLine removed
            Return
        End If

        Dim byteToSend(1) As Byte
        Dim dataValue As Integer = (caseIndex - 1) * 8

        byteToSend(0) = &H24
        byteToSend(1) = CByte(dataValue)

        SerialPort1.Write(byteToSend, 0, 2)

        Dim hexValue = byteToSend(1).ToString("X2")
        ' Console.WriteLine removed
        UpdateLogBox($"Sent command for Case {caseIndex} (Value: &H{hexValue})")
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
        Dim bytesToRead As Integer = SerialPort1.BytesToRead
        If bytesToRead = 0 Then Return

        Dim buffer(bytesToRead - 1) As Byte
        SerialPort1.Read(buffer, 0, bytesToRead)

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
            If i + 2 < currentData.Length AndAlso currentData(i) = &H21 Then
                Dim headerByte As Byte = currentData(i)
                Dim highByte As Byte = currentData(i + 1)
                Dim lowByte As Byte = currentData(i + 2)

                LatestADCFrame = New Byte() {headerByte, highByte, lowByte}
                i += 3
                ' Check for the 2-byte Servo frame
            ElseIf i + 1 < currentData.Length AndAlso currentData(i) = &H24 Then
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
        Me.Invoke(Sub()
                      If servoData.Count > 0 Then
                          UpdateTextBox(VBRecieveServoTextBox, ConvertBytesToHexString(servoData.ToArray()))
                      End If
                  End Sub)

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
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | Helper Functions and Event Handlers - Console Output Removed |
    ' ----------------------------------------------------------------------------------

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

    Private Sub SendDataButton_Click(sender As Object, e As EventArgs) Handles SendDataButton.Click
        If Not SerialPort1.IsOpen Then
            ' Console.WriteLine removed
            UpdateLogBox("ERROR: COM port is closed. Cannot send data.")
            Return
        End If
        Dim hexInput As String = InputTextBox.Text ' Assuming InputTextBox is the control name
        If String.IsNullOrWhiteSpace(hexInput) Then
            ' Console.WriteLine removed
            Return
        End If
        Try
            Dim dataToSend As Byte() = ConvertHexStringToByteArray(hexInput)
            SerialPort1.Write(dataToSend, 0, dataToSend.Length)
            ' Console.WriteLine removed
            UpdateLogBox($"Sent Bytes: {hexInput.Trim()}")
        Catch ex As FormatException
            ' Console.WriteLine removed
            UpdateLogBox($"ERROR: Invalid Hex Format. {ex.Message}")
        Catch ex As Exception
            ' Console.WriteLine removed
            UpdateLogBox($"ERROR: Serial Write Failed. {ex.Message}")
        End Try
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        If RingTimer.Enabled Then
            RingTimer.Stop()
            ' Console.WriteLine removed
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

    ' The ADCTimer_Tick handler is empty in the provided code, but the logic 
    ' was moved to the Invoke block in SerialPort1_DataReceived. I will keep 
    ' the structure consistent with your last provided code.

End Class