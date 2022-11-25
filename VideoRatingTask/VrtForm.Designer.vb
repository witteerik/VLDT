<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class VrtForm
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
        Me.MainTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Content_SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.VideoViewHolder_Panel = New System.Windows.Forms.Panel()
        Me.VideoView = New LibVLCSharp.WinForms.VideoView()
        Me.Replay_Button = New System.Windows.Forms.Button()
        Me.ChangeView_Button = New System.Windows.Forms.Button()
        Me.ShowPreviousItem_Button = New VLDT_lib.ChangeItemButton()
        Me.ShowNextItem_Button = New VLDT_lib.ChangeItemButton()
        Me.ShowNextNonCompleteItem_Button = New VLDT_lib.ChangeItemButton()
        Me.Item_ProgressBar = New VLDT_lib.ProgressBarWithText()
        Me.RatingPanel = New VLDT_lib.RatingPanel()
        Me.MainTableLayoutPanel.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.Content_SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Content_SplitContainer.Panel1.SuspendLayout()
        Me.Content_SplitContainer.Panel2.SuspendLayout()
        Me.Content_SplitContainer.SuspendLayout()
        Me.VideoViewHolder_Panel.SuspendLayout()
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainTableLayoutPanel
        '
        Me.MainTableLayoutPanel.ColumnCount = 3
        Me.MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.MainTableLayoutPanel.Controls.Add(Me.TableLayoutPanel1, 0, 1)
        Me.MainTableLayoutPanel.Controls.Add(Me.ShowNextNonCompleteItem_Button, 2, 1)
        Me.MainTableLayoutPanel.Controls.Add(Me.Item_ProgressBar, 1, 2)
        Me.MainTableLayoutPanel.Controls.Add(Me.Content_SplitContainer, 1, 0)
        Me.MainTableLayoutPanel.Controls.Add(Me.Replay_Button, 0, 0)
        Me.MainTableLayoutPanel.Controls.Add(Me.ChangeView_Button, 2, 0)
        Me.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainTableLayoutPanel.Name = "MainTableLayoutPanel"
        Me.MainTableLayoutPanel.RowCount = 3
        Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainTableLayoutPanel.Size = New System.Drawing.Size(1121, 613)
        Me.MainTableLayoutPanel.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ShowPreviousItem_Button, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ShowNextItem_Button, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 33)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.MainTableLayoutPanel.SetRowSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(106, 577)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Content_SplitContainer
        '
        Me.Content_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Content_SplitContainer.Location = New System.Drawing.Point(115, 3)
        Me.Content_SplitContainer.Name = "Content_SplitContainer"
        Me.Content_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'Content_SplitContainer.Panel1
        '
        Me.Content_SplitContainer.Panel1.Controls.Add(Me.VideoViewHolder_Panel)
        '
        'Content_SplitContainer.Panel2
        '
        Me.Content_SplitContainer.Panel2.Controls.Add(Me.RatingPanel)
        Me.MainTableLayoutPanel.SetRowSpan(Me.Content_SplitContainer, 2)
        Me.Content_SplitContainer.Size = New System.Drawing.Size(890, 577)
        Me.Content_SplitContainer.SplitterDistance = 355
        Me.Content_SplitContainer.TabIndex = 0
        '
        'VideoViewHolder_Panel
        '
        Me.VideoViewHolder_Panel.BackColor = System.Drawing.Color.Black
        Me.VideoViewHolder_Panel.Controls.Add(Me.VideoView)
        Me.VideoViewHolder_Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VideoViewHolder_Panel.Location = New System.Drawing.Point(0, 0)
        Me.VideoViewHolder_Panel.Name = "VideoViewHolder_Panel"
        Me.VideoViewHolder_Panel.Size = New System.Drawing.Size(890, 355)
        Me.VideoViewHolder_Panel.TabIndex = 8
        '
        'VideoView
        '
        Me.VideoView.BackColor = System.Drawing.Color.Black
        Me.VideoView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VideoView.Location = New System.Drawing.Point(0, 0)
        Me.VideoView.Margin = New System.Windows.Forms.Padding(0)
        Me.VideoView.MediaPlayer = Nothing
        Me.VideoView.Name = "VideoView"
        Me.VideoView.Size = New System.Drawing.Size(890, 355)
        Me.VideoView.TabIndex = 6
        Me.VideoView.Text = "VideoView1"
        '
        'Replay_Button
        '
        Me.Replay_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Replay_Button.Enabled = False
        Me.Replay_Button.Location = New System.Drawing.Point(3, 3)
        Me.Replay_Button.Name = "Replay_Button"
        Me.Replay_Button.Size = New System.Drawing.Size(106, 24)
        Me.Replay_Button.TabIndex = 3
        Me.Replay_Button.Text = "Replay"
        Me.Replay_Button.UseVisualStyleBackColor = True
        '
        'ChangeView_Button
        '
        Me.ChangeView_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChangeView_Button.Location = New System.Drawing.Point(1011, 3)
        Me.ChangeView_Button.Name = "ChangeView_Button"
        Me.ChangeView_Button.Size = New System.Drawing.Size(107, 24)
        Me.ChangeView_Button.TabIndex = 4
        Me.ChangeView_Button.Text = "Change view"
        Me.ChangeView_Button.UseVisualStyleBackColor = True
        '
        'ShowPreviousItem_Button
        '
        Me.ShowPreviousItem_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShowPreviousItem_Button.Enabled = False
        Me.ShowPreviousItem_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowPreviousItem_Button.ForeColor = System.Drawing.Color.White
        Me.ShowPreviousItem_Button.Location = New System.Drawing.Point(3, 291)
        Me.ShowPreviousItem_Button.Name = "ShowPreviousItem_Button"
        Me.ShowPreviousItem_Button.ShowText = True
        Me.ShowPreviousItem_Button.Size = New System.Drawing.Size(100, 283)
        Me.ShowPreviousItem_Button.TabIndex = 2
        Me.ShowPreviousItem_Button.Text = "-1"
        Me.ShowPreviousItem_Button.UseVisualStyleBackColor = True
        Me.ShowPreviousItem_Button.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.Previous
        '
        'ShowNextItem_Button
        '
        Me.ShowNextItem_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShowNextItem_Button.Enabled = False
        Me.ShowNextItem_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowNextItem_Button.ForeColor = System.Drawing.Color.White
        Me.ShowNextItem_Button.Location = New System.Drawing.Point(3, 3)
        Me.ShowNextItem_Button.Name = "ShowNextItem_Button"
        Me.ShowNextItem_Button.ShowText = True
        Me.ShowNextItem_Button.Size = New System.Drawing.Size(100, 282)
        Me.ShowNextItem_Button.TabIndex = 1
        Me.ShowNextItem_Button.Text = "+1"
        Me.ShowNextItem_Button.UseVisualStyleBackColor = True
        Me.ShowNextItem_Button.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.[Next]
        '
        'ShowNextNonCompleteItem_Button
        '
        Me.ShowNextNonCompleteItem_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShowNextNonCompleteItem_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShowNextNonCompleteItem_Button.ForeColor = System.Drawing.Color.White
        Me.ShowNextNonCompleteItem_Button.Location = New System.Drawing.Point(1011, 33)
        Me.ShowNextNonCompleteItem_Button.Name = "ShowNextNonCompleteItem_Button"
        Me.MainTableLayoutPanel.SetRowSpan(Me.ShowNextNonCompleteItem_Button, 2)
        Me.ShowNextNonCompleteItem_Button.ShowText = True
        Me.ShowNextNonCompleteItem_Button.Size = New System.Drawing.Size(107, 577)
        Me.ShowNextNonCompleteItem_Button.TabIndex = 0
        Me.ShowNextNonCompleteItem_Button.Text = "Next"
        Me.ShowNextNonCompleteItem_Button.UseVisualStyleBackColor = True
        Me.ShowNextNonCompleteItem_Button.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.[Next]
        '
        'Item_ProgressBar
        '
        Me.Item_ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Item_ProgressBar.Location = New System.Drawing.Point(115, 586)
        Me.Item_ProgressBar.Name = "Item_ProgressBar"
        Me.Item_ProgressBar.ShowProgressText = True
        Me.Item_ProgressBar.Size = New System.Drawing.Size(890, 24)
        Me.Item_ProgressBar.Step = 1
        Me.Item_ProgressBar.TabIndex = 2
        Me.Item_ProgressBar.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        '
        'RatingPanel
        '
        Me.RatingPanel.AutoScroll = True
        Me.RatingPanel.ColumnCount = 1
        Me.RatingPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.RatingPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RatingPanel.Location = New System.Drawing.Point(0, 0)
        Me.RatingPanel.Name = "RatingPanel"
        Me.RatingPanel.RowCount = 1
        Me.RatingPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.RatingPanel.Size = New System.Drawing.Size(890, 218)
        Me.RatingPanel.TabIndex = 0
        '
        'VrtForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1121, 613)
        Me.Controls.Add(Me.MainTableLayoutPanel)
        Me.Name = "VrtForm"
        Me.Text = "Video-Based Rating Task"
        Me.MainTableLayoutPanel.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Content_SplitContainer.Panel1.ResumeLayout(False)
        Me.Content_SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.Content_SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Content_SplitContainer.ResumeLayout(False)
        Me.VideoViewHolder_Panel.ResumeLayout(False)
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainTableLayoutPanel As TableLayoutPanel
    Friend WithEvents ShowNextNonCompleteItem_Button As VLDT_lib.ChangeItemButton
    Friend WithEvents ShowNextItem_Button As VLDT_lib.ChangeItemButton
    Friend WithEvents Item_ProgressBar As VLDT_lib.ProgressBarWithText
    Friend WithEvents VideoViewHolder_Panel As Panel
    Friend WithEvents VideoView As LibVLCSharp.WinForms.VideoView
    Friend WithEvents Content_SplitContainer As SplitContainer
    Friend WithEvents Replay_Button As Button
    Friend WithEvents ChangeView_Button As Button
    Friend WithEvents RatingPanel As VLDT_lib.RatingPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ShowPreviousItem_Button As VLDT_lib.ChangeItemButton
End Class
