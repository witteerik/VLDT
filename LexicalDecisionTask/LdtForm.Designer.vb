<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LdtForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LdtForm))
        Me.Background_TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Content_SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.VideoViewHolder_Panel = New System.Windows.Forms.Panel()
        Me.VideoView = New LibVLCSharp.WinForms.VideoView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Info_SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.Info_RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.Instructions_Label = New System.Windows.Forms.Label()
        Me.Block_ProgressBar = New VLDT_lib.ProgressBarWithText()
        Me.LeftResponseLetter_Label = New System.Windows.Forms.Label()
        Me.RightResponseLetter_Label = New System.Windows.Forms.Label()
        Me.Background_TableLayoutPanel.SuspendLayout()
        CType(Me.Content_SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Content_SplitContainer.Panel1.SuspendLayout()
        Me.Content_SplitContainer.Panel2.SuspendLayout()
        Me.Content_SplitContainer.SuspendLayout()
        Me.VideoViewHolder_Panel.SuspendLayout()
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.Info_SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Info_SplitContainer.Panel1.SuspendLayout()
        Me.Info_SplitContainer.Panel2.SuspendLayout()
        Me.Info_SplitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'Background_TableLayoutPanel
        '
        Me.Background_TableLayoutPanel.BackColor = System.Drawing.SystemColors.Control
        Me.Background_TableLayoutPanel.ColumnCount = 3
        Me.Background_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.0!))
        Me.Background_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.0!))
        Me.Background_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.0!))
        Me.Background_TableLayoutPanel.Controls.Add(Me.Content_SplitContainer, 1, 1)
        Me.Background_TableLayoutPanel.Controls.Add(Me.LeftResponseLetter_Label, 0, 2)
        Me.Background_TableLayoutPanel.Controls.Add(Me.RightResponseLetter_Label, 2, 2)
        Me.Background_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Background_TableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.Background_TableLayoutPanel.Name = "Background_TableLayoutPanel"
        Me.Background_TableLayoutPanel.RowCount = 3
        Me.Background_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.0!))
        Me.Background_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.Background_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.0!))
        Me.Background_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.Background_TableLayoutPanel.Size = New System.Drawing.Size(1233, 616)
        Me.Background_TableLayoutPanel.TabIndex = 1
        '
        'Content_SplitContainer
        '
        Me.Content_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Content_SplitContainer.Location = New System.Drawing.Point(39, 33)
        Me.Content_SplitContainer.Name = "Content_SplitContainer"
        Me.Content_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'Content_SplitContainer.Panel1
        '
        Me.Content_SplitContainer.Panel1.Controls.Add(Me.VideoViewHolder_Panel)
        '
        'Content_SplitContainer.Panel2
        '
        Me.Content_SplitContainer.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Content_SplitContainer.Size = New System.Drawing.Size(1153, 548)
        Me.Content_SplitContainer.SplitterDistance = 281
        Me.Content_SplitContainer.TabIndex = 4
        '
        'VideoViewHolder_Panel
        '
        Me.VideoViewHolder_Panel.BackColor = System.Drawing.Color.Black
        Me.VideoViewHolder_Panel.Controls.Add(Me.VideoView)
        Me.VideoViewHolder_Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VideoViewHolder_Panel.Location = New System.Drawing.Point(0, 0)
        Me.VideoViewHolder_Panel.Name = "VideoViewHolder_Panel"
        Me.VideoViewHolder_Panel.Size = New System.Drawing.Size(1153, 281)
        Me.VideoViewHolder_Panel.TabIndex = 7
        '
        'VideoView
        '
        Me.VideoView.BackColor = System.Drawing.Color.Black
        Me.VideoView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VideoView.Location = New System.Drawing.Point(0, 0)
        Me.VideoView.Margin = New System.Windows.Forms.Padding(0)
        Me.VideoView.MediaPlayer = Nothing
        Me.VideoView.Name = "VideoView"
        Me.VideoView.Size = New System.Drawing.Size(1153, 281)
        Me.VideoView.TabIndex = 6
        Me.VideoView.Text = "VideoView1"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Info_SplitContainer, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Block_ProgressBar, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1153, 263)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'Info_SplitContainer
        '
        Me.Info_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Info_SplitContainer.Location = New System.Drawing.Point(3, 3)
        Me.Info_SplitContainer.Name = "Info_SplitContainer"
        Me.Info_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'Info_SplitContainer.Panel1
        '
        Me.Info_SplitContainer.Panel1.Controls.Add(Me.Info_RichTextBox)
        '
        'Info_SplitContainer.Panel2
        '
        Me.Info_SplitContainer.Panel2.Controls.Add(Me.Instructions_Label)
        Me.Info_SplitContainer.Size = New System.Drawing.Size(1147, 227)
        Me.Info_SplitContainer.SplitterDistance = 182
        Me.Info_SplitContainer.TabIndex = 5
        Me.Info_SplitContainer.TabStop = False
        Me.Info_SplitContainer.Visible = False
        '
        'Info_RichTextBox
        '
        Me.Info_RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Info_RichTextBox.Location = New System.Drawing.Point(0, 0)
        Me.Info_RichTextBox.Name = "Info_RichTextBox"
        Me.Info_RichTextBox.ReadOnly = True
        Me.Info_RichTextBox.Size = New System.Drawing.Size(1147, 182)
        Me.Info_RichTextBox.TabIndex = 3
        Me.Info_RichTextBox.Text = ""
        '
        'Instructions_Label
        '
        Me.Instructions_Label.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Instructions_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Instructions_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Instructions_Label.Location = New System.Drawing.Point(0, 0)
        Me.Instructions_Label.Name = "Instructions_Label"
        Me.Instructions_Label.Size = New System.Drawing.Size(1147, 41)
        Me.Instructions_Label.TabIndex = 0
        Me.Instructions_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Block_ProgressBar
        '
        Me.Block_ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Block_ProgressBar.ForeColor = System.Drawing.Color.Red
        Me.Block_ProgressBar.Location = New System.Drawing.Point(3, 236)
        Me.Block_ProgressBar.Name = "Block_ProgressBar"
        Me.Block_ProgressBar.ShowProgressText = True
        Me.Block_ProgressBar.Size = New System.Drawing.Size(1147, 24)
        Me.Block_ProgressBar.Step = 1
        Me.Block_ProgressBar.TabIndex = 6
        Me.Block_ProgressBar.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Block_ProgressBar.Value = 60
        Me.Block_ProgressBar.Visible = False
        '
        'LeftResponseLetter_Label
        '
        Me.LeftResponseLetter_Label.BackColor = System.Drawing.SystemColors.Control
        Me.LeftResponseLetter_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LeftResponseLetter_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LeftResponseLetter_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LeftResponseLetter_Label.ForeColor = System.Drawing.Color.White
        Me.LeftResponseLetter_Label.Location = New System.Drawing.Point(3, 587)
        Me.LeftResponseLetter_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.LeftResponseLetter_Label.Name = "LeftResponseLetter_Label"
        Me.LeftResponseLetter_Label.Size = New System.Drawing.Size(30, 26)
        Me.LeftResponseLetter_Label.TabIndex = 5
        Me.LeftResponseLetter_Label.Text = "Label1"
        Me.LeftResponseLetter_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LeftResponseLetter_Label.Visible = False
        '
        'RightResponseLetter_Label
        '
        Me.RightResponseLetter_Label.BackColor = System.Drawing.SystemColors.Control
        Me.RightResponseLetter_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RightResponseLetter_Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RightResponseLetter_Label.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RightResponseLetter_Label.ForeColor = System.Drawing.Color.White
        Me.RightResponseLetter_Label.Location = New System.Drawing.Point(1198, 587)
        Me.RightResponseLetter_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.RightResponseLetter_Label.Name = "RightResponseLetter_Label"
        Me.RightResponseLetter_Label.Size = New System.Drawing.Size(32, 26)
        Me.RightResponseLetter_Label.TabIndex = 6
        Me.RightResponseLetter_Label.Text = "Label2"
        Me.RightResponseLetter_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.RightResponseLetter_Label.Visible = False
        '
        'LdtForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1233, 616)
        Me.Controls.Add(Me.Background_TableLayoutPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "LdtForm"
        Me.Text = "Video-Based Lexical Decision Task (VLDT)"
        Me.Background_TableLayoutPanel.ResumeLayout(False)
        Me.Content_SplitContainer.Panel1.ResumeLayout(False)
        Me.Content_SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.Content_SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Content_SplitContainer.ResumeLayout(False)
        Me.VideoViewHolder_Panel.ResumeLayout(False)
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Info_SplitContainer.Panel1.ResumeLayout(False)
        Me.Info_SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.Info_SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Info_SplitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Background_TableLayoutPanel As TableLayoutPanel
    Friend WithEvents Content_SplitContainer As SplitContainer
    Friend WithEvents VideoView As LibVLCSharp.WinForms.VideoView
    Friend WithEvents VideoViewHolder_Panel As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Info_RichTextBox As RichTextBox
    Friend WithEvents Info_SplitContainer As SplitContainer
    Friend WithEvents Block_ProgressBar As VLDT_lib.ProgressBarWithText
    Friend WithEvents LeftResponseLetter_Label As Label
    Friend WithEvents RightResponseLetter_Label As Label
    Friend WithEvents Instructions_Label As Label
End Class
