Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Gma.System.MouseKeyHook 

Namespace AMP
    Public Class FormData
        Public colors As Color() = {Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Magenta, Color.Black}
        Public guidLines As Boolean() = {True, True, True, True, True} ' Controle para exibir as linhas
    End Class

    Friend Class Guides
        ' Atributos para armazenar os pontos das linhas
        Public LinePoints As List(Of Point) = New List(Of Point)()

        Public Sub New(mousePosition As Point)
            ' Inicializar as linhas com base na posição do mouse
            UpdateLines(mousePosition)
        End Sub

        Public Sub DrawGuides(dataf As FormData, e As PaintEventArgs, linesVisible As Boolean)
            ' Desenhar as guias usando os pontos armazenados se as linhas estiverem visíveis
            If linesVisible Then
                If dataf.guidLines(1) Then
                    Dim pen2 As New Pen(dataf.colors(3))
                    e.Graphics.DrawBezier(pen2, LinePoints(4), LinePoints(5), LinePoints(6), LinePoints(7))
                End If
                If dataf.guidLines(2) Then
                    Dim pen3 As New Pen(dataf.colors(4))
                    e.Graphics.DrawBezier(pen3, LinePoints(8), LinePoints(9), LinePoints(10), LinePoints(11))
                End If
            End If
        End Sub

        Public Sub UpdateLines(mousePosition As Point)
            ' Atualiza a posição das linhas para seguir o mouse
            LinePoints.Clear() ' Limpa os pontos antes de atualizar
            LinePoints.Add(New Point(mousePosition.X - 430, mousePosition.Y + 108 - 30))
            LinePoints.Add(New Point(mousePosition.X - 200, mousePosition.Y - 30))
            LinePoints.Add(New Point(mousePosition.X + 200, mousePosition.Y - 30))
            LinePoints.Add(New Point(mousePosition.X + 430, mousePosition.Y + 108 - 30))
            LinePoints.Add(New Point(mousePosition.X - 420, mousePosition.Y + 400 - 103))
            LinePoints.Add(New Point(mousePosition.X - 125, mousePosition.Y - 103))
            LinePoints.Add(New Point(mousePosition.X + 125, mousePosition.Y - 103))
            LinePoints.Add(New Point(mousePosition.X + 420, mousePosition.Y + 400 - 103))
            LinePoints.Add(New Point(mousePosition.X - 210, mousePosition.Y + 400 - 90))
            LinePoints.Add(New Point(mousePosition.X - 88, mousePosition.Y - 108))
            LinePoints.Add(New Point(mousePosition.X + 88, mousePosition.Y - 108))
            LinePoints.Add(New Point(mousePosition.X + 210, mousePosition.Y + 400 - 90))
            LinePoints.Add(New Point(mousePosition.X, mousePosition.Y))
            LinePoints.Add(New Point(mousePosition.X, mousePosition.Y))
        End Sub
    End Class

    Public Class Form1
        Inherits Form

        <DllImport("user32.dll")>
        Private Shared Function SetCursorPos(x As Integer, y As Integer) As Boolean
        End Function

        Private guides As Guides
        Private currentMousePosition As Point
        Private linesVisible As Boolean = True ' Variável para controlar a visibilidade das linhas
        Private Const WS_EX_LAYERED As Integer = &H80000
        Private Const WS_EX_TRANSPARENT As Integer = &H20
        Private Const LWA_COLORKEY As Integer = 1
        Private WithEvents globalHook As IKeyboardMouseEvents

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function SetLayeredWindowAttributes(hWnd As IntPtr, crKey As Integer, bAlpha As Byte, dwFlags As Integer) As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Shared Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer
        End Function

        Public Sub New()
            Me.FormBorderStyle = FormBorderStyle.None
            Me.BackColor = Color.Lime ' Cor de fundo que será removida
            Me.TransparencyKey = Color.Lime ' Tornar a cor de fundo transparente
            Me.TopMost = True
            Me.WindowState = FormWindowState.Maximized
            Me.ShowInTaskbar = False
            Me.Cursor = Cursors.Cross ' Alterar o cursor para uma cruz

            ' Estabelecer o estilo de camada e transparência
            Dim style As Integer = GetWindowLong(Me.Handle, -20) ' GWL_EXSTYLE
            SetWindowLong(Me.Handle, -20, style Or WS_EX_LAYERED Or WS_EX_TRANSPARENT)
            SetLayeredWindowAttributes(Me.Handle, Color.Lime.ToArgb(), 255, LWA_COLORKEY)

            guides = New Guides(Control.MousePosition) ' Inicializa as linhas na posição atual do mouse
            Timer1.Start() ' Iniciar o timer para atualizar a posição do mouse

            ' Inicializa o hook global para capturar teclas
            globalHook = Hook.GlobalEvents()
        End Sub

        Private WithEvents Timer1 As New Timer() With {.Interval = 10} ' Atualização a cada 10ms

        Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
            ' Obter a posição do mouse
            currentMousePosition = Control.MousePosition
            guides.UpdateLines(currentMousePosition) ' Atualiza as posições das linhas
            Me.Invalidate() ' Solicitar atualização da tela
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim dataf As New FormData()
            guides.DrawGuides(dataf, e, linesVisible) ' Desenhar as guias na posição atual do mouse
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            ' Encaminhar o clique do mouse para a janela abaixo
            If e.Button = MouseButtons.Left Then
                Dim cursorPosition As Point = Control.MousePosition
                Cursor.Position = cursorPosition ' Define a posição do cursor
                SendMessage(GetForegroundWindow(), WM_LBUTTONDOWN, 0, MakeLParam(cursorPosition.X, cursorPosition.Y))
                SendMessage(GetForegroundWindow(), WM_LBUTTONUP, 0, MakeLParam(cursorPosition.X, cursorPosition.Y))
            End If
            MyBase.OnMouseDown(e)
        End Sub

        Private Sub globalHook_KeyDown(sender As Object, e As KeyEventArgs) Handles globalHook.KeyDown
            ' Verifica se a tecla F10 foi pressionada
            If e.KeyCode = Keys.F10 Then
                linesVisible = Not linesVisible ' Alterna a visibilidade das linhas
            End If
        End Sub

        <DllImport("user32.dll")>
        Private Shared Function GetForegroundWindow() As IntPtr
        End Function

        <DllImport("user32.dll")>
        Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
        End Function

        Private Const WM_LBUTTONDOWN As Integer = &H201
        Private Const WM_LBUTTONUP As Integer = &H202

        Private Function MakeLParam(x As Integer, y As Integer) As IntPtr
            Return CType((y << 16) Or (x And &HFFFF), IntPtr)
        End Function

        Public Shared Sub Main()
            Application.Run(New Form1())
        End Sub
    End Class
End Namespace
