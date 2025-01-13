using MyApp.Application.Models.DTOs;

namespace MyApp.Application.Models.Responses
{
    public class LikePhotoRes{
        public int Id { get; set; }
        public Boolean HasLiked { get; set; }

        public int LikeCount { get; set; }
    }
}