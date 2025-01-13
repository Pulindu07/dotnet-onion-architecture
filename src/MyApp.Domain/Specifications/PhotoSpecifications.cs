using MyApp.Domain.Entities;
using MyApp.Domain.Enums;
using MyApp.Domain.Core.Specifications;

namespace MyApp.Domain.Specifications
{
    public static class PhotoSpecifications
    {
        public static BaseSpecification<Photo> GetAllPhotosSpec( int page, int pageSize)
        {
            var spec = new BaseSpecification<Photo>(x => x.PrevUrl != null);
            
            // Apply pagination
            spec.ApplyPaging((page - 1) * pageSize, pageSize);
            
            return spec;
        }
    }
}
