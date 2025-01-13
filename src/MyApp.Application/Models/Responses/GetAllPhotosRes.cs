using MyApp.Application.Models.DTOs;

namespace MyApp.Application.Models.Responses
{
    public class GetAllPhotosRes{
        public required List<PhotoDTO> Photos { get; set; }
        public Boolean HasMore { get; set; } = false;
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}