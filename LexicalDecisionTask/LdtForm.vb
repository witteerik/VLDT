Imports LibVLCSharp.Shared
Imports VLDT_lib

Public Class LdtForm

    Public MyLibVLC As LibVLC

    Public WithEvents VideoPlayer As MediaPlayer
    Private Randomizer As Random

    Private CurrentTestSpecification As TestSpecification

    Private RealKey As Keys
    Private PseudoKey As Keys
    Private RealColor As Color = Color.DarkGreen
    Private PseudoColor As Color = Color.Red
    Private TestResultExportFolder As String = ""
    Private PractiseTestMaterial As TestMaterial = Nothing
    Private SharpTestMaterial As TestMaterial = Nothing
    Private ActiveTestMaterial As TestMaterial = Nothing
    Private ParticipantID As String = ""

    ''' <summary>
    ''' A non-zero positive integer representing the sequential order in which the current participant is tested withing the entire data collection. (Is used to keep track of block orders)
    ''' </summary>
    Private ParticipantNumber As Integer

    Private WaitingToStartBlock As Boolean = False

    Private VideoEnded As Boolean

    Private CurrentGuiMode As GuiModes = GuiModes.Test ' Setting to Test so that it gets updated on the first call (as it is actually only updated when changed...)

    Private TrialExportIncludeHeadings As Boolean = True

    ''' <summary>
    ''' Holds the currently presented test item
    ''' </summary>
    Private CurrentItem As TestItem = Nothing

    Private OriginalBackColor As Color

    Private WithEvents InitiateAppTimer As New Windows.Forms.Timer With {.Interval = 10} ' This timer is only oned once, in order to start the application on a short delay. The timer is a Windows forms timer and live on the main UI thread.
    Private WithEvents ShutDownTimer As New Windows.Forms.Timer With {.Interval = 30} ' This timer is only oned once, in order to start the application on a short delay and then directly shutting it down. The timer is a Windows forms timer and live on the main UI thread.

    Private WithEvents TrialInitiationTimer As New System.Timers.Timer With {.Interval = 10, .AutoReset = False}
    Private WithEvents StartTrialTimer As New System.Timers.Timer With {.AutoReset = False}
    Private WithEvents ActivateKeyDownHandlerTimer As New System.Timers.Timer With {.Interval = 50, .AutoReset = False}
    Private WithEvents VideoDurationTimer As New System.Timers.Timer With {.AutoReset = False}
    Private WithEvents ResponseTimeTimer As New System.Timers.Timer With {.AutoReset = False}

    Private WithEvents BackupTimer As New System.Timers.Timer With {.AutoReset = False}

    Private TimedResponseStopWatch As New Stopwatch

    Private PlayCommandTime As DateTime


    Delegate Sub VideoViewVisibleDelegate(ByVal Visible As Boolean)
    Delegate Sub SetGuiModesDelegate(ByVal GuiMode As GuiModes)
    Delegate Sub IssueResponseDelegate(ByVal Result As Response, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)
    Delegate Sub NoArgumentsDelegate()


    Private Sub LdtForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load


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
            ShutDownTimer.Start()
        End Try

        'Sleeps a while to show the splash screen
        Threading.Thread.Sleep(3000)

        Try

            Dim CastSplashScreen As VLDT_SplashScreen = DirectCast(My.Application.SplashScreen, VLDT_SplashScreen)
            CastSplashScreen.CloseSafe()

        Catch ex As Exception
            'Ignores any error here
        End Try


        InitiateAppTimer.Start()

    End Sub


    Private Sub InitiateTest() Handles InitiateAppTimer.Tick
        InitiateAppTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.InitiateAppTimer)

        'Setting culture to invariant (this will most probably not affect other threads.... like from timers...)
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture
        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture

        'Setting the Background_TableLayoutPanel backcolor to black and stores it as the OriginalBackColor
        Background_TableLayoutPanel.BackColor = Color.Black
        OriginalBackColor = Background_TableLayoutPanel.BackColor

        MsgBox("Please click OK and then locate the lexical decision test file (.txt)!", MsgBoxStyle.Information, "Locate test file")

        Dim fd As New OpenFileDialog
        fd.Title = "Please select the lexical decision test file (.txt) and then click OK, or cancel to close the app!"
        fd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        fd.CheckFileExists = True
        fd.Multiselect = False

        Dim DialogResult = fd.ShowDialog(Me)
        If DialogResult = DialogResult.OK Then

            Dim SelectedFilePath = fd.FileName

            CurrentTestSpecification = TestSpecification.LoadTestSpecificationFile(SelectedFilePath)

            If CurrentTestSpecification Is Nothing Then
                MsgBox("Unable to load the lexical decision test file from: " & SelectedFilePath & vbCrLf & vbCrLf & "Unable to continue!", MsgBoxStyle.Exclamation, "Error loading file!")
                ShutDownTimer.Start()
                Exit Sub
            End If
        Else
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Getting the folder in which to store the results
        Dim fbd = New FolderBrowserDialog()
        fbd.Description = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.SelectFolder)
        fbd.ShowNewFolderButton = True
        fbd.SelectedPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        Dim fbd_DialogResult = fbd.ShowDialog(Me)
        If fbd_DialogResult = DialogResult.OK Then

            Dim OutputFolder As String = fbd.SelectedPath

            'Trying to save a log message to the output folder (just to check that it works)
            Try
                Utils.SendInfoToLog("Initiated lexical decision test. Test results wil be saved in the folder: " & OutputFolder,, OutputFolder)

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

        'Setting up the Form randomizer 
        If CurrentTestSpecification.RandomSeed.HasValue = True Then
            Randomizer = New Random(CurrentTestSpecification.RandomSeed)
        Else
            Randomizer = New Random
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


        'Selecting response keys
        If CurrentTestSpecification.RandomizeResponseKeys = True Then

            'Randomizing keys
            Dim RandomIndex As Integer = Randomizer.Next(0, 2)
            RealKey = CurrentTestSpecification.AvailableResponseKeys(RandomIndex)
            'Selecting the other available key for incorrect response
            If RandomIndex = 0 Then
                PseudoKey = CurrentTestSpecification.AvailableResponseKeys(1)
            Else
                PseudoKey = CurrentTestSpecification.AvailableResponseKeys(0)
            End If

        Else
            'Using keys in the specified order: Real, Pseudo, as given in the test file
            If ParticipantNumber Mod 2 = 0 Then
                RealKey = CurrentTestSpecification.AvailableResponseKeys(0)
                PseudoKey = CurrentTestSpecification.AvailableResponseKeys(1)
            Else
                RealKey = CurrentTestSpecification.AvailableResponseKeys(1)
                PseudoKey = CurrentTestSpecification.AvailableResponseKeys(0)
            End If
        End If

        'Setting colors and letter of the LeftResponseLetter_Label and RightResponseLetter_Label
        If TestSpecification.LeftSideKeys.Contains(RealKey) Then
            'Real key on left side
            LeftResponseLetter_Label.Text = RealKey.ToString.ToUpper
            LeftResponseLetter_Label.BackColor = RealColor
            RightResponseLetter_Label.Text = PseudoKey.ToString.ToUpper
            RightResponseLetter_Label.BackColor = PseudoColor
        Else
            'Real key on right side
            RightResponseLetter_Label.Text = RealKey.ToString.ToUpper
            RightResponseLetter_Label.BackColor = RealColor
            LeftResponseLetter_Label.Text = PseudoKey.ToString.ToUpper
            LeftResponseLetter_Label.BackColor = PseudoColor
        End If

        'Setting the response time interval
        ResponseTimeTimer.Interval = Math.Max(1, CurrentTestSpecification.MaxResponseTime)

        'Setting the backup timer interval
        BackupTimer.Interval = CurrentTestSpecification.BackupTimerInterval

        'Creating the practise test material
        PractiseTestMaterial = CurrentTestSpecification.CreateTestMaterial(True, MyLibVLC)
        If PractiseTestMaterial Is Nothing Then
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Creating the sharp test material
        SharpTestMaterial = CurrentTestSpecification.CreateTestMaterial(False, MyLibVLC)
        If SharpTestMaterial Is Nothing Then
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Setup practise test block order (n.b. only one block)
        PractiseTestMaterial.SetupBlockOrder(CurrentTestSpecification, ParticipantNumber)

        'Setup sharp test block order
        SharpTestMaterial.SetupBlockOrder(CurrentTestSpecification, ParticipantNumber)

        'Setting block progress bar maximum
        Block_ProgressBar.Maximum = CurrentTestSpecification.NumberOfBlocks
        Block_ProgressBar.Value = 0

        'Run Practise test
        RunPraciseTest()

    End Sub

    Private Sub ShutDownOnStart() Handles ShutDownTimer.Tick
        ShutDownTimer.Stop()
        Me.Close()
    End Sub


    Public Enum GuiModes
        Info
        Test
    End Enum


    Private Sub SetGuiMode_ThreadSafe(ByVal GuiMode As GuiModes)
        If Me.InvokeRequired = True Then
            Me.Invoke(New SetGuiModesDelegate(AddressOf SetGuiMode_Unsafe), {GuiMode})
        Else
            SetGuiMode_Unsafe(GuiMode)
        End If
    End Sub

    Private Sub SetGuiMode_Unsafe(ByVal GuiMode As GuiModes)

        'Storing the new GuiMode
        CurrentGuiMode = GuiMode

        'Shows the reponse labels
        LeftResponseLetter_Label.Visible = True
        RightResponseLetter_Label.Visible = True

        'And the Info_SplitContainer
        Info_SplitContainer.Visible = True

        If ActiveTestMaterial IsNot Nothing Then
            If ActiveTestMaterial.IsPractiseTestMaterial = False Then
                'Shows the Block_ProgressBar
                Block_ProgressBar.Visible = True
            Else
                'Hides the Block_ProgressBar
                Block_ProgressBar.Visible = False
            End If
        End If

        Select Case GuiMode
            Case GuiModes.Info
                'Resets the background color
                ResetBackgroundColor_UnSafe()

                Content_SplitContainer.Panel1Collapsed = True
                Content_SplitContainer.Panel2Collapsed = False

                If ActiveTestMaterial IsNot Nothing Then
                    If ActiveTestMaterial.IsPractiseTestMaterial = True Then
                        Info_SplitContainer.Panel1Collapsed = False
                        Info_SplitContainer.Panel2Collapsed = False
                    Else
                        Info_SplitContainer.Panel1Collapsed = True
                        Info_SplitContainer.Panel2Collapsed = False
                    End If
                End If

                'Shows the cursor
                Cursor.Show()

            Case GuiModes.Test
                Content_SplitContainer.Panel1Collapsed = False
                Content_SplitContainer.Panel2Collapsed = True

        End Select
        Content_SplitContainer.Invalidate()
        Content_SplitContainer.Update()
        Info_SplitContainer.Invalidate()
        Info_SplitContainer.Update()

    End Sub

    Private Sub RunPraciseTest()

        ActiveTestMaterial = PractiseTestMaterial

        SetGuiMode_ThreadSafe(GuiModes.Info)

        WaitingToStartBlock = True

        ActivateKeyDownHandler()

        Instructions_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.StartBySpace)

        Info_RichTextBox.LoadFile(IO.Path.Combine(CurrentTestSpecification.GetBlockParentFolder(), "Info.rtf"))

    End Sub

    Private Sub RunSharpTest()

        ActiveTestMaterial = SharpTestMaterial

        SetGuiMode_ThreadSafe(GuiModes.Info)

        WaitingToStartBlock = True

        ActivateKeyDownHandler()

        Instructions_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.StartBySpace)

    End Sub


    Private Sub TestIsComplete_ThreadSafe()

        If Me.InvokeRequired = True Then
            Me.Invoke(New NoArgumentsDelegate(AddressOf TestIsComplete_Unsafe))
        Else
            TestIsComplete_Unsafe()
        End If

    End Sub


    Private Sub TestIsComplete_Unsafe()

        SetGuiMode_Unsafe(GuiModes.Info)

        If ActiveTestMaterial.IsPractiseTestMaterial = True Then

            'Check if the user seemed to understand the task
            Dim PractiseScore = ActiveTestMaterial.GetProportionCorrect

            If PractiseScore < CurrentTestSpecification.PractiseScoreLimit Then

                Dim PractiseScoreDialog As New PractiseScoreDialog
                PractiseScoreDialog.SetScore(PractiseScore)
                PractiseScoreDialog.ShowDialog(Me)

                If PractiseScoreDialog.DialogResult = DialogResult.OK Then
                    'OK means skip to sharp test
                    RunSharpTest()
                Else
                    'Anything else means redo practise test
                    ActiveTestMaterial.ClearResults()

                    'Restarts the practise test
                    RunPraciseTest()
                End If

            Else
                RunSharpTest()
            End If

        Else

            Cursor.Show()
            Me.Invalidate()
            Me.Update()

            Block_ProgressBar.ShowProgressText = True
            Block_ProgressBar.PerformStep()

            Instructions_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.FinishedTest) & vbCrLf & vbCrLf & Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.CloseApp)

        End If

    End Sub

    Private Sub NewBlock_Safe()

        If Me.InvokeRequired = True Then
            Me.Invoke(New NoArgumentsDelegate(AddressOf NewBlock_Unsafe))
        Else
            NewBlock_Unsafe()
        End If

    End Sub

    Private Sub NewBlock_Unsafe()

        Instructions_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.StartBySpace)

        Block_ProgressBar.PerformStep()

        SetGuiMode_ThreadSafe(GuiModes.Info)

        ActiveTestMaterial.GotoNextBlock()

        ActivateKeyDownHandler()

        WaitingToStartBlock = True

    End Sub

#Region "Active test"

    Private Sub StopSystemTimers()
        'InitiateAppTimer.Stop() 'This need not be called, as it should not have been restarted
        TrialInitiationTimer.Stop()
        ActivateKeyDownHandlerTimer.Stop()
        VideoDurationTimer.Stop()
        ResponseTimeTimer.Stop()
        StartTrialTimer.Stop()
        BackupTimer.Stop()
    End Sub

    Private Sub InitiateNextTrial() Handles TrialInitiationTimer.Elapsed, BackupTimer.Elapsed

        'Stops all timers when initiating a new trial
        StopSystemTimers()

        'And resets the TimedResponseStopWatch
        TimedResponseStopWatch.Reset()

        If ActiveTestMaterial.PresentedItems = 0 Then
            'Sets the GUI mode to Test, only at the first trial
            SetGuiMode_ThreadSafe(GuiModes.Test)
        End If

        'Hides the cursor
        Cursor.Hide()

        'Resetting values of WaitingToStartBlock and VideoEnded 
        WaitingToStartBlock = False
        VideoEnded = False

        'Exporting last trial if ExportAfterEveryTrial is True
        If CurrentTestSpecification.ExportAfterEveryTrial = True Then
            If CurrentItem IsNot Nothing Then
                Dim TrialExportFileName As String = ActiveTestMaterial.CreateExportFileName(ParticipantID, New SortedSet(Of Integer) From {ActiveTestMaterial.GetCurrentBlockNumber})

                'Exporting headings
                If TrialExportIncludeHeadings = True Then
                    Utils.SendInfoToLog(TestItem.GetExportHeadings(), TrialExportFileName, IO.Path.Combine(TestResultExportFolder, ParticipantID & "_TestTrialExport"), True, True)
                    TrialExportIncludeHeadings = False
                End If

                'Exporting data
                Utils.SendInfoToLog(CurrentItem.ToString(False), TrialExportFileName, IO.Path.Combine(TestResultExportFolder, ParticipantID & "_TestTrialExport"), True, True)
            End If
        End If

        'Getting the next event to occur (including the next stimulus)
        Dim NextEvent = ActiveTestMaterial.GetNextEvent(Randomizer)

        'Selecting what to do next
        If NextEvent.NextEventType = TestMaterial.NextEventTypes.EndOfTest Or NextEvent.NextEventType = TestMaterial.NextEventTypes.EndOfBlock Then

            'Saving the block results
            If ActiveTestMaterial.GetCurrentBlockNumber IsNot Nothing Then
                ActiveTestMaterial.ExportResults(TestResultExportFolder, ParticipantID, New SortedSet(Of Integer) From {ActiveTestMaterial.GetCurrentBlockNumber})
            End If

            'Saving all test results (if at the last block), but only if it's not a practise test
            If ActiveTestMaterial.IsPractiseTestMaterial = False Then
                If NextEvent.NextEventType = TestMaterial.NextEventTypes.EndOfTest Then ActiveTestMaterial.ExportResults(TestResultExportFolder, ParticipantID)
            End If

            'Sets CurrentItem to Nothing, and resets TrialExportUncludeHeadings 
            CurrentItem = Nothing
            TrialExportIncludeHeadings = True

            'Hides the video player
            SetVideoViewVisible_ThreadSafe(False)

            If NextEvent.NextEventType = TestMaterial.NextEventTypes.EndOfTest Then
                'Test is completed
                TestIsComplete_ThreadSafe()
            Else
                'Starts a new block
                NewBlock_Safe()
            End If
            'Exits the sub 
            Exit Sub

        End If


        'There are more trials to present in the current block. Launches next trial
        'Setting the value of the local CurrentItem object
        CurrentItem = NextEvent.NextTrialItem

        'Starts the trial using the StartTrialTimer
        StartTrialTimer.Interval = Math.Max(1, Randomizer.Next(CurrentTestSpecification.MinInterTrialInterval, CurrentTestSpecification.MaxInterTrialInterval) - TrialInitiationTimer.Interval)
        StartTrialTimer.Start()
        'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.StartTrialTimer)

        'Also starts the BackupTimer
        BackupTimer.Start()
        'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.BackupTimer)

    End Sub

    Private Sub LaunchTrial(sender As Object, e As EventArgs) Handles StartTrialTimer.Elapsed
        StartTrialTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.StartTrialTimer)

        'Resets the background color
        ResetBackgroundColor_TreadSafe()

        'Stores the time when the play-command was issued (note that this is not the time when the actual video was presented, as it takes (a different!) amount of time for different videos to actually show up on the screen. 
        ' The timepoint at which the video actually starts showing is best captured, and therefore stored, in the the VideoPlayer.Vout event handler below.
        Try
            CurrentItem.PlayCommandTime = DateTime.Now
            VideoPlayer.Play(CurrentItem.CurrentVideo)
        Catch ex As Exception
            MsgBox("An error has occured! Unable to play the current video. Please click ok to continue!", "Unable to play video!")
            StopSystemTimers()
            'Stops all timers and restarts the trial"
            TrialInitiationTimer.Start()
        End Try

    End Sub


    Private Sub VideoPlayer_VideoOutChanged(sender As Object, e As LibVLCSharp.Shared.MediaPlayerVoutEventArgs) Handles VideoPlayer.Vout
        'The Vout event is used for time taking, since it is called after the video player has been created. The VideoPlayer.Playing event is not used, since it is called before the played has been created. 
        ' The VideoPlayer.Playing event therefore (sometimes, when a new video format is needed) has a time lag of around 100 ms. The Vout event is called after such time lag, and therefore is better timed to the actual video start. 
        ' The downside is that the interstimulus duration is somewhat unreliable (it may be up to 100 ms prolonged compared to what is specified in the test specification file).

        If e.Count <> 0 Then

            CurrentItem.PresentationTime = DateTime.Now

            'Starting the timed response stop watch
            TimedResponseStopWatch.Start()

            'Doing the rest on a thread safe call 
            VideoShowed()

        End If

    End Sub


    Private Sub VideoShowed()

        If Me.InvokeRequired Then
            Dim d As New NoArgumentsDelegate(AddressOf VideoShowed)
            Me.Invoke(d)
        Else

            'Starting a local timer with the same Interval as the video duration
            VideoDurationTimer.Interval = Math.Max(1, VideoPlayer.Media.Duration)
            VideoDurationTimer.Start()
            'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.VideoDurationTimer)


            ' Shows the video if it has been hidden between trials, or if it is the first item presented in the current block
            If CurrentTestSpecification.HideVideoBetweenTrials = True Or CurrentItem.WithinBlock_TrialPresentationOrder = 1 Then SetVideoViewVisible_ThreadSafe(True)

            'Notes the time when the video has appeared (this should only be needed if HideVideoBetweenTrials is True, and then gives a measure of the time it takes to refresh the screen. The difference between PresentationTime and VideoVisibleTime should however be very small, possibly up to a few milliseconds.)
            CurrentItem.VideoVisibleTime = DateTime.Now

            'Stores the duration of the video and its frame rate
            CurrentItem.VideoDuration = VideoPlayer.Media.Duration
            CurrentItem.FrameRate = VideoPlayer.Fps

            'Here we can also get the size of the player
            'Dim px As UInteger
            'Dim py As UInteger
            'VideoPlayer.Size(0, px, py)

            'Starts the response time timer, if PostPresentationResponsePeriod = False (i.e. the timer should be started at the START of the completed stimulus presentation) 
            If CurrentTestSpecification.PostPresentationResponsePeriod = False Then ResponseTimeTimer.Start()
            'If TestSpecification.PostPresentationResponsePeriod = False Then IssueTimerCommandSafe(TimerCommands.Start, FormTimers.ResponseTimeTimer)

            'Starts the timer that activates the response key handlers
            ActivateKeyDownHandlerTimer.Start()
            'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.ActivateKeyDownHandlerTimer)

        End If

    End Sub

    Public Sub ActivateKeyDownHandlerTimer_Tick() Handles ActivateKeyDownHandlerTimer.Elapsed
        ActivateKeyDownHandlerTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.ActivateKeyDownHandlerTimer)

        'Activates the keypress event handlers for the participant response
        ActivateKeyDownHandler()

    End Sub

    ''' <summary>
    ''' This sub is called by the Tick event of the VideoDurationTimer at the end of the video. (The reason for using a local timer for this is that the EndReached (or Stopped) event of the VideoPlayer is all too slow (triggered only every 100 ms or so). This alternative gives a much better precision.
    ''' </summary>
    Public Sub VideoDurationTimer_Tick() Handles VideoDurationTimer.Elapsed
        VideoDurationTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.VideoDurationTimer)

        'Exits the sub if the video has already been stopped / hidden (which should only have happened if TestSpecification.HideVideoBetweenTrials = True And TestSpecification.HideVideoAtResponse = True, and a response has already been given.
        If VideoEnded = True Then Exit Sub

        'Notes that the video has ended
        VideoEnded = True

        'Hides the videp player if HideVideoBetweenTrials is True 
        If CurrentTestSpecification.HideVideoBetweenTrials = True Then SetVideoViewVisible_ThreadSafe(False)

        'Sets the end of presentation time
        CurrentItem.VideoEndTime = DateTime.Now

        'Checks if a response has already been given
        If CurrentItem.ResponseGiven = True Then

            'If so, starting a new trial if a response is given before the video ended
            TrialInitiationTimer.Start()
            'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.TrialInitiationTimer)
        Else

            'If not, starting the response time timer, if PostPresentationResponsePeriod = True (i.e. the timer should be started AFTER the completed stimulus presentation). Otherwise it will already have been started.
            If CurrentTestSpecification.PostPresentationResponsePeriod = True Then ResponseTimeTimer.Start()
            'If TestSpecification.PostPresentationResponsePeriod = True Then IssueTimerCommandSafe(TimerCommands.Start, FormTimers.ResponseTimeTimer)
        End If

    End Sub

    Private Sub ActivateKeyDownHandler()
        AddHandler MyBase.KeyDown, AddressOf LdtForm_KeyDown
    End Sub

    Private Sub InactivateKeyDownHandler()
        RemoveHandler MyBase.KeyDown, AddressOf LdtForm_KeyDown
    End Sub

    ''' <summary>
    ''' Handles the key down event of the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LdtForm_KeyDown(sender As Object, e As KeyEventArgs)

        If WaitingToStartBlock = True Then

            If e.KeyCode = Keys.Space Then

                InactivateKeyDownHandler()

                'Starts a new trial
                TrialInitiationTimer.Start()
                'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.TrialInitiationTimer)
            End If

        Else

            Select Case e.KeyCode
                Case RealKey

                    'Noting a correct response
                    IssueResponse_ThreadSafe(Response.Real, e.KeyCode, DateTime.Now)

                Case PseudoKey

                    'Noting an incorrect response
                    IssueResponse_ThreadSafe(Response.Pseudo, e.KeyCode, DateTime.Now)

            End Select

        End If

        'Ignores any other key downs and sets Handled to True
        e.Handled = True

    End Sub


    ''' <summary>
    ''' Handles the Tick event of the ResponseTimeTimer, which creates a 'missing' response when triggered.
    ''' </summary>
    Public Sub ResponseTimeTimerTick() Handles ResponseTimeTimer.Elapsed
        ResponseTimeTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.ResponseTimeTimer)

        'Noting a missing response
        IssueResponse_ThreadSafe(Response.Missing, Keys.None, DateTime.Now)

    End Sub

    ''' <summary>
    ''' A threadlocking spinlock that should prevent multiple threads to call IssueResponse at the same time (which could mess up the procedure).
    ''' </summary>
    Public IncomingResponseSpinLock As New Threading.SpinLock

    Private Sub IssueResponse_ThreadSafe(ByVal Result As Response, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)

        If Me.InvokeRequired = True Then
            Me.Invoke(New IssueResponseDelegate(AddressOf IssueResponse_Unsafe), {Result, ButtonPressed, ResponseTime})
        Else
            IssueResponse_Unsafe(Result, ButtonPressed, ResponseTime)
        End If

    End Sub


    Private Sub IssueResponse_Unsafe(ByVal Result As Response, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)
        ResponseTimeTimer.Stop()
        'IssueTimerCommandSafe(TimerCommands.Stop, FormTimers.ResponseTimeTimer)

        Dim SpinLockTaken As Boolean = False

        'Attempts to enter a spin lock to avoid multiple thread conflicts if called multiple times
        IncomingResponseSpinLock.Enter(SpinLockTaken)

        'Skips to the end of the sub if a respone is already given (by a previous call/thread)
        If CurrentItem.ResponseGiven = False Then

            'Stopps the response timer
            TimedResponseStopWatch.Stop()

            'And in activates the response keys
            InactivateKeyDownHandler()

            'Stores the trial response / result
            CurrentItem.NewResponse(Result, ButtonPressed, ResponseTime, TimedResponseStopWatch.Elapsed)

            'Hides the video if HideVideoAtResponse is True (and also HideVideoBetweenTrials is True).
            If CurrentTestSpecification.HideVideoBetweenTrials = True And CurrentTestSpecification.HideVideoAtResponse = True Then

                'Notes that the video has been stopped/hidden
                VideoEnded = True

                'Hides the video
                SetVideoViewVisible_ThreadSafe(False)

                'Sets the end of presentation time
                CurrentItem.VideoEndTime = DateTime.Now
            End If

            'Changes colors on the form to note missing or valid responses
            If Result = Response.Missing Then
                'Flashing the background to note a missing response
                FlashBackground()
            Else

                If Result = Response.Real Then
                    'Changes the color to DarkGreen if the answer was 'Real'
                    Background_TableLayoutPanel.BackColor = RealColor
                    Background_TableLayoutPanel.Invalidate()
                    Background_TableLayoutPanel.Update()
                Else
                    'Changes the color to Red if the answer was 'Pseudo'
                    Background_TableLayoutPanel.BackColor = PseudoColor
                    Background_TableLayoutPanel.Invalidate()
                    Background_TableLayoutPanel.Update()
                End If

            End If

            'Initiates the next trial from here if the end of the video has been reached or the video stopped
            If VideoEnded = True Then
                TrialInitiationTimer.Start()
                'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.TrialInitiationTimer)
            Else
                'A response has been given but the video has not ended, nor been hidden. 
                ' Waits for the TrialInitiationTimer to be triggered from within the VideoDurationTimer.Tick event instead of initiating the next trial from here.
            End If

        End If

        'Releases any spinlock
        If SpinLockTaken = True Then IncomingResponseSpinLock.Exit()

    End Sub

    Private Sub FlashBackground()

        For n = 0 To 1
            Background_TableLayoutPanel.BackColor = Color.Orange
            Background_TableLayoutPanel.Invalidate()
            Background_TableLayoutPanel.Update()
            Threading.Thread.Sleep(60)
            'We can call ResetBackgroundColor_UnSafe here, since FlashBackground should have allways been called from the UI thread.
            ResetBackgroundColor_UnSafe()
            Threading.Thread.Sleep(60)
        Next

        Background_TableLayoutPanel.Invalidate()
        Background_TableLayoutPanel.Update()

    End Sub

    Public Sub ResetBackgroundColor_TreadSafe()

        If Me.InvokeRequired = True Then
            Me.Invoke(New NoArgumentsDelegate(AddressOf ResetBackgroundColor_UnSafe))
        Else
            ResetBackgroundColor_UnSafe()
        End If

    End Sub

    Public Sub ResetBackgroundColor_UnSafe()
        Background_TableLayoutPanel.BackColor = OriginalBackColor
        Background_TableLayoutPanel.Invalidate()
        Background_TableLayoutPanel.Update()
    End Sub


    Private Sub LdtForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        'Stops all timers before closing the app
        StopSystemTimers()
        InitiateAppTimer.Stop()
        ShutDownTimer.Stop()

        'Disposes all media, the media player and the LibVLC instances
        If PractiseTestMaterial IsNot Nothing Then PractiseTestMaterial.DisposeVideos()
        If SharpTestMaterial IsNot Nothing Then SharpTestMaterial.DisposeVideos()
        VideoPlayer.Dispose()
        MyLibVLC.Dispose()

    End Sub


    ''' <summary>
    ''' Sets the visible property of the VideoView in a thread safe way.
    ''' </summary>
    Private Sub SetVideoViewVisible_ThreadSafe(ByVal Visible As Boolean)
        If Me.InvokeRequired = True Then
            Me.Invoke(New VideoViewVisibleDelegate(AddressOf SetVideoViewVisible_ThreadSafe), {Visible})
        Else
            Me.VideoView.Visible = Visible
        End If
    End Sub


#End Region


End Class
