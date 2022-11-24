Public Class VrtForm
    Private Sub ChangeItemButton1_Click(sender As Object, e As EventArgs) Handles ChangeItemButton1.Click

        Dim Questions As New List(Of Tuple(Of String, List(Of String)))
        Questions.Add(New Tuple(Of String, List(Of String))("My question...", New List(Of String) From {"1", "2", "3", "4", "5", "6", "7"}))
        Questions.Add(New Tuple(Of String, List(Of String))("My second question...", New List(Of String) From {"1", "2", "3", "4", "5"}))
        Questions.Add(New Tuple(Of String, List(Of String))("My third question...", New List(Of String) From {"1", "2", "3", "4", "5"}))
        Questions.Add(New Tuple(Of String, List(Of String))("My fourth question... which is very very very very very very very very very very very very very very very very very very very very very very very very long...", New List(Of String) From {"Yes", "No", "Maybe"}))


        Me.RatingItems_TableLayoutPanel.RowStyles.Clear()
        For Each Question In Questions
            Dim NewItemRatingPanel As New VLDT_lib.ItemRatingPanel
            NewItemRatingPanel.AddQuestion(Question.Item1, Question.Item2, RatingItems_TableLayoutPanel.ClientRectangle.Width)
            RatingItems_TableLayoutPanel.Controls.Add(NewItemRatingPanel)
            Me.RatingItems_TableLayoutPanel.RowStyles.Add(New Windows.Forms.RowStyle(Windows.Forms.SizeType.Absolute, NewItemRatingPanel.Height + 5))
        Next

    End Sub
End Class