Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel


<Serializable>
    Public Class ChangeItemButton
        Inherits Button

    Private MidcentreStringFormat As New StringFormat

    Private _ViewMode As ViewModes = ViewModes.Next

    Public Property ViewMode As ViewModes
            Get
                Return _ViewMode
            End Get
            Set(value As ViewModes)
                _ViewMode = value
                Me.Invalidate()
            End Set
        End Property

        Public Enum ViewModes
            [Next]
            Previous
        End Enum

        Private _Text As String = ""

    ''' <summary>
    ''' The Text property of the base class (Button) is overidden in this class, and always sets the text to an empty string if ShowText = False, since no text then should be displayed on the control.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property Text As String
            Get
                Return _Text
            End Get
            Set(value As String)
            If ShowText = False Then value = ""
            _Text = value
            End Set
        End Property

    Public Property ShowText As Boolean = True

    'Below is an alternate way to hide the text on the control...
    '<DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    'Public Shadows Property Text As String = ""

    Public Sub New()

            MyBase.New

        Me.MidcentreStringFormat.Alignment = StringAlignment.Center
        Me.MidcentreStringFormat.LineAlignment = StringAlignment.Center

    End Sub


        Private Sub DrawSymbol(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Me.Paint

            Dim MidX = ClientRectangle.Width / 2
            Dim MidY = ClientRectangle.Height / 2
            Dim PaintQuadrantSide As Single = Math.Min(MidX * 1.25, MidY * 1.25)
            Dim PaintQuadrantX As Single = MidX - (PaintQuadrantSide / 2)
            Dim PaintQuadrantY As Single = MidY - (PaintQuadrantSide / 2)
        Dim MidXPush = ClientRectangle.Width / 10

        Select Case ViewMode
                Case ViewModes.Next

                    Dim PlayBrush As SolidBrush
                    Dim PlayPen As Pen
                    If Enabled = True Then
                        PlayBrush = New SolidBrush(Color.Green)
                        PlayPen = New Pen(Color.DarkGreen)
                    Else
                        PlayBrush = New SolidBrush(Color.LightGray)
                        PlayPen = New Pen(Color.Gray)
                    End If

                    Dim Point1 As New PointF(PaintQuadrantX, PaintQuadrantY)
                    Dim Point2 As New PointF(PaintQuadrantX, PaintQuadrantY + PaintQuadrantSide)
                    Dim Point3 As New PointF(PaintQuadrantX + PaintQuadrantSide, PaintQuadrantY + PaintQuadrantSide / 2)
                    Dim Points As PointF() = {Point1, Point2, Point3}

                    e.Graphics.FillPolygon(PlayBrush, Points)
                    e.Graphics.DrawPolygon(PlayPen, Points)

                If ShowText = True Then
                    e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point(MidX - MidXPush, MidY), MidcentreStringFormat)
                End If

            Case ViewModes.Previous

                Dim PlayBrush As SolidBrush
                Dim PlayPen As Pen
                If Enabled = True Then
                    PlayBrush = New SolidBrush(Color.Green)
                    PlayPen = New Pen(Color.DarkGreen)
                Else
                    PlayBrush = New SolidBrush(Color.LightGray)
                    PlayPen = New Pen(Color.Gray)
                End If

                Dim Point1 As New PointF(PaintQuadrantX + PaintQuadrantSide, PaintQuadrantY)
                Dim Point2 As New PointF(PaintQuadrantX + PaintQuadrantSide, PaintQuadrantY + PaintQuadrantSide)
                Dim Point3 As New PointF(PaintQuadrantX, PaintQuadrantY + PaintQuadrantSide / 2)
                Dim Points As PointF() = {Point1, Point2, Point3}

                e.Graphics.FillPolygon(PlayBrush, Points)
                e.Graphics.DrawPolygon(PlayPen, Points)

                If ShowText = True Then
                    e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point(MidX + MidXPush, MidY), MidcentreStringFormat)
                End If

        End Select

        End Sub

        Private Sub AudioButton_Validated(sender As Object, e As EventArgs) Handles Me.Validated

        If ShowText = False Then Me._Text = ""

    End Sub
    End Class
