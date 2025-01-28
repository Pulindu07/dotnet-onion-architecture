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

        public AllPhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
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

        [HttpGet ("dummy")]
        public String GetDummy([FromQuery] int page =1, [FromQuery] int pageSize = 10)
        {
            var key = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY"); 
            return "Hi--->"+key;
        }
        
    }
}
