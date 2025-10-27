Imports System.IO.Ports
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class SerialPortForm
    ' --- NEW: Ring Counter State & Timer ---
    Private WithEvents RingTimer As New System.Windows.Forms.Timer()
    ' Tracks the current index (1-6) for the ring counter sequence
    Private RingCounterStep As Integer = 1
    ' ---------------------------------------

    ' This method now checks if the port is open before attempting to configure and open it.
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
                ' Handle the case where the port cannot be opened (e.g., in use, wrong port name)
                Console.WriteLine($"Error opening COM port: {ex.Message}")
            End Try
        Else
            Console.WriteLine("COM port is already open.")
        End If
    End Sub

    ' The form's Load event is the best place to call Connect initially.
    Private Sub SerialPortForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the Ring Counter Timer
        RingTimer.Interval = 100 ' Set rotation speed to 250ms (4 steps per second)
        RingTimer.Enabled = False ' Start disabled

        Connect()
    End Sub

    ' New: Centralized function to handle the serial write logic based on an index (1-8).
    Sub SendCommand(ByVal caseIndex As Integer)
        If Not SerialPort1.IsOpen Then
            Console.WriteLine("Command skipped: COM port is closed.")
            Return
        End If

        Dim byteToSend(1) As Byte

        ' The caseIndex (from 1 to 32) determines which command is sent.
        Select Case caseIndex
            Case 1 : byteToSend(0) = &H24 : byteToSend(1) = &H0
            Case 2 : byteToSend(0) = &H24 : byteToSend(1) = &H8
            Case 3 : byteToSend(0) = &H24 : byteToSend(1) = &H10
            Case 4 : byteToSend(0) = &H24 : byteToSend(1) = &H18
            Case 5 : byteToSend(0) = &H24 : byteToSend(1) = &H20
            Case 6 : byteToSend(0) = &H24 : byteToSend(1) = &H28
            Case 7 : byteToSend(0) = &H24 : byteToSend(1) = &H30
            Case 8 : byteToSend(0) = &H24 : byteToSend(1) = &H38
            Case 9 : byteToSend(0) = &H24 : byteToSend(1) = &H40
            Case 10 : byteToSend(0) = &H24 : byteToSend(1) = &H48
            Case 11 : byteToSend(0) = &H24 : byteToSend(1) = &H50
            Case 12 : byteToSend(0) = &H24 : byteToSend(1) = &H58
            Case 13 : byteToSend(0) = &H24 : byteToSend(1) = &H60
            Case 14 : byteToSend(0) = &H24 : byteToSend(1) = &H68
            Case 15 : byteToSend(0) = &H24 : byteToSend(1) = &H70
            Case 16 : byteToSend(0) = &H24 : byteToSend(1) = &H78
            Case 17 : byteToSend(0) = &H24 : byteToSend(1) = &H80
            Case 18 : byteToSend(0) = &H24 : byteToSend(1) = &H88
            Case 19 : byteToSend(0) = &H24 : byteToSend(1) = &H90
            Case 20 : byteToSend(0) = &H24 : byteToSend(1) = &H98
            Case 21 : byteToSend(0) = &H24 : byteToSend(1) = &HA0
            Case 22 : byteToSend(0) = &H24 : byteToSend(1) = &HA8
            Case 23 : byteToSend(0) = &H24 : byteToSend(1) = &HB0
            Case 24 : byteToSend(0) = &H24 : byteToSend(1) = &HB8
            Case 25 : byteToSend(0) = &H24 : byteToSend(1) = &HC0
            Case 26 : byteToSend(0) = &H24 : byteToSend(1) = &HC8
            Case 27 : byteToSend(0) = &H24 : byteToSend(1) = &HD0
            Case 28 : byteToSend(0) = &H24 : byteToSend(1) = &HD8
            Case 29 : byteToSend(0) = &H24 : byteToSend(1) = &HE0
            Case 30 : byteToSend(0) = &H24 : byteToSend(1) = &HE8
            Case 31 : byteToSend(0) = &H24 : byteToSend(1) = &HF0
            Case 32 : byteToSend(0) = &H24 : byteToSend(1) = &HF8
            Case Else
                Console.WriteLine($"Invalid index: {caseIndex}")
                Return
        End Select

        ' Write the 2-byte command
        SerialPort1.Write(byteToSend, 0, 2)
        Console.WriteLine($"Sent command for Case {caseIndex} (Value: &H{byteToSend(1).ToString("X2")})")
        UpdateLogBox($"Sent command for Case {caseIndex} (Value: &H{byteToSend(1).ToString("X2")})")
    End Sub
    Private Sub UpdateLogBox(ByVal text As String)
        ' This ensures the update happens safely on the UI thread
        If Me.TransmissionToPicTextBox.InvokeRequired Then
            Me.Invoke(Sub() UpdateLogBox(text))
        Else
            Me.TransmissionToPicTextBox.AppendText(text & Environment.NewLine)
            Me.TransmissionToPicTextBox.ScrollToCaret()
        End If
    End Sub
    Sub Write()
        If SerialPort1.IsOpen Then
            Dim data(0) As Byte 'put bytes into array
            data(0) = &B0 'actual data as a byte
            SerialPort1.Write(data, 0, 1) 'send bytes as array, start at index 0, send 1 byte
        Else
            Console.WriteLine("Error: Cannot Write. COM port is closed.")
        End If
    End Sub

    Sub Output_High()
        ' Now calls SendCommand Case 8 (All High)
        SendCommand(32)
    End Sub

    Sub Output_Low()
        ' Now calls SendCommand Case 7 (All Low)
        SendCommand(1)
    End Sub

    Sub Read()
        ' This function is redundant, as data reception is handled by SerialPort1_DataReceived event
        Try
            Dim bytesToRead As Integer = SerialPort1.BytesToRead
            If bytesToRead > 0 Then
                Dim data(bytesToRead - 1) As Byte ' Array size is bytesToRead - 1 (0-based)
                SerialPort1.Read(data, 0, bytesToRead)

                For i = 0 To UBound(data)
                    Console.WriteLine($"Byte {i}: {Chr(data(i))}")
                Next

                Console.WriteLine($"Bytes read: {bytesToRead}")
            End If
        Catch ex As Exception
            Console.WriteLine($"Error during Read operation: {ex.Message}")
        End Try
    End Sub

    Function CheckIfQuietBoard() As Boolean
        If SerialPort1.IsOpen Then
            Dim bytes(0) As Byte
            bytes(0) = &B11110000
            SerialPort1.Write(bytes, 0, 1)
            Return True
        Else
            Console.WriteLine("Error: Cannot CheckIfQuietBoard. COM port is closed.")
            Return False
        End If
    End Function

    ' --- Ring Counter Logic ---
    Sub RingCounter()
        If RingTimer.Enabled Then
            RingTimer.Stop()
            ' Stop the rotation and turn all outputs OFF (Case 7)
            SendCommand(7)
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
        ' The ring counter cycles through Cases 1 through 32
        If RingCounterStep > 32 Then
            RingCounterStep = 1 ' Wrap back to the first step
        End If

        ' Send the command for the current step
        SendCommand(RingCounterStep)

        ' Move to the next step
        RingCounterStep += 1
    End Sub

    ' --- Event Handlers ---

    Private Sub SerialPortForm_Click(sender As Object, e As EventArgs) Handles Me.Click
        Write()
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | MODIFIED: Data Received Handler with Filtering for &H21 frame                    |
    ' ----------------------------------------------------------------------------------
    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        ' 1. Read ALL available bytes into the buffer.
        Dim bytesToRead As Integer = SerialPort1.BytesToRead
        If bytesToRead = 0 Then Return ' Safety check

        Dim buffer(bytesToRead - 1) As Byte
        SerialPort1.Read(buffer, 0, bytesToRead)

        ' 2. Process the buffer to remove the ADC frame (&H21 followed by 2 data bytes)
        Dim processedData As New System.Collections.Generic.List(Of Byte)()

        Dim i As Integer = 0
        Do While i < buffer.Length
            ' CRITICAL FILTERING LOGIC: Check for start byte and enough bytes remaining
            If buffer(i) = &H21 AndAlso i + 2 < buffer.Length Then
                ' Found the 3-byte ADC frame. Skip all 3 bytes.
                Console.WriteLine($"[FILTERED] ADC frame found and SKIPPED starting with &H21 at index {i}.")
                i += 3
            Else
                ' This is useful data, or a partial frame. Keep it.
                processedData.Add(buffer(i))
                i += 1
            End If
        Loop

        ' 3. Convert the remaining (processed) data to a Hex string
        Dim hexData As String = ConvertBytesToHexString(processedData.ToArray())

        ' 4. Update the UI using Me.Invoke (essential for thread safety)
        Me.Invoke(Sub()
                      If hexData.Length > 0 Then
                          UpdateTextBox(hexData) ' Updates VBRecieveTextBox
                      End If
                  End Sub)

        Try
            Console.WriteLine($"Data received. Raw bytes: {bytesToRead}, Filtered bytes: {processedData.Count}. Remaining: {SerialPort1.BytesToRead}")
        Catch ex As Exception
            Console.WriteLine("oops! Error accessing BytesToRead.")
        End Try
    End Sub
    ' ----------------------------------------------------------------------------------

    Private Function ConvertBytesToHexString(ByVal data As Byte()) As String
        Dim sb As New System.Text.StringBuilder()
        For Each b As Byte In data
            sb.Append(b.ToString("X2") & " ")
        Next
        Return sb.ToString().TrimEnd()
    End Function

    Private Sub UpdateTextBox(ByVal text As String)
        VBRecieveTextBox.AppendText(text & Environment.NewLine)
        VBRecieveTextBox.ScrollToCaret()
    End Sub

    ' ----------------------------------------------------------------------------------
    ' | NEW FUNCTION: Convert Hex String to Byte Array                                 |
    ' ----------------------------------------------------------------------------------
    Private Function ConvertHexStringToByteArray(ByVal hexString As String) As Byte()
        ' Remove leading/trailing spaces and split the string by spaces
        Dim hexValues As String() = hexString.Trim().Split(" "c)

        ' Determine the size of the output array
        Dim byteCount As Integer = hexValues.Count(Function(s) Not String.IsNullOrWhiteSpace(s))
        If byteCount = 0 Then Return New Byte() {} ' Return empty array if input is empty

        Dim bytes As Byte() = New Byte(byteCount - 1) {}
        Dim byteIndex As Integer = 0

        For i As Integer = 0 To hexValues.Length - 1
            Dim hex As String = hexValues(i).Trim().Replace(",", "")
            If String.IsNullOrWhiteSpace(hex) Then Continue For ' Skip empty strings from multiple spaces

            Try
                ' Convert the 1 or 2 character hex string to a Byte
                bytes(byteIndex) = Convert.ToByte(hex, 16) ' Base 16 (Hexadecimal)
                byteIndex += 1
            Catch ex As Exception
                ' Handle conversion error (e.g., "G2" is not valid hex)
                Throw New FormatException($"Invalid hexadecimal value found: '{hexValues(i)}'", ex)
            End Try
        Next

        Return bytes
    End Function
    ' ----------------------------------------------------------------------------------

    ' ----------------------------------------------------------------------------------
    ' | MODIFIED: Send Button Handler to Send Hex Data                                 |
    ' ----------------------------------------------------------------------------------
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
            ' 1. Convert the Hex string (e.g., "24 F8") into a Byte array
            Dim dataToSend As Byte() = ConvertHexStringToByteArray(hexInput)

            ' 2. Write the byte array to the serial port
            SerialPort1.Write(dataToSend, 0, dataToSend.Length)

            ' 3. Log the action
            Console.WriteLine($"Sent {dataToSend.Length} bytes: {hexInput.Trim()}")
            UpdateLogBox($"Sent Bytes: {hexInput.Trim()}")

        Catch ex As FormatException
            ' Handle error from the conversion function
            Console.WriteLine($"Error in hex format: {ex.Message}")
            UpdateLogBox($"ERROR: Invalid Hex Format. {ex.Message}")
        Catch ex As Exception
            ' Handle general serial port error
            Console.WriteLine($"Error sending data: {ex.Message}")
            UpdateLogBox($"ERROR: Serial Write Failed. {ex.Message}")
        End Try
    End Sub
    ' ----------------------------------------------------------------------------------

    Private Sub HighOutputButton_Click(sender As Object, e As EventArgs) Handles HighOutputButton.Click
        Output_High()
    End Sub

    Private Sub LowOutputButton_Click(sender As Object, e As EventArgs) Handles LowOutputButton.Click
        Output_Low()
    End Sub

    ' NEW: TrackBar Scroll Event Handler
    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ' Stop the ring counter if the user manually adjusts the output
        If RingTimer.Enabled Then
            RingTimer.Stop()
            Console.WriteLine("Ring Counter Stopped by TrackBar input.")
        End If
        ' This sends the command whenever the TrackBar position changes.
        SendCommand(TrackBar1.Value)
    End Sub

    Private Sub RingCounterButton_Click(sender As Object, e As EventArgs) Handles RingCounterButton.Click
        RingCounter()
    End Sub

    ' Ensure the port is closed when the form closes
    Private Sub SerialPortForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If
    End Sub

End Class