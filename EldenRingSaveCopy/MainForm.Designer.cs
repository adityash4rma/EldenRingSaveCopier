using System.Drawing;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private EmblemPanel emblem;
        private Label lblTitle;
        private Label lblSubtitle;

        // Banner (Fluent InfoBar, with embedded progress)
        private Banner banner;

        // Step 1 — files
        private RoundedPanel card1;
        private StepBadge badge1;
        private Label lblStep1;
        private Label lblStep1Hint;
        private Label lblSource;
        private PathField srcField;
        private RoundedButton btnBrowseSource;
        private Label lblDest;
        private PathField dstField;
        private RoundedButton btnBrowseDest;

        // Step 2 — slots
        private RoundedPanel card2;
        private StepBadge badge2;
        private Label lblStep2;
        private Label lblCopyFrom;
        private FluentComboBox cmbFrom;
        private SlotSummary fromSummary;
        private Label lblCopyTo;
        private FluentComboBox cmbTo;
        private SlotSummary toSummary;

        // Step 3 — copy
        private RoundedPanel card3;
        private StepBadge badge3;
        private Label lblStep3;
        private BackupNote backupNote;
        private RoundedButton btnCopy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            const int L = 24, R = 24, W = 672;     // outer margins / content width
            const int pad = 18;                    // card inner padding (left/right)
            int colW = (W - pad - pad - 16) / 2;   // two columns, 16px gutter
            int col2 = pad + colW + 16;            // second column X inside a card

            // ===== Header =====
            this.emblem = new EmblemPanel { CornerRadius = 9, Location = new Point(L, 18), Size = new Size(40, 40), BackColor = Theme.Client };
            this.lblTitle = new Label { AutoSize = true, Font = Theme.H1, ForeColor = Theme.Text, BackColor = Theme.Client, Text = "Elden Ring Save Copier", Location = new Point(L + 54, 15) };
            this.lblSubtitle = new Label { AutoSize = true, Font = Theme.Body, ForeColor = Theme.Text2, BackColor = Theme.Client, Text = "Transfer a character between save slots — safely backed up", Location = new Point(L + 56, 47) };

            // ===== Banner =====
            this.banner = new Banner { Location = new Point(L, 80), Size = new Size(W, 56), BackColor = Theme.Client };

            // ===== Step 1 — files =====
            this.card1 = new RoundedPanel { CornerRadius = 8, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 148), Size = new Size(W, 158) };
            this.badge1 = MakeBadge(1, StepBadge.BadgeState.Active);
            this.lblStep1 = MakeHeading("Select save files");
            this.lblStep1Hint = new Label { AutoSize = false, Font = Theme.Mono, ForeColor = Theme.Text3, BackColor = Theme.Surface, Text = "ER0000.sl2", TextAlign = ContentAlignment.MiddleRight, Location = new Point(W - 16 - 140, 18), Size = new Size(140, 18) };
            this.lblSource = MakeFieldLabel("Source file", new Point(pad, 48));
            this.srcField = new PathField { Location = new Point(pad, 70), Size = new Size(colW, 32), BackColor = Theme.Surface };
            this.btnBrowseSource = MakeBrowse(new Point(pad, 110), colW);
            this.lblDest = MakeFieldLabel("Destination file", new Point(col2, 48));
            this.dstField = new PathField { Location = new Point(col2, 70), Size = new Size(colW, 32), BackColor = Theme.Surface };
            this.btnBrowseDest = MakeBrowse(new Point(col2, 110), colW);
            this.card1.Controls.AddRange(new Control[] { this.badge1, this.lblStep1, this.lblStep1Hint, this.lblSource, this.srcField, this.btnBrowseSource, this.lblDest, this.dstField, this.btnBrowseDest });

            // ===== Step 2 — slots =====
            this.card2 = new RoundedPanel { CornerRadius = 8, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 318), Size = new Size(W, 170) };
            this.badge2 = MakeBadge(2, StepBadge.BadgeState.Pending);
            this.lblStep2 = MakeHeading("Choose character slots");
            this.lblCopyFrom = MakeFieldLabel("Copy from", new Point(pad, 48));
            this.cmbFrom = MakeCombo(new Point(pad, 70), colW);
            this.fromSummary = new SlotSummary { Location = new Point(pad, 108), Size = new Size(colW, 46), BackColor = Theme.Surface };
            this.lblCopyTo = MakeFieldLabel("Copy to", new Point(col2, 48));
            this.cmbTo = MakeCombo(new Point(col2, 70), colW);
            this.toSummary = new SlotSummary { Location = new Point(col2, 108), Size = new Size(colW, 46), BackColor = Theme.Surface };
            this.card2.Controls.AddRange(new Control[] { this.badge2, this.lblStep2, this.lblCopyFrom, this.cmbFrom, this.fromSummary, this.lblCopyTo, this.cmbTo, this.toSummary });

            // ===== Step 3 — copy =====
            this.card3 = new RoundedPanel { CornerRadius = 8, FillColor = Theme.Surface, BorderColor = Theme.Border, Location = new Point(L, 500), Size = new Size(W, 140) };
            this.badge3 = MakeBadge(3, StepBadge.BadgeState.Pending);
            this.lblStep3 = MakeHeading("Copy");
            this.backupNote = new BackupNote { Location = new Point(pad, 46), Size = new Size(W - 2 * pad, 20), Text = "Your destination file is automatically backed up before every copy." };
            this.btnCopy = new RoundedButton { Primary = true, CornerRadius = 6, Font = Theme.Button, Text = "Copy character", ForeColor = Theme.Text, BackColor = Theme.Surface, Location = new Point(pad, 76), Size = new Size(W - 2 * pad, 44), Enabled = false };
            this.btnCopy.Click += this.btnCopy_Click;
            this.card3.Controls.AddRange(new Control[] { this.badge3, this.lblStep3, this.backupNote, this.btnCopy });

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96f, 96f);
            this.ClientSize = new Size(L + W + R, 662);
            this.BackColor = Theme.Client;
            this.Font = Theme.Body;
            this.Text = "Elden Ring Save Copier";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.AddRange(new Control[] { this.emblem, this.lblTitle, this.lblSubtitle, this.banner, this.card1, this.card2, this.card3 });

            this.btnBrowseSource.Click += this.btnBrowseSource_Click;
            this.btnBrowseDest.Click += this.btnBrowseDest_Click;
            this.cmbFrom.SelectedIndexChanged += this.Slot_Changed;
            this.cmbTo.SelectedIndexChanged += this.Slot_Changed;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ---- small control factories keep the layout above readable ----
        private static StepBadge MakeBadge(int n, StepBadge.BadgeState state) =>
            new StepBadge { Number = n, State = state, Location = new Point(16, 14), BackColor = Theme.Surface };

        private static Label MakeHeading(string text) =>
            new Label { AutoSize = true, Font = Theme.H2, ForeColor = Theme.Text, BackColor = Theme.Surface, Text = text, Location = new Point(50, 17) };

        private static Label MakeFieldLabel(string text, Point p) =>
            new Label { AutoSize = true, Font = Theme.Label, ForeColor = Theme.Text2, BackColor = Theme.Surface, Text = text, Location = p };

        private RoundedButton MakeBrowse(Point p, int w) =>
            new RoundedButton { Primary = false, CornerRadius = 4, Font = Theme.Body, Text = "Browse…", ForeColor = Theme.Text, BackColor = Theme.Surface, Location = p, Size = new Size(w, 32) };

        private static FluentComboBox MakeCombo(Point p, int w) =>
            new FluentComboBox { Location = p, Size = new Size(w, 32), Enabled = false, PlaceholderText = "Select a file first" };
    }
}
