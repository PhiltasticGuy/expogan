using ExpoGAN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _ = await SeedArtPiecesAsync(context);

            context.SaveChanges();
        }

        private static async Task<IList<ArtPiece>> SeedArtPiecesAsync(ApplicationDbContext context)
        {
            if (context.ArtPieces.Any())
            {
                return Array.Empty<ArtPiece>();
            }
            else
            {
                var pieces = new List<ArtPiece>()
                {
                    new ArtPiece(
                        Guid.Parse("958DDBA8-6980-4847-9B75-140A697BD34E"),
                        "958DDBA8-6980-4847-9B75-140A697BD34E-samples-futurama-fry.jpg",
                        DateTime.Now
                    ),
                    new ArtPiece(
                        Guid.Parse("E2B342B9-BC86-4C8F-8674-9C8B0CC2C189"),
                        "E2B342B9-BC86-4C8F-8674-9C8B0CC2C189-samples-futurama-fry.jpg",
                        DateTime.Now.AddDays(-1)
                    ),
                    new ArtPiece(
                        Guid.Parse("0004D394-CE2B-494A-B4B6-E562D4BB5C4E"),
                        "0004D394-CE2B-494A-B4B6-E562D4BB5C4E-samples-futurama-fry.jpg",
                        DateTime.Now.AddDays(-2)
                    ),
                    new ArtPiece(
                        Guid.Parse("C8DAEFA9-DD10-468A-9E90-6A92700C24F0"),
                        "C8DAEFA9-DD10-468A-9E90-6A92700C24F0-samples-futurama-fry.jpg",
                        DateTime.Now.AddDays(-3)
                    ),
                    new ArtPiece(
                        Guid.Parse("18113734-FE59-4D5C-970F-084B1B1419D5"),
                        "18113734-FE59-4D5C-970F-084B1B1419D5-samples-futurama-fry.jpg",
                        DateTime.Now.AddDays(-3)
                    ),
                    new ArtPiece(
                        Guid.Parse("2880540B-885B-4D5A-B12E-585F6F0AA035"),
                        "2880540B-885B-4D5A-B12E-585F6F0AA035-samples-pablo-picasso.jpg",
                        DateTime.Now.AddDays(-4)
                    ),
                    new ArtPiece(
                        Guid.Parse("2A7FFE81-D4A8-4FB9-847D-80987D9CEF3A"),
                        "2A7FFE81-D4A8-4FB9-847D-80987D9CEF3A-samples-pablo-picasso.jpg",
                        DateTime.Now.AddDays(-5)
                    )
                };

                pieces.ForEach(async _ => await context.ArtPieces.AddAsync(_));

                return pieces;
            }
        }
    }
}
