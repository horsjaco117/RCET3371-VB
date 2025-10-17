<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SerialPortForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SerialPortForm))
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.InputTextBox = New System.Windows.Forms.TextBox()
        Me.InputLabel = New System.Windows.Forms.Label()
        Me.SendInputDataButton = New System.Windows.Forms.Button()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.NotSendButton = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        '
        'InputTextBox
        '
        Me.InputTextBox.Location = New System.Drawing.Point(12, 70)
        Me.InputTextBox.Name = "InputTextBox"
        Me.InputTextBox.Size = New System.Drawing.Size(100, 26)
        Me.InputTextBox.TabIndex = 0
        '
        'InputLabel
        '
        Me.InputLabel.AutoSize = True
        Me.InputLabel.Location = New System.Drawing.Point(8, 47)
        Me.InputLabel.Name = "InputLabel"
        Me.InputLabel.Size = New System.Drawing.Size(46, 20)
        Me.InputLabel.TabIndex = 1
        Me.InputLabel.Text = "Input"
        '
        'SendInputDataButton
        '
        Me.SendInputDataButton.Location = New System.Drawing.Point(167, 47)
        Me.SendInputDataButton.Name = "SendInputDataButton"
        Me.SendInputDataButton.Size = New System.Drawing.Size(75, 73)
        Me.SendInputDataButton.TabIndex = 2
        Me.SendInputDataButton.Text = "Send Input Data"
        Me.SendInputDataButton.UseVisualStyleBackColor = True
        '
        'Timer
        '
        Me.Timer.Interval = 500
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 344)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(655, 31)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(42, 28)
        Me.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1"
        '
        'NotSendButton
        '
        Me.NotSendButton.Location = New System.Drawing.Point(333, 47)
        Me.NotSendButton.Name = "NotSendButton"
        Me.NotSendButton.Size = New System.Drawing.Size(81, 73)
        Me.NotSendButton.TabIndex = 4
        Me.NotSendButton.Text = "Not Send"
        Me.NotSendButton.UseVisualStyleBackColor = True
        '
        'SerialPortForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 375)
        Me.Controls.Add(Me.NotSendButton)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.SendInputDataButton)
        Me.Controls.Add(Me.InputLabel)
        Me.Controls.Add(Me.InputTextBox)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "SerialPortForm"
        Me.Text = "Form1"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents InputTextBox As TextBox
    Friend WithEvents InputLabel As Label
    Friend WithEvents SendInputDataButton As Button
    Friend WithEvents Timer As Timer
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents NotSendButton As Button
End Class
