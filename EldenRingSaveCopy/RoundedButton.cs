using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// Flat, rounded, gradient-fill button used for both the primary "Copy" action
    /// and the subtle "Browse" buttons. Set <see cref="Primary"/> for the gold action style.
    /// </summary>
    public class RoundedButton : Button
    {
        public int CornerRadius { get; set; } = 6;
        public bool Primary { get; set; } = false;
        private bool _hover, _down;

        public RoundedButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Theme.Surface; // matches the card the buttons sit on (override if placed elsewhere)
            Cursor = Cursors.Hand;
            UseVisualStyleBackColor = false;
        }

        protected override void OnMouseEnter(System.EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(System.EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            // Fill behind the rounded shape with the parent's colour so the antialiased
            // corners blend cleanly instead of leaving dark rectangular artifacts.
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);

            Color top, bottom, border, fg;
            if (!Enabled)
            {
                top = bottom = Theme.ControlFillDisabled;
                border = Theme.Border;
                fg = Theme.TextDisabled;
            }
            else if (Primary)
            {
                top = _down ? Theme.GoldHi : (_hover ? Color.FromArgb(0xE2, 0xC0, 0x5A) : Theme.GoldHi);
                bottom = _down ? Color.FromArgb(0xBF, 0x9C, 0x38) : Theme.Gold;
                border = Color.FromArgb(0x00, 0x00, 0x00);
                fg = Theme.GoldText;
            }
            else
            {
                top = bottom = _down ? Theme.ControlFillPressed : (_hover ? Theme.ControlFillHover : Theme.ControlFill);
                border = Theme.BorderStrong;
                fg = Theme.Text;
            }

            using (var path = RoundedPanel.Rounded(r, CornerRadius))
            {
                using (var b = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), top, bottom, LinearGradientMode.Vertical))
                    g.FillPath(b, path);
                using (var p = new Pen(border)) g.DrawPath(p, path);
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, fg,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
