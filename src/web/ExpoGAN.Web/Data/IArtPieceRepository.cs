using ExpoGAN.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Data
{
    public interface IArtPieceRepository
    {
        Task CreateAsync(ArtPiece piece);
        Task<IList<ArtPiece>> GetArtPieces();
        Task<ArtPiece> GetArtPieceById(Guid id);
    }
}