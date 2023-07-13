const tasksList = document.getElementById("tasks-list");

stylePriorityAndStatus();
checkStatus();
checkDeadline();

function checkDeadline() {
	let dangerCount = 0;
	document.querySelectorAll(".deadline-text").forEach(deadline => {
		let timeLeft = deadline.dataset.timeleft;
		let status = deadline.dataset.status;
		if (timeLeft > 0 && timeLeft < 10800 && status === "Active")
			++dangerCount;
	});
	if (dangerCount > 0)
		showNotification(`${dangerCount} task(s) will expire soon`);
}


const observer = new MutationObserver((mutationList, observer) => {
	for (let mutation of mutationList) {
		if (mutation.type === "childList" && mutation.addedNodes.length > 0) {
			stylePriorityAndStatus();
			checkStatus();
			checkDeadline();
		}
	}
});

const config = { childList: true };
observer.observe(tasksList, config);

function stylePriorityAndStatus() {
	let priortyElems = document.querySelectorAll(".task-priority");
	let statusElems = document.querySelectorAll(".task-status");
	priortyElems.forEach(priortyElem => {
		switch (priortyElem.innerText) {
			case "High":
				priortyElem.classList.add("priority-high");
				break;
			case "Low":
				priortyElem.classList.add("priority-low");
				break;
			default:
				priortyElem.classList.add("priority-medium")
				break;
		}
	});
	statusElems.forEach(statusElem => {
		switch (statusElem.innerText) {
			case "Abandoned":
				statusElem.classList.remove("status-completed");
				statusElem.classList.remove("status-active");
				statusElem.classList.add("status-abandoned");
				break;
			case "Completed":
				statusElem.classList.remove("status-active");
				statusElem.classList.remove("status-abandoned");
				statusElem.classList.add("status-completed");
				break;
			default:
				statusElem.classList.remove("status-completed");
				statusElem.classList.remove("status-abandoned");
				statusElem.classList.add("status-active")
				break;
		}
	});
}



function checkStatus() {
	document.querySelectorAll(".task-checkbox").forEach((checkbox) => {
		const taskStatus = checkbox.dataset.status;
		const taskName = checkbox.nextElementSibling.nextElementSibling;
		const taskStatusBool = (taskStatus.toLowerCase() === "true");
		if (taskStatusBool) {
			checkbox.checked = true;
			taskName.style.textDecoration = "line-through";
		}
		else {
			checkbox.checked = false;
			taskName.style.textDecoration = "none";
		}
	});
}

function openForm() {
	document.getElementById("myForm").style.display = "flex";
}

function closeForm() {
	document.getElementById("myForm").style.display = "none";
}