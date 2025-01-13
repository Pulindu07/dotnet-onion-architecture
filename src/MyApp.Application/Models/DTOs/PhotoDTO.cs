using MyApp.Domain.Entities;

namespace MyApp.Application.Models.DTOs
{
    public class PhotoDTO
    {
        public  string Id { get; set; }
        public  string FileName { get; set; }
        public  string Url { get; set; }
        public  string PrevUrl { get; set; }
        public  int LikeCount { get; set; }
        public  string Description { get; set; }

        public PhotoDTO (Photo photo){
            Id = photo.Id.ToString();
            FileName = photo.FileName;
            PrevUrl = photo.PrevUrl;
            Url = photo.BlobUrl;
            Description = photo.Description;
            LikeCount = photo.LikeCount;
        }
    }
}