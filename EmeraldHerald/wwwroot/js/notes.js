document.querySelectorAll(".note-button").forEach(btn => {
	let noteId = btn.dataset.noteid;
	btn.addEventListener("click", async () => { await openNote(noteId); });
})

document.querySelectorAll(".close-note-btn").forEach(btn => {
	btn.addEventListener("click", (e) => {
		e.preventDefault();
		closeNote();
	})
});

function openForm() {
	document.getElementById("myForm").style.display = "flex";
}

function closeForm() {
	document.getElementById("myForm").style.display = "none";
}

async function openNote(noteId) {
	const response = await fetch(`/note/details/${noteId}`, {
		method: "GET",
		headers: { "Accept" : "application/json" }
	});
	if (response.ok) {
		let note = await response.json();
		document.getElementById("noteName").innerText = note.name;
		document.getElementById("noteBody").value = note.body;
		document.getElementById("saveId").value = note.id;
		document.getElementById("deleteId").value = note.id;
		document.getElementById("noteOverlay").style.display = "flex";

	}
	else
		showNotification("Something went wrong");
}

function closeNote() {
	document.getElementById("noteOverlay").style.display = "none";
}