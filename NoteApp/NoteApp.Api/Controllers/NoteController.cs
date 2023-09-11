using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.CustomExceptions;
using NoteApp.DTOs.NoteDTOs;
using NoteApp.Services.Abstraction;
using System.Security.Claims;

namespace NoteApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Gets all notes for the authenticated user.
        /// </summary>
        /// <returns>A list of note objects if found; otherwise, an error response.</returns>
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            try
            {
                var userId = User.FindFirstValue("userId");
                return Ok(_noteService.GetAllNotes(int.Parse(userId)));
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Gets a note by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>The requested note if found; otherwise, an error response.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var noteDto = _noteService.GetById(id);
                return Ok(noteDto);
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Adds a new note to the database.
        /// </summary>
        /// <param name="addNoteDto">The data for the new note.</param>
        /// <returns>A success response if the note is added; otherwise, an error response.</returns>
        [HttpPost]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                return StatusCode(201, "Note Added!");
            }
            catch (NoteDataException ex)
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
        /// Updates an existing note in the database.
        /// </summary>
        /// <param name="updateNoteDto">The data for updating the note.</param>
        /// <returns>A success response if the note is updated; otherwise, an error response.</returns>
        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                return Ok("Note updated");
            }
            catch (NoteDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        /// Deletes a note from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note to delete.</param>
        /// <returns>A success response if the note is deleted; otherwise, an error response.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteNote([FromRoute] int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                return Ok("Note deleted!");
            }
            catch (NoteNotFoundException ex)
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