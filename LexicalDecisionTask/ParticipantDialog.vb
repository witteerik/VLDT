Imports VLDT_lib

Public Class ParticipantDialog

    Public ParticipantNumber As Integer?
    Public ParticipantID As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Participant_ID_TextBox_TextChanged(sender As Object, e As EventArgs) Handles Participant_ID_TextBox.TextChanged

        ParticipantID = Participant_ID_TextBox.Text

        UpdateOKButton()

    End Sub

    Public Sub UpdateOKButton() Handles Participant_Nr_IntegerParsingTextBox.ValueUpdated

        ParticipantNumber = Participant_Nr_IntegerParsingTextBox.Value

        If ParticipantID <> "" And ParticipantNumber.HasValue = True Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If

    End Sub

    Private Sub Participant_Nr_IntegerParsingTextBox_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ParticipantDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.AppPtcTitle)
        ParticipantID_Label.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.AppPtcID)
        ParticipantNr_Label.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.AppPtcNr)
        OK_Button.Text = Utils.GetGuiString(Utils.GuiStrings.GuiStringKeys.AppPtcOK)

    End Sub
End Class


