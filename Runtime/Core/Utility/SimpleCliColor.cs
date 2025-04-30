using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCLI.Runtime.Core.Utility
{
    public struct SimpleCliColor : IEquatable<SimpleCliColor>
    {
        public float A;
        public float R;
        public float G;
        public float B;

        public SimpleCliColor(float a, float r, float g, float b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public SimpleCliColor(float r, float g, float b)
        {
            this.A = 1;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public SimpleCliColor(string hex)
        {
            var temp = FromHex(hex);
            A = temp.A;
            R = temp.R;
            G = temp.G;
            B = temp.B;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is SimpleCliColor color && Equals(color);

        public bool Equals(SimpleCliColor other) =>
            this.A == other.A &&
            this.R == other.R &&
            this.G == other.G &&
            this.B == other.B;

        public override int GetHashCode() =>
            HashCode.Combine(A, R, G, B);

        public static bool operator ==(SimpleCliColor left, SimpleCliColor right) =>
            left.Equals(right);

        public static bool operator !=(SimpleCliColor left, SimpleCliColor right) =>
            !left.Equals(right);

        // Common Colors
        public static SimpleCliColor White => new(1f, 1f, 1f);
        public static SimpleCliColor Black => new(0f, 0f, 0f);
        public static SimpleCliColor Red => new(1f, 0f, 0f);
        public static SimpleCliColor Green => new(0f, 1f, 0f);
        public static SimpleCliColor Blue => new(0f, 0f, 1f);
        public static SimpleCliColor Yellow => new(1f, 1f, 0f);
        public static SimpleCliColor Magenta => new(1f, 0f, 1f);
        public static SimpleCliColor Cyan => new(0f, 1f, 1f);

        // Hex Conversion
        public static SimpleCliColor FromHex(string hex)
        {
            if (hex.StartsWith("#")) hex = hex.Substring(1);

            if (hex.Length == 3 || hex.Length == 4)
            {
                string expanded = "";
                for (int i = 0; i < hex.Length; i++)
                    expanded += hex[i] + hex[i].ToString();
                hex = expanded;
            }

            if (hex.Length != 6 && hex.Length != 8)
                throw new ArgumentException("Hex string must be 6 (RGB) or 8 (RGBA) characters long", nameof(hex));

            try
            {
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                int a = hex.Length >= 8 ? Convert.ToInt32(hex.Substring(6, 2), 16) : 255;

                return new SimpleCliColor(
                    a: a / 255f,
                    r: r / 255f,
                    g: g / 255f,
                    b: b / 255f
                );
            }
            catch (FormatException)
            {
                throw new ArgumentException("Hex string contains invalid characters", nameof(hex));
            }
        }

        public string ToHex()
        {
            int r = (int)Math.Round(this.R * 255);
            int g = (int)Math.Round(this.G * 255);
            int b = (int)Math.Round(this.B * 255);
            int a = (int)Math.Round(this.A * 255);

            return this.A < 1f 
                ? $"#{r:X2}{g:X2}{b:X2}{a:X2}" 
                : $"#{r:X2}{g:X2}{b:X2}";
        }

        // Utility Methods
        public SimpleCliColor Clamp()
        {
            return new SimpleCliColor(
                Math.Clamp(A, 0f, 1f),
                Math.Clamp(R, 0f, 1f),
                Math.Clamp(G, 0f, 1f),
                Math.Clamp(B, 0f, 1f)
            );
        }

        public static SimpleCliColor Lerp(SimpleCliColor a, SimpleCliColor b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return new SimpleCliColor(
                a.A + (b.A - a.A) * t,
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t
            );
        }

        public SimpleCliColor Invert() =>
            new SimpleCliColor(A, 1f - R, 1f - G, 1f - B);

        public SimpleCliColor WithAlpha(float alpha) =>
            new SimpleCliColor(alpha, R, G, B);

        public override string ToString() =>
            $"RGBA({R:F3}, {G:F3}, {B:F3}, {A:F3})";
    }
}