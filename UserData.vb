Imports Newtonsoft.Json


'ユーザデータを保持、更新するクラス
Public Class UserData


#Region "ユーザデータファイル情報"

    'ユーザデータの場所
    Private dataFilePath As String = "../../../../\Data\"

    'ファイル名
    Private dataFileName As String = "userData.json"

#End Region

#Region "Enum"

    Public Enum Enemy
        User = 0
        CPU
    End Enum

    Public Enum Level
        Easy = 0
        Normal
        Hard
    End Enum

    Public Enum Flg
        FlgOff = 0
        FlgOn
    End Enum

    '石の色
    Public Enum Color
        White = 0   '白
        Black       '黒
        NoColor
    End Enum

#End Region

#Region "プロパティ"

    'プロパティ用変数
    Private _userID As String       'ユーザID（一次開発段階では"OseroUser" で固定）
    Private _userStone As Integer   '選んだ石の色（0:白　1:黒）
    Private _breakFlg As Integer    '中断フラグ(0：中断中じゃない　1：中断中)
    Private _winCount As Integer    '勝利回数
    Private _loseCount As Integer   '敗北回数
    Private _drawCount As Integer   '引き分け回数
    Private _enemyType As Integer   '敵タイプ(0:別ユーザ 1:CPU)
    Private _enemyLV As Integer     '敵レベル(0:弱 1:普 2:強)
    Private _lastBoad As Integer()   '中断時の盤面情報
    Private _moveCount As Integer   '何手目で中断したか

    'プロパティ
    Public Property UserID
        Get
            Return _userID
        End Get
        Set(value)
            _userID = value
        End Set
    End Property
    Public Property UserStone
        Get
            Return _userStone
        End Get
        Set(value)
            _userStone = value
        End Set
    End Property
    Public Property BreakFlg
        Get
            Return _breakFlg
        End Get
        Set(value)
            _breakFlg = value
        End Set
    End Property
    Public Property WinCount
        Get
            Return _winCount
        End Get
        Set(value)
            _winCount = value
        End Set
    End Property
    Public Property LoseCount
        Get
            Return _loseCount
        End Get
        Set(value)
            _loseCount = value
        End Set
    End Property
    Public Property DrawCount
        Get
            Return _drawCount
        End Get
        Set(value)
            _drawCount = value
        End Set
    End Property
    Public Property EnemyType
        Get
            Return _enemyType
        End Get
        Set(value)
            _enemyType = value
        End Set
    End Property
    Public Property EnemyLV
        Get
            Return _enemyLV
        End Get
        Set(value)
            _enemyLV = value
        End Set
    End Property
    Public Property LastBoad As Integer()
        Get
            Return _lastBoad
        End Get
        Set(value As Integer())
            _lastBoad = value
        End Set
    End Property
    Public Property MoveCount
        Get
            Return _moveCount
        End Get
        Set(value)
            _moveCount = value
        End Set
    End Property



#End Region

#Region "コンストラクタ"

    'ユーザIDを指定
    Public Sub New(id As String)
        Initialize()
    End Sub

    Public Sub New()
    End Sub

#End Region

#Region "ユーザデータを読み込み"

    Private Function Initialize() As UserData
        Dim ret As New UserData
        Try
            'ユーザデータが存在しない場合何もしない
            If IO.File.Exists(dataFilePath & dataFileName) = False Then
                '初回はユーザファイルが存在しないため、
                'ファイルが存在しなくてもエラーメッセージは表示しないよう変更
                'MessageBox.Show("ユーザーデータファイルが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ret
            End If

            'ユーザデータファイルを読み込む
            Using reader As IO.StreamReader = IO.File.OpenText(dataFilePath & dataFileName)
                ret = JsonConvert.DeserializeObject(Of UserData)(reader.ReadToEnd)
            End Using

            'ユーザデータファイルを読み込んだ結果をプロパティにセット
            'Dim ret As UserData = Initialize()
            If ret IsNot Nothing Then
                UserID = ret.UserID
                UserStone = ret.UserStone
                BreakFlg = ret.BreakFlg
                WinCount = ret.WinCount
                LoseCount = ret.LoseCount
                DrawCount = ret.DrawCount
                EnemyType = ret.EnemyType
                EnemyLV = ret.EnemyLV
                LastBoad = ret.LastBoad
                MoveCount = ret.MoveCount
            End If

        Catch ex As Exception
            MessageBox.Show("ユーザーデータの読み込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ret = Nothing
        End Try

        Return ret
    End Function

#End Region

#Region "ユーザデータを保存"

    'プロパティの値を外部ファイルに保存
    '戻り値： true:成功　false:失敗
    Public Function SaveUserData() As Boolean
        Dim ret As Boolean = False

        Try
            'ユーザデータが存在しない場合、新規作成
            If IO.File.Exists(dataFilePath & dataFileName) = False Then
                'MessageBox.Show("ユーザーデータファイルが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'Return ret
                IO.File.Create(dataFilePath & dataFileName)
            End If

            '保存
            Using writer As New IO.StreamWriter(dataFilePath & dataFileName, False, Text.Encoding.GetEncoding("UTF-8"))
                Dim dataString = JsonConvert.SerializeObject(Me)
                writer.Write(dataString)
            End Using

        Catch ex As Exception
            MessageBox.Show("ユーザーデータの更新に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

        ret = True
        Return ret
    End Function

#End Region



End Class
