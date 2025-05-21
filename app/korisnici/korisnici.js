"use strict";

function getKorisnici() {
  fetch("http://localhost:41322/api/korisnici")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Request failed. Status: " + response.status);
      }
      return response.json();
    })
    .then((korisnici) => renderKorisnici(korisnici))
    .catch((error) => {
      console.error("Error:", error.message);

      let table = document.querySelector("#korisnici-table");
      if (table) {
        table.style.display = "none";
      }

      alert("Error occurred while loading the data. Please try again.");
    });
}

function renderKorisnici(data) {
  let table = document.querySelector("#korisnici-tbody");
  table.innerHTML = "";

  data.forEach((korisnik) => {
    let newRow = document.createElement("tr");

    let cell1 = document.createElement("td");
    cell1.textContent = korisnik["korIme"];

    let cell2 = document.createElement("td");

    let datumRodjenja = korisnik["datumRodjenja"];
    const datum = new Date(datumRodjenja);
    const godina = datum.getFullYear();
    const mesec = String(datum.getMonth() + 1).padStart(2, "0");
    const dan = String(datum.getDate()).padStart(2, "0");
    const formatiranDatum = `${godina}-${mesec}-${dan}`;
    cell2.textContent = formatiranDatum;

    let cell3 = document.createElement("td");
    let editBtn = document.createElement("button");
    editBtn.textContent = "Izmeni";
    editBtn.addEventListener("click", function () {
      window.location.href = "korisniciForma.html?id=" + korisnik["id"];
    });
    cell3.appendChild(editBtn);

    newRow.appendChild(cell1);
    newRow.appendChild(cell2);
    newRow.appendChild(cell3);
    table.appendChild(newRow);
  });
}

let addBtn = document.querySelector("#addBtn");
addBtn.addEventListener("click", function () {
  window.location.href = "korisniciForma.html";
});

document.addEventListener("DOMContentLoaded", getKorisnici);
