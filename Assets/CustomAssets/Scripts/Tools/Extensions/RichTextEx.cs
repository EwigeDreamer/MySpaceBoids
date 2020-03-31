namespace MyTools.Extensions.RichText
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Text;

    public struct RichTextPointer
    {
        public string str;
        public override string ToString() => str;
    }

    public static class RichTextEx
    {
        public static RichTextPointer RichText(this string str) => new RichTextPointer { str = str };

        static StringBuilder m_SB = new StringBuilder();

        const string colorize1 = "<color=";
        const string colorize2 = ">";
        const string uncolorize = "</color>";
        public static RichTextPointer Colorize(this RichTextPointer pointer, Color color, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + colorize1.Length + colorize2.Length + uncolorize.Length + 7);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(colorize1);
            sb.Append('#');
            sb.Append(ColorUtility.ToHtmlStringRGB(color));
            sb.Append(colorize2);
            sb.Append(str);
            sb.Append(uncolorize);
            return new RichTextPointer { str = sb.ToString() };
        }

        const string bold = "<b>";
        const string unbold = "</b>";
        public static RichTextPointer Bold(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + bold.Length + unbold.Length);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(bold);
            sb.Append(str);
            sb.Append(unbold);
            return new RichTextPointer { str = sb.ToString() };
        }

        const string italic = "<i>";
        const string unitalic = "</i>";
        public static RichTextPointer Italic(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + italic.Length + unitalic.Length);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(italic);
            sb.Append(str);
            sb.Append(unitalic);
            return new RichTextPointer { str = sb.ToString() };
        }

        const string underline = "<u>";
        const string ununderline = "</u>";
        public static RichTextPointer Underline(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + underline.Length + ununderline.Length);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(underline);
            sb.Append(str);
            sb.Append(ununderline);
            return new RichTextPointer { str = sb.ToString() };
        }

        const string superscript = "<sup>";
        const string unsuperscript = "</sup>";
        public static RichTextPointer Superscript(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + superscript.Length + unsuperscript.Length);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(superscript);
            sb.Append(str);
            sb.Append(unsuperscript);
            return new RichTextPointer { str = sb.ToString() };
        }

        const string subscript = "<sub>";
        const string unsubscript = "</sub>";
        public static RichTextPointer Subscript(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            StringBuilder sb;
            if (threadSafe) sb = new StringBuilder(str.Length + subscript.Length + unsubscript.Length);
            else { sb = m_SB; sb.Clear(); }
            sb.Append(subscript);
            sb.Append(str);
            sb.Append(unsubscript);
            return new RichTextPointer { str = sb.ToString() };
        }

        public static RichTextPointer InBlack(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 0, 0, 255), threadSafe);
        public static RichTextPointer InBlue(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 0, 255, 255), threadSafe);
        public static RichTextPointer InBrown(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(150, 75, 0, 255), threadSafe);
        public static RichTextPointer InCyan(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 255, 255, 255), threadSafe);
        public static RichTextPointer InGray(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(128, 128, 128, 255), threadSafe);
        public static RichTextPointer InGreen(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 128, 0, 255), threadSafe);
        public static RichTextPointer InLime(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 255, 0, 255), threadSafe);
        public static RichTextPointer InMagenta(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(255, 0, 255, 255), threadSafe);
        public static RichTextPointer InMaroon(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(128, 0, 0, 255), threadSafe);
        public static RichTextPointer InMustard(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(255, 219, 88, 255), threadSafe);
        public static RichTextPointer InNavy(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 0, 128, 255), threadSafe);
        public static RichTextPointer InOchre(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(204, 119, 34, 255), threadSafe);
        public static RichTextPointer InOlive(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(128, 128, 0, 255), threadSafe);
        public static RichTextPointer InPurple(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(128, 0, 128, 255), threadSafe);
        public static RichTextPointer InRed(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(255, 0, 0, 255), threadSafe);
        public static RichTextPointer InSilver(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(192, 192, 192, 255), threadSafe);
        public static RichTextPointer InTeal(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(0, 128, 128, 255), threadSafe);
        public static RichTextPointer InWhite(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(255, 255, 255, 255), threadSafe);
        public static RichTextPointer InYellow(this RichTextPointer pointer, bool threadSafe = false)
        => pointer.Colorize(new Color32(255, 0, 255, 255), threadSafe);

        static Color[] m_Rainbow = new Color[]
        {
            new Color32(255, 0, 0, 255),
            new Color32(255, 128, 0, 255),
            new Color32(255, 255, 0, 255),
            new Color32(0, 255, 0, 255),
            new Color32(0, 191, 255, 255),
            new Color32(0, 0, 255, 255),
            new Color32(90, 0, 157, 255),
        };
        public static RichTextPointer Rainbow(this RichTextPointer pointer, bool threadSafe = false)
        {
            var str = pointer.str;
            System.Text.StringBuilder sb;
            if (threadSafe) sb = new System.Text.StringBuilder(str.Length + bold.Length + unbold.Length);
            else { sb = m_SB; sb.Clear(); }
            int strCount = str.Length;
            var rnbw = m_Rainbow;
            int rnbwCount = rnbw.Length;
            for (int i = 0; i < strCount; ++i)
            {
                sb.Append(colorize1);
                sb.Append('#');
                sb.Append(ColorUtility.ToHtmlStringRGB(rnbw[i % rnbwCount]));
                sb.Append(colorize2);
                sb.Append(str[i]);
                sb.Append(uncolorize);
            }
            return new RichTextPointer { str = sb.ToString() };
        }
    }
}
