Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class PointerApp
    Inherits Form

    ' Declarações de funções da API do Windows para tornar a janela transparente para cliques
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    End Function

    ' Constante para o índice do estilo da janela
    Private Const GWL_EXSTYLE As Integer = -20

    ' Constante para definir a janela como transparente para cliques
    Private Const WS_EX_TRANSPARENT As Integer = &H20
    Private Const WS_EX_LAYERED As Integer = &H80000

    Private pointerBitmap As Bitmap
    Private cursorHotspot As Point

    Public Sub New()
        ' Usa o cursor padrão como ícone e cria uma versão preta dele
        Dim cursorIcon As Cursor = Cursors.Default
        pointerBitmap = CursorToBitmap(cursorIcon, Color.Black)
        cursorHotspot = cursorIcon.HotSpot ' Obtém o "hotspot" do cursor

        Me.DoubleBuffered = True
        Me.FormBorderStyle = FormBorderStyle.None ' Remove as bordas da janela
        Me.StartPosition = FormStartPosition.Manual ' Permite posicionar a janela manualmente
        Me.ShowInTaskbar = False ' Não mostra na barra de tarefas
        Me.TransparencyKey = Color.Magenta ' Define a cor de transparência para esconder o fundo
        Me.BackColor = Color.Magenta ' Define a cor de fundo como a cor transparente

        ' Torna a janela transparente para cliques
        MakeWindowTransparent()
    End Sub

    ' Converte o cursor em um bitmap e aplica uma cor
    Private Function CursorToBitmap(cursor As Cursor, color As Color) As Bitmap
        Dim bmp As New Bitmap(cursor.Size.Width, cursor.Size.Height)
        Using g As Graphics = Graphics.FromImage(bmp)
            cursor.Draw(g, New Rectangle(Point.Empty, cursor.Size))
        End Using

        ' Torna o bitmap preto
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                Dim pixelColor As Color = bmp.GetPixel(x, y)
                If pixelColor.A > 0 Then ' Mantém a transparência, mas troca a cor
                    bmp.SetPixel(x, y, Color.Black)
                End If
            Next
        Next

        Return bmp
    End Function

    ' Pinta o ponteiro preto na posição do cursor
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.DrawImage(pointerBitmap, 0, 0)
    End Sub

    ' Mantém a janela do ponteiro sempre na posição do cursor e com tamanho correto
    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Me.Size = New Size(pointerBitmap.Width, pointerBitmap.Height) ' Define o tamanho da janela do ponteiro
        Me.TopMost = True ' Mantém a janela acima das outras
        Me.Show() ' Mostra a janela
        Me.UpdatePointerPosition()
    End Sub

    ' Atualiza a posição do ponteiro para acompanhar o cursor do mouse
    Private Sub UpdatePointerPosition()
        Dim timer As New Timer()
        AddHandler timer.Tick, Sub(sender, e)
                                   ' Ajusta a localização com base no "hotspot" do cursor
                                   Me.Location = New Point(Cursor.Position.X - cursorHotspot.X, Cursor.Position.Y - cursorHotspot.Y)
                                   Me.Invalidate() ' Redesenha o ponteiro
                               End Sub
        timer.Interval = 10 ' Atualiza a cada 10ms para garantir suavidade
        timer.Start()
    End Sub

    ' Faz com que a janela seja transparente para cliques
    Private Sub MakeWindowTransparent()
        Dim exStyle As Integer = GetWindowLong(Me.Handle, GWL_EXSTYLE)
        SetWindowLong(Me.Handle, GWL_EXSTYLE, exStyle Or WS_EX_TRANSPARENT Or WS_EX_LAYERED)
    End Sub

    <STAThread>
    Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Dim app As New PointerApp()
        Application.Run(app)
    End Sub
End Class
