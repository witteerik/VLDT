Imports LibVLCSharp.Shared

Public Class RatingStimulusSet
    Public Property StimulusList As New List(Of RatingStimulus)
    Public Property CurrentItemIndex As Integer = 0

    Public Function LoadTestStimuli(ByVal VideoFilesToLoad As String(), ByRef LibVLC As LibVLC, ByVal RandomizeOrder As Boolean, ByVal Randomizer As Random) As Boolean

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

        If CurrentItemIndex < StimulusList.Count - 2 Then
            CurrentItemIndex += 1
            Return StimulusList(CurrentItemIndex)
        Else
            Return Nothing
        End If

    End Function

End Class


Public Class RatingStimulus

    Public CurrentVideo As Media = Nothing
    Public Property FilePath As String = ""
    Public Property FileName As String = ""

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
            Select Case Question.GetQuestionType
                Case RatingQuestion.QuestionTypes.Categorical
                    If Question.CategoricalResponse = "" Then
                        Return False
                    End If

                Case RatingQuestion.QuestionTypes.Scale
                    If Question.ScaleResponse.HasValue = False Then
                        Return False
                    End If

                Case Else
                    Throw New Exception("The following question has no response alternatives: " & vbCrLf & vbCrLf & Question.Question)
            End Select

        Next
        Return True

    End Function

End Class

