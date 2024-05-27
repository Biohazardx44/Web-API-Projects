using Microsoft.IdentityModel.Tokens;
using MovieApp.CryptoService;
using MovieApp.CustomExceptions;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Models;
using MovieApp.DTOs.MovieDTOs;
using MovieApp.Services.Abstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieApp.Services.Implementation
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

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our very hidden secret secret key");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("userFullName", $"{userFromDb.FirstName} {userFromDb.LastName}"),
                        new Claim("userId", $"{userFromDb.Id}"),
                        new Claim(ClaimTypes.Name, userFromDb.Username)
                    }
                )
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            ValidateUser(registerUserDto);

            var passwordHash = StringHasher.Hash(registerUserDto.Password);

            var user = new User
            {
                Username = registerUserDto.Username,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                FavoriteGenre = registerUserDto.FavoriteGenre,
                Password = passwordHash
            };

            _userRepository.Add(user);
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
                throw new UserDataException("Username and password are required fields!");
            }

            if (registerUserDto.Username.Length > 30)
            {
                throw new UserDataException("Username: Maximum length for username is 30 characters");
            }

            if (registerUserDto.FirstName.Length > 50)
            {
                throw new UserDataException("Maximum length for FirstName is 50 characters");
            }

            if (registerUserDto.LastName.Length > 50)
            {
                throw new UserDataException("Maximum length for LastName is 50 characters");
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
                throw new UserDataException("Username and password are required fields!");
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
                throw new UserDataException("New Password is required!");
            }

            if (changePasswordDto.NewPassword.Length < 5)
            {
                throw new UserDataException("Password must be at least 5 characters long!");
            }
        }
    }
}