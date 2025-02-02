using MyApp.Application.Models.Requests;
using MyApp.Application.Models.Responses;
using MyApp.Domain.Specifications;
using MyApp.Domain.Entities;
using MyApp.Application.Models.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Application.Core.Services;
using MyApp.Domain.Core.Repositories;

namespace MyApp.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotos _photos;
        private readonly ILoggerService _loggerService;

        public PhotoService(IPhotos photos, ILoggerService loggerService)
        {
            _photos = photos;
            _loggerService = loggerService;
        }

        public async Task<GetAllPhotosRes> GetAllPhotos(GetAllPhotosReq req)
        {
            var photoSpec = PhotoSpecifications.GetPaginatedPhotosSpec(req.Page, req.PageSize);
            var allPhotoSpec = PhotoSpecifications.GetAllPhotosSpec();
        
            // Get paginated results
            var photos = await _photos.Repository<Photo>().ListAsync(photoSpec);
            
            // Get total count for pagination
            var totalCount = await _photos.Repository<Photo>().CountAsync(allPhotoSpec);
            
            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)req.PageSize);
            
            return new GetAllPhotosRes
                {
                    Photos = photos.Select(x => new PhotoDTO(x)).ToList(),
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasMore = req.Page < totalPages
                };
        }
        public async Task<LikePhotoRes> LikePhoto(LikePhotoReq req){
            
            var photoSpec = PhotoSpecifications.GetPhotoById(req.Id, req.HasLiked);
 
            // Get paginated results
            var photo = await _photos.Repository<Photo>().FirstOrDefaultAsync(photoSpec);
            if (photo == null){
                return new LikePhotoRes{
                    Id=req.Id,
                    HasLiked=false,
                    LikeCount=0
                };
            }

            if (req.HasLiked)
            {
                photo.LikeCount += 1;
            }
            else
            {
                // Ensure like count doesn't go below 0
                photo.LikeCount = Math.Max(0, photo.LikeCount - 1);
            }
            
            // Save changes
            await _photos.Repository<Photo>().UpdateAsync(photo);

            return new LikePhotoRes{
                Id = req.Id,
                HasLiked = req.HasLiked,
                LikeCount = photo.LikeCount
            };
           
        }
    }
}