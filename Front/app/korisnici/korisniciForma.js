"use strict";

function incijalizujFormu() {
  let cancelBtn = document.querySelector("#cancelBtn");
  cancelBtn.addEventListener("click", function () {
    window.location.href = "korisnici.html";
  });

  let submitBtn = document.querySelector("#submitBtn");
  submitBtn.addEventListener("click", obradiFormu);

  get();
}

function get() {
  let urlParams = new URLSearchParams(window.location.search);
  const id = urlParams.get("id");
  if (!id) {
    return;
  }

  fetch("http://localhost:41322/api/korisnici/" + id)
    .then((response) => {
      if (!response.ok) {
        const error = new Error("Request failed. Status: " + response.status);
        error.response = response;
        throw error;
      }
      return response.json();
    })
    .then((korisnik) => {
      document.querySelector("#korIme").value = korisnik.korIme;
      document.querySelector("#ime").value = korisnik.ime;
      document.querySelector("#prezime").value = korisnik.prezime;
      document.querySelector("#datumRodjenja").value = korisnik.datumRodjenja;
    })
    .catch((error) => {
      console.error("Error:");
      if (error.response && error.response === 404) {
        alert("Korisnik ne postoji");
      } else {
        alert("Error occurred while loading the data. Please try again.");
      }
    });
}

function obradiFormu() {
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

  let method = "POST";
  let url = "http://localhost:41322/api/korisnici";
  const urlParams = new URLSearchParams(window.location.search);
  const id = urlParams.get("id");
  if (id) {
    method = "PUT";
    url = "http://localhost:41322/api/korisnici/" + id;
  }

  fetch(url, {
    method: method,
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
}

addEventListener("DOMContentLoaded", incijalizujFormu);
