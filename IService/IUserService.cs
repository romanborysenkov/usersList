using System;
using ListUsers.Models;

namespace ListUsers.IService
{
    public interface IUserService
    {
        User Save(User oUser);
        User GetUser();
    }
}

