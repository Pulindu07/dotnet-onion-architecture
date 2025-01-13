using System.ComponentModel.DataAnnotations;
using MyApp.Domain.Core.Models;
using MyApp.Domain.Enums;

namespace MyApp.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string PrevUrl { get; set; }
        public required string BlobUrl { get; set; }
        public required string Description { get; set; }
        public int LikeCount { get; set; }    
    }
    
}
