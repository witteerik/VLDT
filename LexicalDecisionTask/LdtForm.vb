Imports LibVLCSharp.Shared

Public Class LdtForm

    Public MyLibVLC As LibVLC

    Public WithEvents VideoPlayer As MediaPlayer

    Private TestSpecification As TestSpecification

    Private RealKey As Keys

    Private PseudoKey As Keys

    Private RealColor As Color = Color.DarkGreen

    Private PseudoColor As Color = Color.Red

    Private Randomizer As Random

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

    Delegate Sub IssueResponseDelegate(ByVal Result As Results, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)

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

            If LoadTestSpecificationFile(SelectedFilePath) = False Then
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
        fbd.Description = "Please select a folder in which to store test results, and then click OK!"
        fbd.ShowNewFolderButton = True
        fbd.SelectedPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        Dim fbd_DialogResult = fbd.ShowDialog(Me)
        If fbd_DialogResult = DialogResult.OK Then

            Dim OutputFolder As String = fbd.SelectedPath

            'Trying to save a log message to the output folder (just to check that it works)
            Try
                Utils.SendInfoToLog("Initiated lexical descition test.",, OutputFolder)

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
        If TestSpecification.RandomSeed.HasValue = True Then
            Randomizer = New Random(TestSpecification.RandomSeed)
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
        If TestSpecification.RandomizeResponseKeys = True Then

            'Randomizing keys
            Dim RandomIndex As Integer = Randomizer.Next(0, 2)
            RealKey = TestSpecification.AvailableResponseKeys(RandomIndex)
            'Selecting the other available key for incorrect response
            If RandomIndex = 0 Then
                PseudoKey = TestSpecification.AvailableResponseKeys(1)
            Else
                PseudoKey = TestSpecification.AvailableResponseKeys(0)
            End If

        Else
            'Using keys in the specified order: Real, Pseudo, as given in the test file
            If ParticipantNumber Mod 2 = 0 Then
                RealKey = TestSpecification.AvailableResponseKeys(0)
                PseudoKey = TestSpecification.AvailableResponseKeys(1)
            Else
                RealKey = TestSpecification.AvailableResponseKeys(1)
                PseudoKey = TestSpecification.AvailableResponseKeys(0)
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
        ResponseTimeTimer.Interval = Math.Max(1, TestSpecification.MaxResponseTime)

        'Setting the backup timer interval
        BackupTimer.Interval = TestSpecification.BackupTimerInterval

        'Creating the practise test material
        PractiseTestMaterial = TestSpecification.CreateTestMaterial(True, MyLibVLC)
        If PractiseTestMaterial Is Nothing Then
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Creating the sharp test material
        SharpTestMaterial = TestSpecification.CreateTestMaterial(False, MyLibVLC)
        If SharpTestMaterial Is Nothing Then
            ShutDownTimer.Start()
            Exit Sub
        End If

        'Setup practise test block order (n.b. only one block)
        PractiseTestMaterial.SetupBlockOrder(TestSpecification, ParticipantNumber)

        'Setup sharp test block order
        SharpTestMaterial.SetupBlockOrder(TestSpecification, ParticipantNumber)

        'Setting block progress bar maximum
        Block_ProgressBar.Maximum = TestSpecification.NumberOfBlocks
        Block_ProgressBar.Value = 0

        'Run Practise test
        RunPraciseTest()

    End Sub

    Private Sub ShutDownOnStart() Handles ShutDownTimer.Tick
        ShutDownTimer.Stop()
        Me.Close()
    End Sub


    Private Function LoadTestSpecificationFile(ByVal TestSpecificationFilePath As String) As Boolean

        TestSpecification = New TestSpecification

        'Storing the file path
        TestSpecification.TestSpecificationFilePath = TestSpecificationFilePath

        Dim InputLines() As String = System.IO.File.ReadAllLines(TestSpecificationFilePath, System.Text.Encoding.UTF8)

        Dim ReadBlockOrders As Boolean = False
        Dim ManualBlockOrders As New List(Of List(Of Integer))

        For Each Line In InputLines

            If Line.Trim = "" Then Continue For
            If Line.Trim.StartsWith("//") Then Continue For

            If ReadBlockOrders = False Then

                If Line.ToLowerInvariant.Trim.StartsWith("<BlockOrders>".ToLowerInvariant) Then
                    'Starts reading block orders
                    ReadBlockOrders = True
                    Continue For
                End If

                Dim LineValueSplit = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Split("=")

                Dim LineVariable As String = LineValueSplit(0).Trim
                'Skipping if no variable value is available
                If LineValueSplit.Length = 1 Then Continue For
                Dim VariableValueString As String = LineValueSplit(1).Trim

                'Skipping if the variable value is empty
                If VariableValueString = "" Then Continue For

                Select Case LineVariable.ToLowerInvariant
                    Case "RandomSeed".ToLowerInvariant
                        Dim TempValue As Integer
                        If Integer.TryParse(VariableValueString, TempValue) = True Then
                            TestSpecification.RandomSeed = TempValue
                        Else
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "ResponseKeys".ToLowerInvariant
                        TestSpecification.AvailableResponseKeys.Clear()
                        Dim VariableValueStringSplit = VariableValueString.Split(",")
                        Dim FailedReading As Boolean = False
                        If VariableValueStringSplit.Length = 2 Then
                            For i = 0 To 1
                                Try

                                    Dim NewKey As Keys = DirectCast([Enum].Parse(GetType(Keys), VariableValueStringSplit(i).Trim), Integer)
                                    If TestSpecification.LeftSideKeys.Contains(NewKey) Or TestSpecification.RightSideKeys.Contains(NewKey) Then
                                        TestSpecification.AvailableResponseKeys.Add(NewKey)
                                    Else
                                        MsgBox("The response key " & NewKey.ToString & " specified in the lexical decision task file (" & TestSpecificationFilePath & ") is not allowed." & vbCrLf & vbCrLf & "The following response keys are allowed: " & vbCrLf &
                                               "Left side keys: " & String.Join(", ", TestSpecification.LeftSideKeys) & vbCrLf &
                                               "Right side keys: " & String.Join(", ", TestSpecification.RightSideKeys) & vbCrLf & vbCrLf &
                                               "Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid response key value!")
                                        Return False
                                    End If

                                Catch ex As Exception
                                    FailedReading = True
                                End Try
                            Next
                        Else
                            FailedReading = True
                        End If

                        If FailedReading = True Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as two keyboard keys. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "RandomizeResponseKeys".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.RandomizeResponseKeys) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case "BlockCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.NumberOfBlocks) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "TestItemCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.NumberOfTestItems) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "PractiseItemCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.NumberOfPractiseItems) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "MinInterTrialInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.MinInterTrialInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "MaxInterTrialInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.MaxInterTrialInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "MaxResponseTime".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.MaxResponseTime) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "HideVideoBetweenTrials".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.HideVideoBetweenTrials) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case "HideVideoAtResponse".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.HideVideoAtResponse) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case "PostPresentationResponsePeriod".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.PostPresentationResponsePeriod) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case "ExportAfterEveryTrial".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.ExportAfterEveryTrial) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case "BackupTimerInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, TestSpecification.BackupTimerInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return False
                        End If

                    Case "RandomizeBlockOrder".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, TestSpecification.RandomizeBlockOrder) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                        End If

                    Case Else
                        MsgBox("Unknown variable (" & LineVariable & ") detected in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Unkown variable!")
                        Return False
                End Select

            Else

                If Line.ToLowerInvariant.Trim.StartsWith("</BlockOrders>".ToLowerInvariant) Then
                    'Stopps reading block orders
                    ReadBlockOrders = False
                    Continue For
                End If

                Dim LineValueSplit = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Split(",")
                Dim CurrentBlockOrder As New List(Of Integer)
                For Each StringValue In LineValueSplit
                    Dim TempValue As Integer
                    If Integer.TryParse(StringValue.Trim, TempValue) = True Then
                        CurrentBlockOrder.Add(TempValue)
                    Else
                        MsgBox("Unable to read the block order value " & StringValue.Trim & " given at the line " & Line & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                        Return False
                    End If
                Next

                If CurrentBlockOrder.Count > 0 Then
                    ManualBlockOrders.add(CurrentBlockOrder)
                End If

            End If

        Next

        ' References the manual block orders
        TestSpecification.ManualBlockOrders = ManualBlockOrders

        'Checking if block orders are correctly setup
        For Each BlockOrder In TestSpecification.ManualBlockOrders

            '- The number of blocks in each block order must equal NumberOfBlocks
            If BlockOrder.Count <> TestSpecification.NumberOfBlocks Then
                MsgBox("The block order (" & String.Join(", ", BlockOrder) & ") specified in the lexical decision task file (" & TestSpecificationFilePath & ") must contain exactlyk " & TestSpecification.NumberOfBlocks & " (BlockCount) integers. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                Return False
            End If

            '- The BlockOrders must contain all integers from 1 to BlockCount
            For Block As Integer = 1 To TestSpecification.NumberOfBlocks
                If Not BlockOrder.Contains(Block) Then
                    MsgBox("The block order (" & String.Join(", ", BlockOrder) & ") specified in the lexical decision task file (" & TestSpecificationFilePath & ") lacks block number " & Block & ". Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                    Return False
                End If
            Next
        Next

        'Checks that a manual blockorder exist if RandomizeBlockOrder = False
        If TestSpecification.RandomizeBlockOrder = False Then
            If TestSpecification.ManualBlockOrders.Count = 0 Then
                MsgBox("When RandomizeBlockOrder = False, at least one block order needs to be manually specified using the block order tag (<BlockOrder>) in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                Return False
            End If
        End If


        'Check that MinInterTrialInterval is not higher than MaxInterTrialInterval and vice versa!
        If TestSpecification.MinInterTrialInterval > TestSpecification.MaxInterTrialInterval Then
            MsgBox("The value given for MinInterTrialInterval (" & TestSpecification.MinInterTrialInterval & ") must be lower than the value given for MaxInterTrialInterval (" & TestSpecification.MaxInterTrialInterval & ") in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
            Return False
        End If

        If TestSpecification.MinInterTrialInterval = 0 Or TestSpecification.MaxInterTrialInterval = 0 Or TestSpecification.MaxResponseTime = 0 Then
            MsgBox("The value given for the variables MinInterTrialInterval (" & TestSpecification.MinInterTrialInterval & "), MaxInterTrialInterval (" & TestSpecification.MaxInterTrialInterval & ") and MaxResponseTime (" & TestSpecification.MaxResponseTime & "), in the lexical decision task file (" & TestSpecificationFilePath & ") must all be higher than zero. Where values are zero they will be changed to 1 millisecond.", MsgBoxStyle.Information, "Information!")
        End If

        Return True

    End Function

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

        'Shows the reponse labels
        LeftResponseLetter_Label.Visible = True
        RightResponseLetter_Label.Visible = True

        'And the Info_SplitContainer
        Info_SplitContainer.Visible = True

        If ActiveTestMaterial IsNot Nothing Then
            If ActiveTestMaterial.IsPractiseTestMaterial = False Then
                'Shows the Block_ProgressBar
                Block_ProgressBar.Visible = True
            End If
        End If

        'Changing the layout only if the CurrentGuiMode is changed
        If CurrentGuiMode <> GuiMode Then
            Select Case GuiMode
                Case GuiModes.Info
                    'Resets the background color
                    ResetBackgroundColor_UnSafe()
                    Content_SplitContainer.Panel1Collapsed = True
                    Content_SplitContainer.Panel2Collapsed = False

                    'Shows the cursor
                    Cursor.Show()

                Case GuiModes.Test
                    Content_SplitContainer.Panel1Collapsed = False
                    Content_SplitContainer.Panel2Collapsed = True
            End Select
            Content_SplitContainer.Invalidate()
            Content_SplitContainer.Update()
        End If

        CurrentGuiMode = GuiMode

    End Sub

    Private Sub RunPraciseTest()

        ActiveTestMaterial = PractiseTestMaterial

        SetGuiMode_ThreadSafe(GuiModes.Info)

        WaitingToStartBlock = True

        ActivateKeyDownHandler()

        InstructionsTextBox.Text = "Press Space to start!"

        Info_RichTextBox.LoadFile("C:\VLDT\PilotExperiment1\Info.rtf")

    End Sub

    Private Sub RunSharpTest()

        ActiveTestMaterial = SharpTestMaterial

        SetGuiMode_ThreadSafe(GuiModes.Info)

        WaitingToStartBlock = True

        ActivateKeyDownHandler()

        InstructionsTextBox.Text = "Press Space to start real test!"

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

            RunSharpTest()

        Else

            Cursor.Show()
            Me.Invalidate()
            Me.Update()

            Block_ProgressBar.ShowProgressText = True
            Block_ProgressBar.PerformStep()

            InstructionsTextBox.Text = "You have now completed the lexical desicion task! Well done!" & vbCrLf & vbCrLf & "You may now close the app!"

        End If

    End Sub

    Private Sub NewBlock()

        InstructionsTextBox.Text = "You have now completed " & ActiveTestMaterial.CompletedBlocks & " of " & TestSpecification.NumberOfBlocks & " blocks." & vbCrLf & vbCrLf & "When you're ready, press space to start the next block"

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

        'Sets the GUI mode to Test
        SetGuiMode_ThreadSafe(GuiModes.Test)

        'Hides the cursor
        Cursor.Hide()

        'Resetting values of WaitingToStartBlock and VideoEnded 
        WaitingToStartBlock = False
        VideoEnded = False

        'Exporting last trial if ExportAfterEveryTrial is True
        If TestSpecification.ExportAfterEveryTrial = True Then
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

            'Saving all test results (if at the last block)
            If NextEvent.NextEventType = TestMaterial.NextEventTypes.EndOfTest Then ActiveTestMaterial.ExportResults(TestResultExportFolder, ParticipantID)

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
                NewBlock()
            End If
            'Exits the sub 
            Exit Sub

        End If


        'There are more trials to present in the current block. Launches next trial
        'Setting the value of the local CurrentItem object
        CurrentItem = NextEvent.NextTrialItem

        'Starts the trial using the StartTrialTimer
        StartTrialTimer.Interval = Math.Max(1, Randomizer.Next(TestSpecification.MinInterTrialInterval, TestSpecification.MaxInterTrialInterval) - TrialInitiationTimer.Interval)
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
            If TestSpecification.HideVideoBetweenTrials = True Or CurrentItem.WithinBlock_TrialPresentationOrder = 1 Then SetVideoViewVisible_ThreadSafe(True)

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
            If TestSpecification.PostPresentationResponsePeriod = False Then ResponseTimeTimer.Start()
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
        If TestSpecification.HideVideoBetweenTrials = True Then SetVideoViewVisible_ThreadSafe(False)

        'Sets the end of presentation time
        CurrentItem.VideoEndTime = DateTime.Now

        'Checks if a response has already been given
        If CurrentItem.ResponseGiven = True Then

            'If so, starting a new trial if a response is given before the video ended
            TrialInitiationTimer.Start()
            'IssueTimerCommandSafe(TimerCommands.Start, FormTimers.TrialInitiationTimer)
        Else

            'If not, starting the response time timer, if PostPresentationResponsePeriod = True (i.e. the timer should be started AFTER the completed stimulus presentation). Otherwise it will already have been started.
            If TestSpecification.PostPresentationResponsePeriod = True Then ResponseTimeTimer.Start()
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
                    IssueResponse_ThreadSafe(Results.Real, e.KeyCode, DateTime.Now)

                Case PseudoKey

                    'Noting an incorrect response
                    IssueResponse_ThreadSafe(Results.Pseudo, e.KeyCode, DateTime.Now)

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
        IssueResponse_ThreadSafe(Results.Missing, Keys.None, DateTime.Now)

    End Sub

    ''' <summary>
    ''' A threadlocking spinlock that should prevent multiple threads to call IssueResponse at the same time (which could mess up the procedure).
    ''' </summary>
    Public IncomingResponseSpinLock As New Threading.SpinLock

    Private Sub IssueResponse_ThreadSafe(ByVal Result As Results, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)

        If Me.InvokeRequired = True Then
            Me.Invoke(New IssueResponseDelegate(AddressOf IssueResponse_Unsafe), {Result, ButtonPressed, ResponseTime})
        Else
            IssueResponse_Unsafe(Result, ButtonPressed, ResponseTime)
        End If

    End Sub


    Private Sub IssueResponse_Unsafe(ByVal Result As Results, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime)
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
            If TestSpecification.HideVideoBetweenTrials = True And TestSpecification.HideVideoAtResponse = True Then

                'Notes that the video has been stopped/hidden
                VideoEnded = True

                'Hides the video
                SetVideoViewVisible_ThreadSafe(False)

                'Sets the end of presentation time
                CurrentItem.VideoEndTime = DateTime.Now
            End If

            'Changes colors on the form to note missing or valid responses
            If Result = Results.Missing Then
                'Flashing the background to note a missing response
                FlashBackground()
            Else

                If Result = Results.Real Then
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

Public Class TestSpecification

    Public Property RandomSeed As Integer?

    Public Property TestSpecificationFilePath As String = ""

    Public Property AvailableResponseKeys As New List(Of Keys) From {Keys.J, Keys.F}

    Public Property RandomizeResponseKeys As Boolean = True

    Public Property NumberOfBlocks As Integer

    Public Property NumberOfTestItems As Integer

    Public Property NumberOfPractiseItems As Integer

    'Public Property 

    Public Property MinInterTrialInterval As Integer
    Public Property MaxInterTrialInterval As Integer

    Public Property MaxResponseTime As Integer

    Public Property HideVideoBetweenTrials As Boolean = True

    Public Property HideVideoAtResponse As Boolean = True

    Public Property PostPresentationResponsePeriod As Boolean = True ' If set to True, the maximum response timer starts after the completed stimulus presentation. If set to False, the maximum response timer starts at the beginning of the stimulus presentation.
    Public Property ExportAfterEveryTrial As Boolean = True

    Public Property BackupTimerInterval As Integer = 20000

    Public Property RandomizeBlockOrder As Boolean

    Public Property ManualBlockOrders As New List(Of List(Of Integer))

    Public Shared LeftSideKeys As New List(Of Keys) From {Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.Z, Keys.X, Keys.C, Keys.V}

    Public Shared RightSideKeys As New List(Of Keys) From {Keys.U, Keys.I, Keys.O, Keys.P, Keys.H, Keys.J, Keys.K, Keys.L, Keys.N, Keys.M}


    Private Function GetBlockParentFolder() As String
        Return IO.Path.GetDirectoryName(TestSpecificationFilePath)
    End Function

    Private Function GetBlockFolder(ByVal IsPractiseTestMaterial As Boolean, Optional ByVal BlockNumber As Integer = 0) As String
        If IsPractiseTestMaterial = False Then
            Return IO.Path.Combine(IO.Path.GetDirectoryName(TestSpecificationFilePath), "Block" & BlockNumber.ToString("00"))
        Else
            Return IO.Path.Combine(IO.Path.GetDirectoryName(TestSpecificationFilePath), "PractiseBlock")
        End If
    End Function

    Public Function CheckThatBlockFoldersExist(ByVal IsPractiseTestMaterial As Boolean) As Boolean

        If IsPractiseTestMaterial = False Then
            For BlockNumber = 1 To NumberOfBlocks
                If IO.Directory.Exists(GetBlockFolder(IsPractiseTestMaterial, BlockNumber)) = False Then
                    MsgBox("Missing folder for block number " & BlockNumber.ToString("00") & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Missing block folder!")
                    Return False
                End If
            Next
        Else
            If IO.Directory.Exists(GetBlockFolder(IsPractiseTestMaterial)) = False Then
                MsgBox("Missing folder for practise block (It should be named 'PractiseBlock') " & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Missing practise block folder!")
                Return False
            End If
        End If

        Return True
    End Function

    Public Sub CheckCorrectNumberOfBlocks(ByVal IsPractiseTestMaterial As Boolean)

        Dim NumberOfCandidateBlocks = GetNumberOfBlockFolders(IsPractiseTestMaterial)
        If IsPractiseTestMaterial = False Then

            If NumberOfBlocks <> NumberOfCandidateBlocks Then
                MsgBox("Warning! There are more (" & NumberOfCandidateBlocks & " ) block folders in the folder " & GetBlockParentFolder() & " than what is specified in the lexical deciscion test file (" & NumberOfBlocks & ")!" & vbCrLf & vbCrLf &
                   "Is this really correct?", MsgBoxStyle.Exclamation, "Detected more block folders than expected!")
            End If

        Else

            If NumberOfCandidateBlocks <> 1 Then
                MsgBox("Warning! There are more than one (" & NumberOfCandidateBlocks & " ) practise block present in the folder " & GetBlockParentFolder() & "!" & vbCrLf & vbCrLf &
                   "Is this really correct?", MsgBoxStyle.Exclamation, "Detected incorrect number of practise blocks!")
            End If

        End If

    End Sub

    ''' <summary>
    ''' Returns the number of folders in the block parent folder that begin with 'Block'
    ''' </summary>
    ''' <returns></returns>
    Private Function GetNumberOfBlockFolders(ByVal IsPractiseTestMaterial As Boolean) As Integer
        Dim Folders = IO.Directory.EnumerateDirectories(IO.Path.GetDirectoryName(TestSpecificationFilePath))
        Dim BlockFolderCount As Integer = 0
        For Each folder In Folders

            Dim PathSplit = folder.Split(IO.Path.DirectorySeparatorChar)
            Dim LastSubFolder = PathSplit(PathSplit.Length - 1)

            If IsPractiseTestMaterial = False Then
                If LastSubFolder.StartsWith("Block") Then BlockFolderCount += 1
            Else
                If LastSubFolder.StartsWith("PractiseBlock") Then BlockFolderCount += 1
            End If
        Next
        Return BlockFolderCount
    End Function


    Public Function CreateTestMaterial(ByVal IsPractiseTestMaterial As Boolean, ByRef LibVLC As LibVLC) As TestMaterial

        'Checking that the block folders exist in the specified place
        If CheckThatBlockFoldersExist(IsPractiseTestMaterial) = False Then Return Nothing

        'Checking that the block folder only contains the right number of subfolder that begin with Block
        CheckCorrectNumberOfBlocks(IsPractiseTestMaterial)

        'Creating a new test material
        Dim NewTestMaterial As New TestMaterial With {.IsPractiseTestMaterial = IsPractiseTestMaterial}

        Dim ItemNumber As Integer = 1

        'Creating blocks and their test items (and checking that the riht number of files and folders exist along the way)
        For BlockNumber = 1 To NumberOfBlocks

            Dim NewBlock As New Block With {.BlockNumber = BlockNumber, .ParentTestMaterial = NewTestMaterial}

            Dim TypeList As New List(Of ItemTypes) From {ItemTypes.Real, ItemTypes.Pseudo}

            For Each ItemType In TypeList

                Dim NewItemList As New List(Of TestItem)

                Dim ItemFolder As String = IO.Path.Combine(GetBlockFolder(IsPractiseTestMaterial, BlockNumber), ItemType.ToString)

                'Checking that each block folder contains the folder Real And Pseudo
                If IO.Directory.Exists(ItemFolder) = False Then
                    MsgBox("Missing '" & ItemType.ToString & "' folder for block number " & BlockNumber.ToString("00") & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Missing '" & ItemType.ToString & "' block folder!")
                    Return Nothing
                End If

                'Checking that exactly NumberOfTestItems exist in each Real and PseudoFolder
                Dim AllFiles = IO.Directory.GetFiles(ItemFolder)

                'Getting the filepaths to use
                Dim IncludedFiles As New List(Of String)
                For Each File In AllFiles
                    'Here we could add a check that the file is a valid video/media file:
                    'If SupportedVideoFormatsList.Contains(IO.Path.GetExtension(File)) Then IncludedFiles.Add(File)
                    IncludedFiles.Add(File)
                Next

                'Checking that the correct number of video files exist
                If IsPractiseTestMaterial = True Then
                    If IncludedFiles.Count <> NumberOfPractiseItems Then
                        MsgBox("The '" & ItemType.ToString & "' folder for the practise block does not contain exactly " & NumberOfPractiseItems & " video files, as indicated in the lexical decision test file!" & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Incorrect number of video files!")
                        Return Nothing
                    End If
                Else
                    If IncludedFiles.Count <> NumberOfTestItems Then
                        MsgBox("The '" & ItemType.ToString & "' folder for block number " & BlockNumber.ToString("00") & " does not contain exactly " & NumberOfTestItems & " video files, as indicated in the lexical decision test file!" & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Incorrect number of video files!")
                        Return Nothing
                    End If
                End If

                'Creating new items with their file path assigned
                For Each FilePath In IncludedFiles
                    Try

                        Dim NewTestItem = New TestItem With {.ParentBlock = NewBlock, .ItemType = ItemType, .FilePath = FilePath, .FileName = IO.Path.GetFileNameWithoutExtension(FilePath), .ItemNumber = ItemNumber}
                        NewTestItem.SetMedia(LibVLC)
                        NewItemList.Add(NewTestItem)

                        ItemNumber += 1

                    Catch ex As Exception
                        MsgBox("Unable to load the following file as a media file (you should remove it from the folder): " & FilePath & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Invalid media file detected!")
                        Return Nothing
                    End Try
                Next

                'Adding the items
                Select Case ItemType
                    Case ItemTypes.Real
                        NewBlock.RealItems = NewItemList
                    Case ItemTypes.Pseudo
                        NewBlock.PseudoItems = NewItemList
                End Select

            Next

            'Adding the block
            NewTestMaterial.Add(NewBlock)

            'Exits after first looop if practise material
            If IsPractiseTestMaterial = True Then Exit For

        Next

        Return NewTestMaterial

    End Function

End Class

Public Class TestMaterial
    Inherits List(Of Block)

    Public Property IsPractiseTestMaterial As Boolean = False

    Private Property CurrentBlockOrderIndex As Integer? = Nothing
    Public Property BlockOrder As List(Of Integer)

    ''' <summary>
    ''' Holds the number of presented items is the  test material
    ''' </summary>
    Public Property PresentedItems As Integer = 0

    Public Sub SetupBlockOrder(ByRef TestSpecification As TestSpecification, ByVal ParticipantNumber As Integer)
        If IsPractiseTestMaterial = True Then
            BlockOrder = New List(Of Integer) From {0}
        Else

            If TestSpecification.RandomizeBlockOrder = True Then

                Dim Randomizer As Random
                If TestSpecification.RandomSeed.HasValue = True Then
                    Randomizer = New Random(TestSpecification.RandomSeed)
                Else
                    Randomizer = New Random
                End If
                BlockOrder = Utils.Math.SampleWithoutReplacement(TestSpecification.NumberOfBlocks, 0, TestSpecification.NumberOfBlocks, Randomizer).ToList

            Else

                'Setting manual block orders (in the settings file) allows for counter-balanced blocks, based on Latin Squares, or similar technique, see https://en.wikipedia.org/wiki/Block_design

                'Converts the one-based participent order to a zero-based index
                Dim ParticipantIndex As Integer = ParticipantNumber - 1

                'Unwraps the ParticipantIndex to the number of available blockorders
                Dim ManualBlockOrderCount As Integer = TestSpecification.ManualBlockOrders.Count
                Dim UnwrappedParticipantIndex As Integer = ParticipantIndex Mod ManualBlockOrderCount

                'Setting the block order to use
                BlockOrder = TestSpecification.ManualBlockOrders(UnwrappedParticipantIndex)


            End If

        End If

    End Sub

    Public Enum NextEventTypes
        PresetTrial
        EndOfBlock
        EndOfTest
    End Enum

    Public Class NextTrialResult
        Public NextEventType As NextEventTypes
        Public NextTrialItem As TestItem
    End Class

    Public Function GetCurrentBlockNumber() As Integer?
        If CurrentBlockOrderIndex Is Nothing Then
            Return Nothing
        Else
            Return Me(BlockOrder(CurrentBlockOrderIndex)).BlockNumber
        End If
    End Function

    Public Function CompletedBlocks() As Integer
        If CurrentBlockOrderIndex Is Nothing Then
            Return 0
        Else
            Return CurrentBlockOrderIndex + 1
        End If
    End Function

    Public Function GetNextEvent(ByRef Randomizer As Random) As NextTrialResult

        Dim NextTrialResult As New NextTrialResult

        If CurrentBlockOrderIndex Is Nothing Then
            CurrentBlockOrderIndex = 0
        End If

        'Checking if there are any remaining items to present
        Dim UnpresentedItems = Me(BlockOrder(CurrentBlockOrderIndex)).GetUnpresentedItems
        If UnpresentedItems.Count = 0 Then

            'Checks if all blocks have been testet
            If CurrentBlockOrderIndex >= BlockOrder.Count - 1 Then
                'This was the last block
                NextTrialResult.NextEventType = NextEventTypes.EndOfTest
                Return NextTrialResult

            Else
                'This was not the last block
                NextTrialResult.NextEventType = NextEventTypes.EndOfBlock
                Return NextTrialResult

            End If

        Else

            'There are more items to present in the current block
            NextTrialResult.NextEventType = NextEventTypes.PresetTrial

            'Selects a random test item
            Dim RandomItemIndex = Randomizer.Next(0, UnpresentedItems.Count)
            NextTrialResult.NextTrialItem = UnpresentedItems(RandomItemIndex)

            'Increments PresentedItems (in the test as a whole)
            PresentedItems += 1
            NextTrialResult.NextTrialItem.WithinTest_TrialPresentationOrder = PresentedItems

            'Increments PresentedItems (in the current block)
            Me(BlockOrder(CurrentBlockOrderIndex)).PresentedItems += 1
            NextTrialResult.NextTrialItem.WithinBlock_TrialPresentationOrder = Me(BlockOrder(CurrentBlockOrderIndex)).PresentedItems

            'Stores the BlockPresentationOrder
            NextTrialResult.NextTrialItem.BlockPresentationOrder = CurrentBlockOrderIndex + 1

            Return NextTrialResult

        End If

    End Function

    Public Sub DisposeVideos()

        For Each Block In Me
            Block.DisposeVideos()
        Next

    End Sub

    Public Sub GotoNextBlock()
        CurrentBlockOrderIndex += 1
    End Sub

    ''' <summary>
    ''' Exports the test results
    ''' </summary>
    ''' <param name="OutputFolder"></param>
    ''' <param name="ParticipantID"></param>
    ''' <param name="SelectedBlocks">If set, should contain the block number of the blocks to export.</param>
    Public Sub ExportResults(ByVal OutputFolder As String, ByVal ParticipantID As String, Optional ByVal SelectedBlocks As SortedSet(Of Integer) = Nothing)

        'Creating a filename
        Dim FileName As String = CreateExportFileName(ParticipantID, SelectedBlocks)

        Dim ExportList As New List(Of String)

        Dim IncludeHeadings As Boolean = True
        For Each Block In Me

            If SelectedBlocks IsNot Nothing Then
                'Skips to next block if the block number is not in the SelectBlocks List
                If SelectedBlocks.Contains(Block.BlockNumber) = False Then Continue For
            End If

            For Each TestItem In Block.RealItems
                ExportList.Add(TestItem.ToString(IncludeHeadings))
                'Setting IncludeHeadings to False in order to only get headings on the first line
                IncludeHeadings = False
            Next

            For Each TestItem In Block.PseudoItems
                ExportList.Add(TestItem.ToString(IncludeHeadings))
                'Setting IncludeHeadings to False in order to only get headings on the first line
                IncludeHeadings = False
            Next

        Next

        Utils.SendInfoToLog(String.Join(vbCrLf, ExportList), FileName, OutputFolder)

    End Sub

    Public Function CreateExportFileName(ByVal ParticipantID As String, Optional ByVal SelectedBlocks As SortedSet(Of Integer) = Nothing) As String

        Dim FileName As String
        If IsPractiseTestMaterial = True Then
            FileName = ParticipantID & "_PractiseTest"
        Else
            If SelectedBlocks Is Nothing Then
                FileName = ParticipantID & "_AllBlocks"
            Else
                If SelectedBlocks.Count = 1 Then
                    FileName = ParticipantID & "_Block_" & String.Join("_", SelectedBlocks)
                Else
                    FileName = ParticipantID & "_Blocks_" & String.Join("_", SelectedBlocks)
                End If
            End If
        End If

        Return FileName

    End Function

End Class

Public Class Block

    Public Property ParentTestMaterial As TestMaterial
    Public Property BlockNumber As Integer
    Public Property RealItems As List(Of TestItem)
    Public Property PseudoItems As List(Of TestItem)

    ''' <summary>
    ''' Holds the number of presented items is the block
    ''' </summary>
    Public Property PresentedItems As Integer = 0

    Public Function GetUnpresentedItems() As List(Of TestItem)

        Dim UnpresentedItems As New List(Of TestItem)

        For Each Item In RealItems
            If Item.Result = Results.NotPresented Then UnpresentedItems.Add(Item)
        Next

        For Each Item In PseudoItems
            If Item.Result = Results.NotPresented Then UnpresentedItems.Add(Item)
        Next

        Return UnpresentedItems

    End Function

    Public Sub DisposeVideos()
        For Each Item In RealItems
            If Item IsNot Nothing Then
                If Item.CurrentVideo IsNot Nothing Then
                    Item.CurrentVideo.Dispose()
                End If
            End If
        Next
        For Each Item In PseudoItems
            If Item IsNot Nothing Then
                If Item.CurrentVideo IsNot Nothing Then
                    Item.CurrentVideo.Dispose()
                End If
            End If
        Next
    End Sub

End Class

Public Enum ItemTypes
    Real
    Pseudo
End Enum

Public Class TestItem

    Public Property ParentBlock As Block
    Public Property ItemType As ItemTypes
    Public Property FilePath As String = ""
    Public Property FileName As String = ""
    Public Property ItemNumber As Integer

    Public CurrentVideo As Media = Nothing

    Private _Result As Results = Results.NotPresented
    Public ReadOnly Property Result As Results
        Get
            Return _Result
        End Get
    End Property

    Private _ButtonPressed As Keys
    Public ReadOnly Property ButtonPressed As Keys
        Get
            Return _ButtonPressed
        End Get
    End Property

    Public Property WithinTest_TrialPresentationOrder As Integer
    Public Property BlockPresentationOrder As Integer
    Public Property WithinBlock_TrialPresentationOrder As Integer



    Private _ResponseTime As DateTime
    Public ReadOnly Property ResponseTime As DateTime
        Get
            Return _ResponseTime
        End Get
    End Property

    Private _ResponseInterval As TimeSpan
    Public ReadOnly Property ResponseInterval As TimeSpan
        Get
            Return _ResponseInterval
        End Get
    End Property


    Public PlayCommandTime As DateTime
    Public PresentationTime As DateTime
    Public VideoVisibleTime As DateTime
    Public VideoEndTime As DateTime
    Public VideoDuration As String = ""
    Public FrameRate As Integer


    Public ResponseGiven As Boolean = False

    Public Sub SetMedia(ByRef LibVLC As LibVLC)
        CurrentVideo = New Media(LibVLC, FilePath, FromType.FromPath)
    End Sub

    Public Sub NewResponse(ByVal Result As Results, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime, ByVal ResponseInterval As TimeSpan)

        If ResponseGiven = True Then
            'Stops from given responses a second time ( which, by the way, should never occur...)
            Exit Sub
        Else
            ResponseGiven = True
        End If

        Me._Result = Result
        Me._ButtonPressed = ButtonPressed
        Me._ResponseTime = ResponseTime
        Me._ResponseInterval = ResponseInterval

    End Sub

    Public Shared Function GetExportHeadings() As String
        Dim HeadingList As New List(Of String)

        HeadingList.Add("IsPractiseTestBlock")
        HeadingList.Add("ItemNumber")
        HeadingList.Add("WithinTest_TrialPresentationOrder")
        HeadingList.Add("BlockNumber")
        HeadingList.Add("BlockPresentationOrder")
        HeadingList.Add("WithinBlock_TrialPresentationOrder")
        HeadingList.Add("ItemType")
        HeadingList.Add("FileName")
        HeadingList.Add("FilePath")
        HeadingList.Add("ButtonPressed")
        HeadingList.Add("Result")
        HeadingList.Add("ResponseInterval")
        HeadingList.Add("PlayCommandTime")
        HeadingList.Add("PresentationTime")
        HeadingList.Add("VideoVisibleTime")
        HeadingList.Add("ResponseTime")
        HeadingList.Add("VideoEndTime")
        HeadingList.Add("VideoDuration")
        HeadingList.Add("FrameRate")


        Return String.Join(vbTab, HeadingList)

    End Function

    Public Shadows Function ToString(ByVal IncludeHeading As Boolean) As String

        Dim OutputList As New List(Of String)

        If IncludeHeading Then
            OutputList.Add(GetExportHeadings)
        End If

        Dim DataList As New List(Of String)

        DataList.Add(ParentBlock.ParentTestMaterial.IsPractiseTestMaterial.ToString)
        DataList.Add(ItemNumber.ToString)
        DataList.Add(WithinTest_TrialPresentationOrder.ToString)
        DataList.Add(ParentBlock.BlockNumber)
        DataList.Add(BlockPresentationOrder.ToString)
        DataList.Add(WithinBlock_TrialPresentationOrder.ToString)

        DataList.Add(ItemType.ToString)
        DataList.Add(FileName)
        DataList.Add(FilePath)
        DataList.Add(ButtonPressed.ToString)
        DataList.Add(Result.ToString)

        DataList.Add(Math.Round(ResponseInterval.TotalMilliseconds))

        DataList.Add(GetTimeString(PlayCommandTime))
        DataList.Add(GetTimeString(PresentationTime))
        DataList.Add(GetTimeString(VideoVisibleTime))
        DataList.Add(GetTimeString(ResponseTime))
        DataList.Add(GetTimeString(VideoEndTime))
        DataList.Add(VideoDuration)
        DataList.Add(FrameRate.ToString)


        OutputList.Add(String.Join(vbTab, DataList))

        Return String.Join(vbCrLf, OutputList)

    End Function

    Private Function GetTimeString(ByVal Time As DateTime) As String
        Return Time.Hour.ToString("00") & ":" & Time.Minute.ToString("00") & ":" & Time.Second.ToString("00") & ":" & Time.Millisecond.ToString("000")
    End Function


End Class

Public Enum Results
    Real
    Pseudo
    Missing
    NotPresented
End Enum

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
                e.Graphics.DrawString(Me.Value & " / " & Me.Maximum, TextFont, BlackBrush, Me.ClientRectangle, MyStringFormat)
            End If
        End If

    End Sub

End Class