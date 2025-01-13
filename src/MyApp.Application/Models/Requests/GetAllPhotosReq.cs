using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Enums;

namespace MyApp.Application.Models.Requests
{
    public class GetAllPhotosReq{
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}