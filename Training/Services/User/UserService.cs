

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Models;

namespace Training.Services
{
    public class UserService : IUserService
    {

        private readonly TrainingContext _context;
        private readonly IMapper _mapper;

        /* Using dependency injection to get the context and mapper instances . Why dependency injection? Because it losely couples
        the service with the context and mapper, making it easier to maintain. In simple words, we achieved the
        Inversion of Control principle by using dependency injection. The service does not create the context and mapper instances,
        but rather receives them from the outside means Framework
        */
        public UserService(TrainingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList(); //Return the list of all users in the database
        }

        
        public bool AddUser(User user)
        {
            _context.Users.Add(user); // Not yet saved . It bind flag of Added
            return _context.SaveChanges() > 0;
        }

        public User GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(x => x.Id.Equals(id)); // Returns the first user that matches the Id,
                                                                        // or null if no user is found
        }

        // Deletes a user by Id
        public bool DeleteUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id.Equals(id)); //Returns the first user that matches the Id,
                                                                            //or null if no user is found

            if (user == null)
                return false;

            _context.Users.Remove(user); // Now it modifies the flag of the user to be deleted
            return _context.SaveChanges() > 0;
        }
        public bool UpdateUserById(string id, UserDTO userDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id.Equals(id));

            if (user == null)
                return false;

            _mapper.Map(userDto, user); //Mapper copies all related fields from userDto to user

            _context.Users.Update(user); //Now it modifies the flag of the user to be updated

            return _context.SaveChanges() > 0;
        }
    }
}
