using System;
using System.Text;
using Cloudora.Puzzle;

namespace Cloudora.Level
{
    // Tahtanın kısa metin hali: bulutlar "/" ile ayrılır, her bulut alttan üste harflerle yazılır.
    // S=Sun R=Rain N=Snow W=Wind M=Moon L=Lightning B=Rainbow. Örnek: "SRSR/RSRS//" (son iki bulut boş).
    public static class V3BoardText
    {
        private const string Letters = "SRNWMLB";

        public static string Format(WeatherType[][] board)
        {
            var builder = new StringBuilder();
            for (int c = 0; c < board.Length; c++)
            {
                if (c > 0) builder.Append('/');
                foreach (WeatherType weather in board[c]) builder.Append(Letters[(int)weather]);
            }
            return builder.ToString();
        }

        public static WeatherType[][] Parse(string text)
        {
            string[] clouds = text.Split('/');
            var board = new WeatherType[clouds.Length][];
            for (int c = 0; c < clouds.Length; c++)
            {
                board[c] = new WeatherType[clouds[c].Length];
                for (int k = 0; k < clouds[c].Length; k++)
                {
                    int index = Letters.IndexOf(clouds[c][k]);
                    if (index < 0) throw new FormatException($"Unknown weather letter '{clouds[c][k]}' in \"{text}\".");
                    board[c][k] = (WeatherType)index;
                }
            }
            return board;
        }
    }
}
