document.querySelectorAll(".btn-delete").forEach((btn) => {
	btn.addEventListener("click", async () => { await deleteObjective(btn) });
});

document.querySelectorAll(".btn-finish").forEach((btn) => {
	btn.addEventListener("click", async () => { await finishObjective(btn) });
});

document.querySelectorAll(".btn-abandon").forEach((btn) => {
	btn.addEventListener("click", async () => { await abandonObjective(btn) });
});

document.querySelectorAll(".task-name").forEach((p) => {
	p.addEventListener("click", () => {
		const goal = p.dataset.goal;
		showNotification(goal);
	});
});

document.querySelectorAll(".task-checkbox").forEach((checkbox) => {
	const taskStatus = checkbox.dataset.status;
	const taskStatusBool = (taskStatus.toLowerCase() === "true");
	if (taskStatusBool)
		checkbox.checked = true;
	else
		checkbox.checked = false;
});

document.getElementById("add-task-btn").addEventListener("click", async e => {
	let sg = document.getElementById("short-goal").value;
	let prior = document.getElementById("priority").value;
	let g = document.getElementById("goal").value;
	let tl = document.getElementById("timeleft").value;

	if (sg.trim().length == "" || prior.trim().length == "" || g.trim().length == "" || tl < 1) {
		showNotification("Fields must not be empty");
		return;
	}


	e.preventDefault();
	await addObjective(sg, prior, g, tl);
});

async function addObjective(shortGoal, priority, goal, timeLeft) {
	const response = await fetch("/objective/addtask", {
		method: "POST",
		headers: { "Content-Type": "application/json", "Accept": "application/json" },
		body: JSON.stringify({
			shortGoal,
			priority,
			goal,
			deadlineInHours: timeLeft
		})
	});
	if (response.ok) {
		let objective = await response.json();
		generateObjectiveElement(objective);
		closeForm();
		newObjectiveAnnounce();
	}
	else {
		console.error("FailedToAddNewTask");
	}
}

async function deleteObjective(btn) {
	const confirmation = await showConfirmation("Are you sure you want to delete task?");
	if (confirmation) { 
		const objectiveId = btn.dataset.id;
		const response = await fetch("/objective/deletetask/", {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			body: JSON.stringify({ objectiveId })
		});
		if (response.ok)
			btn.closest(".task-info").remove();
		else
			console.error("FailedToDeleteTask");
	}
}

async function finishObjective(btn) {
	const confirmation = await showConfirmation("Have you really finished your task?");
	if (confirmation) {
		let newStatus =  await fetchChangeStatus("finishtask", btn);
		if (newStatus == "Completed")
			await victoryAchievedAnnounce();
	}
}

async function abandonObjective(btn) {
	const confirmation = await showConfirmation("Do you really want to abandon your task?");
	if (confirmation) {
		let newStatus = await fetchChangeStatus("abandontask", btn);
		if (newStatus == "Abandoned")
			await youDiedAnnounce();
	}
}

async function fetchChangeStatus(action, btn) {
	let objectiveId = btn.dataset.id;
	const statusText = btn.closest(".task-info").querySelector(".task-status");
	if (statusText.innerText == "Abandoned" || statusText.innerText == "Completed") {
		const confirmation = await showConfirmation("Task is already completed/abandoned. Do you want to reset task?");
		if (confirmation)
			action = "resettask";
		else
			return "Cancelled";
	}

	const response = await fetch(`/objective/${action}`, {
		method: "POST",
		headers: { "Content-Type": "application/json", "Accept": "application/json" },
		body: JSON.stringify({ objectiveId })
	});
	if (response.ok) {
		let data = await response.json();
		statusText.innerText = data.objectiveStatus;
		btn.closest(".task-info").querySelector(".task-checkbox").dataset.status = data.objectiveStatusBool;
		stylePriorityAndStatus();
		checkStatus();
		return data.objectiveStatus;
	}
	else {
		console.error("FailedToChangeTaskStatus");
		return "Error";
	}
}

function generateObjectiveElement(objective) {
	let taskDiv = document.createElement("div");
	taskDiv.classList.add("task-info");
	let formatedTime = formatDateTime(new Date(objective.created));
	let formatedDeadline = formatDateTime(new Date(objective.deadline));
	let htmlCode = `
			<input type="checkbox" class="task-checkbox" data-status="${objective.statusBool}" disabled />
			<input class="task-id" type="hidden" value="${objective.id}" />
			<p class="task-name btn-dark-souls" data-goal="${objective.goal}">${objective.shortGoal}</p>
			<p class="task-status">${objective.status}</p>
			<p class="task-priority">${objective.priority}</p>
			<p>${formatedTime}</p>
			<p data-timeleft="${objective.timeLeft}" data-timeleft="${objective.status}" class="deadline-text">${formatedDeadline}</p>
	`;

	let taskControls = document.createElement("div");
	taskControls.classList.add("task-controls");

	const createButton = (text, className, id, onClick) => {
		const button = document.createElement("button");
		button.innerText = text;
		button.classList.add("btn-dark-souls-square", "btn-smaller", className);
		button.setAttribute("data-id", id);
		button.addEventListener("click", onClick);
		return button;
	}

	const btnFinish = createButton("Finish", "btn-finish", objective.id, async () => {
		await finishObjective(btnFinish);
	}); 
	const btnAbandon = createButton("Abandon", "btn-abandon", objective.id, async () => {
		await abandonObjective(btnAbandon);
	});
	const btnDelete = createButton("X", "btn-delete", objective.id, async () => {
		await deleteObjective(btnDelete);
	});
	taskControls.append(btnFinish, btnAbandon, btnDelete);

	taskDiv.innerHTML = htmlCode;
	taskDiv.appendChild(taskControls);
	let taskName = taskDiv.querySelector(".task-name");
	taskName.addEventListener("click", () => {
		const goal = taskName.dataset.goal;
		showNotification(goal);
	});;

	document.getElementById("tasks-list").prepend(taskDiv);
}