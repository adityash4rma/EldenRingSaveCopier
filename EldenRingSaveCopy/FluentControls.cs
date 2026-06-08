using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>Rounded gold "app tile" emblem with the ring + dot motif from the mockup.</summary>
    public class EmblemPanel : Control
    {
        public int CornerRadius { get; set; } = 9;

        public EmblemPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rr = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedPanel.Rounded(rr, CornerRadius))
            using (var b = new SolidBrush(Theme.Gold)) g.FillPath(b, path);

            Color mark = Color.FromArgb(210, Theme.GoldText);
            int d = (int)(Width * 0.45f);
            using (var p = new Pen(mark, 2f)) g.DrawEllipse(p, (Width - d) / 2f, (Height - d) / 2f, d, d);
            int dd = Math.Max(3, (int)(Width * 0.09f));
            using (var b = new SolidBrush(mark)) g.FillEllipse(b, (Width - dd) / 2f, (Height - dd) / 2f, dd, dd);
        }
    }

    /// <summary>Read-only Fluent path display: leading file/folder glyph + middle-ellipsised path.</summary>
    public class PathField : Control
    {
        private string _value = "";
        public string Value
        {
            get => _value;
            set { _value = value ?? ""; Invalidate(); }
        }
        public bool HasValue => !string.IsNullOrEmpty(_value);

        public PathField()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Height = 32;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rr = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedPanel.Rounded(rr, 4))
            {
                using (var b = new SolidBrush(Theme.ControlFill)) g.FillPath(b, path);
                using (var p = new Pen(Theme.Border)) g.DrawPath(p, path);
            }
            // bottom-heavier Fluent stroke
            using (var p = new Pen(Theme.BorderStrong)) g.DrawLine(p, 4, Height - 1, Width - 4, Height - 1);

            var iconRect = new Rectangle(10, (Height - 16) / 2, 16, 16);
            if (HasValue) FluentIcons.File(g, iconRect, Theme.Text3);
            else FluentIcons.Folder(g, iconRect, Theme.Text3);

            var textRect = new Rectangle(34, 0, Width - 44, Height);
            string text = HasValue ? _value : "No file selected";
            Color color = HasValue ? Theme.Text : Theme.Text3;
            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix |
                        (HasValue ? TextFormatFlags.PathEllipsis : TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, text, Theme.Body, textRect, color, flags);
        }
    }

    /// <summary>Slot summary card: avatar + name + meta, with an optional status pill.</summary>
    public class SlotSummary : Control
    {
        public enum Variant { Neutral, Safe, Warn }

        private Variant _variant = Variant.Neutral;
        private string _initial = "", _title = "", _meta = "", _tag = "";

        public SlotSummary()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Height = 46;
            Visible = false;
        }

        public void SetContent(Variant v, string initial, string title, string meta, string tag)
        {
            _variant = v;
            _initial = initial ?? "";
            _title = title ?? "";
            _meta = meta ?? "";
            _tag = tag ?? "";
            Visible = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color bg, border, avatarBg, avatarFg, tagBg, tagFg;
            switch (_variant)
            {
                case Variant.Safe:
                    bg = Theme.GreenBg; border = Theme.GreenBorder;
                    avatarBg = Theme.AvatarGreenBg; avatarFg = Theme.Green;
                    tagBg = Theme.TagGreenBg; tagFg = Theme.Green;
                    break;
                case Variant.Warn:
                    bg = Theme.AmberBg; border = Theme.AmberBorder;
                    avatarBg = Theme.AvatarBg; avatarFg = Theme.GoldDeep;
                    tagBg = Theme.TagAmberBg; tagFg = Theme.Amber;
                    break;
                default:
                    bg = Theme.SummaryBg; border = Theme.Border;
                    avatarBg = Theme.AvatarBg; avatarFg = Theme.GoldDeep;
                    tagBg = Theme.SummaryBg; tagFg = Theme.Text2;
                    break;
            }

            var rr = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedPanel.Rounded(rr, 6))
            {
                using (var b = new SolidBrush(bg)) g.FillPath(b, path);
                using (var p = new Pen(border)) g.DrawPath(p, path);
            }

            // avatar
            var av = new Rectangle(11, (Height - 32) / 2, 32, 32);
            using (var path = RoundedPanel.Rounded(av, 6))
            using (var b = new SolidBrush(avatarBg)) g.FillPath(b, path);
            if (_initial == "+")
                FluentIcons.Plus(g, new Rectangle(av.X + 8, av.Y + 8, 16, 16), avatarFg);
            else
                TextRenderer.DrawText(g, _initial, Theme.NameBold, av, avatarFg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // optional tag pill (measured from the right)
            int textRight = Width - 12;
            if (!string.IsNullOrEmpty(_tag))
            {
                Size ts = TextRenderer.MeasureText(_tag, Theme.Tag);
                int pillW = ts.Width + 18, pillH = 20;
                var pill = new Rectangle(Width - 11 - pillW, (Height - pillH) / 2, pillW, pillH);
                using (var path = RoundedPanel.Rounded(pill, 4))
                using (var b = new SolidBrush(tagBg)) g.FillPath(b, path);
                TextRenderer.DrawText(g, _tag, Theme.Tag, pill, tagFg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                textRight = pill.X - 8;
            }

            int tx = av.Right + 11;
            var titleRect = new Rectangle(tx, Height / 2 - 16, textRight - tx, 16);
            var metaRect = new Rectangle(tx, Height / 2, textRight - tx, 16);
            TextRenderer.DrawText(g, _title, Theme.NameBold, titleRect, Theme.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            if (!string.IsNullOrEmpty(_meta))
                TextRenderer.DrawText(g, _meta, Theme.Meta, metaRect, Theme.Text2,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>The "automatically backed up" reassurance line: shield glyph + caption.</summary>
    public class BackupNote : Control
    {
        public BackupNote()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Theme.Surface;
            Height = 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var ic = new Rectangle(0, (Height - 18) / 2, 18, 18);
            FluentIcons.Shield(g, ic, Theme.GoldDeep);
            var textRect = new Rectangle(26, 0, Width - 26, Height);
            TextRenderer.DrawText(g, Text, Theme.Body, textRect, Theme.Text2,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }

    /// <summary>Thin rounded Fluent progress bar with a gold indicator.</summary>
    public class FluentProgressBar : Control
    {
        private int _value;
        public int Maximum { get; set; } = 100;
        public int Value
        {
            get => _value;
            set { _value = Math.Max(0, Math.Min(Maximum, value)); Invalidate(); }
        }

        public FluentProgressBar()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Theme.Surface; // blends with the neutral "copying" banner behind it
            Height = 4;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int radius = Math.Max(1, Height / 2);
            var track = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedPanel.Rounded(track, radius))
            using (var b = new SolidBrush(Color.FromArgb(0x3A, 0x3A, 0x3D))) g.FillPath(b, path);

            if (_value > 0 && Maximum > 0)
            {
                int w = (int)((Width - 1) * (_value / (float)Maximum));
                if (w >= radius * 2)
                {
                    var fill = new Rectangle(0, 0, w, Height - 1);
                    using (var path = RoundedPanel.Rounded(fill, radius))
                    using (var b = new SolidBrush(Theme.Gold)) g.FillPath(b, path);
                }
            }
        }
    }

    /// <summary>Fluent InfoBar: severity icon + message, with an embedded progress bar for the copying state.</summary>
    public class Banner : Control
    {
        public enum Kind { Info, Caution, Critical, Success, Neutral }

        private Kind _kind = Kind.Info;
        private string _text = "";

        public FluentProgressBar Progress { get; } = new FluentProgressBar();

        public Banner()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Height = 56;
            Progress.Visible = false;
            Controls.Add(Progress);
        }

        public void Set(Kind kind, string text)
        {
            _kind = kind;
            _text = text ?? "";
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Progress.SetBounds(44, Height - 14, Math.Max(0, Width - 58), 4);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color bg, border, icon;
            switch (_kind)
            {
                case Kind.Caution:  bg = Theme.AmberBg; border = Theme.AmberBorder; icon = Theme.Amber; break;
                case Kind.Critical: bg = Theme.RedBg;   border = Theme.RedBorder;   icon = Theme.Red;   break;
                case Kind.Success:  bg = Theme.GreenBg; border = Theme.GreenBorder; icon = Theme.Green; break;
                case Kind.Neutral:  bg = Theme.Surface; border = Theme.BorderStrong; icon = Theme.Text2; break;
                default:            bg = Theme.InfoBg;  border = Theme.InfoBorder;  icon = Theme.Info;  break;
            }

            var rr = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedPanel.Rounded(rr, 6))
            {
                using (var b = new SolidBrush(bg)) g.FillPath(b, path);
                using (var p = new Pen(border)) g.DrawPath(p, path);
            }

            var ic = new Rectangle(15, 12, 18, 18);
            switch (_kind)
            {
                case Kind.Caution:
                case Kind.Critical: FluentIcons.Warning(g, ic, icon); break;
                case Kind.Success:  FluentIcons.Success(g, ic, icon); break;
                default:            FluentIcons.Info(g, ic, icon); break;
            }

            int bottom = Progress.Visible ? Height - 20 : Height;
            var textRect = new Rectangle(44, 8, Width - 58, bottom - 12);
            Color tc = _kind == Kind.Neutral ? Theme.Text2 : Theme.Text;
            TextRenderer.DrawText(g, _text, Theme.Body, textRect, tc,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
        }
    }
}
