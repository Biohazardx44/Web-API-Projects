using NoteApp.CryptoService;
using NoteApp.CustomExceptions;
using NoteApp.DTOs.UserDTOs;
using NoteApp.Services.Implementation;
using NoteApp.Tests.FakeRepositories;
using System.Security.Claims;

namespace NoteApp.Tests.TestClasses
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void ChangePassword_ValidPasswordChange_Success()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var currentPassword = "user1";
            var newPassword = "newPassword";

            // Act
            userService.ChangePassword(new ChangePasswordDto
            {
                Username = username,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            });

            // Assert
            var updatedUser = userRepository.GetUserByUsername(username);
            Assert.IsNotNull(updatedUser, "User not found after password change.");
            Assert.AreEqual(StringHasher.Hash(newPassword), updatedUser.Password, "Password not updated.");
        }

        [TestMethod]
        public void ChangePassword_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "nonexistentUser";
            var currentPassword = "currentPassword";
            var newPassword = "newPassword";

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() =>
            {
                userService.ChangePassword(new ChangePasswordDto
                {
                    Username = username,
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                });
            });
        }

        [TestMethod]
        public void ChangePassword_IncorrectCurrentPassword_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var currentPassword = "incorrectPassword";
            var newPassword = "newPassword";

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.ChangePassword(new ChangePasswordDto
                {
                    Username = username,
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                });
            });
        }

        [TestMethod]
        public void ChangePassword_MissingNewPassword_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var currentPassword = "user1";
            var newPassword = "";

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.ChangePassword(new ChangePasswordDto
                {
                    Username = username,
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                });
            });
        }

        [TestMethod]
        public void ChangePassword_InvalidNewPasswordLength_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var currentPassword = "user1";
            var newPassword = "abc";

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.ChangePassword(new ChangePasswordDto
                {
                    Username = username,
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                });
            });
        }

        [TestMethod]
        public void LoginUser_SuccessfulLogin_ReturnsJwtToken()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var password = "user1";

            // Act
            var token = userService.LoginUser(new LoginUserDto
            {
                Username = username,
                Password = password
            });

            // Assert
            Assert.IsNotNull(token, "JWT token is null.");
        }

        [TestMethod]
        public void LoginUser_InvalidPassword_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var username = "user1";
            var invalidPassword = "invalidPassword";

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() =>
            {
                userService.LoginUser(new LoginUserDto
                {
                    Username = username,
                    Password = invalidPassword
                });
            });
        }

        [TestMethod]
        public void LoginUser_InvalidUsername_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var invalidUsername = "invalidUsername";
            var password = "password";

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() =>
            {
                userService.LoginUser(new LoginUserDto
                {
                    Username = invalidUsername,
                    Password = password
                });
            });
        }

        [TestMethod]
        public void LoginUser_EmptyUsernameAndPassword_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.LoginUser(new LoginUserDto
                {
                    Username = "",
                    Password = ""
                });
            });
        }

        [TestMethod]
        public void RegisterUser_ValidUser_SuccessfullyRegistersUser()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var validUserDto = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act
            userService.RegisterUser(validUserDto);

            // Assert
            var registeredUser = userRepository.GetUserByUsername(validUserDto.Username);
            Assert.IsNotNull(registeredUser, "User was not registered.");
        }

        [TestMethod]
        public void RegisterUser_MissingUsername_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithMissingUsername = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithMissingUsername);
            });
        }

        [TestMethod]
        public void RegisterUser_MissingPassword_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithMissingPassword = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithMissingPassword);
            });
        }

        [TestMethod]
        public void RegisterUser_MissingConfirmPassword_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithMissingConfirmPassword = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                Password = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithMissingConfirmPassword);
            });
        }

        [TestMethod]
        public void RegisterUser_PasswordMismatch_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithMismatchedPasswords = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                Password = "Password1",
                ConfirmPassword = "MismatchedPassword"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithMismatchedPasswords);
            });
        }

        [TestMethod]
        public void RegisterUser_InvalidUsernameLength_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithInvalidUsername = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "1234",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithInvalidUsername);
            });
        }

        [TestMethod]
        public void RegisterUser_InvalidPasswordLength_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithInvalidPassword = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                Password = "1234",
                ConfirmPassword = "1234"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithInvalidPassword);
            });
        }

        [TestMethod]
        public void RegisterUser_InvalidFirstNameLength_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithInvalidFirstName = new RegisterUserDto
            {
                FirstName = new string('A', 51),
                LastName = "TestUserLastName",
                Age = 25,
                Username = "Username",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithInvalidFirstName);
            });
        }

        [TestMethod]
        public void RegisterUser_InvalidLastNameLength_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithInvalidLastName = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = new string('B', 51),
                Age = 25,
                Username = "Username",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithInvalidLastName);
            });
        }

        [TestMethod]
        public void RegisterUser_InvalidAge_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithInvalidAge = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 11,
                Username = "Username",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithInvalidAge);
            });
        }

        [TestMethod]
        public void RegisterUser_ExistingUsername_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);

            var userDtoWithExistingUsername = new RegisterUserDto
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                Age = 11,
                Username = "user1",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.RegisterUser(userDtoWithExistingUsername);
            });

            var usersInRepository = userRepository.GetAll();
            Assert.AreEqual(2, usersInRepository.Count);
        }

        [TestMethod]
        public void DeleteUser_ValidUser_SuccessfullyDeletesUser()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("userId", "1")
            }));

            // Act
            userService.DeleteUser(1, userClaims);

            // Assert
            var deletedUser = userRepository.GetById(1);
            Assert.IsNull(deletedUser, "The user was not deleted.");
        }

        [TestMethod]
        public void DeleteUser_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("userId", "1")
            }));

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() =>
            {
                userService.DeleteUser(999, userClaims);
            });
        }

        [TestMethod]
        public void DeleteUser_InvalidToken_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userClaims = new ClaimsPrincipal();

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.DeleteUser(1, userClaims);
            });
        }

        [TestMethod]
        public void DeleteUser_UnauthorizedUser_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("userId", "2")
            }));

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.DeleteUser(1, userClaims);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_ValidUpdate_SuccessfullyUpdatesUser()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Username = "UpdatedUsername",
                Age = 30
            };

            // Act
            userService.UpdateUserDetails(updateUserDetailsDto, userId);

            // Assert
            var updatedUser = userRepository.GetById(userId);
            Assert.IsNotNull(updatedUser, "The user was not updated.");
            Assert.AreEqual(updateUserDetailsDto.FirstName, updatedUser.FirstName, "First name was not updated.");
            Assert.AreEqual(updateUserDetailsDto.LastName, updatedUser.LastName, "Last name was not updated.");
            Assert.AreEqual(updateUserDetailsDto.Username, updatedUser.Username, "Username was not updated.");
            Assert.AreEqual(updateUserDetailsDto.Age, updatedUser.Age, "Age was not updated.");
        }

        [TestMethod]
        public void UpdateUserDetails_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 999;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                FirstName = "UpdatedFirstName"
            };

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_InvalidUsername_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                Username = "user2"
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_InvalidFirstName_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                FirstName = new string('A', 51)
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_InvalidLastName_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                LastName = new string('B', 51)
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_InvalidAge_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                Age = 101
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }

        [TestMethod]
        public void UpdateUserDetails_NoDetailsProvided_ThrowsUserDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var userService = new UserService(userRepository);
            var userId = 1;
            var updateUserDetailsDto = new UpdateUserDetailsDto
            {
                // No details
            };

            // Act & Assert
            Assert.ThrowsException<UserDataException>(() =>
            {
                userService.UpdateUserDetails(updateUserDetailsDto, userId);
            });
        }
    }
}