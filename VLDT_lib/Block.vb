
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
            If Item.Response = Response.NotPresented Then UnpresentedItems.Add(Item)
        Next

        For Each Item In PseudoItems
            If Item.Response = Response.NotPresented Then UnpresentedItems.Add(Item)
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
