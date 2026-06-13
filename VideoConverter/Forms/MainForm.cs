using System.Reflection;
using VideoConverter.Config;
using VideoConverter.Utils;
using VideoConverter.Video;

namespace VideoConverter
{
    public enum PrintType
    {
        Normal,
        NormalOverwriteLast,
        Verbose,
    };

    public enum TaskTab
    {
        Probe = 0,
        ConvertVideo = 1,
        ImagesToVideo = 2,
    }

    public partial class MainForm : Form
    {
        // Consts
        private const string APP_TITLE = "Video Converter";

        // Components
        private readonly TaskbarProgress _taskbar;
        private readonly ShortcutHelper _shortcut = new(APP_TITLE);

        // State
        private readonly AppSettings _config = new();
        private readonly List<string> _consoleNormal = [];
        private readonly List<string> _consoleVerbose = [];
        private bool _verboseOutput = false;
        private Processor? _processor = null;

        public MainForm()
        {
            InitializeComponent();

            _taskbar = new TaskbarProgress(Handle);
            ControlHelper.AddBottomMargin(ArgsPanel, 8);
            UpdateTitle();

            settingsToolStripMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            ControlHelper.AddFileDrop(OutputList, (files) =>
            {
                EnableInterface(false);
                _taskbar.SetState(TaskbarProgress.TaskbarStates.Indeterminate);
                _taskbar.SetValue(0.0, 1.0);

                OutputList.Items.Clear();
                var args = new ProcessorArgs()
                {
                    TaskTab = _config.Tab,
                    Codec = _config.Codec,
                    SkipForSameCodec = _config.SkipIfSameCodec,
                    Downscale4K = _config.Downscale4K,
                    DropAudio = DropAudioCheckBox.Checked,
                    QualityMode = _config.QualityMode,
                    ConstantRateFactor = _config.QualityCRF,
                    FixedSizeMB = _config.FixedSize,
                    GenFrameCount = _config.GenerateFrames,
                    GenFrameLoop = true,
                    FrameRate = _config.FrameRate,
                    BackgroundColor = MathExt.ColorToHex(_config.BackgroundColor)
                };
                Task.Factory.StartNew(() =>
                {
                    _processor = new Processor(args, Print, TaskbarProgressUpdate, _config.GetSaveFolder())
                    {
                        OnCompleted = OnCompletedCallback
                    };
                    TerminateButton.Enabled = true;
                    _processor.Process(files);
                    TerminateButton.Enabled = false;
                    _processor = null;
                });
            });

            // Load configuration
            _config.Load();
            {
                ClientSize = _config.WindowSize;

                Codec264RadioButton.Checked = _config.Codec == CodecType.H264;
                Codec265RadioButton.Checked = !Codec264RadioButton.Checked;

                Downscale4kCheckBox.Checked = _config.Downscale4K;
                SkipSameCodecCheckBox.Checked = _config.SkipIfSameCodec;

                QualityCRFRadioButton.Checked = _config.QualityMode == QualityMode.FixedQuality;
                QualityFixSizeRadioButton.Checked = _config.QualityMode == QualityMode.FixedFileSize;

                CRFTrackBar.Value = _config.QualityCRF;
                CRFLabel.Text = CRFTrackBar.Value.ToString();

                FixSizeNumStepper.Value = (decimal)_config.FixedSize;

                FrameGenNumStepper.Value = _config.GenerateFrames;
                ImageFPSNumStepper.Value = _config.FrameRate;
                PNGColorButton.BackColor = _config.BackgroundColor;

                ProbeTab.Checked = _config.Tab == TaskTab.Probe;
                ConvertVideoTab.Checked = _config.Tab == TaskTab.ConvertVideo;
                ImagesToVideoTab.Checked = _config.Tab == TaskTab.ImagesToVideo;
            }

            // Set input change callbacks
            CRFTrackBar.ValueChanged += (sender, e) =>
            {
                CRFLabel.Text = CRFTrackBar.Value.ToString();
                _config.QualityCRF = CRFTrackBar.Value;
            };

            Codec264RadioButton.CheckedChanged += (sender, e) =>
            {
                _config.Codec = CodecType.H264;
            };

            Codec265RadioButton.CheckedChanged += (sender, e) =>
            {
                _config.Codec = CodecType.H265;
            };

            QualityCRFRadioButton.CheckedChanged += (sender, e) =>
            {
                _config.QualityMode = QualityMode.FixedQuality;
                UpdateQualityEnable();
            };

            QualityFixSizeRadioButton.CheckedChanged += (sender, e) =>
            {
                _config.QualityMode = QualityMode.FixedFileSize;
                UpdateQualityEnable();
            };

            SkipSameCodecCheckBox.CheckedChanged += (sender, e) =>
            {
                _config.SkipIfSameCodec = SkipSameCodecCheckBox.Checked;
            };

            Downscale4kCheckBox.CheckedChanged += (sender, e) =>
            {
                _config.Downscale4K = Downscale4kCheckBox.Checked;
            };

            FixSizeNumStepper.ValueChanged += (sender, e) =>
            {
                _config.FixedSize = (double)FixSizeNumStepper.Value;
            };

            FrameGenNumStepper.ValueChanged += (sender, e) =>
            {
                _config.GenerateFrames = (int)FrameGenNumStepper.Value;
            };

            ImageFPSNumStepper.ValueChanged += (sender, e) =>
            {
                _config.FrameRate = (int)ImageFPSNumStepper.Value;
            };

            PNGColorButton.Click += (object? sender, EventArgs e) =>
            {
                var colorDialog = new ColorDialog
                {
                    FullOpen = true,
                    AllowFullOpen = true,
                    ShowHelp = true,
                    Color = _config.BackgroundColor
                };
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _config.BackgroundColor = colorDialog.Color;
                    PNGColorButton.BackColor = colorDialog.Color;
                }
            };

            ProbeTab.CheckedChanged += (sender, e) => OnTabCheckedChanged(ProbeTab, TaskTab.Probe);
            ConvertVideoTab.CheckedChanged += (sender, e) => OnTabCheckedChanged(ConvertVideoTab, TaskTab.ConvertVideo);
            ImagesToVideoTab.CheckedChanged += (sender, e) => OnTabCheckedChanged(ImagesToVideoTab, TaskTab.ImagesToVideo);

            // Killswitch
            TerminateButton.Enabled = false;
            TerminateButton.Click += (sender, e) =>
            {
                if (_processor?.Stop() ?? false)
                {
                    TerminateButton.Enabled = false;
                    Print("Termination request by user...");
                }
            };

            // Settings
            VerboseOutputMenuItem.Click += (sender, e) =>
            {
                _verboseOutput = !_verboseOutput;
                VerboseOutputMenuItem.Checked = _verboseOutput;
                RefreshConsole();
            };

            AddToStartMenuMenuItem.Click += (sender, e) =>
            {
                AddShortcutToStartMenu();
            };
            AddToStartMenuMenuItem.Checked = _shortcut.Exists;

            // Finalize
            UpdateTab(_config.Tab);
            UpdateQualityEnable();
        }

        private void AddShortcutToStartMenu()
        {
            if (_shortcut.Exists) // Delete old
            {
                if (MessageBox.Show(
                    "Shortcut already exists, would you like to delete it?",
                    "Delete Start Menu shortcut",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _shortcut.Delete();
                    AddToStartMenuMenuItem.Checked = _shortcut.Exists;
                }
            }
            else // Create new
            {
                const string title = "Create Start Menu shortcut";
                if (MessageBox.Show(
                    "Would you like to create new Start Menu shortcut?",
                    title,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _shortcut.Create();

                    bool fileExists = _shortcut.Exists;
                    AddToStartMenuMenuItem.Checked = fileExists;
                    if (fileExists)
                    {
                        MessageBox.Show("Shortcut created successfully!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create shortcut.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EnableInterface(bool enabled)
        {
            ArgsPanel.Enabled = enabled;
            TabsPanel.Enabled = enabled;
        }

        private void OnTabCheckedChanged(RadioButton subject, TaskTab tab)
        {
            if (subject.Checked)
            {
                _config.Tab = tab;
                UpdateTab(tab);
            }
        }

        private void UpdateTab(TaskTab tab)
        {
            if (tab == TaskTab.ImagesToVideo)
            {
                QualityCRFRadioButton.Checked = true;

            }
            QualityFixSizeRadioButton.Enabled = tab != TaskTab.ImagesToVideo;

            DropAudioCheckBox.Enabled = tab != TaskTab.ImagesToVideo;
            ArgsPanel.Visible = tab != TaskTab.Probe;
            ImageSequenceGroup.Visible = tab == TaskTab.ImagesToVideo;
        }

        private void OnCompletedCallback()
        {
            if (InvokeRequired)
            {
                Invoke(OnCompletedCallback);
            }
            _taskbar.SetState(TaskbarProgress.TaskbarStates.NoProgress);
            EnableInterface(true);
        }

        private void UpdateQualityEnable()
        {
            // CRF
            CRFTrackBar.Enabled = QualityCRFRadioButton.Checked;
            CRFLabel.Enabled = QualityCRFRadioButton.Checked;
            // Fixed size
            FixSizeNumStepper.Enabled = QualityFixSizeRadioButton.Checked;
            lbFixSize.Enabled = QualityFixSizeRadioButton.Checked;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ControlHelper.SetFormLocationSafe(this, _config.WindowPosition);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TerminateButton.Enabled)
            {
                TerminateButton.PerformClick();
                e.Cancel = true;
            }
            else if (!ArgsPanel.Enabled)
            {
                e.Cancel = true;
            }
            else
            {
                _config.WindowPosition = Location;
                _config.WindowSize = ClientSize;
                _config.Save();
            }
        }

        private void TaskbarProgressUpdate(ProgressInfo info)
        {
            if (InvokeRequired)
            {
                Invoke(() => TaskbarProgressUpdate(info));
            }
            double percent = (double)info.TaskIndex + info.TaskProgress;
            _taskbar.SetValue(percent, info.TotalTasks);
        }

        private void Print(string text, PrintType type = PrintType.Normal)
        {
            if (InvokeRequired)
            {
                Invoke(() => Print(text, type));
            }
            else
            {
                switch (type)
                {
                    case PrintType.Normal:
                    {
                        _consoleNormal.Add(text);
                        _consoleVerbose.Add(text);

                        OutputList.Items.Add(text);
                        OutputList.SelectedIndex = OutputList.Items.Count - 1;
                        UpdateTitle(text);
                        break;
                    }
                    case PrintType.NormalOverwriteLast: // This one is not added to verbose.
                    {
                        if (_consoleNormal.Count == 0)
                        {
                            _consoleNormal.Add(text);
                        }
                        else
                        {
                            _consoleNormal[_consoleNormal.Count - 1] = text;
                        }
                        if (OutputList.Items.Count == 0)
                        {
                            OutputList.Items.Add(text);
                        }
                        else
                        {
                            OutputList.Items[OutputList.Items.Count - 1] = text;
                        }
                        UpdateTitle(text);
                        break;
                    }
                    case PrintType.Verbose:
                    {
                        _consoleVerbose.Add(text);
                        if (_verboseOutput)
                        {
                            OutputList.Items.Add(text);
                            OutputList.SelectedIndex = OutputList.Items.Count - 1;
                            UpdateTitle(text);
                        }
                        break;
                    }
                }
            }
        }

        private void UpdateTitle(string text = "")
        {
            Text = string.IsNullOrWhiteSpace(text) ? APP_TITLE : $"{text} | {APP_TITLE}";
        }

        private void RefreshConsole()
        {
            OutputList.Items.Clear();
            foreach (var item in _verboseOutput ? _consoleVerbose : _consoleNormal)
            {
                OutputList.Items.Add(item);
            }
            if (OutputList.Items.Count != 0)
            {
                OutputList.SelectedIndex = OutputList.Items.Count - 1;
                UpdateTitle($"{OutputList.Items[OutputList.Items.Count - 1]}");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string displayText = $"Video Converter\nv{GetVersionText()}\n\nSimple GUI for ffmpeg and ffprobe.\n\n";
            displayText += "by Balázs Vecsey\nVB studio © 2026\nhttp://vbstudio.hu/";
            MessageBox.Show(this, displayText, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetVersionText()
        {
            var assemblyVersion = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
            return assemblyVersion?.ToString() ?? "-";
        }
    }
}
