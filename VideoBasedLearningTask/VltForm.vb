Imports LibVLCSharp.Shared
Imports VLDT_lib
Public Class VltForm

    Public MyLibVLC As LibVLC



    Private Sub VltForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            'Setting culture to invariant (this will most probably not affect other threads.... like from timers...)
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture

            'https://wiki.videolan.org/VLC_command-line_help/

            'Dim Options() As String = {"--video-filter=gaussianblur", "--gaussianblur-sigma=5"}
            'MyLibVLC = New LibVLC(Options)

            MyLibVLC = New LibVLC()
            'VideoPlayer = New MediaPlayer(MyLibVLC)
            'VideoView.MediaPlayer = VideoPlayer

            'Some code to export available filter names
            'Dim VideoFilters = MyLibVLC.VideoFilters
            'Dim Filternames As New List(Of String)
            'For i = 0 To VideoFilters.Length - 1
            '    Filternames.Add(VideoFilters(i).Name)
            'Next
            'Utils.SendInfoToLog(String.Join(vbCrLf, Filternames))

        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, My.Application.Info.Title)
            'ShutDownTimer.Start()
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim NewVideoButton As New VLDT_lib.VideoButton(MyLibVLC)

        NewVideoButton.SetMedia("C:\VLDT\PilotExperiment1_Test\Block01\Real\1004-R-2817.mp4")
        NewVideoButton.BackColor = Color.Red
        'NewVideoButton.Dock = DockStyle.Fill

        NewVideoButton.Left = Me.Width / 2
        NewVideoButton.Top = Me.Height / 2
        NewVideoButton.Height = 200
        NewVideoButton.Width = 200


        Me.Controls.Add(NewVideoButton)

        NewVideoButton.Play()

    End Sub

    Private Sub VltForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing


        'Disposes all media, the media player and the LibVLC instances
        'If PractiseTestMaterial IsNot Nothing Then PractiseTestMaterial.DisposeVideos()
        'If SharpTestMaterial IsNot Nothing Then SharpTestMaterial.DisposeVideos()
        'VideoPlayer.Dispose()
        MyLibVLC.Dispose()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'Temprarily high-jacking this app to run calculation of FindMatches

        Dim FindMatches = New VLDT_lib.FindMatches
        FindMatches.Feed("C:\EriksDokument\RStudioProjects\SSMS\Final_Selection\FindMatchesInput_100.txt")
        FindMatches.MatchLists("C:\EriksDokument\RStudioProjects\SSMS\Final_Selection\FindMatchesOutput_80.txt", 80,, False)

    End Sub
End Class