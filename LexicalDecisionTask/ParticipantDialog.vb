Imports System.Windows.Forms
Imports System.Globalization


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



''' <summary>
''' Gets an integer value from user input text. Red text color indicates invalid value, and default color indicates valid value. 
''' Valid values should be retrieved from the property Value.
''' </summary>
Public Class IntegerParsingTextBox
    Inherits TextBox

    Private _Value As Integer? = Nothing
    Public ReadOnly Property Value As Integer?
        Get
            Return _Value
        End Get
    End Property

    Private DefaultTextColor As Drawing.Color

    Public Event ValueUpdated()

    Public Sub New()

        'Stores the default color
        DefaultTextColor = Me.ForeColor

    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)

        'Tries to parse the text as a valid Integer
        Dim ParsedValue As Integer

        If Integer.TryParse(Me.Text.Replace(",", "."), NumberStyles.Integer, CultureInfo.InvariantCulture, ParsedValue) = True Then
            If ParsedValue > 0 Then
                Me._Value = ParsedValue
                Me.ForeColor = DefaultTextColor
            Else
                Me._Value = Nothing
                Me.ForeColor = Drawing.Color.Red
            End If
        Else
            Me._Value = Nothing
            Me.ForeColor = Drawing.Color.Red
        End If

        RaiseEvent ValueUpdated()

    End Sub

End Class
