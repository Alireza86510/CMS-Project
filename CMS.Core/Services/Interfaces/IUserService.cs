using CMS.Core.DTOs;
using CMS.DataLayer.Models;

namespace CMS.Core.Services.Interfaces;

public interface IUserService
{
    List<User> GetAllUsers();
    User GetUserById(int userId);
    User GetUserByEmail(string email);
    int GetUserIdByEmail(string email);
    string GetActiveCodeByEmail(string email);
    ResetPasswordViewModel GetUserForResetPassword(string activeCode);
    User GetUserForLogin(LoginViewModel login);
    ShowPageViewModel GetUserForShow(int userId);
    bool CheckUserExistByEmail(string email);
    bool ActiveAccount(string activeCode);
    bool ResetPassword(string activeCode, string password);
    bool ChangePassword(ChangePasswordViewModel model, string email);
    bool AddUser(User user);
    bool UpdateUser(User user);
}