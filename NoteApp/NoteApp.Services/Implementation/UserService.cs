using NoteApp.CryptoService;
using NoteApp.CustomExceptions;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;
using NoteApp.DTOs.UserDTOs;
using NoteApp.Mappers.Extensions;
using NoteApp.Services.Abstraction;
using System.Security.Claims;

namespace NoteApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userFromDb = _userRepository.GetUserByUsername(changePasswordDto.Username);

            ValidatePassword(changePasswordDto, userFromDb);

            var newPasswordHash = StringHasher.Hash(changePasswordDto.NewPassword);

            userFromDb.Password = newPasswordHash;
            _userRepository.SaveChanges();
        }

        public string LoginUser(LoginUserDto loginUserDto)
        {
            ValidateLogin(loginUserDto);

            var userFromDb = _userRepository.LoginUser(loginUserDto.Username, StringHasher.Hash(loginUserDto.Password));
            if (userFromDb == null)
            {
                throw new UserNotFoundException("User not found!");
            }

            var token = userFromDb.GenerateJwtToken();
            return token;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            ValidateUser(registerUserDto);

            var passwordHash = StringHasher.Hash(registerUserDto.Password);
            var user = registerUserDto.MapRegisterUserDtoToUser(passwordHash);

            _userRepository.Add(user);
        }

        public void DeleteUser(int id, ClaimsPrincipal userClaims)
        {
            var userFromDb = _userRepository.GetById(id);
            if (userFromDb == null)
            {
                throw new UserNotFoundException($"User with ID {id} does not exist!");
            }

            var userIdClaim = userClaims.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userIdFromToken))
            {
                throw new UserDataException("Invalid user token!");
            }

            if (userIdFromToken != id)
            {
                throw new UserDataException("You are not authorized to delete this user!");
            }

            _userRepository.Delete(userFromDb);
        }

        public void UpdateUserDetails(UpdateUserDetailsDto updateUserDetailsDto, int userId)
        {
            var userFromDb = _userRepository.GetById(userId);
            if (userFromDb == null)
            {
                throw new UserNotFoundException($"User with ID {userId} does not exist!");
            }

            ValidateUserDetails(updateUserDetailsDto);

            userFromDb.UpdateUserDetails(updateUserDetailsDto);

            _userRepository.Update(userFromDb);
        }

        private void ValidateUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new UserDataException("Password must match!");
            }

            if (string.IsNullOrEmpty(registerUserDto.Username) ||
                string.IsNullOrEmpty(registerUserDto.Password) ||
                string.IsNullOrEmpty(registerUserDto.ConfirmPassword))
            {
                throw new UserDataException("Username and Password are required fields!");
            }

            if (registerUserDto.Username.Length < 5 || registerUserDto.Username.Length > 30)
            {
                throw new UserDataException("Username must be between 5 and 30 characters long!");
            }

            if ((registerUserDto.Password.Length < 5 || registerUserDto.Password.Length > 30) &&
                (registerUserDto.ConfirmPassword.Length < 5 || registerUserDto.ConfirmPassword.Length > 30))
            {
                throw new UserDataException("Password must be between 5 and 30 characters long!");
            }

            if (registerUserDto.FirstName.Length > 50)
            {
                throw new UserDataException("First name cannot exceed 50 characters!");
            }

            if (registerUserDto.LastName.Length > 50)
            {
                throw new UserDataException("Last name cannot exceed 50 characters!");
            }

            if (registerUserDto.Age < 12 || registerUserDto.Age > 100)
            {
                throw new UserDataException("Age must be between 12 and 100!");
            }

            var userFromDb = _userRepository.GetUserByUsername(registerUserDto.Username);
            if (userFromDb != null)
            {
                throw new UserDataException($"The username {registerUserDto.Username} is already in use!");
            }
        }

        private void ValidateLogin(LoginUserDto loginUserDto)
        {
            if (string.IsNullOrEmpty(loginUserDto.Username) ||
                string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new UserDataException("Username and Password are required fields!");
            }
        }

        private void ValidatePassword(ChangePasswordDto changePasswordDto, User userFromDb)
        {
            if (userFromDb == null)
            {
                throw new UserNotFoundException("User not found!");
            }

            if (userFromDb.Password != StringHasher.Hash(changePasswordDto.CurrentPassword))
            {
                throw new UserDataException("Current password is incorrect!");
            }

            if (string.IsNullOrEmpty(changePasswordDto.NewPassword))
            {
                throw new UserDataException("New Password is a required field!");
            }

            if (changePasswordDto.NewPassword.Length < 5 || changePasswordDto.NewPassword.Length > 30)
            {
                throw new UserDataException("Password must be between 5 and 30 characters long!");
            }
        }

        private void ValidateUserDetails(UpdateUserDetailsDto updateUserDetailsDto)
        {
            if (!string.IsNullOrEmpty(updateUserDetailsDto.FirstName) && updateUserDetailsDto.FirstName.Length > 50)
            {
                throw new UserDataException("First name cannot exceed 50 characters!");
            }

            if (!string.IsNullOrEmpty(updateUserDetailsDto.LastName) && updateUserDetailsDto.LastName.Length > 50)
            {
                throw new UserDataException("Last name cannot exceed 50 characters!");
            }

            if (!string.IsNullOrEmpty(updateUserDetailsDto.Username) && (updateUserDetailsDto.Username.Length < 5 || updateUserDetailsDto.Username.Length > 30))
            {
                throw new UserDataException("Username must be between 5 and 30 characters long!");
            }

            var userFromDb = _userRepository.GetUserByUsername(updateUserDetailsDto.Username);
            if (userFromDb != null)
            {
                throw new UserDataException($"The username {updateUserDetailsDto.Username} is already in use!");
            }

            if (string.IsNullOrEmpty(updateUserDetailsDto.FirstName) &&
                string.IsNullOrEmpty(updateUserDetailsDto.LastName) &&
                string.IsNullOrEmpty(updateUserDetailsDto.Username) &&
                updateUserDetailsDto.Age == 0)
            {
                throw new UserDataException("No details provided. Please fill in at least one field.");
            }

            if (updateUserDetailsDto.Age != 0 && (updateUserDetailsDto.Age < 12 || updateUserDetailsDto.Age > 100))
            {
                throw new UserDataException("Age must be between 12 and 100!");
            }
        }
    }
}