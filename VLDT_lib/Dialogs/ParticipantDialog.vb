Imports VLDT_lib

Public Class ParticipantDialog

    Public ParticipantNumber As Integer?
    Public ParticipantID As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Participant_ID_TextBox_TextChanged(sender As Object, e As EventArgs) Handles Participant_ID_TextBox.TextChanged

        If Participant_ID_TextBox.Text.Contains(" ") Then
            Participant_ID_TextBox.ForeColor = Drawing.Color.Red
        Else
            Participant_ID_TextBox.ForeColor = Drawing.Color.Black
        End If

        ParticipantID = Participant_ID_TextBox.Text

        UpdateOKButton()

    End Sub

    Public Sub UpdateOKButton() Handles Participant_Nr_IntegerParsingTextBox.ValueUpdated

        ParticipantNumber = Participant_Nr_IntegerParsingTextBox.Value

        If ParticipantID <> "" And (Not Participant_ID_TextBox.Text.Contains(" ")) And ParticipantNumber.HasValue = True Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If

    End Sub

    Private Sub ParticipantDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Select Case Utils.CurrentGuiStringType
            Case GetType(Utils.GuiStrings.VldtGuiStringKeys)
                Me.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.AppPtcTitle)
                ParticipantID_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.AppPtcID)
                ParticipantNr_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.AppPtcNr)
                OK_Button.Text = Utils.GetGuiString(Utils.GuiStrings.VldtGuiStringKeys.AppPtcOK)

            Case GetType(Utils.GuiStrings.VrtGuiStringKeys)
                Me.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.AppPtcTitle)
                ParticipantID_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.AppPtcID)
                ParticipantNr_Label.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.AppPtcNr)
                OK_Button.Text = Utils.GetGuiString(Utils.GuiStrings.VrtGuiStringKeys.AppPtcOK)

        End Select



    End Sub
End Class


