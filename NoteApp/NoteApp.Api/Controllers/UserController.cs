using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.CustomExceptions;
using NoteApp.DTOs.UserDTOs;
using NoteApp.Services.Abstraction;
using Serilog;

namespace NoteApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDto">The data for registering a new user.</param>
        /// <returns>A success response if the user is registered; otherwise, an error response.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                Log.Information("Processing registration... Username: '{username}'", registerUserDto?.Username);
                _userService.RegisterUser(registerUserDto);
                Log.Information("User '{username}' registered successfully!", registerUserDto?.Username);
                return StatusCode(201, "User created!");
            }
            catch (UserDataException ex)
            {
                Log.Warning("User registration failed due to data validation. Username: '{username}'. Message: {message}", registerUserDto?.Username, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occured during user registration! Username: '{username}'", registerUserDto?.Username);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Logs in a user and returns an authentication token.
        /// </summary>
        /// <param name="loginUserDto">The data for user login.</param>
        /// <returns>An authentication token if login is successful; otherwise, an error response.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                Log.Information("Processing login... Username: '{username}'", loginUserDto?.Username);
                var token = _userService.LoginUser(loginUserDto);
                Log.Information("User '{username}' logged in successfully!", loginUserDto?.Username);
                return Ok(token);
            }
            catch (UserDataException ex)
            {
                Log.Warning("User login failed due to data validation. Username: '{username}'. Message: {message}", loginUserDto?.Username, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning("User login failed. User not found. Username: '{username}'", loginUserDto?.Username);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during user login! Username: '{username}'", loginUserDto?.Username);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Changes the password for the authenticated user.
        /// </summary>
        /// <param name="changePasswordDto">The data for changing the password.</param>
        /// <returns>A success response if the password is changed; otherwise, an error response.</returns>
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                Log.Information("Processing password change... Username: '{username}'", changePasswordDto?.Username);
                _userService.ChangePassword(changePasswordDto);
                Log.Information("Password changed successfully for user '{username}'!", changePasswordDto?.Username);
                return Ok("Password changed successfully!");
            }
            catch (UserDataException ex)
            {
                Log.Warning("Change password failed due to data validation. Username: '{username}'. Message: {message}", changePasswordDto?.Username, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning("Change password failed. User not found. Username: '{username}'", changePasswordDto?.Username);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during password change! Username: '{username}'", changePasswordDto?.Username);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Deletes the authenticated user.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A success response if the user is deleted; otherwise, an error response.</returns>
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var userClaims = HttpContext.User;
                Log.Information("Processing user deletion... UserId: '{id}'", id);
                _userService.DeleteUser(id, userClaims);
                Log.Information("User with ID {id} was deleted successfully!", id);
                return Ok("User deleted successfully!");
            }
            catch (UserDataException ex)
            {
                Log.Warning("User deletion failed due to data validation. UserId: '{id}'. Message: {message}", id, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning("User deletion failed. User not found. UserId: '{id}'", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during user deletion! UserId: '{id}'", id);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Updates details for the authenticated user.
        /// </summary>
        /// <param name="updateUserDetailsDto">The data for updating user details.</param>
        /// <returns>A success response if the details are updated; otherwise, an error response.</returns>
        [HttpPut("update-details")]
        public IActionResult UpdateUserDetails([FromBody] UpdateUserDetailsDto updateUserDetailsDto)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Invalid user token.");
                }

                Log.Information("Processing user details update... Username: '{username}'", updateUserDetailsDto?.Username);
                _userService.UpdateUserDetails(updateUserDetailsDto, userId);
                Log.Information("User details updated successfully for user '{username}'!", updateUserDetailsDto?.Username);
                return Ok("User details updated successfully!");
            }
            catch (UserDataException ex)
            {
                Log.Warning("User details update failed due to data validation. Username: '{username}'. Message: {message}", updateUserDetailsDto?.Username, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning("User details update failed. User not found. Username: '{username}'", updateUserDetailsDto?.Username);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during user details update! Username: '{username}'", updateUserDetailsDto?.Username);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }
    }
}