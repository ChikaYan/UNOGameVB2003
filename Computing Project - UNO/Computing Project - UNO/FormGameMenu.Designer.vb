<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormGameMenu
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonPlayAgainstPlayer = New System.Windows.Forms.Button()
        Me.ButtonPlayAgainstAI = New System.Windows.Forms.Button()
        Me.ButtonExit = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("SimSun", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(215, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Please Select Game Mode"
        '
        'ButtonPlayAgainstPlayer
        '
        Me.ButtonPlayAgainstPlayer.Location = New System.Drawing.Point(50, 108)
        Me.ButtonPlayAgainstPlayer.Name = "ButtonPlayAgainstPlayer"
        Me.ButtonPlayAgainstPlayer.Size = New System.Drawing.Size(147, 25)
        Me.ButtonPlayAgainstPlayer.TabIndex = 1
        Me.ButtonPlayAgainstPlayer.Text = "Play Against Player"
        Me.ButtonPlayAgainstPlayer.UseVisualStyleBackColor = True
        '
        'ButtonPlayAgainstAI
        '
        Me.ButtonPlayAgainstAI.Location = New System.Drawing.Point(50, 65)
        Me.ButtonPlayAgainstAI.Name = "ButtonPlayAgainstAI"
        Me.ButtonPlayAgainstAI.Size = New System.Drawing.Size(147, 25)
        Me.ButtonPlayAgainstAI.TabIndex = 2
        Me.ButtonPlayAgainstAI.Text = "Play Against AI"
        Me.ButtonPlayAgainstAI.UseVisualStyleBackColor = True
        '
        'ButtonExit
        '
        Me.ButtonExit.Location = New System.Drawing.Point(83, 195)
        Me.ButtonExit.Name = "ButtonExit"
        Me.ButtonExit.Size = New System.Drawing.Size(75, 25)
        Me.ButtonExit.TabIndex = 3
        Me.ButtonExit.Text = "Exit"
        Me.ButtonExit.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(83, 152)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 25)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Help"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FormGameMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(255, 262)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonExit)
        Me.Controls.Add(Me.ButtonPlayAgainstAI)
        Me.Controls.Add(Me.ButtonPlayAgainstPlayer)
        Me.Controls.Add(Me.Label1)
        Me.Name = "FormGameMenu"
        Me.Text = "UNO Game"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonPlayAgainstPlayer As System.Windows.Forms.Button
    Friend WithEvents ButtonPlayAgainstAI As System.Windows.Forms.Button
    Friend WithEvents ButtonExit As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
