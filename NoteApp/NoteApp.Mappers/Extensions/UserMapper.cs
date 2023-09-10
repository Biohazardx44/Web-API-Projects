using NoteApp.Domain.Models;
using NoteApp.DTOs.UserDTOs;

namespace NoteApp.Mappers.Extensions
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
                Age = registerUserDto.Age
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

            if (updateUserDetailsDto.Age >= 12 && updateUserDetailsDto.Age <= 100)
            {
                user.Age = updateUserDetailsDto.Age;
            }

            user.Password = user.Password;
        }
    }
}