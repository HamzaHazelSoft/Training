
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<int> SaveChangesAsync();
        public Task<PaginationResponseDTO<T>> GetAsync(PaginationRequestDTO paginationRequest);
        public Task<T> GetByIdAsync(string id);
        public void Delete(T entity);
        public void Update(T entity);
    }
}
