Imports System.Windows.Forms
Imports LibVLCSharp.Shared

Public Class TestSpecification

    Public Property GuiLanguage As String = "EN"

    Public Property RandomSeed As Integer?

    Public Property TestSpecificationFilePath As String = ""

    Public Property AvailableResponseKeys As New List(Of Keys) From {Keys.J, Keys.F}

    Public Property RandomizeResponseKeys As Boolean = True

    Public Property NumberOfBlocks As Integer

    Public Property NumberOfTestItems As Integer

    Public Property NumberOfPractiseItems As Integer

    Public Property PractiseScoreLimit As Integer

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


    Public Function GetBlockParentFolder() As String
        Return IO.Path.GetDirectoryName(TestSpecificationFilePath)
    End Function

    Private Function GetBlockFolder(ByVal IsPractiseTestMaterial As Boolean, Optional ByVal BlockNumber As Integer = 0) As String
        If IsPractiseTestMaterial = False Then
            Return IO.Path.Combine(GetBlockParentFolder, "Block" & BlockNumber.ToString("00"))
        Else
            Return IO.Path.Combine(GetBlockParentFolder, "PractiseBlock")
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


    Public Shared Function LoadTestSpecificationFile(ByVal TestSpecificationFilePath As String) As TestSpecification

        Dim NewTestSpecification = New TestSpecification

        'Storing the file path
        NewTestSpecification.TestSpecificationFilePath = TestSpecificationFilePath

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

                    Case "GuiLanguage".ToLowerInvariant
                        NewTestSpecification.GuiLanguage = VariableValueString

                    Case "RandomSeed".ToLowerInvariant
                        Dim TempValue As Integer
                        If Integer.TryParse(VariableValueString, TempValue) = True Then
                            NewTestSpecification.RandomSeed = TempValue
                        Else
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "ResponseKeys".ToLowerInvariant
                        NewTestSpecification.AvailableResponseKeys.Clear()
                        Dim VariableValueStringSplit = VariableValueString.Split(",")
                        Dim FailedReading As Boolean = False
                        If VariableValueStringSplit.Length = 2 Then
                            For i = 0 To 1
                                Try

                                    Dim NewKey As Keys = DirectCast([Enum].Parse(GetType(Keys), VariableValueStringSplit(i).Trim), Integer)
                                    If TestSpecification.LeftSideKeys.Contains(NewKey) Or TestSpecification.RightSideKeys.Contains(NewKey) Then
                                        NewTestSpecification.AvailableResponseKeys.Add(NewKey)
                                    Else
                                        MsgBox("The response key " & NewKey.ToString & " specified in the lexical decision task file (" & TestSpecificationFilePath & ") is not allowed." & vbCrLf & vbCrLf & "The following response keys are allowed: " & vbCrLf &
                                               "Left side keys: " & String.Join(", ", TestSpecification.LeftSideKeys) & vbCrLf &
                                               "Right side keys: " & String.Join(", ", TestSpecification.RightSideKeys) & vbCrLf & vbCrLf &
                                               "Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid response key value!")
                                        Return Nothing
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
                            Return Nothing
                        End If

                    Case "RandomizeResponseKeys".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.RandomizeResponseKeys) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "BlockCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.NumberOfBlocks) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "TestItemCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.NumberOfTestItems) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "PractiseItemCount".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.NumberOfPractiseItems) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "PractiseScoreLimit".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.PractiseScoreLimit) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "MinInterTrialInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.MinInterTrialInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "MaxInterTrialInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.MaxInterTrialInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "MaxResponseTime".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.MaxResponseTime) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "HideVideoBetweenTrials".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.HideVideoBetweenTrials) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "HideVideoAtResponse".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.HideVideoAtResponse) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "PostPresentationResponsePeriod".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.PostPresentationResponsePeriod) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "ExportAfterEveryTrial".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.ExportAfterEveryTrial) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "BackupTimerInterval".ToLowerInvariant
                        If Integer.TryParse(VariableValueString, NewTestSpecification.BackupTimerInterval) = False Then
                            MsgBox("Unable to read the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case "RandomizeBlockOrder".ToLowerInvariant
                        If Boolean.TryParse(VariableValueString, NewTestSpecification.RandomizeBlockOrder) = False Then
                            MsgBox("Unable to interpret the value " & VariableValueString & " given for the variable " & LineVariable & " in the lexical decision task file (" & TestSpecificationFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                            Return Nothing
                        End If

                    Case Else
                        MsgBox("Unknown variable (" & LineVariable & ") detected in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Unkown variable!")
                        Return Nothing
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
                        Return Nothing
                    End If
                Next

                If CurrentBlockOrder.Count > 0 Then
                    ManualBlockOrders.Add(CurrentBlockOrder)
                End If

            End If

        Next

        ' References the manual block orders
        NewTestSpecification.ManualBlockOrders = ManualBlockOrders

        'Checking if block orders are correctly setup
        For Each BlockOrder In NewTestSpecification.ManualBlockOrders

            '- The number of blocks in each block order must equal NumberOfBlocks
            If BlockOrder.Count <> NewTestSpecification.NumberOfBlocks Then
                MsgBox("The block order (" & String.Join(", ", BlockOrder) & ") specified in the lexical decision task file (" & TestSpecificationFilePath & ") must contain exactlyk " & NewTestSpecification.NumberOfBlocks & " (BlockCount) integers. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                Return Nothing
            End If

            '- The BlockOrders must contain all integers from 1 to BlockCount
            For Block As Integer = 1 To NewTestSpecification.NumberOfBlocks
                If Not BlockOrder.Contains(Block) Then
                    MsgBox("The block order (" & String.Join(", ", BlockOrder) & ") specified in the lexical decision task file (" & TestSpecificationFilePath & ") lacks block number " & Block & ". Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                    Return Nothing
                End If
            Next
        Next

        'Checks that a manual blockorder exist if RandomizeBlockOrder = False
        If NewTestSpecification.RandomizeBlockOrder = False Then
            If NewTestSpecification.ManualBlockOrders.Count = 0 Then
                MsgBox("When RandomizeBlockOrder = False, at least one block order needs to be manually specified using the block order tag (<BlockOrder>) in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid block order value!")
                Return Nothing
            End If
        End If


        'Check that MinInterTrialInterval is not higher than MaxInterTrialInterval and vice versa!
        If NewTestSpecification.MinInterTrialInterval > NewTestSpecification.MaxInterTrialInterval Then
            MsgBox("The value given for MinInterTrialInterval (" & NewTestSpecification.MinInterTrialInterval & ") must be lower than the value given for MaxInterTrialInterval (" & NewTestSpecification.MaxInterTrialInterval & ") in the lexical decision task file (" & TestSpecificationFilePath & "). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
            Return Nothing
        End If

        If NewTestSpecification.MinInterTrialInterval = 0 Or NewTestSpecification.MaxInterTrialInterval = 0 Or NewTestSpecification.MaxResponseTime = 0 Then
            MsgBox("The value given for the variables MinInterTrialInterval (" & NewTestSpecification.MinInterTrialInterval & "), MaxInterTrialInterval (" & NewTestSpecification.MaxInterTrialInterval & ") and MaxResponseTime (" & NewTestSpecification.MaxResponseTime & "), in the lexical decision task file (" & TestSpecificationFilePath & ") must all be higher than zero. Where values are zero they will be changed to 1 millisecond.", MsgBoxStyle.Information, "Information!")
        End If

        'Loading language strings from file
        If Utils.GuiStrings.Load(NewTestSpecification) = False Then
            Return Nothing
        End If

        Return NewTestSpecification

    End Function


End Class
