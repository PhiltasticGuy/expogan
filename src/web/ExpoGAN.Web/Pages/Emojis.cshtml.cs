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
using System.IO;

namespace ExpoGAN.Web.Pages
{
    //https://rvera.github.io/image-picker/
    public class EmojisModel : PageModel
    {
        public const int DefaultMaxColors = 3;

        private readonly ILogger<PageModel> _logger;
        private readonly IHostEnvironment _environment;
        private readonly EmojisService _recolorService;
        private readonly IArtPieceRepository _artPieceRepository;

        public string EmojisServiceGuid
        { 
            get => HttpContext.Session.GetString(nameof(EmojisServiceGuid)); 
            set => HttpContext.Session.SetString(nameof(EmojisServiceGuid), value);
        }
        public bool EmojisExist
        {
            get => HttpContext.Session.GetBoolean(nameof(EmojisExist)) == true;
            set => HttpContext.Session.SetBoolean(nameof(EmojisExist), value);
        }
        public string FirstEmojiId
        {
            get => HttpContext.Session.GetString(nameof(FirstEmojiId));
            set => HttpContext.Session.SetString(nameof(FirstEmojiId), value);
        }
        public string SecondEmojiId
        {
            get => HttpContext.Session.GetString(nameof(SecondEmojiId));
            set => HttpContext.Session.SetString(nameof(SecondEmojiId), value);
        }
        public bool InterpolationExists
        {
            get => HttpContext.Session.GetBoolean(nameof(InterpolationExists)) == true;
            set => HttpContext.Session.SetBoolean(nameof(InterpolationExists), value);
        }

        public bool GenerateEmojisError { get; set; }
        public bool PickEmojisError { get; set; }
        public bool InterpolateEmojisError { get; set; }
        public string Message { get; set; }

        [BindProperty]
        public int[] SelectedEmojis { get; set; }

        public EmojisModel(ILogger<PageModel> logger, IHostEnvironment environment, EmojisService recolorService, IArtPieceRepository artPieceRepository)
        {
            _logger = logger;
            _environment = environment;
            _recolorService = recolorService;
            _artPieceRepository = artPieceRepository;
        }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(EmojisServiceGuid))
            {
                EmojisServiceGuid = Guid.NewGuid().ToString();
            }
        }

        public async Task<IActionResult> OnPostGenerateAsync()
        {
            EmojisExist = false;
            FirstEmojiId = string.Empty;
            SecondEmojiId = string.Empty;
            InterpolationExists = false;

            try
            {
                var location = await _recolorService.GenerateEmojisAsync(EmojisServiceGuid);
                Console.WriteLine(location);
                EmojisExist = true;
                return Page();
            }
            catch (HttpRequestException ex)
            {
                GenerateEmojisError = true;
                Message = ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostPickAsync()
        {
            if (SelectedEmojis.Length < 2)
            {
                PickEmojisError = true;
                Message = "You must select 2 emojis before proceeding!";
                return Page();
            }

            FirstEmojiId = SelectedEmojis[0].ToString();
            SecondEmojiId = SelectedEmojis[1].ToString();

            try
            {
                var location = await _recolorService.InterpolateEmojisAsync(EmojisServiceGuid, FirstEmojiId, SecondEmojiId);
                Console.WriteLine(location);
                InterpolationExists = true;
                return Page();
            }
            catch (HttpRequestException ex)
            {
                InterpolateEmojisError = true;
                Message = ex.Message;
            }

            return Page();
        }
    }
}
