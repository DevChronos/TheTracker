using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TheTracker
{
    public static class Extensions
    {
        public static List<string> GetValues(this string[] array) => array.Select(element => element.Split('|').FirstOrDefault()).ToList();

        public static bool Contains(this List<string> array, string value) => array.Exists(element => !string.IsNullOrWhiteSpace(value) && element == value);

        public static bool Contains(this string[] array, string value) => array.GetValues().Contains(value);

        public static SharpDX.Color ToDx(this Vector4 color) => new((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255), (int)(color.W * 255));

        public static Vector4 GetColor(this string[] array, string value)
        {
            var element = array.ToList().Find(element => element.Split('|').FirstOrDefault() == value);
            if (string.IsNullOrWhiteSpace(element)) return Colors.White;

            var color = element.Split('|').Skip(1).FirstOrDefault();
            switch (color)
            {
                case "R": return Colors.Red;
                case "O": return Colors.Orange;
                case "Y": return Colors.Yellow;
                case "G": return Colors.Green;
                default:
                    return Colors.White;
            }
        }
    }
}
