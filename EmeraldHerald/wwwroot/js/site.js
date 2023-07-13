function formatDateTime(dateTime) {
	const months = [
		"Jan", "Feb", "Mar", "Apr", "May", "Jun",
		"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
	];

	const day = String(dateTime.getDate()).padStart(2, "0");
	const month = months[dateTime.getMonth()];
	const year = dateTime.getFullYear();
	const hours = String(dateTime.getHours()).padStart(2, "0");
	const minutes = String(dateTime.getMinutes()).padStart(2, "0");
	const period = hours >= 12 ? "PM" : "AM";

	return `${day} ${month} ${year} ${hours}:${minutes} ${period}`;
}

let resolveFunction;
let popupPromise;

function showConfirmation(question) {
	document.getElementById("confirmOverlayQuestion").innerHTML = question;
	document.getElementById("confirmOverlay").style.display = "flex";
	popupPromise = new Promise((resolve) => {
		resolveFunction = resolve;
	});
	return popupPromise;
}

function confirmPopup(result) {
	document.getElementById("confirmOverlay").style.display = "none";
	resolveFunction(result);
}

async function exampleFunction() {
	const result = await showConfirmation("Are you sure you wan to leave?");
	console.log(result);
}

function showNotification(text) {
	document.getElementById("notificationContent").innerHTML = text;
	document.getElementById("notificationOverlay").style.display = "flex";
}

function closeNotification() {
	document.getElementById("notificationOverlay").style.display = "none";
}


async function victoryAchievedAnnounce() {
	var popup = document.getElementById("announce-popup");
	var text = document.getElementById("announce-text");
	var victorySfx = document.getElementById("victory-sfx");
	victorySfx.volume = 0.5;

	victorySfx.play();

	await setTimeout(function () {
		text.innerText = "VICTORY ACHIEVED"
		text.classList.add("victory-popup-text");
		popup.classList.add("show");
		popup.style.visibility = "visible";
	}, 2000);

	await setTimeout(function () {
		popup.classList.remove("show");
	}, 6000);

	await setTimeout(function () {
		text.classList.remove("victory-popup-text");
		popup.style.visibility = "hidden";
	}, 7000)
}

async function youDiedAnnounce() {
	var popup = document.getElementById("announce-popup");
	var text = document.getElementById("announce-text");
	var victorySfx = document.getElementById("death-sfx");
	victorySfx.volume = 0.3;

	victorySfx.play();

	text.innerText = "YOU DIED"
	text.classList.add("abandoned-popup-text");
	popup.classList.add("show");
	popup.style.visibility = "visible";


	await setTimeout(function () {
		popup.classList.remove("show");
	}, 6000);

	await setTimeout(function () {
		text.classList.remove("abandoned-popup-text");
		popup.style.visibility = "hidden";
	}, 7000)
}

async function newObjectiveAnnounce() {
	var popup = document.getElementById("new-objective-popup");
	var victorySfx = document.getElementById("new-area-sfx");
	victorySfx.volume = 0.1;

	victorySfx.play();

	popup.classList.add("show");
	popup.style.visibility = "visible";


	await setTimeout(function () {
		popup.classList.remove("show");
	}, 3000);

	await setTimeout(function () {
		popup.style.visibility = "hidden";
	}, 4000)
}