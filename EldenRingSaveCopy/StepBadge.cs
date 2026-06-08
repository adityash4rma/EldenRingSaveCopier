using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>Numbered step indicator: grey (pending), gold ring (active) or filled gold check (done).</summary>
    public class StepBadge : Control
    {
        public enum BadgeState { Pending, Active, Done }

        private BadgeState _state = BadgeState.Pending;
        public BadgeState State { get => _state; set { _state = value; Invalidate(); } }
        public int Number { get; set; } = 1;

        public StepBadge()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(26, 26);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(1, 1, Width - 3, Height - 3);

            switch (_state)
            {
                case BadgeState.Done:
                    using (var b = new SolidBrush(Theme.Gold)) g.FillEllipse(b, r);
                    using (var p = new Pen(Theme.GoldText, 2.2f))
                    {
                        g.DrawLines(p, new[]
                        {
                            new PointF(r.Left + r.Width*0.28f, r.Top + r.Height*0.52f),
                            new PointF(r.Left + r.Width*0.44f, r.Top + r.Height*0.68f),
                            new PointF(r.Left + r.Width*0.72f, r.Top + r.Height*0.34f)
                        });
                    }
                    break;
                case BadgeState.Active:
                    using (var b = new SolidBrush(Theme.Surface)) g.FillEllipse(b, r);
                    using (var p = new Pen(Theme.Gold, 2f)) g.DrawEllipse(p, r);
                    TextRenderer.DrawText(g, Number.ToString(), Theme.Badge, r, Theme.GoldDeep,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    break;
                default:
                    using (var b = new SolidBrush(Color.FromArgb(0x33, 0x33, 0x33))) g.FillEllipse(b, r);
                    TextRenderer.DrawText(g, Number.ToString(), Theme.Badge, r, Theme.Text3,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    break;
            }
        }
    }
}
