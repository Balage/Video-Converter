namespace VideoConverter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            OutputList = new ListBox();
            ArgsPanel = new Panel();
            ImageSequenceGroup = new GroupBox();
            PNGColorButton = new Button();
            label3 = new Label();
            ImageFPSNumStepper = new NumericUpDown();
            label2 = new Label();
            FrameGenNumStepper = new NumericUpDown();
            label1 = new Label();
            groupBox3 = new GroupBox();
            lbFixSize = new Label();
            FixSizeNumStepper = new NumericUpDown();
            CRFLabel = new Label();
            QualityFixSizeRadioButton = new RadioButton();
            CRFTrackBar = new TrackBar();
            QualityCRFRadioButton = new RadioButton();
            groupBox4 = new GroupBox();
            DropAudioCheckBox = new CheckBox();
            Downscale4kCheckBox = new CheckBox();
            groupBox1 = new GroupBox();
            SkipSameCodecCheckBox = new CheckBox();
            Codec265RadioButton = new RadioButton();
            Codec264RadioButton = new RadioButton();
            TopMenu = new MenuStrip();
            TerminateButton = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            VerboseOutputMenuItem = new ToolStripMenuItem();
            AddToStartMenuMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            TopPanel = new Panel();
            TabsPanel = new Panel();
            ImagesToVideoTab = new RadioButton();
            ConvertVideoTab = new RadioButton();
            ProbeTab = new RadioButton();
            MenuPanel = new Panel();
            BodyPanel = new Panel();
            SeparatorLabel = new Label();
            ArgsPanel.SuspendLayout();
            ImageSequenceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImageFPSNumStepper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrameGenNumStepper).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FixSizeNumStepper).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CRFTrackBar).BeginInit();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            TopMenu.SuspendLayout();
            TopPanel.SuspendLayout();
            TabsPanel.SuspendLayout();
            BodyPanel.SuspendLayout();
            SuspendLayout();
            // 
            // OutputList
            // 
            OutputList.Dock = DockStyle.Fill;
            OutputList.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OutputList.FormattingEnabled = true;
            OutputList.IntegralHeight = false;
            OutputList.Location = new Point(8, 8);
            OutputList.Name = "OutputList";
            OutputList.Size = new Size(501, 498);
            OutputList.TabIndex = 0;
            OutputList.TabStop = false;
            // 
            // ArgsPanel
            // 
            ArgsPanel.BackColor = SystemColors.Control;
            ArgsPanel.Controls.Add(ImageSequenceGroup);
            ArgsPanel.Controls.Add(groupBox3);
            ArgsPanel.Controls.Add(groupBox4);
            ArgsPanel.Controls.Add(groupBox1);
            ArgsPanel.Dock = DockStyle.Right;
            ArgsPanel.Location = new Point(509, 8);
            ArgsPanel.Margin = new Padding(8, 3, 3, 3);
            ArgsPanel.Name = "ArgsPanel";
            ArgsPanel.Padding = new Padding(8, 0, 0, 0);
            ArgsPanel.Size = new Size(260, 498);
            ArgsPanel.TabIndex = 2;
            // 
            // ImageSequenceGroup
            // 
            ImageSequenceGroup.Controls.Add(PNGColorButton);
            ImageSequenceGroup.Controls.Add(label3);
            ImageSequenceGroup.Controls.Add(ImageFPSNumStepper);
            ImageSequenceGroup.Controls.Add(label2);
            ImageSequenceGroup.Controls.Add(FrameGenNumStepper);
            ImageSequenceGroup.Controls.Add(label1);
            ImageSequenceGroup.Dock = DockStyle.Top;
            ImageSequenceGroup.Location = new Point(8, 319);
            ImageSequenceGroup.Name = "ImageSequenceGroup";
            ImageSequenceGroup.Size = new Size(252, 115);
            ImageSequenceGroup.TabIndex = 4;
            ImageSequenceGroup.TabStop = false;
            ImageSequenceGroup.Text = "Image Sequence Options";
            // 
            // PNGColorButton
            // 
            PNGColorButton.Location = new Point(162, 81);
            PNGColorButton.Name = "PNGColorButton";
            PNGColorButton.Size = new Size(80, 25);
            PNGColorButton.TabIndex = 402;
            PNGColorButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 86);
            label3.Name = "label3";
            label3.Size = new Size(131, 15);
            label3.TabIndex = 9;
            label3.Text = "PNG background color:";
            // 
            // ImageFPSNumStepper
            // 
            ImageFPSNumStepper.Location = new Point(162, 23);
            ImageFPSNumStepper.Name = "ImageFPSNumStepper";
            ImageFPSNumStepper.Size = new Size(80, 23);
            ImageFPSNumStepper.TabIndex = 400;
            ImageFPSNumStepper.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 54);
            label2.Name = "label2";
            label2.Size = new Size(103, 15);
            label2.TabIndex = 7;
            label2.Text = "Generated frames:";
            // 
            // FrameGenNumStepper
            // 
            FrameGenNumStepper.Location = new Point(162, 52);
            FrameGenNumStepper.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            FrameGenNumStepper.Name = "FrameGenNumStepper";
            FrameGenNumStepper.Size = new Size(80, 23);
            FrameGenNumStepper.TabIndex = 401;
            FrameGenNumStepper.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 25);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 1;
            label1.Text = "Base frame rate:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lbFixSize);
            groupBox3.Controls.Add(FixSizeNumStepper);
            groupBox3.Controls.Add(CRFLabel);
            groupBox3.Controls.Add(QualityFixSizeRadioButton);
            groupBox3.Controls.Add(CRFTrackBar);
            groupBox3.Controls.Add(QualityCRFRadioButton);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(8, 147);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(7, 3, 7, 3);
            groupBox3.Size = new Size(252, 172);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Quality";
            // 
            // lbFixSize
            // 
            lbFixSize.AutoSize = true;
            lbFixSize.Location = new Point(7, 140);
            lbFixSize.Name = "lbFixSize";
            lbFixSize.Size = new Size(112, 15);
            lbFixSize.TabIndex = 4;
            lbFixSize.Text = "Target file size (MB):";
            // 
            // FixSizeNumStepper
            // 
            FixSizeNumStepper.DecimalPlaces = 2;
            FixSizeNumStepper.Location = new Point(162, 138);
            FixSizeNumStepper.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            FixSizeNumStepper.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            FixSizeNumStepper.Name = "FixSizeNumStepper";
            FixSizeNumStepper.Size = new Size(80, 23);
            FixSizeNumStepper.TabIndex = 303;
            FixSizeNumStepper.TextAlign = HorizontalAlignment.Center;
            FixSizeNumStepper.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // CRFLabel
            // 
            CRFLabel.BackColor = SystemColors.Window;
            CRFLabel.BorderStyle = BorderStyle.Fixed3D;
            CRFLabel.Location = new Point(214, 72);
            CRFLabel.Name = "CRFLabel";
            CRFLabel.Size = new Size(28, 24);
            CRFLabel.TabIndex = 2;
            CRFLabel.Text = "99";
            CRFLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // QualityFixSizeRadioButton
            // 
            QualityFixSizeRadioButton.AutoSize = true;
            QualityFixSizeRadioButton.Location = new Point(10, 113);
            QualityFixSizeRadioButton.Name = "QualityFixSizeRadioButton";
            QualityFixSizeRadioButton.Size = new Size(75, 19);
            QualityFixSizeRadioButton.TabIndex = 302;
            QualityFixSizeRadioButton.TabStop = true;
            QualityFixSizeRadioButton.Text = "Fixed size";
            QualityFixSizeRadioButton.UseVisualStyleBackColor = true;
            // 
            // CRFTrackBar
            // 
            CRFTrackBar.Location = new Point(3, 62);
            CRFTrackBar.Maximum = 51;
            CRFTrackBar.Name = "CRFTrackBar";
            CRFTrackBar.Size = new Size(205, 45);
            CRFTrackBar.TabIndex = 301;
            CRFTrackBar.TickStyle = TickStyle.Both;
            // 
            // QualityCRFRadioButton
            // 
            QualityCRFRadioButton.AutoSize = true;
            QualityCRFRadioButton.Location = new Point(10, 22);
            QualityCRFRadioButton.Name = "QualityCRFRadioButton";
            QualityCRFRadioButton.Size = new Size(201, 34);
            QualityCRFRadioButton.TabIndex = 300;
            QualityCRFRadioButton.TabStop = true;
            QualityCRFRadioButton.Text = "Constant Rate Factor\r\n(0=lossless, 23=default, 51=lossy)";
            QualityCRFRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(DropAudioCheckBox);
            groupBox4.Controls.Add(Downscale4kCheckBox);
            groupBox4.Dock = DockStyle.Top;
            groupBox4.Location = new Point(8, 74);
            groupBox4.Margin = new Padding(8);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(7, 3, 7, 3);
            groupBox4.Size = new Size(252, 73);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "Transform";
            // 
            // DropAudioCheckBox
            // 
            DropAudioCheckBox.AutoSize = true;
            DropAudioCheckBox.Location = new Point(10, 47);
            DropAudioCheckBox.Name = "DropAudioCheckBox";
            DropAudioCheckBox.Size = new Size(114, 19);
            DropAudioCheckBox.TabIndex = 201;
            DropAudioCheckBox.Text = "Drop audio track";
            DropAudioCheckBox.UseVisualStyleBackColor = true;
            // 
            // Downscale4kCheckBox
            // 
            Downscale4kCheckBox.AutoSize = true;
            Downscale4kCheckBox.Location = new Point(10, 22);
            Downscale4kCheckBox.Name = "Downscale4kCheckBox";
            Downscale4kCheckBox.Size = new Size(147, 19);
            Downscale4kCheckBox.TabIndex = 200;
            Downscale4kCheckBox.Text = "Downscale 4K to 1080p";
            Downscale4kCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.Control;
            groupBox1.Controls.Add(SkipSameCodecCheckBox);
            groupBox1.Controls.Add(Codec265RadioButton);
            groupBox1.Controls.Add(Codec264RadioButton);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(8, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(7, 3, 7, 7);
            groupBox1.Size = new Size(252, 74);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Codec";
            // 
            // SkipSameCodecCheckBox
            // 
            SkipSameCodecCheckBox.AutoSize = true;
            SkipSameCodecCheckBox.Location = new Point(10, 47);
            SkipSameCodecCheckBox.Name = "SkipSameCodecCheckBox";
            SkipSameCodecCheckBox.Size = new Size(173, 19);
            SkipSameCodecCheckBox.TabIndex = 102;
            SkipSameCodecCheckBox.Text = "Skip if source is same codec";
            SkipSameCodecCheckBox.UseVisualStyleBackColor = true;
            // 
            // Codec265RadioButton
            // 
            Codec265RadioButton.AutoSize = true;
            Codec265RadioButton.Location = new Point(86, 22);
            Codec265RadioButton.Name = "Codec265RadioButton";
            Codec265RadioButton.Size = new Size(55, 19);
            Codec265RadioButton.TabIndex = 101;
            Codec265RadioButton.TabStop = true;
            Codec265RadioButton.Text = "H.265";
            Codec265RadioButton.UseVisualStyleBackColor = true;
            // 
            // Codec264RadioButton
            // 
            Codec264RadioButton.AutoSize = true;
            Codec264RadioButton.Location = new Point(10, 22);
            Codec264RadioButton.Name = "Codec264RadioButton";
            Codec264RadioButton.Size = new Size(55, 19);
            Codec264RadioButton.TabIndex = 100;
            Codec264RadioButton.TabStop = true;
            Codec264RadioButton.Text = "H.264";
            Codec264RadioButton.UseVisualStyleBackColor = true;
            // 
            // TopMenu
            // 
            TopMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TopMenu.BackColor = SystemColors.Window;
            TopMenu.Dock = DockStyle.None;
            TopMenu.Items.AddRange(new ToolStripItem[] { TerminateButton, settingsToolStripMenuItem, aboutToolStripMenuItem });
            TopMenu.Location = new Point(414, 4);
            TopMenu.Margin = new Padding(0, 0, 0, 8);
            TopMenu.Name = "TopMenu";
            TopMenu.Size = new Size(355, 24);
            TopMenu.TabIndex = 10;
            TopMenu.Text = "menuStrip1";
            // 
            // TerminateButton
            // 
            TerminateButton.Name = "TerminateButton";
            TerminateButton.Size = new Size(114, 20);
            TerminateButton.Text = "Terminate Process";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { VerboseOutputMenuItem, AddToStartMenuMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // VerboseOutputMenuItem
            // 
            VerboseOutputMenuItem.Name = "VerboseOutputMenuItem";
            VerboseOutputMenuItem.Size = new Size(171, 22);
            VerboseOutputMenuItem.Text = "Verbose output";
            // 
            // AddToStartMenuMenuItem
            // 
            AddToStartMenuMenuItem.Name = "AddToStartMenuMenuItem";
            AddToStartMenuMenuItem.Size = new Size(171, 22);
            AddToStartMenuMenuItem.Text = "Add to Start Menu";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // TopPanel
            // 
            TopPanel.BackColor = SystemColors.Window;
            TopPanel.Controls.Add(TopMenu);
            TopPanel.Controls.Add(TabsPanel);
            TopPanel.Controls.Add(MenuPanel);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Padding = new Padding(4, 0, 4, 0);
            TopPanel.Size = new Size(777, 32);
            TopPanel.TabIndex = 0;
            // 
            // TabsPanel
            // 
            TabsPanel.AutoSize = true;
            TabsPanel.Controls.Add(ImagesToVideoTab);
            TabsPanel.Controls.Add(ConvertVideoTab);
            TabsPanel.Controls.Add(ProbeTab);
            TabsPanel.Dock = DockStyle.Left;
            TabsPanel.Location = new Point(4, 0);
            TabsPanel.Name = "TabsPanel";
            TabsPanel.Padding = new Padding(0, 3, 0, 3);
            TabsPanel.Size = new Size(243, 32);
            TabsPanel.TabIndex = 7;
            // 
            // ImagesToVideoTab
            // 
            ImagesToVideoTab.Appearance = Appearance.Button;
            ImagesToVideoTab.AutoSize = true;
            ImagesToVideoTab.Dock = DockStyle.Left;
            ImagesToVideoTab.Location = new Point(140, 3);
            ImagesToVideoTab.Name = "ImagesToVideoTab";
            ImagesToVideoTab.Size = new Size(103, 26);
            ImagesToVideoTab.TabIndex = 2;
            ImagesToVideoTab.TabStop = true;
            ImagesToVideoTab.Text = "Images To Video";
            ImagesToVideoTab.UseVisualStyleBackColor = true;
            // 
            // ConvertVideoTab
            // 
            ConvertVideoTab.Appearance = Appearance.Button;
            ConvertVideoTab.AutoSize = true;
            ConvertVideoTab.Dock = DockStyle.Left;
            ConvertVideoTab.Location = new Point(48, 3);
            ConvertVideoTab.Name = "ConvertVideoTab";
            ConvertVideoTab.Size = new Size(92, 26);
            ConvertVideoTab.TabIndex = 1;
            ConvertVideoTab.TabStop = true;
            ConvertVideoTab.Text = "Convert Video";
            ConvertVideoTab.UseVisualStyleBackColor = true;
            // 
            // ProbeTab
            // 
            ProbeTab.Appearance = Appearance.Button;
            ProbeTab.AutoSize = true;
            ProbeTab.Dock = DockStyle.Left;
            ProbeTab.Location = new Point(0, 3);
            ProbeTab.Name = "ProbeTab";
            ProbeTab.Size = new Size(48, 26);
            ProbeTab.TabIndex = 0;
            ProbeTab.TabStop = true;
            ProbeTab.Text = "Probe";
            ProbeTab.UseVisualStyleBackColor = true;
            // 
            // MenuPanel
            // 
            MenuPanel.Dock = DockStyle.Right;
            MenuPanel.Location = new Point(603, 0);
            MenuPanel.Name = "MenuPanel";
            MenuPanel.Size = new Size(170, 32);
            MenuPanel.TabIndex = 4;
            // 
            // BodyPanel
            // 
            BodyPanel.AutoSize = true;
            BodyPanel.Controls.Add(OutputList);
            BodyPanel.Controls.Add(ArgsPanel);
            BodyPanel.Dock = DockStyle.Fill;
            BodyPanel.Location = new Point(0, 34);
            BodyPanel.Name = "BodyPanel";
            BodyPanel.Padding = new Padding(8, 8, 8, 0);
            BodyPanel.Size = new Size(777, 506);
            BodyPanel.TabIndex = 5;
            // 
            // SeparatorLabel
            // 
            SeparatorLabel.BorderStyle = BorderStyle.Fixed3D;
            SeparatorLabel.Dock = DockStyle.Top;
            SeparatorLabel.Location = new Point(0, 32);
            SeparatorLabel.Name = "SeparatorLabel";
            SeparatorLabel.Size = new Size(777, 2);
            SeparatorLabel.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(777, 550);
            Controls.Add(BodyPanel);
            Controls.Add(SeparatorLabel);
            Controls.Add(TopPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = TopMenu;
            MinimumSize = new Size(600, 508);
            Name = "MainForm";
            Padding = new Padding(0, 0, 0, 10);
            SizeGripStyle = SizeGripStyle.Show;
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ArgsPanel.ResumeLayout(false);
            ImageSequenceGroup.ResumeLayout(false);
            ImageSequenceGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImageFPSNumStepper).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrameGenNumStepper).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FixSizeNumStepper).EndInit();
            ((System.ComponentModel.ISupportInitialize)CRFTrackBar).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            TopMenu.ResumeLayout(false);
            TopMenu.PerformLayout();
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            TabsPanel.ResumeLayout(false);
            TabsPanel.PerformLayout();
            BodyPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox OutputList;
        private Panel ArgsPanel;
        private GroupBox groupBox1;
        private RadioButton Codec265RadioButton;
        private RadioButton Codec264RadioButton;
        private TrackBar CRFTrackBar;
        private CheckBox SkipSameCodecCheckBox;
        private Label CRFLabel;
        private GroupBox groupBox3;
        private RadioButton QualityFixSizeRadioButton;
        private RadioButton QualityCRFRadioButton;
        private Label lbFixSize;
        private NumericUpDown FixSizeNumStepper;
        private CheckBox DropAudioCheckBox;
        private CheckBox Downscale4kCheckBox;
        private GroupBox groupBox4;
        private GroupBox ImageSequenceGroup;
        private NumericUpDown FrameGenNumStepper;
        private Label label1;
        private Label label2;
        private NumericUpDown ImageFPSNumStepper;
        private MenuStrip TopMenu;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Panel TopPanel;
        private Panel MenuPanel;
        private RadioButton ConvertVideoTab;
        private RadioButton ProbeTab;
        private Panel TabsPanel;
        private RadioButton ImagesToVideoTab;
        private ToolStripMenuItem VerboseOutputMenuItem;
        private Panel BodyPanel;
        private Label SeparatorLabel;
        private Button PNGColorButton;
        private Label label3;
        private ToolStripMenuItem AddToStartMenuMenuItem;
        private ToolStripMenuItem TerminateButton;
        private ToolStripMenuItem aboutToolStripMenuItem;
    }
}
