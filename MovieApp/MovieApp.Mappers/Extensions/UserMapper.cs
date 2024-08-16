using MovieApp.Domain.Models;
using MovieApp.DTOs.UserDTOs;

namespace MovieApp.Mappers.Extensions
{
    public static class UserMapper
    {
        public static User MapRegisterUserDtoToUser(this RegisterUserDto registerUserDto, string passwordHash)
        {
            return new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Username = registerUserDto.Username,
                Password = passwordHash,
                FavoriteGenre = registerUserDto.FavoriteGenre
            };
        }

        public static void UpdateUserDetails(this User user, UpdateUserDetailsDto updateUserDetailsDto)
        {
            if (!string.IsNullOrEmpty(updateUserDetailsDto.FirstName))
            {
                user.FirstName = updateUserDetailsDto.FirstName;
            }

            if (!string.IsNullOrEmpty(updateUserDetailsDto.LastName))
            {
                user.LastName = updateUserDetailsDto.LastName;
            }

            if (!string.IsNullOrEmpty(updateUserDetailsDto.Username))
            {
                user.Username = updateUserDetailsDto.Username;
            }

            user.FavoriteGenre = updateUserDetailsDto.FavoriteGenre;
            user.Password = user.Password;
        }
    }
}