using AutoMapper;
using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using HARRepo.FileManager.Logic.Exceptions;
using HARRepo.FileManager.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HARRepo.FileManager.Logic.Implementations
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolver _resolver;

        public AuthorizationManager(DbContext context, IMapper mapper, IUserResolver resolver)
        {
            _context = context;
            _mapper = mapper;
            _resolver = resolver;
        }

        public async Task<UserDTO> GetCurrentUserAsync()
        {
            UserDTO user = null;
            var userId = _resolver.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var userEntity = await _context.Set<User>().SingleOrDefaultAsync(x => x.Email == userId);
                if (userEntity != null)
                {
                    user = _mapper.Map<UserDTO>(userEntity);
                    return user;
                }
            }
            var resolvedUser = await _resolver.GetUserAsync();
            if (resolvedUser != null)
            {
                var userEntity = await _context.Set<User>().SingleOrDefaultAsync(x => x.Email == resolvedUser.Email);
                if (userEntity == null)
                {
                    userEntity = (await _context.AddAsync(_mapper.Map<User>(resolvedUser))).Entity;
                    await _context.SaveChangesAsync();
                }
                return _mapper.Map<UserDTO>(userEntity);
            }

            return null;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new NotFoundException("User with specified email has not been found.");
            }
            return _mapper.Map<UserDTO>(user);
        }
    }
}
