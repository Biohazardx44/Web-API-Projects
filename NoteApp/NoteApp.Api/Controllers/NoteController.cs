using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.CustomExceptions;
using NoteApp.DTOs.NoteDTOs;
using NoteApp.Services.Abstraction;
using System.Security.Claims;
using Serilog;

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
                Log.Information("Fetching all notes...");
                var notes = _noteService.GetAllNotes(int.Parse(userId));
                Log.Information("Fetched all notes successfully!");
                return Ok(notes);
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning("No notes were found. Error: {message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching all notes.");
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
                Log.Information("Fetching note with ID: {id}", id);
                var noteDto = _noteService.GetById(id);
                Log.Information("Fetched note with ID: {id} successfully!", id);
                return Ok(noteDto);
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning("Note with ID {id} was not found. Error: {message}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching note with ID: {id}", id);
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
                Log.Information("Adding Note...");
                _noteService.AddNote(addNoteDto);
                Log.Information("Note added successfully!");
                return StatusCode(201, "Note Added!");
            }
            catch (NoteDataException ex)
            {
                Log.Warning(ex, "An error occurred while adding a note: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning(ex, "User was not found while adding a note: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a note.");
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
                Log.Information("Updating note...");
                _noteService.UpdateNote(updateNoteDto);
                Log.Information("Note updated successfully!");
                return Ok("Note updated");
            }
            catch (NoteDataException ex)
            {
                Log.Warning(ex, "An error occurred while updating a note: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning(ex, "Note was not found while updating a note: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warning(ex, "User was not found while updating a note: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating a note.");
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
                Log.Information("Deleting note with ID: {id}", id);
                _noteService.DeleteNote(id);
                Log.Information("Note with ID: {id} was deleted successfully!", id);
                return Ok("Note deleted!");
            }
            catch (NoteNotFoundException ex)
            {
                Log.Warning("Note with ID {id} was not found. Error: {message}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting note with ID: {id}", id);
                return StatusCode(500, "Yikes, that's not good! :(");
            }
        }
    }
}