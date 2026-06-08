using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EldenRingSaveCopy
{
    /// <summary>
    /// Enables the Windows 11 Mica backdrop + dark title bar via DWM.
    /// Mica blurs the desktop wallpaper into a subtle tint behind the window —
    /// exactly the effect shown in the HTML mockup. Safe no-op on older Windows.
    /// </summary>
    internal static class DarkMica
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        // DWMWINDOWATTRIBUTE values (Windows 11 build 22000+)
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20; // dark title bar
        private const int DWMWA_SYSTEMBACKDROP_TYPE      = 38; // 2 = Mica, 3 = Acrylic, 4 = Tabbed
        private const int DWMSBT_MAINWINDOW              = 2;  // Mica

        public static void Apply(Form form)
        {
            try
            {
                IntPtr h = form.Handle;

                int dark = 1;
                DwmSetWindowAttribute(h, DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));

                int backdrop = DWMSBT_MAINWINDOW;
                DwmSetWindowAttribute(h, DWMWA_SYSTEMBACKDROP_TYPE, ref backdrop, sizeof(int));

                // Mica shows through where the window background is transparent.
                // TransparencyKey lets the form's base color read as the Mica material.
                form.BackColor = Theme.Window;
            }
            catch
            {
                // Pre-Windows-11: silently fall back to the solid dark BackColor.
            }
        }
    }
}
