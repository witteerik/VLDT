Imports System.Windows.Forms

Public Class PractiseScoreDialog

    Private Sub SKIP_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SKIP_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub REDO_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles REDO_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub SetScore(ByVal ProportionCorrect As Double)

        Score_Label.Text = Math.Round(100 * ProportionCorrect, 0) & " %"

    End Sub

    Private Sub PractiseScoreDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.PractiseTitle)

        Info1_Label.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.PractiseText1)
        Info3_Label.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.PractiseText2)

        REDO_Button.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.PractiseREDO)
        SKIP_Button.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.PractiseSKIP)

    End Sub
End Class
