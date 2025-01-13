using MyApp.Domain.Core.Models;
using MyApp.Domain.Core.Repositories;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Repositories
{
    public class Photos : IPhotos
    {
        protected readonly MyAppDbContext _dbContext;
        private readonly IDictionary<Type, dynamic> _repositories;

        public Photos(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, dynamic>();
        }

        public IBaseRepositoryAsync<T> Repository<T>() where T : BaseEntity
        {
            var entityType = typeof(T);

            if (_repositories.ContainsKey(entityType))
            {
                return _repositories[entityType];
            }

            var repositoryType = typeof(BaseRepositoryAsync<>);

            var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

            if (repository == null)
            {
                throw new NullReferenceException("Repository should not be null");
            }

            _repositories.Add(entityType, repository);

            return (IBaseRepositoryAsync<T>)repository;
        }
    }
}