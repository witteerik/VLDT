Imports LibVLCSharp.Shared
Imports VLDT_lib

Public Class TestStimulus

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

Public Class TestStimulusSet
    Public Property StimulusList As New List(Of TestStimulus)

    Public Property PresentedStimulusHistory As New List(Of TestStimulus)

    Public Function LoadTestStimuli(ByVal VideoFilesToLoad As String(), ByRef LibVLC As LibVLC) As Boolean

        'Creating new items with their file path assigned
        For Each FilePath In VideoFilesToLoad
            Try

                Dim NewTestStimulus = New TestStimulus With {.FilePath = FilePath, .FileName = IO.Path.GetFileNameWithoutExtension(FilePath)}
                NewTestStimulus.SetMedia(LibVLC)
                StimulusList.Add(NewTestStimulus)

            Catch ex As Exception
                MsgBox("Unable to load the following file as a media file (you should remove it from the folder): " & FilePath & vbCr & vbCr & "Unable to continue!", MsgBoxStyle.Exclamation, "Invalid media file detected!")
                Return False
            End Try
        Next

        Return True

    End Function

    Public Function GetNextStimulus() As TestStimulus


        Dim SelectedStimulus As TestStimulus = Nothing

        For Each Stimulus In StimulusList
            If Stimulus.HasAllResponses = False Then
                SelectedStimulus = Stimulus
                Exit For
            End If
        Next

        Return SelectedStimulus

    End Function

    Public Function GetLastPresentedStimulus() As TestStimulus

        If PresentedStimulusHistory.Count = 0 Then
            Return Nothing
        Else
            Return PresentedStimulusHistory(PresentedStimulusHistory.Count - 1)
        End If

    End Function

End Class
