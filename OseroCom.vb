
'オセロゲームで必要なプロパティやメソッドを持つ
Imports System.Media
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.VisualBasic.ApplicationServices
Imports Osero.Game

Public Class OseroCom


#Region "Enum"

    '石の色
    Public Enum Colorr
        White = 0   '白
        Black       '黒
        NoColor
        Yellow
    End Enum

#End Region

#Region "盤面関係"

    '8×8の盤面データを取得
    Public Function GetBoad() As Integer(,)
        Return _masu
    End Function

    '盤面全部をセットする
    Public Sub SetBoad(boad As Integer(,))
        _masu = boad.Clone
    End Sub

    'マス目に値をセットする
    'x 横座標（左上が0）
    'y 縦座標（左上が0）
    'myStone 0:白 1:黒 2:空白
    Public Sub SetBattleField(x As Integer, y As Integer, myStone As Integer)
        _masu(x, y) = myStone
    End Sub

    '盤面を見て石が何個ずつあるか設定
    Public Sub SetStonesCount()
        Dim wCount As Integer = 0
        Dim bCount As Integer = 0
        For i = 0 To BoadSize - 1
            For ii = 0 To BoadSize - 1
                If _masu(i, ii) = Colorr.White Then
                    wCount = wCount + 1
                ElseIf _masu(i, ii) = Colorr.Black Then
                    bCount = bCount + 1
                End If
            Next
        Next
        SetWhiteCount(wCount)
        SetBlackCount(bCount)
    End Sub

    '指定したマスに置かれている石を取得
    'x 横座標（左上が0）
    'y 縦座標（左上が0）
    '戻り値 0:白 1:黒 2:空白
    Public Function GetBattleField(x As Integer, y As Integer) As Integer
        Return BattleField(x, y)
    End Function


#End Region

#Region "初期設定関係"

    'オセロの初期設定
    Private Sub InitializeOsero(myStone As Integer)
        '石の初期配置
        SetBoadIni()

        '石数の初期化(白黒２個ずつ)
        SetBlackCount(2)
        SetWhiteCount(2)

        'ユーザの石色を設定
        SetMyStone(myStone)
    End Sub

    'オセロの初期設定（再開時）
    Public Sub InitiaLizeOsero_Restart()
        '盤面を白紙に
        ImageDraw.ClearBoad(Osr001.Pnl_GameArea)

        'Boad プロパティを参照し、Boadと同じように画面に石を描写
        Dim stone As Integer
        For i = 0 To BoadSize - 1
            For ii = 0 To BoadSize - 1
                '盤面データから指定マスの石色を取得
                stone = GetBattleField(i, ii)

                If stone <> Colorr.NoColor Then
                    '画面に描写
                    ImageDraw.DrawStone(Osr001.Pnl_GameArea, i, ii, stone)
                End If
            Next
        Next

    End Sub


    '石の配置データを初期化
    Public Sub SetBoadIni()
        '空白で埋める
        For i = 0 To BoadSize - 1
            For ii = 0 To BoadSize - 1
                _masu(i, ii) = Colorr.NoColor
            Next
        Next
        'マス目の中央に石を初期配置
        _masu(3, 3) = Colorr.Black
        _masu(4, 3) = Colorr.White
        _masu(3, 4) = Colorr.White
        _masu(4, 4) = Colorr.Black
    End Sub


#End Region


#Region "ゲーム関係の処理"

    '残りのマスで置ける場所があるかどうか
    Public Function CanSetStone(stone As Integer) As Boolean
        Dim ret As Boolean = False

        For i = 0 To BoadSize - 1
            For ii = 0 To BoadSize - 1
                If BattleField(i, ii) = Colorr.NoColor And CanSetStone(stone, i, ii, 9) Then
                    'Osr001.p_1.Location = New Point((i * 60) + 227, (ii * 60) + 41)
                    'Osr001.p_1.Visible = True

                    ret = True
                    Return ret
                End If
            Next
        Next
        Return ret
    End Function

    '指定したマスに石を置けるかどうか
    'This function checks the valid place in all direction
    Public Function CanSetStone(myStone As Integer, x As Integer, y As Integer, turn As Integer) As Boolean
        Dim ret As Boolean = False
        Dim enemyStone As Integer = 1 - myStone

        'マスが空欄じゃないとダメ
        If BattleField(x, y) <> Colorr.NoColor Then

            Return ret
        End If

        'ひっくり返せる石がない場合もダメ
        'Again looping the function CanSetVectol will check the valid place in all direction setting the value of vector = i
        For i = 0 To BoadSize - 1
            If CanSetStoneVectol(myStone, enemyStone, x, y, i, turn) Then
                ret = True
                Return ret
            End If

        Next

        Return ret
    End Function

    '指定したマスから1方向に検証して石を置ける条件を満たしているか返す
    'uStone : ユーザの石(0:白 1:黒）
    'eStone : 敵の石(0:白 1:黒）
    'x      : 置きたいマスの横座標（左上が起点）
    'y      : 置きたいマスの縦座標（左上が起点）
    'vectol : 検証する方向（0：真上 1:右上 2:右 ～時計回りに方角を指定～ 7:左上）
    'This function will check the valid place in one direction only
    Public Function CanSetStoneVectol(uStone As Integer, eStone As Integer, x As Integer, y As Integer, vectol As Integer, turn As Integer) As Boolean
        Dim ret As Boolean = False
        Dim nextX As Integer = x
        Dim nextY As Integer = y
        Dim InitX As Integer = x
        Dim InitY As Integer = y




        '隣が敵の石フラグ、その先に自分の石フラグ
        Dim flgTonari As Boolean = False
        Dim flgOwari As Boolean = False

        For i = 0 To BoadSize - 1
            '検証する座標を設定
            'Checking the table boundary from all sides.(if false its outside the table)
            Select Case vectol
                Case 0
                    nextX = nextX
                    nextY = nextY - 1
                    If nextY < 0 Then
                        Return ret
                    End If
                Case 1
                    nextX = nextX + 1
                    nextY = nextY - 1
                    If nextX > 7 Or nextY < 0 Then
                        Return ret
                    End If
                Case 2
                    nextX = nextX + 1
                    nextY = nextY
                    If nextX > 7 Then
                        Return ret
                    End If
                Case 3
                    nextX = nextX + 1
                    nextY = nextY + 1
                    If nextX > 7 Or nextY > 7 Then
                        Return ret
                    End If
                Case 4
                    nextX = nextX
                    nextY = nextY + 1
                    If nextY > 7 Then
                        Return ret
                    End If
                Case 5
                    nextX = nextX - 1
                    nextY = nextY + 1
                    If nextX < 0 Or nextY > 7 Then
                        Return ret
                    End If
                Case 6
                    nextX = nextX - 1
                    nextY = nextY
                    If nextX < 0 Then
                        Return ret
                    End If
                Case 7

                    nextX = nextX - 1
                    nextY = nextY - 1
                    If nextX < 0 Or nextY < 0 Then
                        Return ret
                    End If
            End Select

            '隣が空白の時点でアウト
            'if the next place is empty
            If BattleField(nextX, nextY) = Colorr.NoColor Then
                Return ret
            End If
            If flgTonari = False And BattleField(nextX, nextY) = uStone Then
                Return ret
            End If

            '検証
            If flgTonari = False And BattleField(nextX, nextY) = eStone Then
                flgTonari = True
                Continue For
            End If
            If flgTonari And BattleField(nextX, nextY) = uStone Then
                flgOwari = True

            End If

            'Added by myself for testing purpose
            'When there is the sequence of balck stones in the direction.
            If flgTonari And BattleField(nextX, nextY) = eStone Then
                Continue For
            End If
            If flgTonari And flgOwari Then
                ret = True
                Return ret
            End If
        Next
        Return ret
    End Function

    'Creating new function for the color change=================================
    '===========================================================================
    Public Function Changecolor(uStone As Integer, eStone As Integer, Optional turn As Integer = 9)
        Dim ret As Boolean = True
        Dim nextX As Integer
        Dim nextY As Integer
        Dim InitX As Integer
        Dim InitY As Integer


        'If turn <> Player.User Then
        '    Exit Sub
        'End If

        '隣が敵の石フラグ、その先に自分の石フラグ
        Dim flgTonari As Boolean = False
        Dim flgOwari As Boolean = False

        For i = 7 To 0 Step -1
            nextX = i
            nextY = 7
            'If BattleField(nextX, nextY) = Colorr.NoColor Then
            For ii = 7 To 0 Step -1
                For vectol = 0 To 7
kishan:             Select Case vectol
                        Case 0
                            nextX = nextX
                            nextY = nextY - 1
                            If nextY < 0 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For

                            Else
                                ret = True
                            End If

                        Case 1
                            nextX = nextX + 1
                            nextY = nextY - 1
                            If nextX > 7 Or nextY < 0 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For

                            Else
                                ret = True

                            End If
                        Case 2
                            nextX = nextX + 1
                            nextY = nextY
                            If nextX > 7 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For


                            Else
                                ret = True

                            End If
                        Case 3
                            nextX = nextX + 1
                            nextY = nextY + 1
                            If nextX > 7 Or nextY > 7 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For


                            Else
                                ret = True
                            End If
                        Case 4
                            nextX = nextX
                            nextY = nextY + 1
                            If nextY > 7 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For

                            Else
                                ret = True

                            End If
                        Case 5
                            nextX = nextX - 1
                            nextY = nextY + 1
                            If nextX < 0 Or nextY > 7 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For


                            Else
                                ret = True
                            End If
                        Case 6
                            nextX = nextX - 1
                            nextY = nextY
                            If nextX < 0 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For

                            Else
                                ret = True

                            End If
                        Case 7

                            nextX = nextX - 1
                            nextY = nextY - 1
                            If nextX < 0 Or nextY < 0 Then
                                ret = False
                                nextX = i
                                nextY = ii
                                Continue For

                            Else
                                ret = True

                            End If

                    End Select
                    'Checking the table boundary from all sides.(if false its outside the table)

                    '隣が空白の時点でアウト
                    'if the next place is empty
                    If ret Then

                        If flgTonari = False And BattleField(nextX, nextY) = uStone Then
                            ret = False


                        End If

                        '検証
                        If flgTonari = False And BattleField(nextX, nextY) = eStone Then
                            flgTonari = True
                            Select Case vectol
                                Case 0
                                    InitX = nextX
                                    InitY = nextY + 1
                                Case 1
                                    InitX = nextX - 1
                                    InitY = nextY + 1

                                Case 2
                                    InitX = nextX - 1
                                    InitY = nextY

                                Case 3
                                    InitX = nextX - 1
                                    InitY = nextY - 1

                                Case 4
                                    InitX = nextX
                                    InitY = nextY - 1

                                Case 5
                                    InitX = nextX + 1
                                    InitY = nextY - 1

                                Case 6
                                    InitX = nextX + 1
                                    InitY = nextY

                                Case 7

                                    InitX = nextX + 1
                                    InitY = nextY + 1

                            End Select
                            GoTo kishan
                        End If
                        If flgTonari And BattleField(nextX, nextY) = eStone Then
                            GoTo kishan

                        End If
                        If flgTonari And BattleField(nextX, nextY) = uStone Then
                            flgOwari = True

                        End If

                        'Added by myself for testing purpose
                        'When there is the sequence of balck stones in the direction.

                        If flgTonari And flgOwari Then
                            'If turn = Player.Enemy Then
                            If BattleField(InitX, InitY) = Colorr.NoColor Then

                                Select Case vectol
                                    Case 0

                                        Osr001.p_0.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_0.Visible = True
                                        Osr001.p_0.BackColor = Color.DarkBlue

                                    Case 1
                                        Osr001.p_1.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_1.BackColor = Color.DarkBlue

                                        Osr001.p_1.Visible = True
                                    Case 2
                                        Osr001.p_2.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_2.Visible = True
                                        Osr001.p_2.BackColor = Color.DarkBlue

                                    Case 3

                                        Osr001.p_3.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_3.Visible = True
                                        Osr001.p_3.BackColor = Color.DarkBlue

                                    Case 4
                                        Osr001.p_4.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_4.Visible = True
                                        Osr001.p_4.BackColor = Color.DarkBlue

                                    Case 5
                                        Osr001.p_5.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_5.Visible = True
                                        Osr001.p_5.BackColor = Color.DarkBlue

                                    Case 6
                                        Osr001.p_6.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_6.Visible = True
                                        Osr001.p_6.BackColor = Color.DarkBlue


                                    Case 7
                                        Osr001.p_7.Location = New Point((InitX * 60) + 227, (InitY * 60) + 41)
                                        Osr001.p_7.Visible = True
                                        Osr001.p_7.BackColor = Color.DarkBlue

                                End Select
                                'End If

                                ret = True
                            End If
                        End If
                    End If
                    nextX = i
                    nextY = ii
                    flgTonari = False
                    flgOwari = False
                Next

                nextY = ii - 1
            Next
            'End If
        Next




    End Function

    'End of new function for the color change=================================
    '===========================================================================

    '指定マスに石を置く(画面に描画してデータと同期させる)
    '処理が終わったら1を返す
    Public Function SetStone(x As Integer, y As Integer, stoneColor As Integer) As Integer
        '画面に描写
        ImageDraw.drawStone(Osr001.Pnl_GameArea, x, y, stoneColor)
        'データと画面を同期
        SetBattleField(x, y, stoneColor)
        Return 1
    End Function

    '指定マスから変えられるだけ石を置く(画面に描画してデータと同期させる)
    Public Sub SetStones(startX As Integer, startY As Integer, stoneColor As Integer)
        '起点に石を置く
        SetStone(startX, startY, stoneColor)  'This setstone function will draw the stone of stoneColor at specified points.

        'ひっくり返せる石を裏返す
        Dim vector As Integer
        Dim flgChangeEnd As Boolean = False
        Dim nextX As Integer
        Dim nextY As Integer
        For vector = 0 To BoadSize - 1
            'vector(上とか下とか) 方向に裏返せる石があるかどうか
            If CanSetStoneVectol(stoneColor, 1 - stoneColor, startX, startY, vector, 9) Then
                nextX = startX
                nextY = startY
                '全部裏返す
                flgChangeEnd = False
                Do While flgChangeEnd = False
                    '検証する座標を設定
                    Select Case vector
                        Case 0
                            nextX = nextX
                            nextY = nextY - 1
                            If nextY < 0 Then
                                Continue For
                            End If
                        Case 1
                            nextX = nextX + 1
                            nextY = nextY - 1
                            If nextX >= BoadSize Or nextY < 0 Then
                                Continue For
                            End If
                        Case 2
                            nextX = nextX + 1
                            nextY = nextY
                            If nextX >= BoadSize Then
                                Continue For
                            End If
                        Case 3
                            nextX = nextX + 1
                            nextY = nextY + 1
                            If nextX >= BoadSize Or nextY >= BoadSize Then
                                Continue For
                            End If
                        Case 4
                            nextX = nextX
                            nextY = nextY + 1
                            If nextY >= BoadSize Then
                                Continue For
                            End If
                        Case 5
                            nextX = nextX - 1
                            nextY = nextY + 1
                            If nextX < 0 Or nextY >= BoadSize Then
                                Continue For
                            End If
                        Case 6
                            nextX = nextX - 1
                            nextY = nextY
                            If nextX < 0 Then
                                Continue For
                            End If
                        Case 7
                            nextX = nextX - 1
                            nextY = nextY - 1
                            If nextX < 0 Or nextY < 0 Then
                                Continue For
                            End If
                    End Select

                    If BattleField(nextX, nextY) = stoneColor Then
                        flgChangeEnd = True
                        Continue For
                    End If
                    SetStone(nextX, nextY, stoneColor)
                Loop
            End If
        Next
    End Sub

    '石を置くと決めたマスが盤面の外ではないかチェック
    '盤面内なら True を返す
    Public Function OutofBoadCheck(x As Integer, y As Integer) As Boolean
        Dim ret As Boolean = False
        If x < 0 Or x >= BoadSize Then
            Return ret
        ElseIf y < 0 Or y >= boadsize Then
            Return ret
        End If
        ret = True
        Return ret
    End Function

    'クリックしたマスの石色を表示
    '(データと画面表示で石の色が反転していることがあったため、確認用の関数として作成)
    Public Sub ShowStoneColor(x As Integer, y As Integer)

        Dim col As Integer = GetBattleField(x, y)
        Dim aaa As String
        If col = Colorr.White Then
            aaa = "白いよ"
        ElseIf col = Colorr.Black Then
            aaa = "黒いよ"
        Else
            aaa = "何もないよ"
        End If
        MessageBox.Show(aaa)

    End Sub

#End Region


#Region "コンストラクタ"

    'コンストラクタ
    '引数：userStone  ->  ユーザの石
    Public Sub New(userStone As Integer)
        '初期設定
        InitializeOsero(userStone)
    End Sub

    Public Sub New()
    End Sub

#End Region

#Region "プロパティ"

    Private _boadSize As Integer = 8                                '盤面1辺のマス数
    Private _masu(Me.BoadSize - 1, Me.BoadSize - 1) As Integer      '8×8のマス
    Private _siro As Integer                                        '白石の数
    Private _kuro As Integer                                        '黒石の数
    Private _myStone As Integer                                     'ユーザの石（基本的に白）白:0 黒:1

    Public ReadOnly Property BattleField(x As Integer, y As Integer) As Integer
        Get
            Return _masu(x, y)
        End Get
    End Property
    Public ReadOnly Property BoadSize As Integer
        Get
            Return _boadSize
        End Get
    End Property
    Public ReadOnly Property WhiteCount As Integer
        Get
            Return _siro
        End Get
    End Property
    Public ReadOnly Property BlackCount As Integer
        Get
            Return _kuro
        End Get
    End Property
    Public ReadOnly Property UserStone As Integer
        Get
            Return _myStone
        End Get
    End Property
#End Region

#Region "プロパティの値を設定"

    '白石の数をセット
    Public Sub SetWhiteCount(value As Integer)
        _siro = value
    End Sub

    '黒石の数をセット
    Public Sub SetBlackCount(value As Integer)
        _kuro = value
    End Sub

    '自分の石が何色かをセット
    'stone 0:白 1:黒
    Public Sub SetMyStone(stone As Integer)
        _myStone = stone
    End Sub

#End Region

#Region "プロパティの値を取得"

    '白石の数を取得
    Public Function GetWhiteCount() As Integer
        Return _siro
    End Function

    '黒石の数を取得
    Public Function GetBlackCount() As Integer
        Return _kuro
    End Function

#End Region


End Class
