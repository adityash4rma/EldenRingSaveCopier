using System.Drawing;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// Central palette + fonts for the Fluent / Windows 11 DARK theme (Mica) with gold accents.
    /// Values are taken straight from the HTML mockup so the desktop app matches 1:1.
    /// </summary>
    internal static class Theme
    {
        // Surfaces (dark). Window/Client are the Mica fallback base; cards sit slightly lighter.
        public static readonly Color Window     = Color.FromArgb(0x1C, 0x1C, 0x1E);
        public static readonly Color Client     = Color.FromArgb(0x1C, 0x1C, 0x1E);
        public static readonly Color Surface    = Color.FromArgb(0x2A, 0x2A, 0x2C);
        public static readonly Color Surface2   = Color.FromArgb(0x30, 0x30, 0x33);
        public static readonly Color SummaryBg  = Color.FromArgb(0x2E, 0x2E, 0x31);

        // Control fills (dark)
        public static readonly Color ControlFill         = Color.FromArgb(0x2F, 0x2F, 0x32);
        public static readonly Color ControlFillHover    = Color.FromArgb(0x38, 0x38, 0x3B);
        public static readonly Color ControlFillPressed  = Color.FromArgb(0x2A, 0x2A, 0x2D);
        public static readonly Color ControlFillDisabled = Color.FromArgb(0x29, 0x29, 0x2B);

        // Lines / borders (dark)
        public static readonly Color Border       = Color.FromArgb(0x3A, 0x3A, 0x3D);
        public static readonly Color BorderStrong = Color.FromArgb(0x4A, 0x4A, 0x4E);

        // Text (dark)
        public static readonly Color Text         = Color.FromArgb(0xF7, 0xF7, 0xF8);
        public static readonly Color Text2        = Color.FromArgb(0xCB, 0xCB, 0xCE);
        public static readonly Color Text3        = Color.FromArgb(0x95, 0x95, 0x9A);
        public static readonly Color TextDisabled = Color.FromArgb(0x66, 0x66, 0x6B);

        // Gold accent (lightened for dark mode, the way Windows lightens a custom accent)
        public static readonly Color Gold     = Color.FromArgb(0xD4, 0xAF, 0x43);
        public static readonly Color GoldDeep = Color.FromArgb(0xE7, 0xC8, 0x7A); // light gold for text/icons on dark
        public static readonly Color GoldHi   = Color.FromArgb(0xDD, 0xBB, 0x59);
        public static readonly Color GoldText = Color.FromArgb(0x2A, 0x21, 0x00); // dark text on the gold button
        public static readonly Color GoldSoft = Color.FromArgb(0x3B, 0x34, 0x22); // accent-subtle fill

        // Status colors (Fluent InfoBar, dark) — solid tints approximating the translucent mockup
        public static readonly Color Info       = Color.FromArgb(0x60, 0xCD, 0xFF);
        public static readonly Color InfoBg     = Color.FromArgb(0x22, 0x2C, 0x33);
        public static readonly Color InfoBorder = Color.FromArgb(0x32, 0x44, 0x52);

        public static readonly Color Red       = Color.FromArgb(0xFF, 0x99, 0xA4);
        public static readonly Color RedBg     = Color.FromArgb(0x33, 0x23, 0x26);
        public static readonly Color RedBorder = Color.FromArgb(0x53, 0x37, 0x3B);

        public static readonly Color Green       = Color.FromArgb(0x6C, 0xCB, 0x5F);
        public static readonly Color GreenBg     = Color.FromArgb(0x22, 0x30, 0x21);
        public static readonly Color GreenBorder = Color.FromArgb(0x37, 0x4C, 0x34);

        public static readonly Color Amber       = Color.FromArgb(0xF5, 0xC8, 0x4A);
        public static readonly Color AmberBg     = Color.FromArgb(0x33, 0x2D, 0x1C);
        public static readonly Color AmberBorder = Color.FromArgb(0x4C, 0x40, 0x22);

        // Tag pill fills
        public static readonly Color TagAmberBg = Color.FromArgb(0x3E, 0x35, 0x1F);
        public static readonly Color TagGreenBg = Color.FromArgb(0x24, 0x3A, 0x22);

        // Avatar / summary accents
        public static readonly Color AvatarBg      = Color.FromArgb(0x3B, 0x34, 0x22);
        public static readonly Color AvatarGreenBg = Color.FromArgb(0x24, 0x3A, 0x22);

        // Fonts (Segoe UI is always present on Win10/11 and matches the mockup's ramp; px*0.75 = pt)
        private const string Family = "Segoe UI";
        public static readonly Font H1       = new Font(Family, 15f,   FontStyle.Bold);    // 20px title
        public static readonly Font H2       = new Font(Family, 10.5f, FontStyle.Bold);    // 14px card heading
        public static readonly Font Body     = new Font(Family, 9.75f, FontStyle.Regular); // 13px body
        public static readonly Font Meta     = new Font(Family, 9f,    FontStyle.Regular); // 12px secondary
        public static readonly Font Label    = new Font(Family, 9f,    FontStyle.Bold);    // 12.5px field labels (600)
        public static readonly Font NameBold = new Font(Family, 9.75f, FontStyle.Bold);    // 13px slot name (600)
        public static readonly Font Button   = new Font(Family, 10.5f, FontStyle.Bold);    // 14px copy action
        public static readonly Font Badge    = new Font(Family, 9.75f, FontStyle.Bold);    // step number
        public static readonly Font Tag      = new Font(Family, 8.25f, FontStyle.Bold);    // 11.5px pill
        public static readonly Font Mono     = new Font("Consolas", 8.5f, FontStyle.Regular);
    }
}
