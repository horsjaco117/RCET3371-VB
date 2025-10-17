Imports System.IO.Ports

Public Class SerialPortForm

    Sub Connect()

        SerialPort1.Close()
        SerialPort1.BaudRate = 115200 'Q@ Board Default
        SerialPort1.Parity = Parity.None
        SerialPort1.StopBits = StopBits.One
        SerialPort1.DataBits = 8
        SerialPort1.PortName = "COM4"

        SerialPort1.Open()

    End Sub



    Sub Write()
        Dim data(0) As Byte 'put bytes into array
        data(0) = &B11110000 'actual data as a byte
        SerialPort1.Write(data, 0, 1) 'send bytes as array, start at index 0, send 1 byte
    End Sub

    Sub Output_High()
        Dim data(1) As Byte 'put bytes into array
        data(0) = &H20
        data(1) = &HFF
        SerialPort1.Write(data, 0, 2)
    End Sub

    Sub Output_Low()
        Dim data(1) As Byte
        data(0) = &H20
        data(1) = &H0
        SerialPort1.Write(data, 0, 2)
    End Sub

    Sub Read()
        Dim data(SerialPort1.BytesToRead) As Byte

        SerialPort1.Read(data, 0, SerialPort1.BytesToRead)

        For i = 0 To UBound(data)
            Console.WriteLine($"Byte {i}: {Chr(data(i))}")
        Next

        ' Console.WriteLine($"Is this Q@ Board: {IsQuietBoard(data)}")
        Console.WriteLine(UBound(data))

    End Sub

    'Function IsQuietBoard(data() As Byte) As Boolean


    '    If UBound(data) = 64 And Chr(data(60)) = "@" Then
    '        Return True
    '    Else
    '        Return False
    '    End If


    'End Function

    Function CheckIfQuietBoard() As Boolean
        Dim bytes(0) As Byte
        bytes(0) = &B11110000
        SerialPort1.Write(bytes, 0, 1)
        Return True
    End Function

    Private Sub SerialPortForm_Click(sender As Object, e As EventArgs) Handles Me.Click
        Connect()
        Write()
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Try
            Console.WriteLine(SerialPort1.BytesToRead)
        Catch ex As Exception
            Console.WriteLine("oops!")
        End Try

        Read()

    End Sub

    Private Sub HighOutputButton_Click(sender As Object, e As EventArgs) Handles HighOutputButton.Click
        Connect()
        Output_High()
        'SerialPort1.Open()

    End Sub

    Private Sub LowOutputButton_Click(sender As Object, e As EventArgs) Handles LowOutputButton.Click
        Connect()
        Output_Low()
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        Dim CurrentCount As Integer

        CurrentCount += 1



        If CurrentCount >= 8 Then
            CurrentCount = 0
        End If
    End Sub
End Class
