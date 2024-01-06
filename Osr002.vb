
'敵レベルを選択する画面
Public Class Osr002

    Private selectedEnemyLv As Integer

    Private Sub Txt_Easy_Click(sender As Object, e As EventArgs) Handles Txt_Easy.Click
        'Me.DialogResult = Enemy.Level.easy
        selectedEnemyLv = Enemy.Level.easy
        Me.Close()
    End Sub

    Private Sub Txt_Normal_TextChanged(sender As Object, e As EventArgs) Handles Txt_Normal.Click
        'Me.DialogResult = Enemy.Level.normal
        selectedEnemyLv = Enemy.Level.normal
        Me.close()
    End Sub

    Private Sub Txt_Hard_TextChanged(sender As Object, e As EventArgs) Handles Txt_Hard.Click
        'Me.DialogResult = Enemy.Level.hard
        selectedEnemyLv = Enemy.Level.hard
        Me.Close()
    End Sub

    Private Sub Osr002_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub

    Private Sub Osr002_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public ReadOnly Property EnemyLv As Integer
        Get
            Return selectedEnemyLv
        End Get
    End Property

End Class