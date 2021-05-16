using Microsoft.EntityFrameworkCore;
using ExpoGAN.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Data
{
    public class ArtPieceRepository : IArtPieceRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtPieceRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task CreateAsync(ArtPiece piece)
        {
            _context.ArtPieces.Add(piece);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<ArtPiece>> GetArtPieces()
        {
            return await _context.ArtPieces
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<ArtPiece> GetArtPieceById(Guid id)
        {
            return await _context.ArtPieces
                .AsNoTracking()
                .SingleOrDefaultAsync(_ => _.Id == id)
                .ConfigureAwait(false);
        }
    }
}
