using System.Globalization;

namespace VideoConverter.Utils
{
    public static class MathExt
    {
        public static double? ParseDouble(string? input)
        {
            return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double result)
                ? result
                : null;
        }

        public static double ParseDouble(string? input, double defaultValue)
        {
            return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double result)
                ? result
                : defaultValue;
        }

        public static long? ParseLong(string? input)
        {
            return long.TryParse(input, out long result)
                ? result
                : null;
        }

        public static long ParseLong(string? input, long defaultValue)
        {
            return long.TryParse(input, out long result)
                ? result
                : defaultValue;
        }

        public static int? ParseInt(string? input)
        {
            return int.TryParse(input, out int result)
                ? result
                : null;
        }

        public static int ParseInt(string? input, int defaultValue)
        {
            return int.TryParse(input, out int result)
                ? result
                : defaultValue;
        }

        private static int HexToDec(char input)
        {
            if (47 < input && input < 58) return input - 48; // numerals
            if (64 < input && input < 71) return input - 55; // lowercase
            if (96 < input && input < 103) return input - 87; // uppercase
            return 0;
        }

        public static Color ParseHexColor(string hex)
        {
            if (hex.Length == 7 && hex[0] == '#')
            {
                return Color.FromArgb(
                    (HexToDec(hex[1]) << 4) | HexToDec(hex[2]),
                    (HexToDec(hex[3]) << 4) | HexToDec(hex[4]),
                    (HexToDec(hex[5]) << 4) | HexToDec(hex[6])
                );
            }
            return Color.Black;
        }

        private static readonly char[] HexNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        public static string ColorToHex(Color color)
        {
            return string.Format("#{0}{1}{2}{3}{4}{5}",
                HexNumbers[color.R >> 4], HexNumbers[color.R & 0xF],
                HexNumbers[color.G >> 4], HexNumbers[color.G & 0xF],
                HexNumbers[color.B >> 4], HexNumbers[color.B & 0xF]
            );
        }
    }
}
