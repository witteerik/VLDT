Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel


<Serializable>
Public Class ItemRatingPanel
    Inherits TableLayoutPanel

    Public Sub New()

        Me.BorderStyle = Windows.Forms.BorderStyle.Fixed3D

    End Sub

    Public Sub AddQuestion(ByVal Question As String, ByVal ResponseAlternatives As List(Of String), ByVal ParentWidth As Integer)

        Me.RowCount = 3
        Me.ColumnCount = ResponseAlternatives.Count

        Me.Dock = DockStyle.Top

        Me.GrowStyle = TableLayoutPanelGrowStyle.FixedSize

        'Adding question text box
        Dim QuestionTextBox As New RichTextBox
        QuestionTextBox.Font = New Font(QuestionTextBox.Font.FontFamily, QuestionTextBox.Font.Size + 2)
        QuestionTextBox.Text = Question
        QuestionTextBox.SelectAll()
        QuestionTextBox.SelectionAlignment = HorizontalAlignment.Center
        QuestionTextBox.DeselectAll()
        QuestionTextBox.Multiline = True
        QuestionTextBox.ScrollBars = RichTextBoxScrollBars.None
        QuestionTextBox.WordWrap = True
        QuestionTextBox.Dock = DockStyle.Fill
        QuestionTextBox.Height = QuestionTextBox.GetPreferredSize(New Size(0.5 * ParentWidth, 30)).Height
        Me.Height = QuestionTextBox.Height + 40

        Me.Controls.Add(QuestionTextBox, 0, 0)
        Me.SetColumnSpan(QuestionTextBox, ResponseAlternatives.Count)

        'Adding reponse radiobuttons
        For n = 0 To ResponseAlternatives.Count - 1
            Dim NewResponseAlternativeButton As New RadioButton
            'NewResponseAlternativeButton.Text = ResponseAlternatives(n)
            NewResponseAlternativeButton.CheckAlign = ContentAlignment.MiddleCenter
            NewResponseAlternativeButton.Dock = DockStyle.Fill
            Me.Controls.Add(NewResponseAlternativeButton, n, 1)
        Next

        For n = 0 To ResponseAlternatives.Count - 1
            Dim NewResponseAlternativeLabel As New Label
            NewResponseAlternativeLabel.Text = ResponseAlternatives(n)
            NewResponseAlternativeLabel.TextAlign = ContentAlignment.MiddleCenter
            NewResponseAlternativeLabel.AutoSize = False
            NewResponseAlternativeLabel.Dock = DockStyle.Fill
            Me.Controls.Add(NewResponseAlternativeLabel, n, 2)
        Next

        RowStyles.Clear()
        ColumnStyles.Clear()
        RowStyles.Add(New RowStyle(SizeType.Absolute, QuestionTextBox.Height))
        RowStyles.Add(New RowStyle(SizeType.Absolute, 20))
        RowStyles.Add(New RowStyle(SizeType.Absolute, 20))
        For Each ResponseAlternative In ResponseAlternatives
            ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / ResponseAlternatives.Count))
        Next


    End Sub

End Class
