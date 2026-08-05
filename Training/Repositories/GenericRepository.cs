


using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Training.DTOs;
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
