using MyApp.Domain.Core.Models;

namespace MyApp.Domain.Core.Repositories
{
    public interface IPhotos
    {
        IBaseRepositoryAsync<T> Repository<T>() where T : BaseEntity;
    }
}