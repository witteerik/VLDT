Imports LibVLCSharp.Shared

Public Class RatingStimulusSet
    Public Property StimulusList As New List(Of RatingStimulus)
    Public Property CurrentItemIndex As Integer = 0
    Public Property SetupFileLocation As String
        Public Property GuiLanguage As String = "EN"

    Private Enum ReadModes
        Setup
        Questions
        VideoFolders
        VideoFilepaths
    End Enum


    Public Shared Function LoadSetupFile(ByVal SetupFilePath As String, ByRef LibVLC As LibVLC, ByVal Randomizer As Random) As RatingStimulusSet

        Dim Output As New RatingStimulusSet

        Dim Questions As New List(Of RatingQuestion)
        Dim FullVideoPaths As New List(Of String)

        'Storing the setup file SetupFileLocation 
        Output.SetupFileLocation = IO.Path.GetDirectoryName(SetupFilePath)

        Dim InputLines() As String = System.IO.File.ReadAllLines(SetupFilePath, System.Text.Encoding.UTF8)
        Dim CurrentReadMode As ReadModes = ReadModes.Setup

        Dim NumberOfQuestions As Integer = -1
        Dim NumberOfVideos As Integer = -1
        Dim RandomizeVideos As Boolean = True
        Dim VideoSubFolders As New SortedSet(Of String)
        Dim VideoFileSubPaths As New List(Of String)

        Dim CurrentQuestion As RatingQuestion = Nothing

        For Each Line In InputLines

            If Line.Trim = "" Then Continue For
            If Line.Trim.StartsWith("//") Then Continue For
            If Line.ToLowerInvariant.Trim.StartsWith("<Setup>".ToLowerInvariant) Then
                CurrentReadMode = ReadModes.Setup
                Continue For
            ElseIf Line.ToLowerInvariant.Trim.StartsWith("<Questions>".ToLowerInvariant) Then
                CurrentReadMode = ReadModes.Questions
                Continue For
            ElseIf Line.ToLowerInvariant.Trim.StartsWith("<VideoFolders>".ToLowerInvariant) Then
                CurrentReadMode = ReadModes.VideoFolders
                Continue For
            ElseIf Line.ToLowerInvariant.Trim.StartsWith("<VideoFilepaths>".ToLowerInvariant) Then
                CurrentReadMode = ReadModes.VideoFilepaths
                Continue For
            Else
                'keep the same type and read the value off the line
            End If

            Dim LineValueSplit = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Split("=")
            Dim LineFirstBit As String = LineValueSplit(0).Trim
            'Skipping if no variable value is available
            Dim LineSecondBit As String = ""
            If LineValueSplit.Length > 1 Then
                LineSecondBit = LineValueSplit(1).Trim
                'Re-joining bits split off by multiple equality signs
                If LineValueSplit.Length > 2 Then
                    For c = 2 To LineValueSplit.Length - 1
                        LineSecondBit = LineSecondBit & "=" & LineValueSplit(c).Trim
                    Next
                End If
            End If

            Select Case CurrentReadMode
                Case ReadModes.Setup

                    Select Case LineFirstBit.ToLowerInvariant

                        Case "GuiLanguage".ToLowerInvariant
                            Output.GuiLanguage = LineSecondBit

                        Case "NumberOfQuestions".ToLowerInvariant
                            Dim TempValue As Integer
                            If Integer.TryParse(LineSecondBit, TempValue) = True Then
                                NumberOfQuestions = TempValue
                            Else
                                MsgBox("Unable to read the value " & LineSecondBit & " given for the variable " & LineFirstBit & " in the video rating task file (" & SetupFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                                Return Nothing
                            End If

                        Case "NumberOfVideos".ToLowerInvariant
                            Dim TempValue As Integer
                            If Integer.TryParse(LineSecondBit, TempValue) = True Then
                                NumberOfVideos = TempValue
                            Else
                                MsgBox("Unable to read the value " & LineSecondBit & " given for the variable " & LineFirstBit & " in the video rating task file (" & SetupFilePath & ") as an integer number. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                                Return Nothing
                            End If

                        Case "RandomizeVideos".ToLowerInvariant
                            If Boolean.TryParse(LineSecondBit, RandomizeVideos) = False Then
                                MsgBox("Unable to interpret the value " & LineSecondBit & " given for the variable " & LineFirstBit & " in the video rating task file (" & SetupFilePath & ") as a boolean value (True or False). Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                                Return Nothing
                            End If

                    End Select

                Case ReadModes.Questions

                    Select Case LineFirstBit.ToLowerInvariant

                        Case "Question".ToLowerInvariant
                            CurrentQuestion = New RatingQuestion
                            CurrentQuestion.Question = LineSecondBit
                            Questions.Add(CurrentQuestion)

                        Case "Type".ToLowerInvariant
                            Dim CurrentQuestionType As RatingQuestion.QuestionTypes
                            If [Enum].TryParse(LineSecondBit, True, CurrentQuestionType) = True Then
                                CurrentQuestion.QuestionType = CurrentQuestionType
                            Else
                                MsgBox("Unable to interpret the value " & LineSecondBit & " given for the variable " & LineFirstBit & " in the video rating task file (" & SetupFilePath & ") as a any of Categorical, ContinousScale, or IntegerScale. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                                Return Nothing
                            End If

                        Case "Responses".ToLowerInvariant

                            Dim ResponseAlternativesString() As String = LineSecondBit.Split("|")

                            If CurrentQuestion.QuestionType = RatingQuestion.QuestionTypes.Categorical Then

                                For i = 0 To ResponseAlternativesString.Length - 1
                                    Dim TrimmedValue = ResponseAlternativesString(i).Trim
                                    If CurrentQuestion.CategoricalResponseAlternatives Is Nothing Then CurrentQuestion.CategoricalResponseAlternatives = New List(Of String)
                                    If TrimmedValue <> "" Then CurrentQuestion.CategoricalResponseAlternatives.Add(TrimmedValue)
                                Next

                            ElseIf CurrentQuestion.QuestionType = RatingQuestion.QuestionTypes.Text Then
                                'Ignores any value given for response if it's a text type question

                            Else

                                'It will be either ContinousScale or IntegerScale, which are treated the same here
                                For i = 0 To ResponseAlternativesString.Length - 1
                                    Dim TrimmedValue = ResponseAlternativesString(i).Trim
                                    Dim CastValue As Double
                                    If Double.TryParse(TrimmedValue, CastValue) Then
                                        If CurrentQuestion.ScaleValues Is Nothing Then CurrentQuestion.ScaleValues = New List(Of Double)
                                        CurrentQuestion.ScaleValues.Add(CastValue)
                                    Else
                                        MsgBox("Unable to interpret the value " & LineSecondBit & " given for the variable " & LineFirstBit & " in the video rating task file (" & SetupFilePath & ") as a numeric value. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                                        Return Nothing
                                    End If
                                Next
                            End If

                    End Select

                Case ReadModes.VideoFolders
                    If LineFirstBit <> "" Then VideoSubFolders.Add(LineFirstBit)

                Case ReadModes.VideoFilepaths
                    If LineFirstBit <> "" Then VideoFileSubPaths.Add(LineFirstBit)
            End Select
        Next

        'Adding video file full paths
        For Each SubFolder In VideoSubFolders
            Dim FullFolderPath As String = IO.Path.Combine(Output.SetupFileLocation, SubFolder)
            If IO.Directory.Exists(FullFolderPath) = False Then
                MsgBox("The directory " & FullFolderPath & " assumed from the video sub-folder " & SubFolder & " supplied in the video rating task file (" & SetupFilePath & ") does not exist. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                Return Nothing
            End If

            'Adding files in the SubFolder
            'TODO. Here we add all files. Files that later cannot be read as videofiles by VLClib will throw an exception!
            Dim FolderFiles = IO.Directory.GetFiles(FullFolderPath)
            FullVideoPaths.AddRange(FolderFiles)

        Next

        'Adding video files specified with file names
        For Each SubPath In VideoFileSubPaths
            Dim FullVideoPath As String = IO.Path.Combine(Output.SetupFileLocation, SubPath)
            If IO.File.Exists(FullVideoPath) = False Then
                MsgBox("The file path " & FullVideoPath & " supplied in the video rating task file (" & SetupFilePath & ") does not exist. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
                Return Nothing
            End If
            FullVideoPaths.Add(FullVideoPath)
        Next

        'Check number of questions read by comparing with NumberOfQuestions
        If NumberOfQuestions = -1 Then
            'This means that the user did not enter the number of questions in the settings files
            MsgBox("The file " & SetupFilePath & " must contain the intended and correct number of questions specified by a variable called 'NumberOfQuestions', for example 'NumberOfQuestions = 5'. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
            Return Nothing
        End If

        'Check number of videos read by comparing with NumberOfVideos
        If NumberOfVideos = -1 Then
            'This means that the user did not enter the number of videos in the settings files
            MsgBox("The file " & SetupFilePath & " must contain the intended and correct number of videos specified by a variable called 'NumberOfVideos' to show, for example 'NumberOfVideos = 58'. Cannot proceed.", MsgBoxStyle.Exclamation, "Invalid variable value!")
        End If

        'Checking that the actual number of questions and video file paths agree with their corresponding specified numbers
        If NumberOfQuestions <> Questions.Count Then
            MsgBox("The number of questions specied by the variable NumberOfQuestions (" & NumberOfQuestions & ") in the file " & SetupFilePath & " do not agree with the actual number of successfully loaded questions (" & Questions.Count & ") from the same file.", MsgBoxStyle.Exclamation, "Invalid variable value!")
            Return Nothing
        End If

        If NumberOfVideos <> FullVideoPaths.Count Then
            MsgBox("The number of video files specied by the variable NumberOfVideos (" & NumberOfVideos & ") in the file " & SetupFilePath & " do not agree with the actual number of infered video paths (" & FullVideoPaths.Count & ") from the data under the <VideoFolders> and <VideoFilepaths> tags and in the same file.", MsgBoxStyle.Exclamation, "Invalid variable value!")
            Return Nothing
        End If

        'Checks that all questions have all needed and valid items
        For Each Question In Questions
            If Question.Question = "" Then
                MsgBox("A question without any question text has been loaded from the file " & SetupFilePath & ". This is not allowed. Unable to continue!", MsgBoxStyle.Exclamation, "Invalid variable value!")
                Return Nothing
            End If

            Select Case Question.QuestionType
                Case RatingQuestion.QuestionTypes.Categorical
                    If Question.CategoricalResponseAlternatives.Count < 2 Then
                        MsgBox("The following categorical question loaded from the file " & SetupFilePath & " has less than two response alternatives, which is not allowed. Unable to continue!" & vbCrLf & vbCrLf & Question.Question, MsgBoxStyle.Exclamation, "Invalid variable value!")
                        Return Nothing
                    End If

                Case RatingQuestion.QuestionTypes.IntegerScale
                    If Question.ScaleValues.Max - Question.ScaleValues.Min = 0 Then
                        MsgBox("The following integer-scale question loaded from the file " & SetupFilePath & " has invalid response values. The variable 'Responses' in must contain at least two integer values differing by at least one. Unable to continue!" & vbCrLf & vbCrLf & Question.Question, MsgBoxStyle.Exclamation, "Invalid variable value!")
                        Return Nothing
                    End If
                Case RatingQuestion.QuestionTypes.ContinousScale
                    If Question.ScaleValues.Max - Question.ScaleValues.Min = 0 Then
                        MsgBox("The following continous-scale question loaded from the file " & SetupFilePath & " has invalid response values. The variable 'Responses' in must contain at least two non-equal numeric values. Unable to continue!" & vbCrLf & vbCrLf & Question.Question, MsgBoxStyle.Exclamation, "Invalid variable value!")
                        Return Nothing
                    End If
                Case RatingQuestion.QuestionTypes.Text
                    'We need to do nothing here, as no response values are loaded for text typ questions
            End Select
        Next

        'Loading videos and thereby creating rating stimuli
        Output.LoadVideoFiles(FullVideoPaths, LibVLC, RandomizeVideos, Randomizer)

        'Adding copies of all questions to each rating stimulus
        For Each Stimulus In Output.StimulusList
            Stimulus.SetQuestions(Questions)
        Next

        'Setting StimulusNumber
        Dim StimulusNumber As Integer = 1
        For Each Stimulus In Output.StimulusList
            Stimulus.StimulusNumber = StimulusNumber
            StimulusNumber += 1
        Next

        'Loading language strings from file
        Dim GuiStringsFilePath = IO.Path.Combine(Output.SetupFileLocation, "GuiStringsVRT.txt")

        If Utils.GuiStrings.Load(GuiStringsFilePath, Output.GuiLanguage, GetType(Utils.VrtGuiStringKeys)) = False Then
            Return Nothing
        End If

        Return Output

    End Function


    Public Function LoadVideoFiles(ByVal VideoFilesToLoad As List(Of String), ByRef LibVLC As LibVLC, ByVal RandomizeOrder As Boolean, ByVal Randomizer As Random) As Boolean

        'Creating new items with their file path assigned
        For Each FilePath In VideoFilesToLoad
            Try

                Dim NewTestStimulus = New RatingStimulus With {.FilePath = FilePath, .FileName = IO.Path.GetFileNameWithoutExtension(FilePath)}
                NewTestStimulus.SetMedia(LibVLC)
                StimulusList.Add(NewTestStimulus)

            Catch ex As Exception
                MsgBox("Unable to load the following file as a media file (you should remove it from the folder): " & FilePath & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Invalid media file detected!")
                Return False
            End Try
        Next

        If RandomizeOrder = True Then
            Dim TempList As New List(Of RatingStimulus)
            Do Until StimulusList.Count = 0
                Dim RandomIndex As Integer = Randomizer.Next(0, StimulusList.Count)
                TempList.Add(StimulusList(RandomIndex))
                StimulusList.RemoveAt(RandomIndex)
            Loop
            StimulusList = TempList
        End If

        Return True

    End Function

    Public Function GetNextNonCompleteStimulus() As RatingStimulus

        Dim SelectedStimulus As RatingStimulus = Nothing

        For n = 0 To StimulusList.Count - 1
            If StimulusList(n).HasAllResponses = False Then
                CurrentItemIndex = n
                SelectedStimulus = StimulusList(n)
                Exit For
            End If
        Next

        Return SelectedStimulus

    End Function

    Public Function GetPreviousStimulus() As RatingStimulus

        If CurrentItemIndex > 0 Then
            CurrentItemIndex -= 1
            Return StimulusList(CurrentItemIndex)
        Else
            Return Nothing
        End If

    End Function

    Public Function GetNextStimulus() As RatingStimulus

        If CurrentItemIndex < StimulusList.Count - 1 Then
            CurrentItemIndex += 1
            Return StimulusList(CurrentItemIndex)
        Else
            Return Nothing
        End If

    End Function

    Public Sub SaveResults(ByVal ExportFileName As String, ByVal ParticipantID As String, ByVal ParticipantNumber As Integer)

        If StimulusList.Count > 0 Then

            Dim ExportList As New List(Of String)

            Dim IncludeHeadings As Boolean = True
            For n = 0 To StimulusList.Count - 1
                ExportList.Add(StimulusList(n).ToString(IncludeHeadings, ParticipantID, ParticipantNumber))
                IncludeHeadings = False
            Next

            'Exporting data
            Utils.SendInfoToLog(String.Join(vbCrLf, ExportList), IO.Path.GetFileName(ExportFileName), IO.Path.GetDirectoryName(ExportFileName), True, True)

            MsgBox(Utils.GetGuiString(Utils.VrtGuiStringKeys.SavedToFile) & " " & ExportFileName & vbCrLf & vbCrLf & Utils.GetGuiString(Utils.VrtGuiStringKeys.CloseApp2), MsgBoxStyle.Information, My.Application.Info.Title)

        End If

    End Sub

End Class


Public Class RatingStimulus

    Public CurrentVideo As Media = Nothing
    Public Property FilePath As String = ""
    Public Property FileName As String = ""

    Public Property ResponseTime As DateTime

    Public Property StimulusNumber As Integer

    Public Sub SetMedia(ByRef LibVLC As LibVLC)
        CurrentVideo = New Media(LibVLC, FilePath, FromType.FromPath)
    End Sub

    Public Property Questions As New List(Of RatingQuestion)

    Public Sub SetQuestions(ByRef Questions As List(Of RatingQuestion))

        For Each Question In Questions
            Me.Questions.Add(Question.CreateDeepCopy)
        Next

    End Sub


    Public Function HasAllResponses() As Boolean

        For Each Question In Questions
            Select Case Question.QuestionType
                Case RatingQuestion.QuestionTypes.Categorical
                    If Question.CategoricalResponse = "" Then
                        Return False
                    End If

                Case RatingQuestion.QuestionTypes.ContinousScale, RatingQuestion.QuestionTypes.IntegerScale
                    If Question.ScaleResponse.HasValue = False Then
                        Return False
                    End If

                Case RatingQuestion.QuestionTypes.Text
                    If Question.TextResponse.trim = "" Then
                        Return False
                    End If

                Case Else
                    Throw New NotImplementedException("Unknown QuestionType (" & Question.QuestionType & ") supplied  for question: " & Question.Question)
            End Select

        Next
        Return True

    End Function


    Private Function GetExportHeadings() As String

        Dim HeadingList As New List(Of String)

        HeadingList.Add("ParticipantID")
        HeadingList.Add("ParticipantNumber")
        HeadingList.Add("StimulusNumber")
        HeadingList.Add("VideoFileName")
        HeadingList.Add("VideoFilePath")
        HeadingList.Add("QuestionNr")
        HeadingList.Add("Question")
        HeadingList.Add("QuestionType")
        HeadingList.Add("PresentedResponseAlternatives")
        HeadingList.Add("Response")
        HeadingList.Add("ResponseTime(HH:MM:SS:MMM)")

        Return String.Join(vbTab, HeadingList)

    End Function

    Public Shadows Function ToString(ByVal IncludeHeading As Boolean, ByVal ParticipantID As String, ByVal ParticipantNumber As Integer) As String

        Dim OutputList As New List(Of String)

        If IncludeHeading Then
            OutputList.Add(GetExportHeadings)
        End If

        For q = 0 To Questions.Count - 1

            Dim ColumnList As New List(Of String)

            ColumnList.Add(ParticipantID)
            ColumnList.Add(ParticipantNumber)

            ColumnList.Add(StimulusNumber)
            ColumnList.Add(FileName)
            ColumnList.Add(FilePath)

            ColumnList.Add(q + 1)

            Dim Question = Questions(q)

            ColumnList.Add(Question.Question)
            ColumnList.Add(Question.QuestionType.ToString)

            If Question.QuestionType = RatingQuestion.QuestionTypes.Categorical Then
                ColumnList.Add(String.Join(" | ", Question.CategoricalResponseAlternatives))
            ElseIf Question.QuestionType = RatingQuestion.QuestionTypes.Text Then
                'There are no alternatives for text type questions. Exports NA
                ColumnList.Add("NA")
            Else
                ColumnList.Add(String.Join(" | ", Question.ScaleValues))
            End If

            If Question.QuestionType = RatingQuestion.QuestionTypes.Categorical Then
                If Question.CategoricalResponse <> "" Then
                    ColumnList.Add(Question.CategoricalResponse)
                Else
                    ColumnList.Add("MissingValue")
                End If
            ElseIf Question.QuestionType = RatingQuestion.QuestionTypes.Text Then
                If Question.TextResponse.Trim <> "" Then
                    ColumnList.Add(Question.TextResponse)
                Else
                    ColumnList.Add("MissingValue")
                End If
            Else
                If Question.ScaleResponse.HasValue = True Then
                    ColumnList.Add(Question.ScaleResponse)
                Else
                    ColumnList.Add("MissingValue")
                End If
            End If

            ColumnList.Add(GetTimeString(ResponseTime))

            OutputList.Add(String.Join(vbTab, ColumnList))

        Next


        Return String.Join(vbCrLf, OutputList)

    End Function

    Private Function GetTimeString(ByVal Time As DateTime) As String
        Return Time.Hour.ToString("00") & ":" & Time.Minute.ToString("00") & ":" & Time.Second.ToString("00") & ":" & Time.Millisecond.ToString("000")
    End Function


End Class

