using Azure.Core;
using EmeraldHerald.Migrations;
using FirelinkShrine.Models;
using FirelinkShrine.Models.ViewModels;
using FirelinkShrine.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using System.Security.Claims;

namespace FirelinkShrine.Controllers {
	public record NoteIdRequest(int NoteId);
	public class NoteController : Controller {
		private readonly INoteRepository _noteRepo;

		public NoteController(INoteRepository noteRepo) {
			_noteRepo = noteRepo;
		}

		[Authorize]
		public async Task<IActionResult> Index(int page = 1) {
			string? userId = User.FindFirstValue("UserId");
			if (userId is null)
				return View(null);
			var notes = await _noteRepo.GetNotesByUser(int.Parse(userId));
			int pageSize = 7;
			var count = notes is not null ? notes.Count : 0;
			var items = LimitNotesOnPage(notes, page, pageSize);
			NoteIndexViewModel viewModel = new(
				items,
				new PageViewModel(count, page, pageSize)
			);
			return View(viewModel);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddNote(Note note) {
			if (note is not null && User.Identity!.IsAuthenticated) {
				int userId = int.Parse(User.FindFirstValue("UserId")!);
				note.UserId = userId;

				await _noteRepo.AddNote(note);
				return RedirectToAction("Index", "Note");
			}
			return BadRequest();
		}

		[Authorize]
		public async Task<IActionResult> Details(int id) {
			var note = await _noteRepo.FindById(id);
			if (note is null)
				return BadRequest();
			if (!CheckNoteOwner(note))
				return Unauthorized();
			return Ok(note);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteNote(int id) {
			var note = await _noteRepo.FindById(id);
			if (note is null)
				return BadRequest();
			if (!CheckNoteOwner(note))
				return Unauthorized();

			await _noteRepo.DeleteNote(note);
			return RedirectToAction("Index", "Note");
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditNote(int id, string body) {
			var note = await _noteRepo.FindById(id);
			if (note is null)
				return BadRequest();
			note.Body = body;
			note.Updated = DateTime.Now;

			await _noteRepo.UpdateNote(note);
			return RedirectToAction("Index", "Note");
		}

		[NonAction]
		private bool CheckNoteOwner(Note note) {
			if (note.UserId == int.Parse(User.FindFirstValue("UserId")!))
				return true;
			else
				return false;
		}

		private IEnumerable<Note>? LimitNotesOnPage(IEnumerable<Note>? items, int page, int pageSize) {
			var notes = items?.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return notes;
		}
	}
}
