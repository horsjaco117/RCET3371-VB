Imports System.IO.Ports
Imports System.Windows.Forms
Imports System.Collections.Generic ' Needed for List(Of Byte)

Public Class SerialPortForm
    ' --- Class-Level Declarations ---
    Private WithEvents RingTimer As New System.Windows.Forms.Timer()
    ' Tracks the current index (1-32) for the ring counter sequence
    Private RingCounterStep As Integer = 1
    ' NEW: Private Field to hold any unmatched byte for frame reassembly
    Private TrailingByte As Byte? = Nothing

    Private LatestADCFrame As Byte() = Nothing
    ' ---------------------------------------

    Sub Connect()
        If Not SerialPort1.IsOpen Then
            ' Set port configuration
            SerialPort1.BaudRate = 9600 'Q@ Board Default
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = "COM5"
            Try
                SerialPort1.Open()
                Console.WriteLine("COM port opened successfully.")
            Catch ex As Exception
                Console.WriteLine($"Error opening COM port: {ex.Message}")
            End Try
        Else
            Console.WriteLine("COM port is already open.")
        End If
    End Sub

    Private Sub SerialPortForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the Ring Counter Timer
        RingTimer.Interval = 100 ' Set rotation speed
        RingTimer.Enabled = False ' Start disabled
        Connect()
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | SIMPLIFIED: Centralized function to calculate and send the 2-byte command |
    ' ----------------------------------------------------------------------------------
    Sub SendCommand(ByVal caseIndex As Integer)
        If Not SerialPort1.IsOpen Then
            Console.WriteLine("Command skipped: COM port is closed.")
            Return
        End If

        If caseIndex < 1 OrElse caseIndex > 32 Then
            Console.WriteLine($"Invalid index: {caseIndex}")
            Return
        End If

        Dim byteToSend(1) As Byte

        ' SIMPLIFICATION: Calculate data byte using math instead of 32 cases.
        ' Pattern: (caseIndex - 1) * 8. This correctly generates 0x00, 0x08, ... 0xF8.
        Dim dataValue As Integer = (caseIndex - 1) * 8

        byteToSend(0) = &H24 ' The fixed servo command header
        byteToSend(1) = CByte(dataValue) ' The calculated data byte

        SerialPort1.Write(byteToSend, 0, 2)

        Dim hexValue = byteToSend(1).ToString("X2")
        Console.WriteLine($"Sent command for Case {caseIndex} (Value: &H{hexValue})")
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
        ' Now calls SendCommand Case 32 (All High / Max value)
        SendCommand(32)
    End Sub

    Sub Output_Low()
        ' Now calls SendCommand Case 1 (All Low / Min value)
        SendCommand(1)
    End Sub

    ' --- Ring Counter Logic ---
    Sub RingCounter()
        If RingTimer.Enabled Then
            RingTimer.Stop()
            ' Stop the rotation and turn all outputs OFF (Case 1)
            SendCommand(1)
            Console.WriteLine("Ring Counter Stopped.")
        Else
            ' Reset the step to start at the first output (Case 1)
            RingCounterStep = 1
            RingTimer.Start()
            Console.WriteLine("Ring Counter Started.")
        End If
    End Sub

    ' Event handler that fires every time the RingTimer interval elapses
    Private Sub RingTimer_Tick(sender As Object, e As EventArgs) Handles RingTimer.Tick
        If RingCounterStep > 32 Then
            RingCounterStep = 1 ' Wrap back to the first step
        End If
        SendCommand(RingCounterStep)
        RingCounterStep += 1
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | MODIFIED: Data Received Handler – USES BUFFER FOR ROBUST FRAMING |
    ' ----------------------------------------------------------------------------------
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim bytesToRead As Integer = SerialPort1.BytesToRead
        If bytesToRead = 0 Then Return

        Dim buffer(bytesToRead - 1) As Byte
        SerialPort1.Read(buffer, 0, bytesToRead)

        ' 1. Combine the previous trailing byte (if any) with the new buffer
        Dim combinedData As New List(Of Byte)()
        If TrailingByte.HasValue Then
            combinedData.Add(TrailingByte.Value)
            TrailingByte = Nothing ' Clear the buffer now that we've used it
        End If
        combinedData.AddRange(buffer)

        ' ----------------------------------------------------------------------------------
        ' | MODIFIED: Data Received Handler – Stores ADC data instead of displaying |
        ' ----------------------------------------------------------------------------------
        Dim currentData As Byte() = combinedData.ToArray()

        Dim adcData As New List(Of Byte)()
        Dim servoData As New List(Of Byte)()

        Dim i As Integer = 0
        Do While i < currentData.Length
            If i + 1 < currentData.Length Then
                Dim headerByte As Byte = currentData(i)
                Dim dataByte As Byte = currentData(i + 1)

                If headerByte = &H21 Then
                    ' --- MODIFICATION HERE: Store the latest frame ---
                    LatestADCFrame = New Byte() {headerByte, dataByte}
                    i += 2
                ElseIf headerByte = &H24 Then
                    servoData.Add(headerByte)
                    servoData.Add(dataByte)
                    i += 2
                Else
                    i += 1
                End If
            Else
                TrailingByte = currentData(i)
                i += 1
            End If
        Loop

        ' ---- Update UI (thread-safe) ----
        Me.Invoke(Sub()
                      ' **REMOVED ADC UPDATE**

                      ' Keep the servo update immediate since it's sporadic
                      If servoData.Count > 0 Then
                          UpdateTextBox(VBRecieveServoTextBox, ConvertBytesToHexString(servoData.ToArray()))
                      End If
                  End Sub)

        Console.WriteLine($"Data received. Raw: {bytesToRead}, ADC: {adcData.Count}, Servo: {servoData.Count}, Trailing Byte: {If(TrailingByte.HasValue, TrailingByte.Value.ToString("X2"), "None")}")
    End Sub

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

    ' ... (Keep ConvertHexStringToByteArray and SendDataButton_Click for manual sending) ...

    ' ----------------------------------------------------------------------------------
    ' | EXISTING / AUXILIARY FUNCTIONS (Not modified) |
    ' ----------------------------------------------------------------------------------

    Private Function ConvertHexStringToByteArray(ByVal hexString As String) As Byte()
        ' Remove leading/trailing spaces and split the string by spaces
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
            Console.WriteLine("Error: Cannot Send Data. COM port is closed.")
            UpdateLogBox("ERROR: COM port is closed. Cannot send data.")
            Return
        End If
        ' **ASSUMPTION:** The text box for sending data is named 'InputTextBox'
        Dim hexInput As String = InputTextBox.Text
        If String.IsNullOrWhiteSpace(hexInput) Then
            Console.WriteLine("Cannot send: Text box is empty.")
            Return
        End If
        Try
            Dim dataToSend As Byte() = ConvertHexStringToByteArray(hexInput)
            SerialPort1.Write(dataToSend, 0, dataToSend.Length)
            Console.WriteLine($"Sent {dataToSend.Length} bytes: {hexInput.Trim()}")
            UpdateLogBox($"Sent Bytes: {hexInput.Trim()}")
        Catch ex As FormatException
            Console.WriteLine($"Error in hex format: {ex.Message}")
            UpdateLogBox($"ERROR: Invalid Hex Format. {ex.Message}")
        Catch ex As Exception
            Console.WriteLine($"Error sending data: {ex.Message}")
            UpdateLogBox($"ERROR: Serial Write Failed. {ex.Message}")
        End Try
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ' Stop the ring counter if the user manually adjusts the output
        If RingTimer.Enabled Then
            RingTimer.Stop()
            Console.WriteLine("Ring Counter Stopped by TrackBar input.")
        End If
        ' TrackBar.Value should be mapped to the case index (1 to 32)
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

    Private Sub ADCTimer_Tick(sender As Object, e As EventArgs) Handles ADCTimer.Tick
        If LatestADCFrame IsNot Nothing Then
            ' Convert the stored frame to a hex string
            Dim adcHex = ConvertBytesToHexString(LatestADCFrame)

            ' Update the ADC textbox (UpdateTextBox handles the thread safety/Invoke)
            UpdateTextBox(VBRecieveTextBox, adcHex)

            ' Clear the stored value to show we've consumed it
            LatestADCFrame = Nothing
        End If
    End Sub
End Class