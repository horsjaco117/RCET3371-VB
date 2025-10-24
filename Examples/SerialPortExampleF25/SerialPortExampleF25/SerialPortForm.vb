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

        ' The caseIndex (from 1 to 8) now determines which command is sent.
        Select Case caseIndex
            Case 1 : byteToSend(0) = &H24 : byteToSend(1) = &H1
            Case 2 : byteToSend(0) = &H24 : byteToSend(1) = &H2
            Case 3 : byteToSend(0) = &H24 : byteToSend(1) = &H4
            Case 4 : byteToSend(0) = &H24 : byteToSend(1) = &H8
            Case 5 : byteToSend(0) = &H24 : byteToSend(1) = &H10
            Case 6 : byteToSend(0) = &H24 : byteToSend(1) = &H20
            Case 7 ' All Low / All Off
                byteToSend(0) = &H24 : byteToSend(1) = &H0
            Case 8 ' All High / All On
                byteToSend(0) = &H24 : byteToSend(1) = &HFF
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
        SendCommand(8)
    End Sub

    Sub Output_Low()
        ' Now calls SendCommand Case 7 (All Low)
        SendCommand(7)
    End Sub

    Sub Read()
        ' Reading only happens if data is available (triggered by DataReceived event)
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
        ' The ring counter cycles through Cases 1 through 6
        If RingCounterStep > 8 Then
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

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        ' 1. Read ALL available bytes into the buffer.
        '    BytesToRead is volatile, but this operation will empty the buffer
        '    of whatever was there when it executes.
        Dim bytesToRead As Integer = SerialPort1.BytesToRead
        Dim buffer(bytesToRead - 1) As Byte

        ' This single Read() command extracts all the data
        SerialPort1.Read(buffer, 0, bytesToRead)

        ' 2. Convert and update the UI using Me.Invoke (essential for thread safety)
        Dim hexData As String = ConvertBytesToHexString(buffer)

        Me.Invoke(Sub()
                      UpdateTextBox(hexData)
                  End Sub)

        Try
            Console.WriteLine($"Data received. Bytes read: {bytesToRead}. Remaining: {SerialPort1.BytesToRead}")
        Catch ex As Exception
            Console.WriteLine("oops! Error accessing BytesToRead.")
        End Try
    End Sub

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