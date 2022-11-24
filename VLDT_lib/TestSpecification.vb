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

End Class
