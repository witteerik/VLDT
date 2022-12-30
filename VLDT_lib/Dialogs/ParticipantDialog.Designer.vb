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
        Me.SkipBlocks_CheckBox = New System.Windows.Forms.CheckBox()
        Me.SkipBlock_TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.SkipBlock_Label = New System.Windows.Forms.Label()
        Me.SkipBlockCounts_ComboBox = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SkipBlock_TableLayoutPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.OK_Button, 2)
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.Enabled = False
        Me.OK_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(3, 139)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(633, 34)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Continue"
        '
        'ParticipantID_Label
        '
        Me.ParticipantID_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ParticipantID_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ParticipantID_Label.Location = New System.Drawing.Point(3, 20)
        Me.ParticipantID_Label.Name = "ParticipantID_Label"
        Me.ParticipantID_Label.Size = New System.Drawing.Size(167, 32)
        Me.ParticipantID_Label.TabIndex = 1
        Me.ParticipantID_Label.Text = "Participant ID:"
        Me.ParticipantID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ParticipantNr_Label
        '
        Me.ParticipantNr_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ParticipantNr_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ParticipantNr_Label.Location = New System.Drawing.Point(3, 52)
        Me.ParticipantNr_Label.Name = "ParticipantNr_Label"
        Me.ParticipantNr_Label.Size = New System.Drawing.Size(167, 32)
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
        Me.Participant_ID_TextBox.Size = New System.Drawing.Size(460, 26)
        Me.Participant_ID_TextBox.TabIndex = 3
        Me.Participant_ID_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 173.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ParticipantID_Label, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.ParticipantNr_Label, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Participant_ID_TextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Participant_Nr_IntegerParsingTextBox, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SkipBlocks_CheckBox, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.SkipBlock_TableLayoutPanel, 1, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(639, 176)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'Participant_Nr_IntegerParsingTextBox
        '
        Me.Participant_Nr_IntegerParsingTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Participant_Nr_IntegerParsingTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Participant_Nr_IntegerParsingTextBox.ForeColor = System.Drawing.Color.Red
        Me.Participant_Nr_IntegerParsingTextBox.Location = New System.Drawing.Point(176, 55)
        Me.Participant_Nr_IntegerParsingTextBox.Name = "Participant_Nr_IntegerParsingTextBox"
        Me.Participant_Nr_IntegerParsingTextBox.Size = New System.Drawing.Size(460, 26)
        Me.Participant_Nr_IntegerParsingTextBox.TabIndex = 4
        Me.Participant_Nr_IntegerParsingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SkipBlocks_CheckBox
        '
        Me.SkipBlocks_CheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SkipBlocks_CheckBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SkipBlocks_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SkipBlocks_CheckBox.Location = New System.Drawing.Point(3, 87)
        Me.SkipBlocks_CheckBox.Name = "SkipBlocks_CheckBox"
        Me.SkipBlocks_CheckBox.Size = New System.Drawing.Size(167, 26)
        Me.SkipBlocks_CheckBox.TabIndex = 5
        Me.SkipBlocks_CheckBox.Text = "Skip blocks"
        Me.SkipBlocks_CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SkipBlocks_CheckBox.UseVisualStyleBackColor = True
        '
        'SkipBlock_TableLayoutPanel
        '
        Me.SkipBlock_TableLayoutPanel.ColumnCount = 2
        Me.SkipBlock_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.SkipBlock_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88.0!))
        Me.SkipBlock_TableLayoutPanel.Controls.Add(Me.SkipBlock_Label, 0, 0)
        Me.SkipBlock_TableLayoutPanel.Controls.Add(Me.SkipBlockCounts_ComboBox, 1, 0)
        Me.SkipBlock_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SkipBlock_TableLayoutPanel.Location = New System.Drawing.Point(173, 84)
        Me.SkipBlock_TableLayoutPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.SkipBlock_TableLayoutPanel.Name = "SkipBlock_TableLayoutPanel"
        Me.SkipBlock_TableLayoutPanel.RowCount = 1
        Me.SkipBlock_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.SkipBlock_TableLayoutPanel.Size = New System.Drawing.Size(466, 32)
        Me.SkipBlock_TableLayoutPanel.TabIndex = 6
        Me.SkipBlock_TableLayoutPanel.Visible = False
        '
        'SkipBlock_Label
        '
        Me.SkipBlock_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SkipBlock_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SkipBlock_Label.Location = New System.Drawing.Point(3, 0)
        Me.SkipBlock_Label.Name = "SkipBlock_Label"
        Me.SkipBlock_Label.Size = New System.Drawing.Size(372, 32)
        Me.SkipBlock_Label.TabIndex = 0
        Me.SkipBlock_Label.Text = "Select number of blocks to skip:"
        Me.SkipBlock_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'SkipBlockCounts_ComboBox
        '
        Me.SkipBlockCounts_ComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SkipBlockCounts_ComboBox.FormattingEnabled = True
        Me.SkipBlockCounts_ComboBox.Location = New System.Drawing.Point(381, 4)
        Me.SkipBlockCounts_ComboBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 3)
        Me.SkipBlockCounts_ComboBox.Name = "SkipBlockCounts_ComboBox"
        Me.SkipBlockCounts_ComboBox.Size = New System.Drawing.Size(82, 24)
        Me.SkipBlockCounts_ComboBox.TabIndex = 1
        '
        'ParticipantDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(639, 176)
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
        Me.SkipBlock_TableLayoutPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents ParticipantID_Label As Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As Windows.Forms.TableLayoutPanel
    Friend WithEvents ParticipantNr_Label As Windows.Forms.Label
    Friend WithEvents Participant_ID_TextBox As Windows.Forms.TextBox
    Friend WithEvents Participant_Nr_IntegerParsingTextBox As VLDT_lib.IntegerParsingTextBox
    Friend WithEvents SkipBlocks_CheckBox As Windows.Forms.CheckBox
    Friend WithEvents SkipBlock_TableLayoutPanel As Windows.Forms.TableLayoutPanel
    Friend WithEvents SkipBlock_Label As Windows.Forms.Label
    Friend WithEvents SkipBlockCounts_ComboBox As Windows.Forms.ComboBox
End Class
