using CMS.Core.DTOs;
using CMS.Core.Generators;
using CMS.Core.Security;
using CMS.Core.Services.Interfaces;
using CMS.DataLayer.Context;
using CMS.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CMS.Core.Services.Implements;

public class UserService : IUserService
{
    private CmsContext _context;

    public UserService(CmsContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int userId)
    {
        return _context.Users.Find(userId);
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.Single(u => u.Email == email);
    }

    public User GetUserForLogin(LoginViewModel login)
    {
        return _context.Users.SingleOrDefault(u =>
            u.Email == login.Email.ToLower() && u.Password == PasswordHash.Hash(login.Password));
    }

    public ShowPageViewModel GetUserForShow(int userId)
    {
        return _context.Users
            .Where(u => u.UserId == userId)
            .Select(s => new ShowPageViewModel
            {
                Email = s.Email,
                Description = s.Description,
                Name = s.Name,
                ImagePath = s.ImagePath,
                RegisterDate = s.RegisterDate,
                IsActive = s.IsActive,
            }).Single();
    }

    public bool CheckUserExistByEmail(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public bool ActiveAccount(string activeCode)
    {
        var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        if (user != null)
        {
            user.ActiveCode = NameGenerator.GenerateCode();
            user.IsActive = true;
            UpdateUser(user);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddUser(User user)
    {
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateUser(User user)
    {
        try
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetActiveCodeByEmail(string email)
    {
        return _context.Users
            .Where(u => u.Email == email)
            .Select(u => u.ActiveCode)
            .SingleOrDefault();
    }

    public User GetUserByActiveCode(string activeCode)
    {
        return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
    }

    public bool ResetPassword(string activeCode, string password)
    {
        try
        {
            var user = GetUserByActiveCode(activeCode);
            user.ActiveCode = NameGenerator.GenerateCode();
            user.Password = password;
            UpdateUser(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ResetPasswordViewModel GetUserForResetPassword(string activeCode)
    {
        return _context.Users
            .Where(u => u.ActiveCode == activeCode)
            .Select(s => new ResetPasswordViewModel
            {
                ActiveCode = s.ActiveCode
            }).SingleOrDefault();
    }

    public int GetUserIdByEmail(string email)
    {
        return _context.Users.Where(u => u.Email == email.ToLower())
            .Select(u => u.UserId).SingleOrDefault();
    }

    public bool ChangePassword(ChangePasswordViewModel model, string email)
    {
        var user = _context.Users
            .SingleOrDefault(u => u.Email == email.ToLower());

        if (user != null && user.Password == PasswordHash.Hash(model.CurrentPassword))
        {
            user.Password = PasswordHash.Hash(model.NewPassword);
            UpdateUser(user);
            return true;
        }
        else
        {
            return false;
        }
    }
}