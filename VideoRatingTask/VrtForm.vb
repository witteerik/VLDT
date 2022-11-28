Imports LibVLCSharp.Shared
Imports VLDT_lib
Public Class VrtForm

    Public MyLibVLC As LibVLC

    Public WithEvents VideoPlayer As MediaPlayer

    Private RatingStimulusSet As New RatingStimulusSet

    Private Randomizer As New Random

    Private CurrentTestStimulus As RatingStimulus

    Private TestResultExportFolder As String = ""

    Private ParticipantID As String = ""
    Private ParticipantNumber As Integer

    Private WithEvents LoadFilesTimer As New Windows.Forms.Timer With {.Interval = 500}
    Private WithEvents ShutDownTimer As New Windows.Forms.Timer With {.Interval = 500}

    Delegate Sub NoArgumentsDelegate()

    Private Sub VrtForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            'Setting culture to invariant (this will most probably not affect other threads.... like from timers...)
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture

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
            MsgBox(ex.ToString, MsgBoxStyle.Critical, My.Application.Info.Title)
            'ShutDownTimer.Start()
        End Try

        Threading.Thread.Sleep(3000)

        Try

            Dim CastSplashScreen As VRT_SplashScreen = DirectCast(My.Application.SplashScreen, VRT_SplashScreen)
            CastSplashScreen.CloseSafe()

        Catch ex As Exception
            'Ignores any error here
        End Try

        LoadFilesTimer.Start()

    End Sub

    Private Sub LoadInputFiles() Handles LoadFilesTimer.Tick
        LoadFilesTimer.Stop()

        Dim fd As New OpenFileDialog
        fd.Title = "Please select the video rating task file (.txt) and then click OK, or cancel to close the app!"
        fd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        fd.CheckFileExists = True
        fd.Multiselect = False

        Dim DialogResult = fd.ShowDialog(Me)
        If DialogResult = DialogResult.OK Then

            Dim SelectedFilePath = fd.FileName

            RatingStimulusSet = RatingStimulusSet.LoadSetupFile(SelectedFilePath, MyLibVLC, Randomizer)

            If RatingStimulusSet Is Nothing Then
                MsgBox("Unable to load the video rating task file from: " & SelectedFilePath & vbCrLf & vbCrLf & "Unable to continue!", MsgBoxStyle.Exclamation, "Error loading file!")
                ShutDownTimer.Start()
                Exit Sub
            End If
        Else
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Setting GUI strings
        Replay_Button.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.Replay)
        ChangeView_Button.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.ChangeView)
        ShowNextNonCompleteItem_Button.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.Next)

        'Getting the folder in which to store the results
        Dim fbd = New FolderBrowserDialog()
        fbd.Description = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.SelectFolder)
        fbd.ShowNewFolderButton = True
        fbd.SelectedPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        Dim fbd_DialogResult = fbd.ShowDialog(Me)
        If fbd_DialogResult = DialogResult.OK Then

            Dim OutputFolder As String = fbd.SelectedPath

            'Trying to save a log message to the output folder (just to check that it works)
            Try
                Utils.SendInfoToLog("Initiated video-based rating decision test. Test results will be saved in the folder: " & OutputFolder,, OutputFolder)

                'String the output folder
                TestResultExportFolder = OutputFolder

            Catch ex As Exception
                MsgBox("Unable to save to the specified output folder: " & OutputFolder & vbCrLf & vbCrLf & "Please restart the app and specify a different folder!" & vbCrLf & vbCrLf & "Unable to continue!", MsgBoxStyle.Exclamation, "Invalid output folder selected!")
                ShutDownTimer.Start()
                Exit Sub
            End Try
        Else
            MsgBox("No folder in which to store test results was selected!" & vbCrLf & vbCrLf & "Unable to continue!", MsgBoxStyle.Exclamation, "No output folder selected!")
            ShutDownTimer.Start()
            Exit Sub
        End If

        ' Getting the participant ID (and number, if response keys and the order of blocks and should be counter balanced)
        Dim ParticipantDialog As New ParticipantDialog
        ParticipantDialog.ShowDialog()

        If ParticipantDialog.DialogResult = DialogResult.OK Then
            ParticipantNumber = ParticipantDialog.ParticipantNumber
            ParticipantID = ParticipantDialog.ParticipantID
        Else
            ShutDownTimer.Start()
            Exit Sub
        End If


        Item_ProgressBar.Minimum = 0
        Item_ProgressBar.Maximum = RatingStimulusSet.StimulusList.Count
        Item_ProgressBar.Value = 0

        'Enables the MainTableLayoutPanel
        MainTableLayoutPanel.Enabled = True

    End Sub

    Private Sub ShutDownOnStart() Handles ShutDownTimer.Tick
        ShutDownTimer.Stop()
        Me.Close()
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
            MsgBox(Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.FinishedTest) & vbCrLf & vbCrLf & Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.CloseApp), MsgBoxStyle.Information, My.Application.Info.Title)

            SetDynamicNextPreviousButtonsEnabledState()

        End If

    End Sub

    Private Sub ShowPreviousStimulus() Handles ShowPreviousItem_Button.Click

        CurrentTestStimulus = RatingStimulusSet.GetPreviousStimulus

        If CurrentTestStimulus IsNot Nothing Then
            ShowNewStimulus()
        Else
            ShowNextItem_Button.Enabled = True
            MsgBox(Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.FirstVideo), MsgBoxStyle.Information, My.Application.Info.Title)
        End If

    End Sub

    Private Sub ShowNextStimulus() Handles ShowNextItem_Button.Click

        CurrentTestStimulus = RatingStimulusSet.GetNextStimulus

        If CurrentTestStimulus IsNot Nothing Then
            ShowNewStimulus()
        Else
            ShowNextItem_Button.Enabled = True
            MsgBox(Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.LastVideo), MsgBoxStyle.Information, My.Application.Info.Title)
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

    Private Function CreateExportFileName(ByVal ParticipantID As String, ByVal ParticipantNumber As Integer, ByVal IsTrialExport As Boolean) As String

        Dim ParticipantString As String = ParticipantID & "_" & ParticipantNumber.ToString("000")

        If IsTrialExport = True Then
            Return IO.Path.Combine(TestResultExportFolder, "VRT_" & ParticipantString & "_TestTrialExport", ParticipantString)
        Else
            Return IO.Path.Combine(TestResultExportFolder, "VRT_" & ParticipantString)
        End If

    End Function

    Private TrialExportIncludeHeadings As Boolean = True
    Private Sub ResponseGiven() Handles RatingPanel.ResponseGiven

        'Saving results from the current rating stimulus
        If CurrentTestStimulus IsNot Nothing Then
            Dim TrialExportFileName As String = CreateExportFileName(ParticipantID, ParticipantNumber, True)

            'Exporting data
            Utils.SendInfoToLog(CurrentTestStimulus.ToString(TrialExportIncludeHeadings), IO.Path.GetFileName(TrialExportFileName), IO.Path.GetDirectoryName(TrialExportFileName), True, True)
            TrialExportIncludeHeadings = False

        End If

        'Prepares for next stimulus
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

    Private Sub VrtForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        'Saving results on close
        If RatingStimulusSet IsNot Nothing Then
            Dim ExportFileName As String = CreateExportFileName(ParticipantID, ParticipantNumber, False)


            RatingStimulusSet.SaveResults(ExportFileName)
        End If

    End Sub

End Class



