using ExpoGAN.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExpoGAN.Web.Logic
{
    public static class ArtPieceListExtensions
    {
        public static IEnumerable<IEnumerable<ArtPiece>> Split(this IList<ArtPiece> pieces, int columns)
        {
            for (int i = 0; i < (float)pieces.Count / columns; i++)
            {
                yield return pieces.Skip(i * columns).Take(columns);
            }
        }
    }
}
