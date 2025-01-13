using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Enums;

namespace MyApp.Application.Models.Requests
{
    public class LikePhotoReq{
        public int Id { get; set; }
        public Boolean HasLiked { get; set; }
    }
}