using System.Drawing;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private RoundedPanel emblem;
        private Label lblTitle;
        private Label lblSubtitle;

        // Banner
        private RoundedPanel bannerPanel;
        private Label lblBanner;
        private ProgressBar progress;

        // Step 1 — files
        private RoundedPanel card1;
        private StepBadge badge1;
        private Label lblStep1;
        private Label lblStep1Hint;
        private Label lblSource;
        private TextBox txtSource;
        private RoundedButton btnBrowseSource;
        private Label lblDest;
        private TextBox txtDest;
        private RoundedButton btnBrowseDest;

        // Step 2 — slots
        private RoundedPanel card2;
        private StepBadge badge2;
        private Label lblStep2;
        private Label lblCopyFrom;
        private ComboBox cmbFrom;
        private Label lblFromSummary;
        private Label lblCopyTo;
        private ComboBox cmbTo;
        private Label lblToSummary;

        // Step 3 — copy
        private RoundedPanel card3;
        private StepBadge badge3;
        private Label lblStep3;
        private Label lblBackupNote;
        private RoundedButton btnCopy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            int L = 24, R = 24, W = 672;                 // outer margins / content width
            int colW = (W - 20) / 2;                     // two columns with a 20px gutter
            int col2X = L + colW + 20;

            // ===== Header =====
            this.emblem = new RoundedPanel { CornerRadius = 21, FillColor = Theme.Gold, BorderColor = Theme.GoldDeep, Location = new Point(L, 18), Size = new Size(42, 42) };
            this.lblTitle = new Label { AutoSize = true, Font = Theme.H1, ForeColor = Theme.Text, Text = "Elden Ring Save Copier", Location = new Point(L + 54, 20) };
            this.lblSubtitle = new Label { AutoSize = true, Font = Theme.Body, ForeColor = Theme.Text2, Text = "Transfer a character between save slots — safely backed up", Location = new Point(L + 56, 44) };

            // ===== Banner =====
            this.bannerPanel = new RoundedPanel { CornerRadius = 7, FillColor = Theme.InfoBg, BorderColor = Theme.InfoBorder, Location = new Point(L, 78), Size = new Size(W, 52) };
            this.lblBanner = new Label { AutoSize = false, Font = Theme.Body, ForeColor = Theme.Text, Text = "Select your source and destination save files to begin.", TextAlign = ContentAlignment.MiddleLeft, Location = new Point(14, 6), Size = new Size(W - 28, 26), BackColor = Color.Transparent };
            this.progress = new ProgressBar { Location = new Point(14, 34), Size = new Size(W - 28, 6), Style = ProgressBarStyle.Continuous, Visible = false };
            this.bannerPanel.Controls.Add(this.lblBanner);
            this.bannerPanel.Controls.Add(this.progress);

            // ===== Step 1 — files =====
            this.card1 = new RoundedPanel { CornerRadius = 9, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 142), Size = new Size(W, 150) };
            this.badge1 = new StepBadge { Number = 1, State = StepBadge.BadgeState.Active, Location = new Point(16, 14) };
            this.lblStep1 = new Label { AutoSize = true, Font = Theme.H2, ForeColor = Theme.Text, Text = "Select save files", Location = new Point(50, 16), BackColor = Color.Transparent };
            this.lblStep1Hint = new Label { AutoSize = false, Font = Theme.Small, ForeColor = Theme.Text3, Text = "ER0000.sl2", TextAlign = ContentAlignment.MiddleRight, Location = new Point(W - 130, 18), Size = new Size(114, 18), BackColor = Color.Transparent };
            this.lblSource = MakeFieldLabel("Source file", new Point(16, 50));
            this.txtSource = MakePath(new Point(16, 70), colW);
            this.btnBrowseSource = MakeBrowse(new Point(16, 104), colW);
            this.lblDest = MakeFieldLabel("Destination file", new Point(col2X - L + 16, 50));
            this.txtDest = MakePath(new Point(col2X - L + 16, 70), colW);
            this.btnBrowseDest = MakeBrowse(new Point(col2X - L + 16, 104), colW);
            this.card1.Controls.AddRange(new Control[] { this.badge1, this.lblStep1, this.lblStep1Hint, this.lblSource, this.txtSource, this.btnBrowseSource, this.lblDest, this.txtDest, this.btnBrowseDest });

            // ===== Step 2 — slots =====
            this.card2 = new RoundedPanel { CornerRadius = 9, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 304), Size = new Size(W, 162) };
            this.badge2 = new StepBadge { Number = 2, State = StepBadge.BadgeState.Pending, Location = new Point(16, 14) };
            this.lblStep2 = new Label { AutoSize = true, Font = Theme.H2, ForeColor = Theme.Text, Text = "Choose character slots", Location = new Point(50, 16), BackColor = Color.Transparent };
            this.lblCopyFrom = MakeFieldLabel("Copy from", new Point(16, 50));
            this.cmbFrom = MakeCombo(new Point(16, 70), colW);
            this.lblFromSummary = MakeSummary(new Point(16, 106), colW);
            this.lblCopyTo = MakeFieldLabel("Copy to", new Point(col2X - L + 16, 50));
            this.cmbTo = MakeCombo(new Point(col2X - L + 16, 70), colW);
            this.lblToSummary = MakeSummary(new Point(col2X - L + 16, 106), colW);
            this.card2.Controls.AddRange(new Control[] { this.badge2, this.lblStep2, this.lblCopyFrom, this.cmbFrom, this.lblFromSummary, this.lblCopyTo, this.cmbTo, this.lblToSummary });

            // ===== Step 3 — copy =====
            this.card3 = new RoundedPanel { CornerRadius = 9, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 478), Size = new Size(W, 142) };
            this.badge3 = new StepBadge { Number = 3, State = StepBadge.BadgeState.Pending, Location = new Point(16, 14) };
            this.lblStep3 = new Label { AutoSize = true, Font = Theme.H2, ForeColor = Theme.Text, Text = "Copy", Location = new Point(50, 16), BackColor = Color.Transparent };
            this.lblBackupNote = new Label { AutoSize = false, Font = Theme.Body, ForeColor = Theme.Text2, Text = "🛡  Your destination file is automatically backed up before every copy.", TextAlign = ContentAlignment.MiddleLeft, Location = new Point(16, 48), Size = new Size(W - 32, 22), BackColor = Color.Transparent };
            this.btnCopy = new RoundedButton { Primary = true, CornerRadius = 7, Font = Theme.Button, Text = "Copy character", Location = new Point(16, 78), Size = new Size(W - 32, 46), Enabled = false };
            this.btnCopy.Click += this.btnCopy_Click;
            this.card3.Controls.AddRange(new Control[] { this.badge3, this.lblStep3, this.lblBackupNote, this.btnCopy });

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(L + W + R, 640);
            this.BackColor = Theme.Client;
            this.Font = Theme.Body;
            this.Text = "Elden Ring Save Copier";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.AddRange(new Control[] { this.emblem, this.lblTitle, this.lblSubtitle, this.bannerPanel, this.card1, this.card2, this.card3 });

            this.btnBrowseSource.Click += this.btnBrowseSource_Click;
            this.btnBrowseDest.Click += this.btnBrowseDest_Click;
            this.cmbFrom.SelectedIndexChanged += this.Slot_Changed;
            this.cmbTo.SelectedIndexChanged += this.Slot_Changed;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ---- small control factories keep the layout above readable ----
        private static Label MakeFieldLabel(string text, Point p) =>
            new Label { AutoSize = true, Font = Theme.Label, ForeColor = Theme.Text2, Text = text, Location = p, BackColor = Color.Transparent };

        private static TextBox MakePath(Point p, int w) =>
            new TextBox { ReadOnly = true, BorderStyle = BorderStyle.FixedSingle, Font = Theme.Body, ForeColor = Theme.Text3, BackColor = Theme.ControlFill, Text = "No file selected", Location = p, Size = new Size(w, 28) };

        private RoundedButton MakeBrowse(Point p, int w) =>
            new RoundedButton { Primary = false, CornerRadius = 5, Font = Theme.Body, Text = "Browse…", ForeColor = Theme.Text, Location = p, Size = new Size(w, 32) };

        private static ComboBox MakeCombo(Point p, int w) =>
            new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, Font = Theme.Body, BackColor = Theme.ControlFill, ForeColor = Theme.Text, Location = p, Size = new Size(w, 28), Enabled = false };

        private static Label MakeSummary(Point p, int w) =>
            new Label { AutoSize = false, Font = Theme.Small, ForeColor = Theme.Text3, Text = "", TextAlign = ContentAlignment.MiddleLeft, Location = p, Size = new Size(w, 40), BackColor = Color.Transparent, Visible = false };
    }
}
