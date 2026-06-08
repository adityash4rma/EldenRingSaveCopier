using System.Drawing;
using System.Drawing.Drawing2D;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// Hand-drawn vector glyphs (no icon-font dependency) used across the Fluent UI.
    /// Every icon scales to fill the supplied rectangle and strokes in the given colour.
    /// </summary>
    internal static class FluentIcons
    {
        private static PointF P(Rectangle r, float fx, float fy) =>
            new PointF(r.X + r.Width * fx, r.Y + r.Height * fy);

        private static PointF[] Pts(Rectangle r, float[,] f)
        {
            var a = new PointF[f.GetLength(0)];
            for (int i = 0; i < a.Length; i++) a[i] = P(r, f[i, 0], f[i, 1]);
            return a;
        }

        private static Pen Stroke(Color c, float w) =>
            new Pen(c, w) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };

        public static void Folder(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pts = Pts(r, new float[,] { { 0.10f, 0.34f }, { 0.38f, 0.34f }, { 0.46f, 0.44f }, { 0.90f, 0.44f }, { 0.90f, 0.78f }, { 0.10f, 0.78f } });
            using (var p = Stroke(c, 1.6f))
            using (var path = new GraphicsPath()) { path.AddPolygon(pts); g.DrawPath(p, path); }
        }

        public static void File(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var body = Pts(r, new float[,] { { 0.26f, 0.18f }, { 0.58f, 0.18f }, { 0.74f, 0.34f }, { 0.74f, 0.82f }, { 0.26f, 0.82f } });
            using (var p = Stroke(c, 1.6f))
            using (var path = new GraphicsPath()) { path.AddPolygon(body); g.DrawPath(p, path); }
            var fold = Pts(r, new float[,] { { 0.58f, 0.18f }, { 0.58f, 0.34f }, { 0.74f, 0.34f } });
            using (var p = Stroke(c, 1.6f)) g.DrawLines(p, fold);
        }

        public static void Chevron(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pts = Pts(r, new float[,] { { 0.34f, 0.42f }, { 0.50f, 0.58f }, { 0.66f, 0.42f } });
            using (var p = Stroke(c, 1.6f)) g.DrawLines(p, pts);
        }

        public static void Check(Graphics g, Rectangle r, Color c, float w = 2f)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pts = Pts(r, new float[,] { { 0.22f, 0.52f }, { 0.42f, 0.72f }, { 0.78f, 0.30f } });
            using (var p = Stroke(c, w)) g.DrawLines(p, pts);
        }

        public static void Plus(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = Stroke(c, 2f))
            {
                g.DrawLine(p, P(r, 0.5f, 0.28f), P(r, 0.5f, 0.72f));
                g.DrawLine(p, P(r, 0.28f, 0.5f), P(r, 0.72f, 0.5f));
            }
        }

        public static void Info(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = Stroke(c, 1.6f))
                g.DrawEllipse(p, r.X + r.Width * 0.10f, r.Y + r.Height * 0.10f, r.Width * 0.80f, r.Height * 0.80f);
            using (var p = Stroke(c, 1.9f)) g.DrawLine(p, P(r, 0.5f, 0.46f), P(r, 0.5f, 0.70f));
            using (var b = new SolidBrush(c)) g.FillEllipse(b, r.X + r.Width * 0.43f, r.Y + r.Height * 0.27f, r.Width * 0.14f, r.Height * 0.14f);
        }

        public static void Warning(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var tri = Pts(r, new float[,] { { 0.50f, 0.15f }, { 0.90f, 0.83f }, { 0.10f, 0.83f } });
            using (var p = Stroke(c, 1.6f))
            using (var path = new GraphicsPath()) { path.AddPolygon(tri); g.DrawPath(p, path); }
            using (var p = Stroke(c, 1.9f)) g.DrawLine(p, P(r, 0.5f, 0.40f), P(r, 0.5f, 0.62f));
            using (var b = new SolidBrush(c)) g.FillEllipse(b, r.X + r.Width * 0.44f, r.Y + r.Height * 0.68f, r.Width * 0.12f, r.Height * 0.12f);
        }

        public static void Success(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = Stroke(c, 1.6f))
                g.DrawEllipse(p, r.X + r.Width * 0.10f, r.Y + r.Height * 0.10f, r.Width * 0.80f, r.Height * 0.80f);
            var pts = Pts(r, new float[,] { { 0.31f, 0.52f }, { 0.44f, 0.65f }, { 0.70f, 0.37f } });
            using (var p = Stroke(c, 1.8f)) g.DrawLines(p, pts);
        }

        public static void Shield(Graphics g, Rectangle r, Color c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pts = Pts(r, new float[,] { { 0.50f, 0.14f }, { 0.84f, 0.26f }, { 0.84f, 0.50f }, { 0.50f, 0.86f }, { 0.16f, 0.50f }, { 0.16f, 0.26f } });
            using (var p = Stroke(c, 1.5f))
            using (var path = new GraphicsPath()) { path.AddPolygon(pts); g.DrawPath(p, path); }
            var chk = Pts(r, new float[,] { { 0.34f, 0.48f }, { 0.45f, 0.59f }, { 0.66f, 0.37f } });
            using (var p = Stroke(c, 1.5f)) g.DrawLines(p, chk);
        }
    }
}
