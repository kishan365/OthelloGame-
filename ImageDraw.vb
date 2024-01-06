
'画面に線を引いたり石を描いたりするクラス
Public Class ImageDraw

    '描画するペンと円のサイズ
    Public Shared penSize As Integer = 2
    Public Shared circleSize As Integer = 54


    '盤面の線を引く
    Public Shared Sub DrawField(panel As Panel, e As PaintEventArgs)
        Dim gra As Graphics = e.Graphics
        Dim pen As Pen = New Pen(Color.Black, 1)

        '枠線を引く
        gra.DrawLine(pen, 0, 0, 479, 0)
        gra.DrawLine(pen, 479, 0, 479, 479)
        gra.DrawLine(pen, 479, 479, 0, 479)
        gra.DrawLine(pen, 0, 479, 0, 0)

        '8×8のマス目状にする
        Dim bunkatu As Integer = 8
        Dim size As Integer = panel.Width / bunkatu
        For i = 1 To bunkatu
            gra.DrawLine(pen, 0, size, panel.Width, size)
            gra.DrawLine(pen, size, 0, size, panel.Height)
            size = size + (size / i)
        Next

        gra.Dispose()
        pen.Dispose()
    End Sub


    '指定したマスに○ or ● を描く
    'stoneColor 白:0 黒:1
    Public Shared Sub DrawStone(panel As Panel, x As Integer, y As Integer, stoneColor As Integer)
        'The values x and y here is the cordinates where to draw the stone
        Dim gra As Graphics = panel.CreateGraphics
        Dim pen As New Pen(Color.Black, penSize)
        Dim brush As SolidBrush
        'ペンの色
        If stoneColor = 0 Then
            brush = New SolidBrush(Color.White)
        Else
            brush = New SolidBrush(Color.Black)
        End If

        '8×8マスだと1マスのサイズは60×60
        'その中の54×54サイズに円を描く
        '円を描く起点
        Dim startX As Integer
        Dim startY As Integer
        startX = (x * 60) + 3
        startY = (y * 60) + 3
        '描写
        gra.DrawEllipse(pen, startX, startY, circleSize, circleSize)
        gra.FillEllipse(brush, startX, startY, circleSize, circleSize)

        gra.Dispose()
        pen.Dispose()
        brush.Dispose()
    End Sub


    '盤面を全部空白に
    Public Shared Sub ClearBoad(panel As Panel)
        Dim gra As Graphics = panel.CreateGraphics
        Dim pen As New Pen(Color.White, penSize)
        Dim brush As New SolidBrush(Color.White)

        '全部のマスを白で塗りつぶす
        Dim startX As Integer
        Dim startY As Integer
        For x = 0 To 7
            For y = 0 To 7
                startX = (x * 60) + 3
                startY = (y * 60) + 3
                gra.DrawEllipse(pen, startX, startY, circleSize, circleSize)
                gra.FillEllipse(brush, startX, startY, circleSize, circleSize)
            Next
        Next

    End Sub


    '盤面を最初の状態に
    Public Shared Sub Refresh(panel As Panel)
        ClearBoad(panel)

        '初期配置
        DrawStone(panel, 3, 3, 1)
        drawStone(panel, 4, 3, 0)
        drawStone(panel, 3, 4, 0)
        drawStone(panel, 4, 4, 1)
    End Sub

End Class
