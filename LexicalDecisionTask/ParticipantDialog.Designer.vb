<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParticipantDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ParticipantDialog))
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.ParticipantID_Label = New System.Windows.Forms.Label()
        Me.ParticipantNr_Label = New System.Windows.Forms.Label()
        Me.Participant_ID_TextBox = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Participant_Nr_IntegerParsingTextBox = New VLDT_lib.IntegerParsingTextBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.OK_Button, 2)
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.Enabled = False
        Me.OK_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(3, 105)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(637, 35)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Continue"
        '
        'ParticipantID_Label
        '
        Me.ParticipantID_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ParticipantID_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ParticipantID_Label.Location = New System.Drawing.Point(3, 20)
        Me.ParticipantID_Label.Name = "ParticipantID_Label"
        Me.ParticipantID_Label.Size = New System.Drawing.Size(167, 31)
        Me.ParticipantID_Label.TabIndex = 1
        Me.ParticipantID_Label.Text = "Participant ID:"
        Me.ParticipantID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ParticipantNr_Label
        '
        Me.ParticipantNr_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ParticipantNr_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ParticipantNr_Label.Location = New System.Drawing.Point(3, 51)
        Me.ParticipantNr_Label.Name = "ParticipantNr_Label"
        Me.ParticipantNr_Label.Size = New System.Drawing.Size(167, 31)
        Me.ParticipantNr_Label.TabIndex = 2
        Me.ParticipantNr_Label.Text = "Participant number:"
        Me.ParticipantNr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Participant_ID_TextBox
        '
        Me.Participant_ID_TextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Participant_ID_TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Participant_ID_TextBox.Location = New System.Drawing.Point(176, 23)
        Me.Participant_ID_TextBox.Name = "Participant_ID_TextBox"
        Me.Participant_ID_TextBox.Size = New System.Drawing.Size(464, 26)
        Me.Participant_ID_TextBox.TabIndex = 3
        Me.Participant_ID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 173.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ParticipantID_Label, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.ParticipantNr_Label, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Participant_ID_TextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Participant_Nr_IntegerParsingTextBox, 1, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(643, 143)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'Participant_Nr_IntegerParsingTextBox
        '
        Me.Participant_Nr_IntegerParsingTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Participant_Nr_IntegerParsingTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Participant_Nr_IntegerParsingTextBox.ForeColor = System.Drawing.Color.Red
        Me.Participant_Nr_IntegerParsingTextBox.Location = New System.Drawing.Point(176, 54)
        Me.Participant_Nr_IntegerParsingTextBox.Name = "Participant_Nr_IntegerParsingTextBox"
        Me.Participant_Nr_IntegerParsingTextBox.Size = New System.Drawing.Size(464, 26)
        Me.Participant_Nr_IntegerParsingTextBox.TabIndex = 4
        Me.Participant_Nr_IntegerParsingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ParticipantDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(643, 143)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ParticipantDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add participant details"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents ParticipantID_Label As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ParticipantNr_Label As Label
    Friend WithEvents Participant_ID_TextBox As TextBox
    Friend WithEvents Participant_Nr_IntegerParsingTextBox As VLDT_lib.IntegerParsingTextBox
End Class
