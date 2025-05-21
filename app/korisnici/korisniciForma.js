"use strict";

let cancelBtn = document.querySelector("#cancelBtn");
cancelBtn.addEventListener("click", function () {
  window.location.href = "korisnici.html";
});

let submitBtn = document.querySelector("#submitBtn");
submitBtn.addEventListener("click", function () {
  const form = document.querySelector("#form");
  const formData = new FormData(form);

  // let korIme = formData.get("korIme");
  // let ime = formData.get("ime");
  // let prezime = formData.get("prezime");
  // let datumRodjenja = formData.get("datumRodjenja");

  const reqBody = {
    korIme: formData.get("korIme"),
    ime: formData.get("ime"),
    prezime: formData.get("prezime"),
    datumRodjenja: formData.get("datumRodjenja"),
  };

  const korImeErrorMessage = document.querySelector("#korImeError");
  korImeErrorMessage.textContent = "";
  const imeErrorMessage = document.querySelector("#imeError");
  imeErrorMessage.textContent = "";
  const prezimeErrorMessage = document.querySelector("#prezimeError");
  prezimeErrorMessage.textContent = "";
  const datumRodjenjaErrorMessage = document.querySelector(
    "#datumRodjenjaError"
  );
  datumRodjenjaErrorMessage.textContent = "";

  if (reqBody.korIme.trim() === "") {
    // Validacije da uneti podaci nisu prazni
    korImeErrorMessage.textContent =
      "Polje korisnicko ime je obavezno popuniti.";
    return;
  }

  if (reqBody.ime.trim() === "") {
    imeErrorMessage.textContent = "Polje ime je obavezno popuniti.";
    return;
  }

  if (reqBody.prezime.trim() === "") {
    prezimeErrorMessage.textContent = "Polje prezime je obavezno popuniti.";
    return;
  }

  if (reqBody.datumRodjenja.trim() === "") {
    datumRodjenjaErrorMessage.textContent =
      "Polje datum rodjenja je obavezno popuniti.";
    return;
  }

  fetch("http://localhost:41322/api/korisnici", {
    method: "POST",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(reqBody),
  })
    .then((response) => {
      if (!response.ok) {
        const error = new Error("Request failed. Status: " + response.status);
        error.response = error;
      }
      return response.json();
    })
    .then((data) => {
      alert("Korisnik uspesno dodat.");
      window.location.href = "korisnici.html";
    })
    .catch((error) => {
      console.error("Error:", error.message);
      if (error.response && error.response.status === 400) {
        alert("Data is invalid!");
      } else {
        alert("An error occurred while updating the data. Please try again.");
      }
    });
});
