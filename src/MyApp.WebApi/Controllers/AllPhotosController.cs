using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Application.Models.Requests;
using MyApp.Application.Models.Responses;

namespace MyApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/getPhotos")]
    public class AllPhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public AllPhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<GetAllPhotosRes>> CreateUser([FromQuery] int page =1, [FromQuery] int pageSize = 10)
        {
            var request = new GetAllPhotosReq { Page = page, PageSize = pageSize };
            var result = await _photoService.GetAllPhotos(request);
            return Ok(result);
        }
    }
}
