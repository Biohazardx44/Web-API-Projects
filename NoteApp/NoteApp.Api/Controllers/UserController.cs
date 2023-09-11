using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.CustomExceptions;
using NoteApp.DTOs.UserDTOs;
using NoteApp.Services.Abstraction;

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
                _userService.RegisterUser(registerUserDto);
                return StatusCode(201, "User created!");
            }
            catch (UserDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
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
                var token = _userService.LoginUser(loginUserDto);
                return Ok(token);
            }
            catch (UserDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
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
                _userService.ChangePassword(changePasswordDto);
                return Ok("Password changed successfully!");
            }
            catch (UserDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
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
                _userService.DeleteUser(id, userClaims);
                return Ok("User deleted successfully!");
            }
            catch (UserDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
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

                _userService.UpdateUserDetails(updateUserDetailsDto, userId);
                return Ok("User details updated successfully!");
            }
            catch (UserDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }
    }
}