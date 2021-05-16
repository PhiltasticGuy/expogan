using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpoGAN.Web.Data;
using ExpoGAN.Web.Models;

namespace ExpoGAN.Web.Pages.Mergan
{
    public class DetailsModel : PageModel
    {
        private readonly IArtPieceRepository _artPieceRepository;

        public ArtPiece ArtPiece { get; set; }

        public DetailsModel(IArtPieceRepository artPieceRepository)
        {
            _artPieceRepository = artPieceRepository;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            ArtPiece = await _artPieceRepository.GetArtPieceById(id);

            if (ArtPiece == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
