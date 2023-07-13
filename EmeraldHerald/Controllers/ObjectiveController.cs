using Azure.Core;
using FirelinkShrine.Models;
using FirelinkShrine.Models.ViewModels;
using FirelinkShrine.Repositories;
using FirelinkShrine.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirelinkShrine.Controllers {
	public record ObjectiveIdRequest(int ObjectiveId);
	public class ObjectiveController : Controller {
		private readonly IObjectiveRepository _objectiveRepo;

		public ObjectiveController(IObjectiveRepository objectiveRepo) {
			_objectiveRepo = objectiveRepo;
		}

		[Authorize]
		public async Task<IActionResult> Index(string? name, string status = "All", int page = 1, SortState sortOrder = SortState.CreatedDesc) {
			string? userId = User.FindFirstValue("UserId");
			if (userId is null)
				return View(null);

			var objectives = await _objectiveRepo.GetAllUserObjectives(int.Parse(userId));
			if (objectives is null)
				return View(null);

			objectives = FilterObjectives(objectives, status, name);
			objectives = SortObjectives(objectives, sortOrder);

			int pageSize = 10;
			var count = objectives.Count();
			var items = LimitObjectivesOnPage(objectives, page, pageSize);

			ObjectiveIndexViewModel viewModel = new(
				items,
				new PageViewModel(count, page, pageSize),
				new FilterViewModel(status, name),
				new SortViewModel(sortOrder)
			);

			return View(viewModel);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddTask([FromBody] Objective objective) {
			if (objective is not null && User.Identity!.IsAuthenticated) {
				int userId = int.Parse(User.FindFirstValue("UserId")!);
				objective.UserId = userId;
				objective.Deadline = DateTime.Now.AddHours(objective.DeadlineInHours);

				await _objectiveRepo.AddObjective(objective);
				return Ok(objective);
			}
			return BadRequest();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteTask([FromBody] ObjectiveIdRequest request) {
			var objective = await _objectiveRepo.TryFindObjective(request.ObjectiveId);
			if (objective is null)
				return BadRequest();
			if (!CheckObjectiveOwner(objective))
				return Unauthorized();

			await _objectiveRepo.DeleteObjective(objective);

			return Ok();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> FinishTask([FromBody] ObjectiveIdRequest request) {
			return await ChangeObjectiveStatus(request.ObjectiveId, "Completed");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AbandonTask([FromBody] ObjectiveIdRequest request) {
			return await ChangeObjectiveStatus(request.ObjectiveId, "Abandoned");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ResetTask([FromBody] ObjectiveIdRequest request) {
			return await ChangeObjectiveStatus(request.ObjectiveId, "Active");
		}

		[NonAction]
		private bool CheckObjectiveOwner(Objective objective) {
			if (objective.UserId == int.Parse(User.FindFirstValue("UserId")!))
				return true;
			else
				return false;
		}

		[NonAction]
		private async Task<IActionResult> ChangeObjectiveStatus(int id, string newStatus) {
			var objective = await _objectiveRepo.TryFindObjective(id);
			if (objective is null)
				return BadRequest();
			if (!CheckObjectiveOwner(objective))
				return Unauthorized();

			objective.Status = newStatus;
			objective.Completed = DateTime.Now;

			await _objectiveRepo.UpdateObjective(objective);

			return Ok(new { objectiveStatus = objective.Status, objectiveStatusBool = objective.StatusBool });
		}

		[NonAction]
		private IEnumerable<Objective> FilterObjectives(IEnumerable<Objective> items, string status, string? name) {
			var filteredItems = items;
			if (status != "All") {
				filteredItems = filteredItems.Where(o => o.Status == status);
			}
			if (!string.IsNullOrEmpty(name))
				filteredItems = filteredItems.Where(o => o.ShortGoal.Contains(name));
			return filteredItems;
		}

		private IEnumerable<Objective> SortObjectives(IEnumerable<Objective> items, SortState sortOrder) {
			var objectives = items;
			switch (sortOrder) {
				case SortState.CreatedAsc:
					objectives = objectives.OrderBy(o => o.Created);
					break;
				case SortState.NameAsc:
					objectives = objectives.OrderBy(o => o.ShortGoal);
					break;
				case SortState.NameDesc:
					objectives = objectives.OrderByDescending(o => o.ShortGoal);
					break;
				case SortState.StatusAsc:
					objectives = objectives.OrderBy(o => o.Status, new StatusComparer()!);
					break;
				case SortState.StatusDesc:
					objectives = objectives.OrderByDescending(o => o.Status, new StatusComparer()!);
					break;
				case SortState.PriorityAsc:
					objectives = objectives.OrderBy(o => o.Priority, new PriorityComparer()!);
					break;
				case SortState.PriorityDesc:
					objectives = objectives.OrderByDescending(o => o.Priority, new PriorityComparer()!);
					break;
				case SortState.DeadlineAsc:
					objectives = objectives.OrderBy(o => o.Deadline);
					break;
				case SortState.DeadlineDesc:
					objectives = objectives.OrderByDescending(o => o.Deadline);
					break;
				default:
					objectives = objectives.OrderByDescending(o => o.Created);
					break;
			}
			return objectives;
		}

		private IEnumerable<Objective> LimitObjectivesOnPage(IEnumerable<Objective> items, int page, int pageSize) {
			var objectives = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			return objectives;
		}
	}
}
