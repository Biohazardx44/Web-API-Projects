using NoteApp.DTOs.UserDTOs;
using System.Security.Claims;

namespace NoteApp.Services.Abstraction
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
        void ChangePassword(ChangePasswordDto changePasswordDto);
        void DeleteUser(int id, ClaimsPrincipal userClaims);
        void UpdateUserDetails(UpdateUserDetailsDto updateUserDetailsDto, int userId);
    }
}