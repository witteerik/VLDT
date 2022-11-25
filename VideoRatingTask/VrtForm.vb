Imports LibVLCSharp.Shared
Imports VLDT_lib
Public Class VrtForm

    Public MyLibVLC As LibVLC

    Public WithEvents VideoPlayer As MediaPlayer

    Private RatingStimulusSet As New RatingStimulusSet

    Private Randomizer As New Random

    Private CurrentTestStimulus As RatingStimulus

    Delegate Sub NoArgumentsDelegate()

    Private Sub VrtForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            'https://wiki.videolan.org/VLC_command-line_help/

            'Dim Options() As String = {"--video-filter=gaussianblur", "--gaussianblur-sigma=5"}
            'MyLibVLC = New LibVLC(Options)

            MyLibVLC = New LibVLC()
            VideoPlayer = New MediaPlayer(MyLibVLC)
            VideoView.MediaPlayer = VideoPlayer

            'Some code to export available filter names
            'Dim VideoFilters = MyLibVLC.VideoFilters
            'Dim Filternames As New List(Of String)
            'For i = 0 To VideoFilters.Length - 1
            '    Filternames.Add(VideoFilters(i).Name)
            'Next
            'Utils.SendInfoToLog(String.Join(vbCrLf, Filternames))

        Catch ex As Exception
            MsgBox(ex.ToString)
            'ShutDownTimer.Start()
        End Try


        Dim VideoFiles = {"C:\VLDT\PilotExperiment1_Test\Block01\Pseudo\1001-P-16294.mp4", "C:\VLDT\PilotExperiment1_Test\Block01\Pseudo\1002-P-10864.mp4", "C:\VLDT\PilotExperiment1_Test\Block01\Pseudo\1005-P-2817.mp4"}

        RatingStimulusSet.LoadTestStimuli(VideoFiles, MyLibVLC, True, Randomizer)

        Item_ProgressBar.Minimum = 0
        Item_ProgressBar.Maximum = RatingStimulusSet.StimulusList.Count
        Item_ProgressBar.Value = 0

        Dim QuestionSet As New List(Of RatingQuestion)
        QuestionSet.Add(New RatingQuestion With {.Question = "My question...", .CategoricalResponseAlternatives = New List(Of String) From {"1", "2", "3", "4", "5", "6", "7"}})
        QuestionSet.Add(New RatingQuestion With {.Question = "My second question...", .CategoricalResponseAlternatives = New List(Of String) From {"1", "2", "3", "4", "5"}})
        QuestionSet.Add(New RatingQuestion With {.Question = "My third question...", .CategoricalResponseAlternatives = New List(Of String) From {"1", "2", "3"}})
        QuestionSet.Add(New RatingQuestion With {.Question = "My fourth question... which is very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very long...", .CategoricalResponseAlternatives = New List(Of String) From {"Yes", "No", "Maybe"}})
        QuestionSet.Add(New RatingQuestion With {.Question = "My fifth question...", .ScaleValues = New List(Of Integer) From {1, 2, 3, 4, 5}})

        For Each Stimulus In RatingStimulusSet.StimulusList
            Stimulus.SetQuestions(QuestionSet)
        Next


    End Sub

    Private Sub ChangeItemButton1_Click(sender As Object, e As EventArgs) Handles ShowNextNonCompleteItem_Button.Click
        ShowNextNonCompleteStimulus()
    End Sub

    ''' <summary>
    ''' Shows the next stimulus which do not have a complete answer.
    ''' </summary>
    Private Sub ShowNextNonCompleteStimulus()

        If RatingStimulusSet.StimulusList.Count = 0 Then
            MsgBox("No videos have been loaded!", MsgBoxStyle.Information, "Nothing to show!")
            Exit Sub
        End If

        CurrentTestStimulus = RatingStimulusSet.GetNextNonCompleteStimulus

        If CurrentTestStimulus IsNot Nothing Then
            ShowNewStimulus()
        Else
            Item_ProgressBar.Value = RatingStimulusSet.StimulusList.Count
            MsgBox("The rating task is now completed! You may now close the app!", MsgBoxStyle.Information, "Finished!")

            SetDynamicNextPreviousButtonsEnabledState()

        End If

    End Sub

    Private Sub ShowPreviousStimulus() Handles ShowPreviousItem_Button.Click

        CurrentTestStimulus = RatingStimulusSet.GetPreviousStimulus

        If CurrentTestStimulus IsNot Nothing Then
            ShowNewStimulus()
        Else
            ShowNextItem_Button.Enabled = True
            MsgBox("You're back at the first presented video!", MsgBoxStyle.Information, "You reached the first video!")
        End If

    End Sub

    Private Sub ShowNextStimulus() Handles ShowNextItem_Button.Click

        CurrentTestStimulus = RatingStimulusSet.GetNextStimulus

        If CurrentTestStimulus IsNot Nothing Then
            ShowNewStimulus()
        Else
            ShowNextItem_Button.Enabled = True
            MsgBox("You've reached the last back at the first presented video!", MsgBoxStyle.Information, "You reached the first video!")
        End If

    End Sub

    Private Sub ShowNewStimulus()

        ShowNextNonCompleteItem_Button.Enabled = False
        ShowNextItem_Button.Enabled = False
        ShowNextItem_Button.Enabled = False

        RatingPanel.Controls.Clear()

        'Play video
        Try
            MainTableLayoutPanel.Enabled = False
            VideoPlayer.Play(CurrentTestStimulus.CurrentVideo)

            Item_ProgressBar.Value = RatingStimulusSet.CurrentItemIndex + 1

        Catch ex As Exception
            MsgBox("An error has occured! Unable to play the current video. Please click ok to continue!", MsgBoxStyle.Critical, "Unable to play video!")
        End Try

    End Sub

    Private Sub PlayAgain() Handles Replay_Button.Click

        If CurrentTestStimulus IsNot Nothing Then

            'Play video
            Try
                MainTableLayoutPanel.Enabled = False
                VideoPlayer.Play(CurrentTestStimulus.CurrentVideo)

            Catch ex As Exception
                MsgBox("An error has occured! Unable to play the current video. Please click ok to continue!", MsgBoxStyle.Critical, "Unable to play video!")
            End Try
        End If

    End Sub


    Private Sub VideoPlayer_VideoEndChanged(sender As Object, e As EventArgs) Handles VideoPlayer.EndReached

        If Me.InvokeRequired = True Then
            Me.Invoke(New NoArgumentsDelegate(AddressOf ShowResponseAlternatives))
        Else
            ShowResponseAlternatives()
        End If

    End Sub


    Private Sub ShowResponseAlternatives()

        MainTableLayoutPanel.Enabled = True
        Replay_Button.Enabled = True

        If RatingPanel.Controls.Count > 0 Then
            'This means that the user pressed play again. No need to add stuff again
            Exit Sub
        End If

        RatingPanel.AddQuestions(CurrentTestStimulus)

    End Sub


    Private Sub ResponseGiven() Handles RatingPanel.ResponseGiven

        SetDynamicNextPreviousButtonsEnabledState()

        ShowNextNonCompleteItem_Button.Enabled = True

    End Sub

    Private Sub SetDynamicNextPreviousButtonsEnabledState()

        If RatingStimulusSet.CurrentItemIndex <= 0 Then
            ShowPreviousItem_Button.Enabled = False
        Else
            ShowPreviousItem_Button.Enabled = True
        End If

        If RatingStimulusSet.CurrentItemIndex >= RatingStimulusSet.StimulusList.Count - 1 Then
            ShowNextItem_Button.Enabled = False
        Else
            ShowNextItem_Button.Enabled = True
        End If

    End Sub

    Private Sub RatingItems_TableLayoutPanel_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        RatingPanel.Visible = True
    End Sub

    Private Sub ChangeView_Button_Click(sender As Object, e As EventArgs) Handles ChangeView_Button.Click

        If Content_SplitContainer.Orientation = Orientation.Horizontal Then
            Content_SplitContainer.Orientation = Orientation.Vertical
        ElseIf Content_SplitContainer.Orientation = Orientation.Vertical Then
            Content_SplitContainer.Orientation = Orientation.Horizontal
        End If

    End Sub

    Private Sub ShowPreviousStimulus(sender As Object, e As EventArgs) Handles ShowNextItem_Button.Click, ShowPreviousItem_Button.Click

    End Sub

    Private Sub ShowNextStimulus(sender As Object, e As EventArgs) Handles ShowNextItem_Button.Click

    End Sub
End Class



