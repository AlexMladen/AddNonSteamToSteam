// Core
using AddNonSteamToSteam.Core.Common;
using AddNonSteamToSteam.Core.Models;
using AddNonSteamToSteam.Core.Services;
using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AddNonSteamToSteam
{
    public partial class MainForm : Form
    {
        private readonly ISteamPathService _steamPath;
        private readonly IProcessService _proc;
        private readonly IUrlService _Url;
        private readonly IVdfService _vdf;
        private readonly IAppIdService _appid;
        private readonly IShortcutService _shortcut;
        private readonly ISteamStoreService _store;
        private readonly IArtworkService _art;

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                var cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        public MainForm()
        {
            InitializeComponent();

            //=============Style================
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            this.BackgroundImage = AddNonSteamToSteam.UI.Properties.Resources.bg_gamepad;
            this.BackgroundImageLayout = ImageLayout.Tile;

            titleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            // Close button
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.BackColor = Color.Transparent;
            btnClose.ForeColor = Color.White;
            btnClose.Text = "✕";

            // change hover/press colors to pink
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(0xEC, 0x48, 0x8D);  // bright pink
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(0xC0, 0x36, 0x6F); // darker pink

            btnClose.Click += (_, __) => this.Close();

            // Minimize button
            btnMin.FlatStyle = FlatStyle.Flat;
            btnMin.FlatAppearance.BorderSize = 0;
            btnMin.BackColor = Color.Transparent;
            btnMin.ForeColor = Color.White;
            btnMin.Text = "–";

            btnMin.FlatAppearance.MouseOverBackColor = Color.FromArgb(0xEC, 0x48, 0x8D);
            btnMin.FlatAppearance.MouseDownBackColor = Color.FromArgb(0xC0, 0x36, 0x6F);

            btnMin.Click += (_, __) => this.WindowState = FormWindowState.Minimized;

            void StyleButton(System.Windows.Forms.Button b, Color baseColor, Color hoverColor, Color downColor, Color textColor)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.UseVisualStyleBackColor = false;
                b.BackColor = baseColor;
                b.ForeColor = textColor;

                b.FlatAppearance.MouseOverBackColor = hoverColor;
                b.FlatAppearance.MouseDownBackColor = downColor;

                b.Padding = new Padding(10, 6, 10, 6);
                b.TextImageRelation = TextImageRelation.ImageBeforeText;
            }

            // Colors
            var pink = Color.FromArgb(0xEC, 0x48, 0x8D); // pink
            var pinkHover = Color.FromArgb(0xF4, 0x72, 0xA6); // lighter pink on hover
            var pinkDown = Color.FromArgb(0xC0, 0x36, 0x6F); // darker when pressed

            var darkPurple = Color.FromArgb(0x6D, 0x28, 0xD9); // dark purple
            var darkPurpleHover = Color.FromArgb(0x7C, 0x3A, 0xE0); // lighter hover
            var darkPurpleDown = Color.FromArgb(0x4C, 0x1D, 0x95); // darker pressed

            // Apply:
            // Pick shortcut -> pink
            StyleButton(btnPickShortcut /* or btnPickURL */, pink, pinkHover, pinkDown, Color.White);

            // Add/Update -> dark purple
            StyleButton(btnAddUpdate, darkPurple, darkPurpleHover, darkPurpleDown, Color.White);

            rbUri.Appearance = Appearance.Button;
            rbExe.Appearance = Appearance.Button;

            rbUri.CheckedChanged += (s, e) => UpdateRadioButtonStyles();
            rbExe.CheckedChanged += (s, e) => UpdateRadioButtonStyles();

            UpdateRadioButtonStyles();

            //=============Style================

            _steamPath = new SteamPathService();
            _proc = new ProcessService();
            _Url = new UrlService();
            _vdf = new VdfService();
            _appid = new AppIdService();
            _shortcut = new ShortcutService(_vdf, _appid);
            _store = new SteamStoreService();
            _art = new ArtworkService();

            rbUri.CheckedChanged += LaunchModeChanged;
            rbExe.CheckedChanged += LaunchModeChanged;
            btnPickShortcut.Click += BtnPickShortcut_Click;
            btnAddUpdate.Click += async (_, __) => await AddUpdateAsync();

            LaunchModeChanged(this, EventArgs.Empty);
            this.AcceptButton = btnAddUpdate;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LaunchModeChanged(object? sender, EventArgs e)
        {
            bool urlMode = rbUri.Checked;
        }

        private void BtnPickShortcut_Click(object? sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using var ofd = new OpenFileDialog
            {
                Title = "Pick a Desktop shortcut",
                InitialDirectory = desktop,
                Filter = "Shortcuts (*.url;*.lnk)|*.url;*.lnk|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            SetNamesFromFilename(ofd.FileName);

            var isUriMode = rbUri.Checked;
            var ext = Path.GetExtension(ofd.FileName).ToLowerInvariant();

            if (isUriMode)
            {
                // Expect a .url that contains the com.epicgames.launcher:// URI
                if (ext == ".url")
                {
                    try
                    {
                        var uri = _Url.ReadUriFromUrlFile(ofd.FileName);
                        txtUrl.Text = uri;
                        txtExe.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"Couldn't read URI from the shortcut:\n{ex.Message}",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (ext == ".lnk")
                {
                    MessageBox.Show(this,
                        "You selected an exe shortcut while Launch via Epic or other is selected.\n" +
                        "Please pick a shortcut with a link in its properties,\n" +
                        "or switch to Launch via EXE.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else // Direct EXE mode
            {
                if (ext == ".lnk")
                {
                    var target = ResolveLnkTarget(ofd.FileName);
                    if (!string.IsNullOrWhiteSpace(target) && System.IO.File.Exists(target))
                    {
                        txtExe.Text = target;
                        txtUrl.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show(this,
                            "Could not find the .exe file.\n" +
                            "You can pick a different shortcut or switch to Launcher mode for the shortcuts with links.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (ext == ".url")
                {
                    MessageBox.Show(this,
                        "You selected a shortcut with a link while Launch via EXE is selected.\n" +
                        "Please pick a Windows shortcut thatpoints to the game’s .exe,\n" +
                        "or switch to Launch via Epic or other to use the .url.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        // --- Main action ---
        private async Task AddUpdateAsync()
        {
            // 1) Validate basic inputs
            var displayName = txtDisplayName.Text.Trim();
            if (string.IsNullOrWhiteSpace(displayName))
            {
                MessageBox.Show(this, "Please enter a Display Name.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDisplayName.Focus();
                return;
            }

            LaunchMode mode = rbUri.Checked ? LaunchMode.Uri : LaunchMode.DirectExe;

            string? uri = null;
            string? exePath = null;

            if (mode == LaunchMode.Uri)
            {
                uri = txtUrl.Text.Trim();
                if (string.IsNullOrWhiteSpace(uri) || !uri.StartsWith("com.", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(this, "Please provide a valid Epic/launcher URI (e.g., com.epicgames.launcher://...).",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUrl.Focus();
                    return;
                }
            }
            else
            {
                exePath = txtExe.Text.Trim();
                if (string.IsNullOrWhiteSpace(exePath) || !System.IO.File.Exists(exePath))
                {
                    MessageBox.Show(this, "Please pick a valid .exe file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                // 2) Ensure Steam is closed
                if (_proc.IsSteamRunning())
                {
                    var ans = MessageBox.Show(this,
                        "Steam is currently running. Close it now so we can update shortcuts?\n(If you choose No, the operation will cancel.)",
                        Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ans == DialogResult.Yes) _proc.TryCloseSteam(); else return;
                }

                // 3) Resolve paths
                var steamRoot = _steamPath.GetSteamRoot();
                var userConfig = _steamPath.GetActiveUserConfigDir(steamRoot);
                var shortcutsVdf = _steamPath.GetShortcutsPath(userConfig);
                var gridDir = _steamPath.GetGridDir(userConfig);

                // 4) Write / update shortcut
                var req = new ShortcutRequest
                {
                    DisplayName = displayName,
                    Mode = mode,
                    Uri = uri,
                    ExePath = exePath,
                    AllowOverlay = true
                };

                var result = _shortcut.AddOrUpdateShortcut(req, shortcutsVdf, out var shortcutModel);
                if (!result.Success)
                {
                    MessageBox.Show(this, $"Failed to write shortcuts.vdf:\n{result.Error}",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 5) Artwork
                int? steamAppId = null;
                if (int.TryParse(txtSteamAppId.Text.Trim(), out var parsed)) steamAppId = parsed;

                if (steamAppId is null)
                {
                    var searchTitle = string.IsNullOrWhiteSpace(txtSteamName.Text) ? displayName : txtSteamName.Text.Trim();
                    steamAppId = await _store.FindSteamAppIdByNameAsync(searchTitle);
                }

                if (steamAppId is null)
                {
                    MessageBox.Show(this,
                        "Shortcut added/updated, but a matching Steam app wasn’t found for artwork.\nYou can type a Steam AppID and click Add/Update again.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Download images
                    var portrait = await _store.DownloadPortraitAsync(steamAppId.Value);
                    var hero = await _store.DownloadHeroAsync(steamAppId.Value);
                    var logo = await _store.DownloadLogoAsync(steamAppId.Value);
                    var header = await _store.DownloadHeaderAsync(steamAppId.Value);

                    var artResult = _art.SaveArtwork(gridDir, shortcutModel.NonSteamAppId, portrait, hero, logo, header);
                    if (!artResult.Success)
                    {
                        MessageBox.Show(this, $"Shortcut saved, but artwork failed:\n{artResult.Error}",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(this, "Done! Shortcut + artwork saved. Start Steam to see it.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Unexpected error:\n{ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetNamesFromFilename(string path)
        {
            var baseName = Path.GetFileNameWithoutExtension(path);
            txtDisplayName.Text = baseName;
            txtSteamName.Text = baseName;
        }
        private string? ResolveLnkTarget(string lnkPath)
        {
            try
            {
                var shell = new WshShell();
                var sc = (IWshShortcut)shell.CreateShortcut(lnkPath);
                return string.IsNullOrWhiteSpace(sc.TargetPath) ? null : sc.TargetPath;
            }
            catch
            {
                return null;
            }
        }

        private void titleBar_Paint(object sender, PaintEventArgs e)
        {
            var r = titleBar.ClientRectangle;
            using var lg = new System.Drawing.Drawing2D.LinearGradientBrush(
                r,
                Color.FromArgb(0x3B, 0x82, 0xF6),
                Color.FromArgb(0xEC, 0x48, 0x8D),
                0f);
            e.Graphics.FillRectangle(lg, r);

            using var font = new Font("Segoe UI", 10, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, this.Text, font, new Point(12, 9), Color.White);
        }
        private void UpdateRadioButtonStyles()
        {
            StyleRadioButton(rbUri, rbUri.Checked);
            StyleRadioButton(rbExe, rbExe.Checked);
        }

        private void StyleRadioButton(RadioButton rb, bool isSelected)
        {
            Color normalColor = Color.LightGray;                   // disabled/other one selected
            Color selectedColor = Color.FromArgb(0x1E, 0x3A, 0x8A); // dark blue when pressed
            Color hoverColor = Color.FromArgb(0x60, 0xA5, 0xF8);    // light blue on hover

            rb.FlatStyle = FlatStyle.Flat;
            rb.FlatAppearance.BorderSize = 0;
            rb.BackColor = isSelected ? selectedColor : normalColor;
            rb.ForeColor = isSelected ? Color.White : Color.Black;

            rb.MouseEnter -= RadioHover;
            rb.MouseLeave -= RadioLeave;
            rb.MouseEnter += RadioHover;
            rb.MouseLeave += RadioLeave;
        }
        private void RadioHover(object? sender, EventArgs e)
        {
            var rb = (RadioButton)sender!;
            if (!rb.Checked) rb.BackColor = Color.FromArgb(0xA0, 0xC4, 0xFF);
        }
        private void RadioLeave(object? sender, EventArgs e)
        {
            var rb = (RadioButton)sender!;
            if (!rb.Checked) rb.BackColor = Color.LightGray;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
    }
}
