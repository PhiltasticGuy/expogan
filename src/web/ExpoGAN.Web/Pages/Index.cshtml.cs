using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ExpoGAN.Web.Data;
using ExpoGAN.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IArtPieceRepository _artPieceRepository;

        public IList<ArtPiece> ArtPieces { get; set; }
        public bool HasArtPieceErrors { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IArtPieceRepository artPieceRepository)
        {
            _logger = logger;
            _artPieceRepository = artPieceRepository;
        }

        public String GetPieceAge(DateTime dateCreated)
        {
            var days = (DateTime.Now - dateCreated).Days;
            
            // Hack to prevent '0 days ago'.
            if (days == 0)
            {
                return $"Today!";
            }
            else if (days == 1)
            {
                return $"{days} day ago";
            }
            else
            {
                return $"{days} days ago";
            }

        }

        public async Task OnGetAsync()
        {
            try
            {
                ArtPieces = await _artPieceRepository.GetArtPieces();
            }
            catch (Exception)
            {
                HasArtPieceErrors = true;
            }
        }
    }
}
