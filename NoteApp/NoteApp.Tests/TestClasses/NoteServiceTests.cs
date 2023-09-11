using NoteApp.CustomExceptions;
using NoteApp.Domain.Enums;
using NoteApp.Domain.Models;
using NoteApp.DTOs.NoteDTOs;
using NoteApp.Services.Implementation;
using NoteApp.Tests.FakeRepositories;

namespace NoteApp.Tests.TestClasses
{
    [TestClass]
    public class NoteServiceTests
    {
        [TestMethod]
        public void AddNote_ValidNote_SuccessfullyAddsNote()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 1,
                Text = "Test Note",
                Priority = Priority.Low,
                Tag = Tag.SocialLife
            };

            // Act
            noteService.AddNote(addNoteDto);

            // Assert
            var addedNote = noteRepository.GetAll().LastOrDefault();

            Assert.IsNotNull(addedNote, "No note was added.");
            Assert.AreEqual(addNoteDto.Text, addedNote.Text, "Texts do not match.");
        }

        [TestMethod]
        public void AddNote_NullUser_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 999,
                Text = "Test Note",
                Priority = Priority.Low,
                Tag = Tag.SocialLife
            };

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() => noteService.AddNote(addNoteDto));
        }

        [TestMethod]
        public void AddNote_InvalidPriority_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 1,
                Text = "Test Note",
                Priority = (Priority)999,
                Tag = Tag.SocialLife
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(addNoteDto));
        }

        [TestMethod]
        public void AddNote_InvalidTag_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 1,
                Text = "Test Note",
                Priority = Priority.Low,
                Tag = (Tag)999
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(addNoteDto));
        }

        [TestMethod]
        public void AddNote_NullText_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 1,
                Text = null,
                Priority = Priority.Low,
                Tag = Tag.SocialLife
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(addNoteDto));
        }

        [TestMethod]
        public void AddNote_TextTooLong_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var addNoteDto = new AddNoteDto
            {
                UserId = 1,
                Text = new string('A', 101),
                Priority = Priority.Low,
                Tag = Tag.SocialLife
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.AddNote(addNoteDto));
        }

        [TestMethod]
        public void DeleteNote_ExistingNote_DeletesNote()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var initialNote = new Note
            {
                Id = 1,
                Text = "Test Note",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(initialNote);

            // Act
            noteService.DeleteNote(1);

            // Assert
            var deletedNote = noteRepository.GetById(1);
            Assert.IsNull(deletedNote, "The note was not deleted.");
        }

        [TestMethod]
        public void DeleteNote_NonExistingNote_ThrowsNoteNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => { noteService.DeleteNote(999); });
        }

        [TestMethod]
        public void GetAllNotes_ValidUserId_ReturnsListOfNotes()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var userId = 2;
            noteRepository.Add(new Note { Id = 1, Text = "Note 1", UserId = userId });
            noteRepository.Add(new Note { Id = 2, Text = "Note 2", UserId = userId });

            // Act
            var userNotes = noteService.GetAllNotes(userId);

            // Assert
            Assert.IsNotNull(userNotes, "No notes were retrieved.");
            Assert.AreEqual(2, userNotes.Count, "Incorrect number of notes retrieved.");
        }

        [TestMethod]
        public void GetAllNotes_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetAllNotes(2));
        }

        [TestMethod]
        public void GetAllNotes_InvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetAllNotes(999));
        }

        [TestMethod]
        public void GetById_ExistingNoteId_ReturnsNoteDto()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            // Act
            var retrievedNote = noteService.GetById(3);

            // Assert
            Assert.IsNotNull(retrievedNote, "NoteDto is null.");
            Assert.AreEqual(sampleNote.Text, retrievedNote.Text, "Texts do not match.");
        }

        [TestMethod]
        public void GetById_NonExistingNoteId_ThrowsNoteNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetById(999));
        }

        [TestMethod]
        public void UpdateNote_ExistingNote_SuccessfullyUpdatesNote()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = "Updated Note",
                Priority = Priority.High,
                Tag = Tag.Work,
                UserId = 1
            };

            // Act
            noteService.UpdateNote(updateNoteDto);

            // Assert
            var updatedNote = noteRepository.GetById(3);
            Assert.IsNotNull(updatedNote, "Updated note is null.");
            Assert.AreEqual(updateNoteDto.Text, updatedNote.Text, "Text was not updated.");
        }

        [TestMethod]
        public void UpdateNote_NonExistingNoteId_ThrowsNoteNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 999,
                UserId = 1,
                Text = "Updated Note",
                Priority = Priority.High,
                Tag = Tag.Work
            };

            // Act & Assert
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.UpdateNote(updateNoteDto));
        }

        [TestMethod]
        public void UpdateNote_InvalidUserId_ThrowsUserNotFoundException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = "Updated Note",
                Priority = Priority.High,
                Tag = Tag.Work,
                UserId = 999
            };

            // Act & Assert
            Assert.ThrowsException<UserNotFoundException>(() => noteService.UpdateNote(updateNoteDto));
        }

        [TestMethod]
        public void UpdateNote_InvalidPriority_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = "Updated Note",
                Priority = (Priority)999,
                Tag = Tag.Work,
                UserId = 1
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.UpdateNote(updateNoteDto));
        }

        [TestMethod]
        public void UpdateNote_InvalidTag_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = "Updated Note",
                Priority = Priority.High,
                Tag = (Tag)999,
                UserId = 1
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.UpdateNote(updateNoteDto));
        }

        [TestMethod]
        public void UpdateNote_NullText_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = null,
                Priority = Priority.High,
                Tag = Tag.Work,
                UserId = 1
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.UpdateNote(updateNoteDto));
        }

        [TestMethod]
        public void UpdateNote_TextTooLong_ThrowsNoteDataException()
        {
            // Arrange
            var userRepository = new FakeUserRepository();
            var noteRepository = new FakeNoteRepository();
            var noteService = new NoteService(noteRepository, userRepository);

            var sampleNote = new Note
            {
                Id = 3,
                Text = "Sample Note 3",
                Priority = Priority.Low,
                Tag = Tag.SocialLife,
                UserId = 1
            };
            noteRepository.Add(sampleNote);

            var updateNoteDto = new UpdateNoteDto
            {
                Id = 3,
                Text = new string('A', 101),
                Priority = Priority.High,
                Tag = Tag.Work,
                UserId = 1
            };

            // Act & Assert
            Assert.ThrowsException<NoteDataException>(() => noteService.UpdateNote(updateNoteDto));
        }
    }
}