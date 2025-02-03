using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Application.Models.Requests;
using MyApp.Application.Models.Responses;

namespace MyApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AllPhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IConfiguration _configuration;

        public AllPhotosController(IPhotoService photoService,IConfiguration configuration)
        {
            _photoService = photoService;
            _configuration = configuration;
        }

        [HttpGet ("getPhotos")]
        public async Task<ActionResult<GetAllPhotosRes>> GetAllPhotos([FromQuery] int page =1, [FromQuery] int pageSize = 10)
        {
            var request = new GetAllPhotosReq { Page = page, PageSize = pageSize };
            var result = await _photoService.GetAllPhotos(request);
            return Ok(result);
        }
        [HttpPut("likePhoto")]
        public async Task<ActionResult<LikePhotoRes>> LikePhoto(LikePhotoReq request)
        {
            var result = await _photoService.LikePhoto(request);
            return Ok(result);
        }

    }
}
