using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// A Windows 11 Fluent styled ComboBox: dark rounded control, custom chevron,
    /// accent focus underline, and an owner-drawn dark dropdown list.
    /// </summary>
    public class FluentComboBox : ComboBox
    {
        private const int WM_PAINT = 0x000F;
        private bool _hover, _focused;

        public string PlaceholderText { get; set; } = "";

        public FluentComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            FlatStyle = FlatStyle.Flat;
            BackColor = Theme.Surface; // corners blend with the card behind us
            ForeColor = Theme.Text;
            ItemHeight = 26;
            DoubleBuffered = true;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { _focused = true; Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { _focused = false; Invalidate(); base.OnLostFocus(e); }

        // Dark dropdown list rows.
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color bg = selected ? Theme.ControlFillHover : Theme.Surface2;
            using (var b = new SolidBrush(bg)) e.Graphics.FillRectangle(b, e.Bounds);
            var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 12, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), Font, textRect, Theme.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT) PaintClosed();
        }

        private void PaintClosed()
        {
            using (var g = Graphics.FromHwnd(Handle))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var rr = new Rectangle(0, 0, Width - 1, Height - 1);
                Color fill = !Enabled ? Theme.ControlFillDisabled : (_hover ? Theme.ControlFillHover : Theme.ControlFill);
                using (var path = RoundedPanel.Rounded(rr, 4))
                {
                    using (var b = new SolidBrush(fill)) g.FillPath(b, path);
                    using (var p = new Pen(Theme.Border)) g.DrawPath(p, path);
                }
                using (var p = new Pen(Theme.BorderStrong)) g.DrawLine(p, 4, Height - 1, Width - 4, Height - 1);

                if (_focused && Enabled)
                    using (var p = new Pen(Theme.Gold, 2f)) g.DrawLine(p, 5, Height - 1.5f, Width - 5, Height - 1.5f);

                bool hasSelection = SelectedIndex >= 0;
                string text = hasSelection ? GetItemText(SelectedItem) : PlaceholderText;
                Color tc = !Enabled ? Theme.TextDisabled : (hasSelection ? Theme.Text : Theme.Text3);
                var textRect = new Rectangle(11, 0, Width - 40, Height);
                TextRenderer.DrawText(g, text, Font, textRect, tc,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

                FluentIcons.Chevron(g, new Rectangle(Width - 27, (Height - 14) / 2, 14, 14),
                    Enabled ? Theme.Text2 : Theme.TextDisabled);
            }
        }
    }
}
