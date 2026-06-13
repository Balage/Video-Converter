namespace VideoConverter.Utils
{
    public static class ControlHelper
    {
        // Sets form to location, while making sure it will not end up off-screen.
        // Setting position null will make it appear at default location.
        public static void SetFormLocationSafe(Form form, Point? position)
        {
            if (position.HasValue)
            {
                var newPosition = position.Value;
                form.Location = newPosition;
                var bounds = Screen.FromControl(form).Bounds;
                if (newPosition.X < bounds.X) newPosition.X = bounds.X;
                if (newPosition.Y < bounds.Y) newPosition.Y = bounds.Y;
                if (newPosition.X + form.Width > bounds.Right) newPosition.X = bounds.Right - form.Width;
                if (newPosition.Y + form.Height > bounds.Bottom) newPosition.Y = bounds.Bottom - form.Height;
                form.Location = newPosition;
            }
        }

        public static void AddFileDrop(Control target, Action<string[]> callback)
        {
            target.AllowDrop = true;
            target.DragEnter += (sender, e) =>
            {
                e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
            };
            target.DragDrop += (sender, e) =>
            {
                //try
                {
                    var data = (string[]?)e.Data?.GetData(DataFormats.FileDrop);
                    if (data != null)
                    {
                        var result = new List<string>();
                        foreach (var item in data)
                        {
                            if (File.GetAttributes(item).HasFlag(FileAttributes.Directory))
                            {
                                EnumerateDir(item, result);
                            }
                            else
                            {
                                result.Add(item);
                            }
                        }

                        if (result.Count > 0)
                        {
                            callback.Invoke(result.ToArray());
                        }
                    }
                }
                /*catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
            };
        }

        private static void EnumerateDir(string rootDir, List<string> result)
        {
            foreach (var dir in Directory.EnumerateDirectories(rootDir))
            {
                EnumerateDir(dir, result);
            }
            foreach (var file in Directory.EnumerateFiles(rootDir))
            {
                result.Add(file);
            }
        }

        // Puts all controls inside [parent] into a Panel with padding at the bottom.
        public static void AddBottomMargin(Control parent, int bottomMargin)
        {
            List<Control> original = [];
            foreach (Control control in parent.Controls)
            {
                original.Add(control);
            }
            parent.Controls.Clear();

            int tabIndex = original.Count;
            foreach (Control control in original)
            {
                var container = new Panel
                {
                    Dock = control.Dock,
                    AutoSize = true,
                    Padding = new Padding(0, 0, 0, bottomMargin),
                    TabIndex = tabIndex--
                };
                container.Controls.Add(control);
                parent.Controls.Add(container);
            }
        }
    }
}
