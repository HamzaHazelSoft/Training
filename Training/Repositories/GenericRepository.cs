


using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Training.DTOs;
using Training.Enums;
using Training.Helper;
using Training.Models;

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

            int totalRecords = await query.CountAsync();

            //By default we use query.orderBy(x=>x.id) but here property passing by user in runtime . To access property passing in runtime,
            //we use EF.Property<object>(x, "Name") it means x object mein se wo property nikal do jo k "Name" hai"


            query = paginationRequest.SortOrder == SortDirection.DESC
                ? query.OrderByDescending(x => EF.Property<object>(x, paginationRequest.SortBy))
                : query.OrderBy(x => EF.Property<object>(x, paginationRequest.SortBy));

            if(!string.IsNullOrEmpty(paginationRequest.SearchItem))
                query =  query.Where(x => EF.Property<string>(x, "Email").Contains(paginationRequest.SearchItem)); //WHERE Email LIKE '%Ham%'


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
