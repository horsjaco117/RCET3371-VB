Public Class SerialCommunicationsNotes
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Imports System.IO.Ports

Public Class SerialCommunicationsNotes

        Sub Connect()
            SerialPort1.Close()
            SerialPort1.BaudRate = 9600
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = "COM4"

            SerialPort1.Open()
            ' SerialPort1.Close()



        End Sub


        Sub Write()
            Dim data(0) As Byte 'put bytes into array
            data(0) = &B11110000 'actual data as a byte
            SerialPort1.Write(data, 0, 1) 'Send butes as array, start at index 0, send 1 byte
        End Sub

        Sub Read()
            Dim data(SerialPort1.BytesToRead) As Byte

            SerialPort1.Read(data, 0, SerialPort1.BytesToRead)

            For i = 0 To UBound(data)
                Console.WriteLine(Chr(data(i)))

            Next

            Console.WriteLine($"Is this Q@ Board: {IsQuietBoard(data)}")
            Console.WriteLine(UBound(data))
        End Sub

        Function IsQuietBoard(data() As Byte) As Boolean

            If UBound(data) And Chr(data(60)) = "@" Then
                Return True
            Else
                Return False
            End If
        End Function
        Function CheckIfQuietBoard() As Boolean
            Dim bytes(0) As Byte
            bytes(0) = &B11110000
            SerialPort1.Write(bytes, 0, 1)
            Return True
        End Function
        Private Sub SerialCommunicationsNotes_Click(sender As Object, e As EventArgs) Handles Me.Click
            Connect()
            Write()
            'SerialPort1.Close()
        End Sub

        Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived

            Try

                '   Me.Text = CStr(SerialPort1.BytesToRead)

                Me.Text = CStr(SerialPort1.BytesToRead)
                Console.WriteLine(SerialPort1.BytesToRead)
            Catch ex As Exception
                Console.WriteLine("oops!")
            End Try

            Read()
        End Sub
    End Class