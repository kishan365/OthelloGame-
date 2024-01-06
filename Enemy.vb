
'敵クラス
Public Class Enemy
    Inherits Brain


    'どこに石を置くか考える
    Public Overloads Function Think(ByRef x As Integer, ByRef y As Integer, userStone As Integer, Index As Integer) As Boolean
        '敵が人なら何もせず終了
        If EnemyType = EnemyIs.User Then
            Return False
        End If

        'どこに置くか考える
        Return MyBase.Think(x, y, userStone, EnemyLevel, Index)
    End Function


#Region "Enum"

    Public Enum EnemyIs
        User = 0
        CPU
    End Enum

    Public Enum Level
        easy = 0
        normal
        hard
        nakamura
    End Enum

#End Region

#Region "プロパティ"

    Private _enemyType As Integer   '敵のタイプ(0:CPU 1:ユーザ）
    Private _level As Integer       '敵の強さ(0:弱い 1:普通 2:強い)
    Private _thinkTime As Long      '敵の考える時間

    Public Property EnemyType As Integer
        Get
            Return _enemyType
        End Get
        Set(value As Integer)
            _enemyType = value
        End Set
    End Property
    Public Property EnemyLevel As Integer
        Get
            Return _level
        End Get
        Set(value As Integer)
            _level = value
        End Set
    End Property
    Public Property ThinkTime As Long
        Get
            Return _thinkTime
        End Get
        Set(value As Long)
            _thinkTime = value
        End Set
    End Property

#End Region

#Region "コンストラクタ"

    '引数：enemyType   ->  敵のタイプ(0:CPU 1:ユーザ）
    '      enemyLevel  ->  敵の強さ(0:弱い 1:普通 2:強い)
    '      boad        ->　8×8の盤面
    Public Sub New(enemyType As Integer, enemyLevel As Integer, boad As Integer(,))
        MyBase.New(boad)
        Me.EnemyType = enemyType
        Me.EnemyLevel = enemyLevel
    End Sub

    '引数：enemyType   ->  敵のタイプ(0:CPU 1:ユーザ）
    '      enemyLevel  ->  敵の強さ(0:弱い 1:普通 2:強い)
    '      thinkTime   ->  敵の考える時間
    '      boad        ->　8×8の盤面
    Public Sub New(enemyType As Integer, enemyLevel As Integer, thinkTime As Long, boad As Integer(,))
        MyBase.New(boad)
        Me.EnemyType = enemyType
        Me.EnemyLevel = enemyLevel
        Me.ThinkTime = thinkTime
    End Sub

#End Region

End Class
