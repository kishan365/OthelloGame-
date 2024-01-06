<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Osr002
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Txt_Easy = New System.Windows.Forms.TextBox()
        Me.Txt_Hard = New System.Windows.Forms.TextBox()
        Me.Txt_Normal = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Txt_Easy
        '
        Me.Txt_Easy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Txt_Easy.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Txt_Easy.Location = New System.Drawing.Point(7, 75)
        Me.Txt_Easy.Multiline = True
        Me.Txt_Easy.Name = "Txt_Easy"
        Me.Txt_Easy.Size = New System.Drawing.Size(120, 67)
        Me.Txt_Easy.TabIndex = 1
        Me.Txt_Easy.TabStop = False
        Me.Txt_Easy.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "弱い"
        Me.Txt_Easy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Txt_Hard
        '
        Me.Txt_Hard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Txt_Hard.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Txt_Hard.Location = New System.Drawing.Point(308, 75)
        Me.Txt_Hard.Multiline = True
        Me.Txt_Hard.Name = "Txt_Hard"
        Me.Txt_Hard.Size = New System.Drawing.Size(120, 67)
        Me.Txt_Hard.TabIndex = 2
        Me.Txt_Hard.TabStop = False
        Me.Txt_Hard.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "強い"
        Me.Txt_Hard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Txt_Normal
        '
        Me.Txt_Normal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Txt_Normal.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Txt_Normal.Location = New System.Drawing.Point(158, 75)
        Me.Txt_Normal.Multiline = True
        Me.Txt_Normal.Name = "Txt_Normal"
        Me.Txt_Normal.Size = New System.Drawing.Size(120, 67)
        Me.Txt_Normal.TabIndex = 3
        Me.Txt_Normal.TabStop = False
        Me.Txt_Normal.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "普通"
        Me.Txt_Normal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Yu Gothic UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TextBox4.Location = New System.Drawing.Point(1, 3)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(429, 33)
        Me.TextBox4.TabIndex = 4
        Me.TextBox4.TabStop = False
        Me.TextBox4.Text = "敵の強さを選んでください"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Osr002
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 179)
        Me.ControlBox = False
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Txt_Normal)
        Me.Controls.Add(Me.Txt_Hard)
        Me.Controls.Add(Me.Txt_Easy)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(450, 195)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(450, 195)
        Me.Name = "Osr002"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Txt_Easy As TextBox
    Friend WithEvents Txt_Hard As TextBox
    Friend WithEvents Txt_Normal As TextBox
    Friend WithEvents TextBox4 As TextBox
End Class
