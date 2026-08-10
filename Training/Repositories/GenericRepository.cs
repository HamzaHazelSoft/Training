


using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using UserManagementSystem.Context;
using UserManagementSystem.DTOs;
using DbContext = UserManagementSystem.Context.DbContext;

namespace UserManagementSystem.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _context = context;
            // Gets the DbSet associated with the entity type T.
            // This allows the repository to work with any entity.
            _dbset = context.Set<T>();

        }
        public async Task<int> SaveChangesAsync()
        {
            // Persists all tracked changes to the database.
            // SaveChangesAsync returns the number of affected records.
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginationResponseDTO<T>> GetAsync(PaginationRequestDTO paginationRequest)
        {

            // Creates an IQueryable query without executing it immediately.
            // EF Core builds the SQL query and executes it when a terminal
            // operation such as CountAsync() or ToListAsync() is called.
            IQueryable<T> query = _dbset.AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationRequest.SearchItem))
                query = query.Where(paginationRequest.SearchItem); 

            int totalRecords = await query.CountAsync();

            query = query.OrderBy(paginationRequest.SortBy);

            var items = await query
                .Skip((paginationRequest.CurrentPage - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            return new PaginationResponseDTO<T> 
            {
                Total = totalRecords,
                PageSize =  paginationRequest.PageSize,
                CurrentPage =  paginationRequest.CurrentPage,
                Items =  items
            };

        }
        public async Task<T> GetByIdAsync(string id) {
            return await _dbset.FindAsync(id); // Returns the first entity that matches the Id or null if no entity is found
        }
        public void Delete(T entity) {
            _dbset.Remove(entity); // Now it modifies the flag of the entity to be deleted
        }
        public void Update(T entity) 
        {
            _dbset.Update(entity); //Now it modifies the flag of the entity to be updated
        }
    }

}
