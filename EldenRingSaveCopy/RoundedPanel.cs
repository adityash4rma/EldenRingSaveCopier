using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>A panel that paints as a rounded "card" with a 1px border — the Fluent surface look.</summary>
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 9;
        public Color FillColor { get; set; } = Theme.Surface;
        public Color BorderColor { get; set; } = Theme.Border;
        public int BorderThickness { get; set; } = 1;

        public RoundedPanel()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Rounded(r, CornerRadius))
            {
                using (var b = new SolidBrush(FillColor)) e.Graphics.FillPath(b, path);
                if (BorderThickness > 0)
                    using (var p = new Pen(BorderColor, BorderThickness)) e.Graphics.DrawPath(p, path);
            }
            base.OnPaint(e);
        }

        internal static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(r); path.CloseFigure(); return path; }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
