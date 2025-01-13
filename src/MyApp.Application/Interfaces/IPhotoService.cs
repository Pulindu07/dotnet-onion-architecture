using MyApp.Application.Models.Requests;
using MyApp.Application.Models.Responses;

namespace MyApp.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<GetAllPhotosRes> GetAllPhotos(GetAllPhotosReq req);
        Task<LikePhotoRes> LikePhoto(LikePhotoReq req);
    }
}