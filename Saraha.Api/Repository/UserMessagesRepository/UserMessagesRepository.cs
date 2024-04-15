﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saraha.Api.Data;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Repository.UserMessagesRepository
{
    public class UserMessagesRepository : IUserMessages
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        public UserMessagesRepository(ApplicationDbContext _dbContext, UserManager<AppUser> _userManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
        }
        public async Task<UserMessages> AddUserMessage(UserMessages userMessages)
        {
            try
            {
                await _dbContext.UserMessages.AddAsync(userMessages);
                await SaveChangesAsync();
                return userMessages;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserMessages> DeleteUserMessageById(Guid userMessageId)
        {
            try
            {
                UserMessages userMessage = await GetUserMessageById(userMessageId);
                _dbContext.UserMessages.Remove(userMessage);
                await SaveChangesAsync();
                return userMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserMessages>> GetAllUserMessages()
        {
            try
            {
                return await _dbContext.UserMessages.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserMessages>> GetAllUserMessagesByUserEmail(string userEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userEmail);
                return
                    from u in await GetAllUserMessages()
                    where u.UserId == user.Id
                    select u;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserMessages>> GetAllUserMessagesByUserId(string userId)
        {
            try
            {
                return
                    from u in await GetAllUserMessages()
                    where u.UserId == userId
                    select u;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserMessages> GetUserMessageById(Guid userMessageId)
        {
            try
            {
                UserMessages? userMessage = await _dbContext.UserMessages
                    .Where(e => e.Id == userMessageId).FirstOrDefaultAsync();
                return userMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserMessages> UpdateUserMessage(UserMessages userMessages)
        {
            try
            {
                UserMessages userMessages1 = await GetUserMessageById(userMessages.Id);
                userMessages1.Message = userMessages.Message;
                await SaveChangesAsync();
                return userMessages1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
