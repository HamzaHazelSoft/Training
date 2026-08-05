
using Training.DTOs;
using Training.Helper;
using Training.Models;

namespace Training.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<PaginationResponse<T>> GetAllAsync(PaginationRequest paginationRequest);
        public Task<bool> AddAsync(T entity);
        public Task<T> GetByIdAsync(string id);
        public Task<bool> DeleteByIdAsync(string id);
        public Task<bool> UpdateAsync(T entity);
    }
}
