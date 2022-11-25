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
        Me.Content_SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.VideoViewHolder_Panel = New System.Windows.Forms.Panel()
        Me.VideoView = New LibVLCSharp.WinForms.VideoView()
        Me.Replay_Button = New System.Windows.Forms.Button()
        Me.ChangeView_Button = New System.Windows.Forms.Button()
        Me.ShowNextItem_Button = New VLDT_lib.ChangeItemButton()
        Me.ProgressBarWithText1 = New VLDT_lib.ProgressBarWithText()
        Me.RatingPanel = New VLDT_lib.RatingPanel()
        Me.ShowLastItem_Button = New VLDT_lib.ChangeItemButton()
        Me.MainTableLayoutPanel.SuspendLayout()
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
        Me.MainTableLayoutPanel.Controls.Add(Me.ShowNextItem_Button, 2, 1)
        Me.MainTableLayoutPanel.Controls.Add(Me.ProgressBarWithText1, 1, 2)
        Me.MainTableLayoutPanel.Controls.Add(Me.Content_SplitContainer, 1, 0)
        Me.MainTableLayoutPanel.Controls.Add(Me.ShowLastItem_Button, 0, 1)
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
        'ShowNextItem_Button
        '
        Me.ShowNextItem_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShowNextItem_Button.Location = New System.Drawing.Point(1011, 33)
        Me.ShowNextItem_Button.Name = "ShowNextItem_Button"
        Me.MainTableLayoutPanel.SetRowSpan(Me.ShowNextItem_Button, 2)
        Me.ShowNextItem_Button.Size = New System.Drawing.Size(107, 577)
        Me.ShowNextItem_Button.TabIndex = 0
        Me.ShowNextItem_Button.UseVisualStyleBackColor = True
        Me.ShowNextItem_Button.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.[Next]
        '
        'ProgressBarWithText1
        '
        Me.ProgressBarWithText1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBarWithText1.Location = New System.Drawing.Point(115, 586)
        Me.ProgressBarWithText1.Name = "ProgressBarWithText1"
        Me.ProgressBarWithText1.ShowProgressText = True
        Me.ProgressBarWithText1.Size = New System.Drawing.Size(890, 24)
        Me.ProgressBarWithText1.TabIndex = 2
        Me.ProgressBarWithText1.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
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
        'ShowLastItem_Button
        '
        Me.ShowLastItem_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShowLastItem_Button.Enabled = False
        Me.ShowLastItem_Button.Location = New System.Drawing.Point(3, 33)
        Me.ShowLastItem_Button.Name = "ShowLastItem_Button"
        Me.MainTableLayoutPanel.SetRowSpan(Me.ShowLastItem_Button, 2)
        Me.ShowLastItem_Button.Size = New System.Drawing.Size(106, 577)
        Me.ShowLastItem_Button.TabIndex = 1
        Me.ShowLastItem_Button.UseVisualStyleBackColor = True
        Me.ShowLastItem_Button.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.Previous
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
        Me.Content_SplitContainer.Panel1.ResumeLayout(False)
        Me.Content_SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.Content_SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Content_SplitContainer.ResumeLayout(False)
        Me.VideoViewHolder_Panel.ResumeLayout(False)
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainTableLayoutPanel As TableLayoutPanel
    Friend WithEvents ShowNextItem_Button As VLDT_lib.ChangeItemButton
    Friend WithEvents ShowLastItem_Button As VLDT_lib.ChangeItemButton
    Friend WithEvents ProgressBarWithText1 As VLDT_lib.ProgressBarWithText
    Friend WithEvents VideoViewHolder_Panel As Panel
    Friend WithEvents VideoView As LibVLCSharp.WinForms.VideoView
    Friend WithEvents Content_SplitContainer As SplitContainer
    Friend WithEvents Replay_Button As Button
    Friend WithEvents ChangeView_Button As Button
    Friend WithEvents RatingPanel As VLDT_lib.RatingPanel
End Class
