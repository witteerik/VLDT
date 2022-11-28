Imports System.Windows.Forms
Imports System.Drawing

Public Class ProgressBarWithText
    Inherits ProgressBar

    Private _ShowProgressText As Boolean = True
    Public Property ShowProgressText As Boolean
        Get
            Return _ShowProgressText
        End Get
        Set(value As Boolean)
            _ShowProgressText = value
            Me.Invalidate()
            Me.Update()
        End Set
    End Property


    Private BlackBrush As Brush = Brushes.Black
    Private GrayBrush As Brush = Brushes.LightGray

    Private MyStringFormat As StringFormat

    Public Property TextFont As Font

    Public Sub New()

        MyStringFormat = New StringFormat
        MyStringFormat.Alignment = StringAlignment.Center
        MyStringFormat.LineAlignment = StringAlignment.Center

        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)

        TextFont = New Font(Me.Font, FontStyle.Bold)

    End Sub

    Public Sub DrawText(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        ProgressBarRenderer.DrawHorizontalBar(e.Graphics, Me.ClientRectangle)

        If Me.Value > 0 Then
            Dim ValueRect = New Rectangle With {.X = Me.ClientRectangle.X, .Y = Me.ClientRectangle.Y, .Height = Me.ClientRectangle.Height, .Width = Me.ClientRectangle.Width * (Me.Value / Me.Maximum)}
            'ProgressBarRenderer.DrawHorizontalChunks(e.Graphics, ValueRect)

            Dim MyBrush As Drawing2D.LinearGradientBrush = New Drawing2D.LinearGradientBrush(ValueRect, Color.LightGreen, Color.Green, Drawing2D.LinearGradientMode.Vertical)
            e.Graphics.FillRectangle(MyBrush, ValueRect)
        End If

        If ShowProgressText = True Then
            If Me.Value <= Me.Maximum And Me.Value >= Me.Minimum Then
                If Me.Enabled = True Then
                    e.Graphics.DrawString(Me.Value & " / " & Me.Maximum, TextFont, BlackBrush, Me.ClientRectangle, MyStringFormat)
                Else
                    e.Graphics.DrawString(Me.Value & " / " & Me.Maximum, TextFont, GrayBrush, Me.ClientRectangle, MyStringFormat)
                End If
            End If
        End If

    End Sub

End Class