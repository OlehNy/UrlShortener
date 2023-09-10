using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : BaseController
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("{shortUrl}")]
        public IActionResult RedirectToOriginalUrl(string shortUrl)
        {
            var urlModel = _urlService.GetUrl(shortUrl);

            if (urlModel == null)
            {
                return NotFound();
            }

            return Ok(urlModel.OriginalURL);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUrl([FromBody] UrlCreationDto urlCreationDto)
        {
            await _urlService.CreateUrl(UserId.ToString(), urlCreationDto.OriginalUrl);
        
            return Ok();
        }

        [HttpGet]
        public async Task<IReadOnlyList<URL>> GetUrls()
            => await _urlService.GetAll();

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            await _urlService.DeleteUrl(id);

            return Ok();
        }
    }
}
