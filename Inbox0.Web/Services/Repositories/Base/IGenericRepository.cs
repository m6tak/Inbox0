using Inbox0.Core.Models.Database;

namespace Inbox0.Web.Services.Repositories.Base
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        public T GetById(string id);
        public IEnumerable<T> GetAll();
        public void Add(T entity);
        public void AddRange(IEnumerable<T> entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
