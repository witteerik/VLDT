<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PractiseScoreDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PractiseScoreDialog))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.REDO_Button = New System.Windows.Forms.Button()
        Me.SKIP_Button = New System.Windows.Forms.Button()
        Me.Info1_Label = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Score_Label = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.REDO_Button, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.SKIP_Button, 2, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Info1_Label, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Score_Label, 1, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(435, 246)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'REDO_Button
        '
        Me.REDO_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.REDO_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.REDO_Button.ForeColor = System.Drawing.Color.Green
        Me.REDO_Button.Location = New System.Drawing.Point(13, 178)
        Me.REDO_Button.Name = "REDO_Button"
        Me.REDO_Button.Size = New System.Drawing.Size(201, 54)
        Me.REDO_Button.TabIndex = 0
        Me.REDO_Button.Text = "Redo practise test"
        '
        'SKIP_Button
        '
        Me.SKIP_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.SKIP_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SKIP_Button.ForeColor = System.Drawing.Color.Red
        Me.SKIP_Button.Location = New System.Drawing.Point(220, 178)
        Me.SKIP_Button.Name = "SKIP_Button"
        Me.SKIP_Button.Size = New System.Drawing.Size(201, 54)
        Me.SKIP_Button.TabIndex = 1
        Me.SKIP_Button.Text = "Skip to real test anyway"
        '
        'Info1_Label
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Info1_Label, 2)
        Me.Info1_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Info1_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Info1_Label.Location = New System.Drawing.Point(13, 10)
        Me.Info1_Label.Name = "Info1_Label"
        Me.Info1_Label.Size = New System.Drawing.Size(408, 31)
        Me.Info1_Label.TabIndex = 2
        Me.Info1_Label.Text = "Your practise score of..."
        Me.Info1_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label1, 2)
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 134)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(408, 31)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "...is too low to start the real test!"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Score_Label
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Score_Label, 2)
        Me.Score_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Score_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 40.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Score_Label.ForeColor = System.Drawing.Color.Red
        Me.Score_Label.Location = New System.Drawing.Point(13, 41)
        Me.Score_Label.Name = "Score_Label"
        Me.Score_Label.Size = New System.Drawing.Size(408, 93)
        Me.Score_Label.TabIndex = 4
        Me.Score_Label.Text = "XX %"
        Me.Score_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PractiseScoreDialog
        '
        Me.AcceptButton = Me.REDO_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.SKIP_Button
        Me.ClientSize = New System.Drawing.Size(435, 246)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PractiseScoreDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Practise score"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents REDO_Button As System.Windows.Forms.Button
    Friend WithEvents SKIP_Button As System.Windows.Forms.Button
    Friend WithEvents Info1_Label As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Score_Label As Label
End Class
