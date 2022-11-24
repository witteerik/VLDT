Imports System.Windows.Forms
Imports LibVLCSharp.Shared

Public Enum ItemTypes
    Real
    Pseudo
End Enum

Public Enum Response
    Real
    Pseudo
    Missing
    NotPresented
End Enum

Public Enum Result
    Incorrect
    Correct
    Missing
    NotPresented
End Enum


Public Class TestItem

    Public Property ParentBlock As Block
    Public Property ItemType As ItemTypes
    Public Property FilePath As String = ""
    Public Property FileName As String = ""
    Public Property ItemNumber As Integer

    Public CurrentVideo As Media = Nothing

    Private _Response As Response = Response.NotPresented
    Public ReadOnly Property Response As Response
        Get
            Return _Response
        End Get
    End Property

    Private _ButtonPressed As Keys
    Public ReadOnly Property ButtonPressed As Keys
        Get
            Return _ButtonPressed
        End Get
    End Property

    Private _Result As Result = Result.NotPresented
    Public ReadOnly Property Result As Result
        Get
            Return _Result
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

    Public Sub NewResponse(ByVal Response As Response, ByVal ButtonPressed As Keys, ByVal ResponseTime As DateTime, ByVal ResponseInterval As TimeSpan)

        If ResponseGiven = True Then
            'Stops from given responses a second time ( which, by the way, should never occur...)
            Exit Sub
        Else
            ResponseGiven = True
        End If

        Me._Response = Response
        Me._ButtonPressed = ButtonPressed
        Me._ResponseTime = ResponseTime
        Me._ResponseInterval = ResponseInterval

        Select Case Response
            Case Response.Real
                If ItemType = ItemTypes.Real Then
                    _Result = Result.Correct
                Else
                    _Result = Result.Incorrect
                End If

            Case Response.Pseudo
                If ItemType = ItemTypes.Pseudo Then
                    _Result = Result.Correct
                Else
                    _Result = Result.Incorrect
                End If

            Case Response.Missing
                _Result = Result.Missing

        End Select

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
        HeadingList.Add("Response")
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
        DataList.Add(Response.ToString)
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


    ''' <summary>
    ''' Clears all results and prepares the test item to be run again (useful to redo the practise test)
    ''' </summary>
    Public Sub ClearResults()

        If ParentBlock.ParentTestMaterial.IsPractiseTestMaterial = False Then
            MsgBox("Only practise blocks can be reset!", , "Invalid operation")
            Exit Sub
        End If

        ResponseGiven = False
        _ButtonPressed = Nothing
        _ResponseTime = Nothing
        _ResponseInterval = Nothing
        _Response = Response.NotPresented
        _Result = Result.NotPresented

    End Sub

End Class

