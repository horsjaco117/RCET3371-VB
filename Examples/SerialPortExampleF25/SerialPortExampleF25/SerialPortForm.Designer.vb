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
        Me.HighOutputButton = New System.Windows.Forms.Button()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.LowOutputButton = New System.Windows.Forms.Button()
        Me.RingCounterButton = New System.Windows.Forms.Button()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.VBRecieveTextBox = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RecievedDataLabel = New System.Windows.Forms.Label()
        Me.TransmissionToPicTextBox = New System.Windows.Forms.TextBox()
        Me.TransmittedtoPicLabel = New System.Windows.Forms.Label()
        Me.SendDataButton = New System.Windows.Forms.Button()
        Me.VBRecieveServoTextBox = New System.Windows.Forms.TextBox()
        Me.ServoDataLabel = New System.Windows.Forms.Label()
        Me.ClearADCButton = New System.Windows.Forms.Button()
        Me.ClearServoButton = New System.Windows.Forms.Button()
        Me.ADCTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ADCCheckBox = New System.Windows.Forms.CheckBox()
        Me.ADCContinuousButton = New System.Windows.Forms.Button()
        Me.ADCOnceButton = New System.Windows.Forms.Button()
        Me.AllDataTextBox = New System.Windows.Forms.TextBox()
        Me.VoltageTextBox = New System.Windows.Forms.TextBox()
        Me.TemperatureTextBox = New System.Windows.Forms.TextBox()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'HighOutputButton
        '
        Me.HighOutputButton.Location = New System.Drawing.Point(173, 152)
        Me.HighOutputButton.Name = "HighOutputButton"
        Me.HighOutputButton.Size = New System.Drawing.Size(75, 73)
        Me.HighOutputButton.TabIndex = 2
        Me.HighOutputButton.Text = "High Button"
        Me.HighOutputButton.UseVisualStyleBackColor = True
        '
        'Timer
        '
        Me.Timer.Enabled = True
        Me.Timer.Interval = 1000
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 438)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1063, 31)
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
        'LowOutputButton
        '
        Me.LowOutputButton.Location = New System.Drawing.Point(167, 362)
        Me.LowOutputButton.Name = "LowOutputButton"
        Me.LowOutputButton.Size = New System.Drawing.Size(81, 73)
        Me.LowOutputButton.TabIndex = 4
        Me.LowOutputButton.Text = "Low Button"
        Me.LowOutputButton.UseVisualStyleBackColor = True
        '
        'RingCounterButton
        '
        Me.RingCounterButton.Location = New System.Drawing.Point(167, 270)
        Me.RingCounterButton.Name = "RingCounterButton"
        Me.RingCounterButton.Size = New System.Drawing.Size(75, 68)
        Me.RingCounterButton.TabIndex = 5
        Me.RingCounterButton.Text = "RingCounter"
        Me.RingCounterButton.UseVisualStyleBackColor = True
        '
        'TrackBar1
        '
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Location = New System.Drawing.Point(26, 121)
        Me.TrackBar1.Maximum = 32
        Me.TrackBar1.Minimum = 1
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TrackBar1.Size = New System.Drawing.Size(69, 295)
        Me.TrackBar1.TabIndex = 6
        Me.TrackBar1.Value = 1
        '
        'VBRecieveTextBox
        '
        Me.VBRecieveTextBox.Location = New System.Drawing.Point(465, 70)
        Me.VBRecieveTextBox.Name = "VBRecieveTextBox"
        Me.VBRecieveTextBox.Size = New System.Drawing.Size(221, 26)
        Me.VBRecieveTextBox.TabIndex = 7
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'RecievedDataLabel
        '
        Me.RecievedDataLabel.AutoSize = True
        Me.RecievedDataLabel.Location = New System.Drawing.Point(470, 47)
        Me.RecievedDataLabel.Name = "RecievedDataLabel"
        Me.RecievedDataLabel.Size = New System.Drawing.Size(112, 20)
        Me.RecievedDataLabel.TabIndex = 9
        Me.RecievedDataLabel.Text = "ADC PIC Data"
        '
        'TransmissionToPicTextBox
        '
        Me.TransmissionToPicTextBox.Location = New System.Drawing.Point(285, 61)
        Me.TransmissionToPicTextBox.Multiline = True
        Me.TransmissionToPicTextBox.Name = "TransmissionToPicTextBox"
        Me.TransmissionToPicTextBox.Size = New System.Drawing.Size(156, 365)
        Me.TransmissionToPicTextBox.TabIndex = 10
        '
        'TransmittedtoPicLabel
        '
        Me.TransmittedtoPicLabel.AutoSize = True
        Me.TransmittedtoPicLabel.Location = New System.Drawing.Point(271, 47)
        Me.TransmittedtoPicLabel.Name = "TransmittedtoPicLabel"
        Me.TransmittedtoPicLabel.Size = New System.Drawing.Size(180, 20)
        Me.TransmittedtoPicLabel.TabIndex = 11
        Me.TransmittedtoPicLabel.Text = "Data Transmitted to PIC"
        '
        'SendDataButton
        '
        Me.SendDataButton.Location = New System.Drawing.Point(134, 61)
        Me.SendDataButton.Name = "SendDataButton"
        Me.SendDataButton.Size = New System.Drawing.Size(108, 53)
        Me.SendDataButton.TabIndex = 12
        Me.SendDataButton.Text = "Send text Data"
        Me.SendDataButton.UseVisualStyleBackColor = True
        '
        'VBRecieveServoTextBox
        '
        Me.VBRecieveServoTextBox.Location = New System.Drawing.Point(711, 70)
        Me.VBRecieveServoTextBox.Name = "VBRecieveServoTextBox"
        Me.VBRecieveServoTextBox.Size = New System.Drawing.Size(200, 26)
        Me.VBRecieveServoTextBox.TabIndex = 13
        '
        'ServoDataLabel
        '
        Me.ServoDataLabel.AutoSize = True
        Me.ServoDataLabel.Location = New System.Drawing.Point(704, 47)
        Me.ServoDataLabel.Name = "ServoDataLabel"
        Me.ServoDataLabel.Size = New System.Drawing.Size(89, 20)
        Me.ServoDataLabel.TabIndex = 14
        Me.ServoDataLabel.Text = "Servo Data"
        '
        'ClearADCButton
        '
        Me.ClearADCButton.Location = New System.Drawing.Point(956, 80)
        Me.ClearADCButton.Name = "ClearADCButton"
        Me.ClearADCButton.Size = New System.Drawing.Size(70, 51)
        Me.ClearADCButton.TabIndex = 15
        Me.ClearADCButton.Text = "Clear ADC"
        Me.ClearADCButton.UseVisualStyleBackColor = True
        '
        'ClearServoButton
        '
        Me.ClearServoButton.Location = New System.Drawing.Point(956, 189)
        Me.ClearServoButton.Name = "ClearServoButton"
        Me.ClearServoButton.Size = New System.Drawing.Size(71, 55)
        Me.ClearServoButton.TabIndex = 16
        Me.ClearServoButton.Text = "Clear Servo"
        Me.ClearServoButton.UseVisualStyleBackColor = True
        '
        'ADCTimer
        '
        Me.ADCTimer.Enabled = True
        Me.ADCTimer.Interval = 1
        '
        'ADCCheckBox
        '
        Me.ADCCheckBox.AutoSize = True
        Me.ADCCheckBox.Location = New System.Drawing.Point(466, 392)
        Me.ADCCheckBox.Name = "ADCCheckBox"
        Me.ADCCheckBox.Size = New System.Drawing.Size(116, 24)
        Me.ADCCheckBox.TabIndex = 17
        Me.ADCCheckBox.Text = "CheckBox1"
        Me.ADCCheckBox.UseVisualStyleBackColor = True
        '
        'ADCContinuousButton
        '
        Me.ADCContinuousButton.Location = New System.Drawing.Point(909, 270)
        Me.ADCContinuousButton.Name = "ADCContinuousButton"
        Me.ADCContinuousButton.Size = New System.Drawing.Size(132, 68)
        Me.ADCContinuousButton.TabIndex = 18
        Me.ADCContinuousButton.Text = "ADC free run "
        Me.ADCContinuousButton.UseVisualStyleBackColor = True
        '
        'ADCOnceButton
        '
        Me.ADCOnceButton.Location = New System.Drawing.Point(909, 376)
        Me.ADCOnceButton.Name = "ADCOnceButton"
        Me.ADCOnceButton.Size = New System.Drawing.Size(132, 59)
        Me.ADCOnceButton.TabIndex = 19
        Me.ADCOnceButton.Text = "Button1"
        Me.ADCOnceButton.UseVisualStyleBackColor = True
        '
        'AllDataTextBox
        '
        Me.AllDataTextBox.Location = New System.Drawing.Point(486, 331)
        Me.AllDataTextBox.Name = "AllDataTextBox"
        Me.AllDataTextBox.Size = New System.Drawing.Size(386, 26)
        Me.AllDataTextBox.TabIndex = 21
        '
        'VoltageTextBox
        '
        Me.VoltageTextBox.Location = New System.Drawing.Point(465, 105)
        Me.VoltageTextBox.Name = "VoltageTextBox"
        Me.VoltageTextBox.Size = New System.Drawing.Size(221, 26)
        Me.VoltageTextBox.TabIndex = 22
        '
        'TemperatureTextBox
        '
        Me.TemperatureTextBox.Location = New System.Drawing.Point(465, 152)
        Me.TemperatureTextBox.Name = "TemperatureTextBox"
        Me.TemperatureTextBox.Size = New System.Drawing.Size(221, 26)
        Me.TemperatureTextBox.TabIndex = 23
        '
        'SerialPortForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1063, 469)
        Me.Controls.Add(Me.TemperatureTextBox)
        Me.Controls.Add(Me.VoltageTextBox)
        Me.Controls.Add(Me.AllDataTextBox)
        Me.Controls.Add(Me.ADCOnceButton)
        Me.Controls.Add(Me.ADCContinuousButton)
        Me.Controls.Add(Me.ADCCheckBox)
        Me.Controls.Add(Me.ClearServoButton)
        Me.Controls.Add(Me.ClearADCButton)
        Me.Controls.Add(Me.ServoDataLabel)
        Me.Controls.Add(Me.VBRecieveServoTextBox)
        Me.Controls.Add(Me.SendDataButton)
        Me.Controls.Add(Me.TransmittedtoPicLabel)
        Me.Controls.Add(Me.TransmissionToPicTextBox)
        Me.Controls.Add(Me.RecievedDataLabel)
        Me.Controls.Add(Me.VBRecieveTextBox)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.RingCounterButton)
        Me.Controls.Add(Me.LowOutputButton)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.HighOutputButton)
        Me.Controls.Add(Me.InputLabel)
        Me.Controls.Add(Me.InputTextBox)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "SerialPortForm"
        Me.Text = "Form1"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents InputTextBox As TextBox
    Friend WithEvents InputLabel As Label
    Friend WithEvents HighOutputButton As Button
    Friend WithEvents Timer As Timer
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents LowOutputButton As Button
    Friend WithEvents RingCounterButton As Button
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents VBRecieveTextBox As TextBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents RecievedDataLabel As Label
    Friend WithEvents TransmissionToPicTextBox As TextBox
    Friend WithEvents TransmittedtoPicLabel As Label
    Friend WithEvents SendDataButton As Button
    Friend WithEvents VBRecieveServoTextBox As TextBox
    Friend WithEvents ServoDataLabel As Label
    Friend WithEvents ClearADCButton As Button
    Friend WithEvents ClearServoButton As Button
    Friend WithEvents ADCTimer As Timer
    Friend WithEvents ADCCheckBox As CheckBox
    Friend WithEvents ADCContinuousButton As Button
    Friend WithEvents ADCOnceButton As Button
    Friend WithEvents AllDataTextBox As TextBox
    Friend WithEvents VoltageTextBox As TextBox
    Friend WithEvents TemperatureTextBox As TextBox
End Class
