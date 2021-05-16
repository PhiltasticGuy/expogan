using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExpoGAN.Web.Data;
using ExpoGAN.Web.Models;
using ExpoGAN.Web.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ExpoGAN.Web.Pages
{
    public class SimpsonifyModel : PageModel
    {
        public const int DefaultMaxColors = 3;

        private readonly ILogger<PageModel> _logger;
        private readonly IHostEnvironment _environment;
        private readonly EmojisService _recolorService;
        private readonly IArtPieceRepository _artPieceRepository;

        public bool GetMessageError { get; set; }
        public string Message { get; set; }

        public bool PostImageUrlError { get; set; }

        [BindProperty]
        public string ImageUrl { get; set; }

        [BindProperty]
        public int? MaxColors { get; set; } = DefaultMaxColors;

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public SimpsonifyModel(ILogger<PageModel> logger, IHostEnvironment environment, EmojisService recolorService, IArtPieceRepository artPieceRepository)
        {
            _logger = logger;
            _environment = environment;
            _recolorService = recolorService;
            _artPieceRepository = artPieceRepository;
        }

        public async Task OnGetAsync()
        {
            try
            {
                //Message = await _recolorService.GetMessageAsync();
            }
            catch(HttpRequestException)
            {
                GetMessageError = true;
                Message = string.Empty;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(ImageUrl))
            //    {
            //        var location = await _recolorService.SendImageUrlAsync(
            //            new Uri(ImageUrl),
            //            MaxColors.Value
            //        );
            //        Message = location?.ToString();
            //        Response.Redirect("http://localhost:5000" + location.PathAndQuery);
            //        var queryString = HttpUtility.ParseQueryString(location.Query);
            //        var filename = queryString["filename"];
            //        var guidPart = filename.Substring(0, 36);

            //        var piece = new ArtPiece(Guid.Parse(guidPart), filename, DateTime.Now);
            //        await _artPieceRepository.CreateAsync(piece);

            //        return RedirectToPage("/ArtPieces/Details", new { id = piece.Id });
            //    }
            //    else
            //    {
            //        if (UploadedFile == null || UploadedFile.Length == 0)
            //        {
            //            return BadRequest();
            //        }

            //        using var fileStream = UploadedFile.OpenReadStream();
            //        byte[] bytes = new byte[UploadedFile.Length];
            //        await fileStream.ReadAsync(bytes.AsMemory(0, (int)UploadedFile.Length));

            //        var piece = new ArtPiece(UploadedFile.FileName);

            //        var location = await _recolorService.SendImageFileAsync(
            //            piece.Filename,
            //            bytes,
            //            MaxColors.Value
            //        );

            //        await _artPieceRepository.CreateAsync(piece);

            //        Message = location?.ToString();

            //        return RedirectToPage("/ArtPieces/Details", new { id = piece.Id });
            //    }
            //}
            //catch (HttpRequestException ex)
            //{
            //    PostImageUrlError = true;
            //    Message = ex.Message;
            //}

            return StatusCode(200);
        }
    }
}
