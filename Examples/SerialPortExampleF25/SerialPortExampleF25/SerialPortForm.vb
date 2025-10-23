Imports System.IO.Ports

Public Class SerialPortForm
    ' Removed Private CurrentCount As Integer = 0 since the TrackBar handles the indexing

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
            Case 7 : byteToSend(0) = &H24 : byteToSend(1) = &H40
            Case 8 : byteToSend(0) = &H24 : byteToSend(1) = &H80
            Case Else
                Console.WriteLine($"Invalid index: {caseIndex}")
                Return
        End Select

        ' Write the 2-byte command
        SerialPort1.Write(byteToSend, 0, 2)
        Console.WriteLine($"Sent command for Case {caseIndex} (Value: &H{byteToSend(1).ToString("X2")})")
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
        If SerialPort1.IsOpen Then
            Dim data(1) As Byte 'put bytes into array
            data(0) = &H24
            data(1) = &HFF
            SerialPort1.Write(data, 0, 2)
        Else
            Console.WriteLine("Error: Cannot Output_High. COM port is closed.")
        End If
    End Sub

    Sub Output_Low()
        If SerialPort1.IsOpen Then
            Dim data(1) As Byte
            data(0) = &H24
            data(1) = &H0
            SerialPort1.Write(data, 0, 2)
        Else
            Console.WriteLine("Error: Cannot Output_Low. COM port is closed.")
        End If
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

    ' --- Event Handlers ---

    Private Sub SerialPortForm_Click(sender As Object, e As EventArgs) Handles Me.Click
        Write()
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Try
            Console.WriteLine($"Data received. Bytes available: {SerialPort1.BytesToRead}")
        Catch ex As Exception
            Console.WriteLine("oops! Error accessing BytesToRead.")
        End Try

        Read() ' Read the available data
    End Sub

    Private Sub HighOutputButton_Click(sender As Object, e As EventArgs) Handles HighOutputButton.Click
        Output_High()
    End Sub

    Private Sub LowOutputButton_Click(sender As Object, e As EventArgs) Handles LowOutputButton.Click
        Output_Low()
    End Sub

    ' THE TIMER_TICK EVENT IS NOW COMMENTED OUT/REMOVED to prevent interference with the TrackBar.
    ' Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
    '     ' ... (Timer logic was here) ...
    ' End Sub

    ' NEW: TrackBar Scroll Event Handler
    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ' This sends the command whenever the TrackBar position changes.
        SendCommand(TrackBar1.Value)
    End Sub

    Sub RingCounter()
    End Sub

    Private Sub RingCounterButton_Click(sender As Object, e As EventArgs) Handles RingCounterButton.Click
        'RingCounter()
    End Sub

    ' Ensure the port is closed when the form closes
    Private Sub SerialPortForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If
    End Sub
End Class