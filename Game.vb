'ゲームに関するクラス

Public Class Game
    Inherits OseroCom

#Region "Private変数"

    'ゲーム終了時に表示するメッセージ
    Private gameEndMessage As String = ""

    'ユーザ名
    '第一次開発ではユーザ名を固定とする
    Private userName As String = "OseroUser"

    '敵
    Private enemy As Enemy

#End Region

#Region "Enum"

    'ゲーム状況
    Public Enum Status
        Game_status_NoGame = 0  'ゲーム中じゃないよ
        Game_status_Game        'ゲーム中
        Game_status_Break       '中断中
    End Enum

    'プレイヤー
    Public Enum Player
        User = 0    'ユーザ
        Enemy       '敵
        other       'どちらでもない
    End Enum

    '1ターン経過した結果
    Public Enum AfterTurnEnd
        MyTurn = 0
        NextTurn
        GameEnd
    End Enum
#End Region

#Region "開始処理"

    'ゲームスタート
    Public Sub GameStart(enemyType As Integer, enemyLV As Integer)

        Try
            'ゲーム状況をセット
            SetStatus(Status.Game_status_Game)

            '敵インスタンスを作成
            enemy = New Enemy(enemyType, enemyLV, GetBoad)

            'ユーザーターンに設定
            SetTurn(Player.User)

        Catch ex As Exception
            Throw
        End Try

        'ゲーム開始時に「あなたの番です」みたいなメッセージを表示したいけど
        'メッセージボックスで表示させると毎回クリックするのが面倒
        '1秒くらい小さいポップアップで表示させたい
    End Sub

#End Region

#Region "毎ターン処理"

    '1手毎に呼ばれる処理
    '引数： clickX  ->  クリックしたマス(横軸)
    '       clickY  ->  クリックしたマス(縦軸)
    '戻り値:0       ->  置きなおし
    '       1       ->  次のターン
    '       2       ->  ゲーム終了
    Public Function GameAdvance(clickX As Integer, clickY As Integer) As Integer

        '処理中盤面に触らせないため、相手ターンに設定
        SetTurn(Player.Enemy)

        'ユーザ側の処理
        Dim result As Integer = UserTurn(UserStone, clickX, clickY)
        If result <> AfterTurnEnd.NextTurn Then
            '置きなおし、ゲーム終了の場合ここで終了
            Return result
        End If

        '敵側の処理
        If EnemyTurn() = AfterTurnEnd.GameEnd Then
            Return AfterTurnEnd.GameEnd
        End If

        'このターンで決着が付かなかった場合、ユーザの番でゲーム続行
        SetTurn(Player.User)
        Return AfterTurnEnd.NextTurn
    End Function

    '1ターンに行う処理
    '引数：stone   ->  石
    '      x       ->  置きたいマス
    '      y       ->  置きたいマス
    '      turn    ->  0:ユーザ 1：敵


    Private Function OneTurn(stone As Integer, x As Integer, y As Integer, turn As Integer) As Integer
        '選んだマスに置けるかどうか

        If CanSetStone(stone, x, y, turn) = False Then
            MessageBox.Show("そこには置けないよー")
            SetTurn(turn)
            Return AfterTurnEnd.MyTurn

        Else
            If turn = Player.User Then

            End If
        End If

            '石を設置する（描画+データと画面の同期）
            'MsgBox("newx,newy" & x & y)
            SetStones(x, y, stone)

        '盤面の石の個数を数える
        SetStonesCount()

        'ゲームを続行できるかどうか
        If IsGameEnd() Then
            Return AfterTurnEnd.GameEnd
        End If

        '手数を増やして続行
        AddGameIndex()
        Return AfterTurnEnd.NextTurn
    End Function

    'ユーザのターン
    Private Function UserTurn(stone As Integer, x As Integer, y As Integer) As Integer
        Return OneTurn(stone, x, y, Player.User)
    End Function

    '相手のターン
    Public Function EnemyTurn()
        'パスフラグ

        Dim flgPAss As Boolean = False

        '敵クラスはゲーム開始時に作成するためコメント化
        'Dim enemy As New Enemy(Enemy.EnemyIs.CPU, Enemy.Level.nakamura, GetBoad)

        Do While flgPAss = False
            If Turn = Player.Enemy Then
                Osr001.p_0.Visible = False
                Osr001.p_1.Visible = False
                Osr001.p_2.Visible = False
                Osr001.p_3.Visible = False
                Osr001.p_4.Visible = False
                Osr001.p_5.Visible = False
                Osr001.p_6.Visible = False
                Osr001.p_7.Visible = False
            End If
            '現在の盤面情報をセット
            enemy.SetBoad(GetBoad)

            '石をどこに置くか考える
            Dim x As Integer
            Dim y As Integer

            If enemy.Think(x, y, UserStone, GameIndex) = False Then
                '置ける場所がないためパス
                MessageBox.Show("相手がパスしました")
                SetTurn(Player.User)
                Return AfterTurnEnd.NextTurn
            End If

            'TEST
            'MsgBox("考え中")



            '敵側の処理
            If OneTurn(1 - UserStone, x, y, Player.Enemy) = AfterTurnEnd.GameEnd Then
                Return AfterTurnEnd.GameEnd
            End If

            'ここ　あいてが石をおいたあと★★
            'MsgBox("testx,testy" & x & y)

            'MsgBox("(x,y)" & x & y)


            Changecolor(UserStone, 1 - UserStone, Turn)




            'ユーザが石を置ける場所があるかどうか判定
            flgPAss = CanSetStone(UserStone)
            If flgPAss = False Then
                MessageBox.Show("置ける場所がないので相手の番です")
                x = 0
                y = 0
            End If




        Loop

        Return AfterTurnEnd.NextTurn
    End Function


#End Region

#Region "中断処理"

    '中断
    Public Function GameBreak() As Boolean
        Dim ret As Boolean = False

        Dim LastBoad(BoadSize * BoadSize) As Integer
        Dim index = 0

        Try
            Dim data As New UserData
            data.UserID = userName
            data.UserStone = UserStone
            data.BreakFlg = UserData.Flg.FlgOn
            data.EnemyType = enemy.EnemyType
            data.EnemyLV = enemy.EnemyLevel
            data.MoveCount = GameIndex

            For i = 0 To BoadSize - 1
                For ii = 0 To BoadSize - 1
                    LastBoad(index) = BattleField(ii, i)
                    index += 1
                Next
            Next

            data.LastBoad = LastBoad
            'data.LastBoad = GetBoad()

            '保存する処理
            data.SaveUserData()

            MessageBox.Show("中断データを保存しました")
            ret = True
            Return ret

        Catch ex As Exception
            MessageBox.Show("中断情報の保存に失敗しました")
            Return ret
        End Try
    End Function

#End Region


#Region "再開処理"

    '再開
    '引数：GameData -> ユーザデータファイルの内容
    Public Sub GameRestart(GameData As UserData)
        Dim Field(BoadSize - 1, BoadSize - 1) As Integer
        Dim count = 0

        Try
            SetMyStone(GameData.UserStone)
            SetTurnCount(GameData.MoveCount)
            SetTurn(Player.User)

            For i = 0 To BoadSize - 1
                For ii = 0 To BoadSize - 1
                    Field(ii, i) = GameData.LastBoad(count)
                    count = count + 1
                Next
            Next
            SetBoad(Field)

            'ユーザーデータの内容で盤面を描写
            'ここで描写すると後で初期盤面に上書きされるためコメント化
            'InitiaLizeOsero_Restart()

            'ゲーム開始
            GameStart(GameData.EnemyType, GameData.EnemyLV)

        Catch ex As Exception
            MessageBox.Show("画面表示に失敗しました")
            Throw
        End Try
    End Sub

#End Region

#Region "終了処理"

    'ゲーム終了
    Public Sub GameEnd()
        Dim win As Integer = GetWinner()
        Dim winName As String = ""

        '終了前メッセージ
        If gameEndMessage <> "" Then
            MessageBox.Show(gameEndMessage)
        End If

        If win = Player.User Then
            winName = "ユーザー"
        ElseIf win = Player.Enemy Then
            winName = "CPU"
        End If

        If winName <> "" Then
            MessageBox.Show("白 " & GetWhiteCount() & " 個、黒 " & GetBlackCount() & " 個で " & winName & " の勝利✩")
        Else
            MessageBox.Show("引き分けだね～")
        End If

        '戦歴を実装したら保存処理を追加
        SaveResult(win)

        '画面や変数を初期化
        Reflesh()
        SetStatus(Status.Game_status_NoGame)
        Me.Finalize()
    End Sub

#End Region

#Region "戦歴保存処理"

    '戦歴を保存
    'winner : 勝者
    Private Sub SaveResult(winner As Integer)
        Try
            'ユーザデータを取得
            Dim data As New UserData(userName)

            If IsNothing(data) Then
                Throw New Exception
            End If

            '勝者によってデータを更新
            Select Case winner
                Case Player.User
                    data.WinCount += 1
                Case Player.Enemy
                    data.LoseCount += 1
                Case Else
                    data.DrawCount += 1
            End Select

            '中断フラグをオフ
            data.BreakFlg = UserData.Flg.FlgOff

            '保存
            data.SaveUserData()

        Catch ex As Exception
            MessageBox.Show("戦歴の保存に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


#End Region

#Region "メソッド"

    'ゲーム終了かどうか
    'ゲーム終了の場合 True
    Private Function IsGameEnd() As Boolean
        Dim ret As Boolean = False
        Dim wCount As Integer
        Dim bCount As Integer

        SetStonesCount()
        wCount = GetWhiteCount()
        bCount = GetBlackCount()

        '終了判定
        If wCount + bCount = BoadSize * BoadSize Then
            '盤面に空欄があるかどうか
            ret = True
        ElseIf wCount = 0 Or bCount = 0 Then
            '石が白か黒1色になった場合
            gameEndMessage = "盤面が１色になったため終了します"
            ret = True
        ElseIf CanSetStone(0) = False And CanSetStone(1) = False Then
            '白も黒も置ける場所がない場合
            gameEndMessage = "どちらも置く場所がないため終了します"
            ret = True
        End If

        Return ret
    End Function

    'どっちが勝ったか
    '戻り値 0:白 1:黒 2:引き分け
    Public Function GetWinner() As Integer
        Dim ret As Integer
        Dim white As Integer = GetWhiteCount()
        Dim black As Integer = GetBlackCount()
        If white > black Then
            ret = Colorr.White
        ElseIf black > white Then
            ret = Colorr.Black
        Else
            ret = Colorr.NoColor
        End If
        Return ret
    End Function


    '手数を1進める
    Public Sub AddGameIndex()
        Dim index = Me.GameIndex
        Me._nanteme = index + 1
    End Sub


    'ゲームを初期化（画面描写+画面とデータを同期）
    Private Sub Reflesh()
        ImageDraw.Refresh(Osr001.Pnl_GameArea)
        SetBoadIni()
    End Sub

#End Region


#Region "コンストラクタ"

    'コンストラクタ
    Public Sub New(userStone As Integer)
        MyBase.New(userStone)

        '何手目か
        SetTurnCount(1)

        'ユーザ先手
        SetTurn(Player.User)

        '状況
        SetStatus(Status.Game_status_NoGame)
    End Sub

#End Region

#Region "プロパティ"

    Private _nanteme As Integer         '何手目か
    Private _dotti As Integer           'どっちの番か(0:ユーザ 1:対戦相手(CPU))
    Private _joukyou As Integer         'ゲームの状況(0:ゲーム前 1:ゲーム中 2:中断中)

    Public ReadOnly Property GameIndex As Integer
        Get
            Return _nanteme
        End Get
    End Property
    Public ReadOnly Property Turn As Integer
        Get
            Return _dotti
        End Get
    End Property
    Public ReadOnly Property GameStatus As Integer
        Get
            Return _joukyou
        End Get
    End Property

#End Region

#Region "プロパティを設定"

    'ゲーム状況をセットする
    '0:ゲーム前 1:ゲーム中 2:中断中
    Public Sub SetStatus(stat As Integer)
        _joukyou = stat
    End Sub

    'どっちの番かセットする
    'dotti 0:ユーザ 1:対戦相手(CPU)
    Public Sub SetTurn(dotti As Integer)
        _dotti = dotti
    End Sub

    '今何手目かをセット
    Public Sub SetTurnCount(count As Integer)
        _nanteme = count
    End Sub


#End Region

#Region "コメント化した物"

    'ギブアップ処理は第一次開発では不要なためコメント化
    ''途中で終了
    'Public Sub GameGiveup()
    '    '戦歴を実装したら、それを保存する処理が必要
    '    SaveResult(Player.Enemy)

    '    '画面や変数を初期化
    '    Reflesh()
    '    SetStatus(Status.Game_status_NoGame)
    '    close()
    'End Sub


#End Region

#Region "リソース解放"

    Private Sub close()
        Me.Finalize()
    End Sub

#End Region


End Class
