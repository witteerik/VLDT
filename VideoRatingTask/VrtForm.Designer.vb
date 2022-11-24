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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.VideoViewHolder_Panel = New System.Windows.Forms.Panel()
        Me.VideoView = New LibVLCSharp.WinForms.VideoView()
        Me.RatingItems_TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.ChangeItemButton1 = New VLDT_lib.ChangeItemButton()
        Me.ChangeItemButton2 = New VLDT_lib.ChangeItemButton()
        Me.ProgressBarWithText1 = New VLDT_lib.ProgressBarWithText()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.VideoViewHolder_Panel.SuspendLayout()
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainer1, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ChangeItemButton1, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ChangeItemButton2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ProgressBarWithText1, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1121, 613)
        Me.TableLayoutPanel1.TabIndex = 0
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
        'RatingItems_TableLayoutPanel
        '
        Me.RatingItems_TableLayoutPanel.AutoScroll = True
        Me.RatingItems_TableLayoutPanel.ColumnCount = 1
        Me.RatingItems_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.RatingItems_TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.RatingItems_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RatingItems_TableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.RatingItems_TableLayoutPanel.Name = "RatingItems_TableLayoutPanel"
        Me.RatingItems_TableLayoutPanel.RowCount = 1
        Me.RatingItems_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.RatingItems_TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.RatingItems_TableLayoutPanel.Size = New System.Drawing.Size(890, 218)
        Me.RatingItems_TableLayoutPanel.TabIndex = 9
        '
        'ChangeItemButton1
        '
        Me.ChangeItemButton1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChangeItemButton1.Location = New System.Drawing.Point(1011, 3)
        Me.ChangeItemButton1.Name = "ChangeItemButton1"
        Me.ChangeItemButton1.Size = New System.Drawing.Size(107, 577)
        Me.ChangeItemButton1.TabIndex = 0
        Me.ChangeItemButton1.UseVisualStyleBackColor = True
        Me.ChangeItemButton1.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.[Next]
        '
        'ChangeItemButton2
        '
        Me.ChangeItemButton2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChangeItemButton2.Location = New System.Drawing.Point(3, 3)
        Me.ChangeItemButton2.Name = "ChangeItemButton2"
        Me.ChangeItemButton2.Size = New System.Drawing.Size(106, 577)
        Me.ChangeItemButton2.TabIndex = 1
        Me.ChangeItemButton2.UseVisualStyleBackColor = True
        Me.ChangeItemButton2.ViewMode = VLDT_lib.ChangeItemButton.ViewModes.Previous
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
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(115, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.VideoViewHolder_Panel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.RatingItems_TableLayoutPanel)
        Me.SplitContainer1.Size = New System.Drawing.Size(890, 577)
        Me.SplitContainer1.SplitterDistance = 355
        Me.SplitContainer1.TabIndex = 0
        '
        'VrtForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1121, 613)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "VrtForm"
        Me.Text = "Video-Based Rating Task"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.VideoViewHolder_Panel.ResumeLayout(False)
        CType(Me.VideoView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ChangeItemButton1 As VLDT_lib.ChangeItemButton
    Friend WithEvents ChangeItemButton2 As VLDT_lib.ChangeItemButton
    Friend WithEvents ProgressBarWithText1 As VLDT_lib.ProgressBarWithText
    Friend WithEvents VideoViewHolder_Panel As Panel
    Friend WithEvents VideoView As LibVLCSharp.WinForms.VideoView
    Friend WithEvents RatingItems_TableLayoutPanel As TableLayoutPanel
    Friend WithEvents SplitContainer1 As SplitContainer
End Class
