
'ゲーム画面に関するクラス
Public Class Osr001



#Region "Private変数"

    'ユーザが選択した敵のレベル
    Private selectedEnemyLV As Integer

    '再開フラグ
    Private flgReStart As Boolean = False

#End Region

#Region "他クラス"

    'ゲームクラス
    Private game As New Game(Game.Colorr.White)
    Private Kishan As New OseroCom
#End Region

#Region "フォームロード関係"

    'FormLoad
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'フォームサイズを固定
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

    End Sub

    'Shown
    Private Sub Osr001_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            If Not System.IO.File.Exists("../../../../Data/UserData.json") Then
                Exit Sub
            End If

            'ユーザデータファイルを取得
            Dim showdata As New UserData("OseroUser")

            'Nothingの場合ここで処理終了
            If showdata Is Nothing Then
                Exit Sub
            Else
                '中断フラグが１じゃない場合ここで処理終了
                If showdata.BreakFlg = UserData.Flg.FlgOn Then
                    '再開処理
                    Dim log As DialogResult = MessageBox.Show("前回中断したデータが存在します。途中から再開しますか？", "確認メッセージ", MessageBoxButtons.YesNo)

                    '再開する場合(はい)
                    If log = DialogResult.Yes Then
                        game.GameRestart(showdata)
                        flgReStart = True
                        Btn_Start.Text = "中断する"
                    End If

                    '再開の有無に関わらずユーザデータファイルを更新
                    showdata.BreakFlg = UserData.Flg.FlgOff
                    'ShowData.EnemyType = ""
                    'ShowData.EnemyLV = ""
                    'ShowData.MoveCount = 0
                    'ShowData.LastBoad = {""}
                    showdata.SaveUserData()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("画面表示に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        End Try
    End Sub

#End Region

#Region "ボタンクリック関係"

    'ゲームスタート(中断)ボタン
    Private Sub Btn_Start_Click(sender As Object, e As EventArgs) Handles Btn_Start.Click
        Try
            If game.GameStatus = Game.Status.Game_status_NoGame Then
                '敵レベルを選択してゲーム開始
                Dim selectForm As New Osr002
                'Dim enemyLV As Integer = selectForm.ShowDialog()
                selectForm.ShowDialog()
                Dim enemyLV As Integer = selectForm.EnemyLv

                game.GameStart(Enemy.EnemyIs.CPU, enemyLV)
                For check = 0 To 7
                    'MsgBox("check=" & check)

                Next

                Btn_Start.Text = "中断する"
            Else
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show("中断処理に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


#End Region

#Region "オセロ盤 関係"

    '画面にオセロ用の線を引く
    Private Sub Pnl_GameArea_Paint(sender As Object, e As PaintEventArgs) Handles Pnl_GameArea.Paint
        ImageDraw.DrawField(Pnl_GameArea, e)

        'ゲーム中じゃない場合初期盤面を描画する
        If game.GameStatus <> Game.Status.Game_status_Game Then
            ImageDraw.Refresh(Pnl_GameArea)
        Else
            game.InitiaLizeOsero_Restart()
        End If

    End Sub

    'マス目をクリックした時
    Private Sub Pnl_GameArea_Click(sender As Object, e As EventArgs) Handles Pnl_GameArea.Click
        'ゲーム中でないかユーザーのターンでない場合
        If game.GameStatus <> Game.Status.Game_status_Game Or game.Turn <> 0 Then
            Exit Sub
        End If

        'クリックしたマスを取得
        Dim x As Integer
        Dim y As Integer
        getMasu(x, y)

        '勝敗が決まった終了処理
        If game.GameAdvance(x, y) = Game.AfterTurnEnd.GameEnd Then
            game.GameEnd()
            Btn_Start.Text = "ゲームスタート"
        End If
    End Sub

    'クリックしたマスを取得
    Private Sub getMasu(ByRef x As Integer, ByRef y As Integer)
        'マスの左上を取得
        Dim sp As System.Drawing.Point = System.Windows.Forms.Cursor.Position
        Dim cp As System.Drawing.Point = Me.PointToClient(sp)
        Dim fieldX As Integer = Pnl_GameArea.Location.X
        Dim fieldY As Integer = Pnl_GameArea.Location.Y
        'カーソルの位置を取得
        Dim xx As Integer = cp.X
        Dim yy As Integer = cp.Y
        'クリックしたマス目
        Dim masuX As Integer = Math.Floor((yy - fieldY + 60) / 60)
        Dim masuY As Integer = Math.Floor((xx - fieldX + 60) / 60)

        x = masuY - 1
        y = masuX - 1
    End Sub

#End Region

#Region "フォームクローズ"

    'フォームを閉じるとき
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            '中断ボタンをクリックした時の処理
            Dim result As DialogResult = MessageBox.Show("盤面を保存して中断しますか？",
                                         "質問",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button2)
            If result = DialogResult.Yes Then
                '「はい」が選択された時 
                If game.GameBreak() Then
                    'Application.Exit()
                End If


            ElseIf result = DialogResult.No Then
                '「いいえ」が選択された時 
                Btn_Start.Text = "ゲームスタート"
                'Application.Exit()

            ElseIf result = DialogResult.Cancel Then
                '「キャンセル」が選択された時 
                e.Cancel = True
            End If
        Catch ex As Exception
        End Try

        Exit Sub

    End Sub






#End Region



End Class
