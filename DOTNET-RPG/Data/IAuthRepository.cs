﻿using DOTNET_RPG.Models;
using DOTNET_RPG.Models.Responses;

namespace DOTNET_RPG.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
