using System;
using System.IO;
using System.Windows.Media;

namespace Client
{
    public class MaterialDesignFonts
    {
        public static FontFamily CustomFont { get; }
        public static FontFamily IconFont { get; }

        static MaterialDesignFonts()
        {
            var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\fonts\");
            CustomFont = new FontFamily(new Uri($"file:///{fontPath}"), "./#Source Han Sans SC");

            var iconFontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\fonts\");
            IconFont = new FontFamily(new Uri($"file:///{iconFontPath}"), "./#iconfont");
        }
    }
}
