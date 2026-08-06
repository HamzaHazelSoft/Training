


using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Training.Context;
using Training.DTOs;
using Training.Helper;

namespace Training.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TrainingContext _context;
        private readonly DbSet<T> _dbset;

        public GenericRepository(TrainingContext context)
        {
            this._context = context;
            this._dbset = context.Set<T>();
        }
        private async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PaginationResponse<T>> GetAsync(PaginationRequest paginationRequest)
        {
            IQueryable<T> query = _dbset.AsQueryable(); // Means i want to creat DB Query, but not yet execute it. It will be executed when we
                                                        // call ToListAsync() or CountAsync()

            if (!string.IsNullOrWhiteSpace(paginationRequest.SearchItem))
                query = query.Where(paginationRequest.SearchItem); //internally dynamic LINQ converting it to LAMBDA expression
                                                                   //query.Where(x => x.Email.Contains("hamza"));

            int totalRecords = await query.CountAsync();

            query = query = query.OrderBy(paginationRequest.SortBy); // e.g. "Id DESC" ya "Name ASC" . ORDER BY Id DESC

            var items = await query
                .Skip((paginationRequest.CurrentPage - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            return new PaginationResponse<T> 
            {
                Total = totalRecords,
                PageSize =  paginationRequest.PageSize,
                CurrentPage =  paginationRequest.CurrentPage,
                Items =  items
            };

        }
        public async Task<bool> AddAsync(T entity) {
            _dbset.Add(entity); // Not yet saved . It bind flag of Added
            return await SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(string id) {
            return await _dbset.FindAsync(id); // Returns the first entity that matches the Id,
                                                                                      // or null if no entity is found
        }
        public async Task<bool> DeleteByIdAsync(string id) {

            var entity = await GetByIdAsync(id); //Returns the first entity that matches the Id,
                                          //or null if no entity is found
            if (entity == null)
                return false;
 
            _dbset.Remove(entity); // Now it modifies the flag of the entity to be deleted
            return await SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(T entity) 
        {
            _dbset.Update(entity); //Now it modifies the flag of the entity to be updated
            return await SaveChangesAsync();
        }
    }

}
