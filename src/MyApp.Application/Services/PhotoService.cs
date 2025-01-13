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
            var photoSpec = PhotoSpecifications.GetAllPhotosSpec(req.Page, req.PageSize);
        
        // Get paginated results
        var photos = await _photos.Repository<Photo>().ListAsync(photoSpec);
        
        // Get total count for pagination
        var totalCount = await _photos.Repository<Photo>().CountAsync(photoSpec);
        
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
    }
}