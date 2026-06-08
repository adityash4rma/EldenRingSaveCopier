using System.Drawing;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// Central palette + fonts for the Fluent / Windows 11 DARK theme (Mica) with gold accents.
    /// Mirrors the HTML mockup so the desktop app matches 1:1.
    /// </summary>
    internal static class Theme
    {
        // Surfaces (dark). Window/Client are the Mica fallback base; cards sit slightly lighter.
        public static readonly Color Window     = Color.FromArgb(0x20, 0x20, 0x20);
        public static readonly Color Client     = Color.FromArgb(0x20, 0x20, 0x20);
        public static readonly Color Surface    = Color.FromArgb(0x2B, 0x2B, 0x2B);
        public static readonly Color Surface2   = Color.FromArgb(0x30, 0x30, 0x30);

        // Control fills (dark)
        public static readonly Color ControlFill         = Color.FromArgb(0x2D, 0x2D, 0x2D);
        public static readonly Color ControlFillHover    = Color.FromArgb(0x34, 0x34, 0x34);
        public static readonly Color ControlFillPressed  = Color.FromArgb(0x28, 0x28, 0x28);
        public static readonly Color ControlFillDisabled = Color.FromArgb(0x29, 0x29, 0x29);

        // Lines / borders (dark)
        public static readonly Color Border       = Color.FromArgb(0x3A, 0x3A, 0x3A);
        public static readonly Color BorderStrong = Color.FromArgb(0x4A, 0x4A, 0x4A);

        // Text (dark)
        public static readonly Color Text       = Color.FromArgb(0xF5, 0xF5, 0xF5);
        public static readonly Color Text2      = Color.FromArgb(0xC8, 0xC8, 0xC8);
        public static readonly Color Text3      = Color.FromArgb(0x9A, 0x9A, 0x9A);
        public static readonly Color TextDisabled = Color.FromArgb(0x6A, 0x6A, 0x6A);

        // Gold accent (lightened for dark mode, the way Windows lightens a custom accent)
        public static readonly Color Gold       = Color.FromArgb(0xD4, 0xAF, 0x43);
        public static readonly Color GoldDeep   = Color.FromArgb(0xE7, 0xC8, 0x7A); // light gold for text/icons on dark
        public static readonly Color GoldHi     = Color.FromArgb(0xDD, 0xBB, 0x59);
        public static readonly Color GoldText   = Color.FromArgb(0x2A, 0x21, 0x00); // dark text on the gold button
        public static readonly Color GoldSoft   = Color.FromArgb(0x3A, 0x33, 0x20);

        // Status colors (Fluent InfoBar, dark) — solid tints approximating the translucent mockup
        public static readonly Color Info       = Color.FromArgb(0x60, 0xCD, 0xFF);
        public static readonly Color InfoBg     = Color.FromArgb(0x24, 0x30, 0x39);
        public static readonly Color InfoBorder = Color.FromArgb(0x34, 0x49, 0x5A);

        public static readonly Color Red        = Color.FromArgb(0xFF, 0x99, 0xA4);
        public static readonly Color RedBg      = Color.FromArgb(0x3A, 0x26, 0x29);
        public static readonly Color RedBorder  = Color.FromArgb(0x5A, 0x3A, 0x3E);

        public static readonly Color Green      = Color.FromArgb(0x6C, 0xCB, 0x5F);
        public static readonly Color GreenBg    = Color.FromArgb(0x24, 0x35, 0x24);
        public static readonly Color GreenBorder= Color.FromArgb(0x3A, 0x52, 0x38);

        public static readonly Color Amber      = Color.FromArgb(0xF5, 0xC8, 0x4A);
        public static readonly Color AmberBg    = Color.FromArgb(0x38, 0x31, 0x1E);

        // Fonts (Segoe UI Variable falls back to Segoe UI on < Win11)
        private const string Family = "Segoe UI";
        public static Font H1     = new Font(Family, 13.5f, FontStyle.Bold);
        public static Font H2     = new Font(Family, 10.5f, FontStyle.Bold);
        public static Font Body   = new Font(Family, 9.75f, FontStyle.Regular);
        public static Font Small   = new Font(Family, 8.25f, FontStyle.Regular);
        public static Font Label  = new Font(Family, 9.0f,  FontStyle.Regular);
        public static Font Button = new Font(Family, 11.5f, FontStyle.Bold);
        public static Font Badge  = new Font(Family, 9.0f,  FontStyle.Bold);
    }
}
