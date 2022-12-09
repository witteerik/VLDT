Imports LibVLCSharp.Shared
Imports VLDT_lib

Imports System.Windows.Forms

Public Class VideoButton
    Inherits LibVLCSharp.WinForms.VideoView

    'Public WithEvents VideoPlayer As MediaPlayer

    Public CurrentVideo As Media = Nothing

    Public WithEvents VideoPlayer As MediaPlayer

    Delegate Sub NoArgumentsDelegate()

    Private LibVLC As LibVLC

    Public Sub New(ByRef LibVLC As LibVLC)

        Me.LibVLC = LibVLC
        MediaPlayer = New MediaPlayer(LibVLC)

        VideoPlayer = MediaPlayer

        'These seem to both be set to False in order for the mouse events to go through...
        Me.VideoPlayer.EnableKeyInput = False
        Me.VideoPlayer.EnableMouseInput = False

    End Sub

    Public Sub Play()

        MediaPlayer.Play(CurrentVideo)

    End Sub

    Public Sub AddOnTopControl()

        Dim NewPanel As New Panel
        NewPanel.BackColor = System.Drawing.Color.Transparent
        NewPanel.Dock = DockStyle.Fill
        AddHandler NewPanel.Click, AddressOf Test
        Me.Controls.Add(NewPanel)

    End Sub



    Public Sub SetMedia(ByVal FilePath As String)
        CurrentVideo = New Media(LibVLC, FilePath, FromType.FromPath)
    End Sub

    Private Sub VideoPlayer_VideoEndChanged(sender As Object, e As EventArgs) Handles VideoPlayer.EndReached

        If Me.InvokeRequired = True Then
            Me.Invoke(New NoArgumentsDelegate(AddressOf VideoEnded))
        Else
            VideoEnded()
        End If

    End Sub

    Private Sub VideoEnded()
        'MsgBox("Slut")
    End Sub

    Private GripPoint As Drawing.Point = Nothing

    Private Sub Test(sender As Object, e As MouseEventArgs) 'Handles Me.Click
        MsgBox("!")
    End Sub

    Private Sub ResponseItem_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        'Dim CastSender As ResponseItem = DirectCast(sender, ResponseItem)
        GripPoint = e.Location

        AddHandler Me.MouseMove, AddressOf ResponseItem_MouseMove

    End Sub

    Private Sub ResponseItem_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp

        'Dim CastSender As ResponseItem = DirectCast(sender, ResponseItem)
        RemoveHandler Me.MouseMove, AddressOf ResponseItem_MouseMove

        'SetItemLocations()

    End Sub

    Private Sub ResponseItem_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave

        'Dim CastSender As ResponseItem = DirectCast(sender, ResponseItem)
        RemoveHandler Me.MouseMove, AddressOf ResponseItem_MouseMove

        'SetItemLocations()

    End Sub


    Private Sub ResponseItem_MouseMove(sender As Object, e As MouseEventArgs)

        'Dim CastSender As ResponseItem = DirectCast(sender, ResponseItem)

        'Calculating the new location
        Dim NewLocation = Me.Location + e.Location - GripPoint

        'Limiting to parent client area
        NewLocation.X = Math.Max(0, NewLocation.X)
        NewLocation.Y = Math.Max(0, NewLocation.Y)
        NewLocation.X = Math.Min(NewLocation.X, Me.Parent.ClientRectangle.Width - Me.ClientRectangle.Width)
        NewLocation.Y = Math.Min(NewLocation.Y, Me.Parent.ClientRectangle.Height - Me.ClientRectangle.Height)

        'Setting the new location
        Me.Location = NewLocation

        ''Check if the sender is on a target
        'Dim OverlappedTarget = CheckIfOnTarget(CastSender)
        'If OverlappedTarget.IsEmpty = False Then
        '    LockToTarget(CastSender, OverlappedTarget)
        'End If

    End Sub


End Class


