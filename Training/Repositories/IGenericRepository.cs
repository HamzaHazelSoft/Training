
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<PaginationResponseDTO<T>> GetAsync(PaginationRequestDTO paginationRequest);
        public Task<bool> AddAsync(T entity);
        public Task<T> GetByIdAsync(string id);
        public Task<bool> DeleteAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
    }
}
