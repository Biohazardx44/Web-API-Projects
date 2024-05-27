using MovieApp.DTOs.MovieDTOs;

namespace MovieApp.Services.Abstraction
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string LoginUser(LoginUserDto loginUserDto);
        void ChangePassword(ChangePasswordDto changePasswordDto);
    }
}