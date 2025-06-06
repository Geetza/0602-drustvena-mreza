function getKorisnici() {
  let urlParams = new URLSearchParams(window.location.search);
  let id = urlParams.get("id");
  if (!id) {
    return;
  }

  fetch(`http://localhost:41322/api/grupe/${id}/korisnici`)
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
    editBtn.textContent = "Izbaci iz grupe";
    editBtn.addEventListener("click", function () {
      let urlParams = new URLSearchParams(window.location.search);
      let grupaId = urlParams.get("id");
      izbaciIzGrupe(grupaId, korisnik["id"]);
    });
    cell3.appendChild(editBtn);

    newRow.appendChild(cell1);
    newRow.appendChild(cell2);
    newRow.appendChild(cell3);
    table.appendChild(newRow);
  });
}

function getWithNoGrp() {

  let urlParams = new URLSearchParams(window.location.search);
  let id = urlParams.get("id");

  if (!id) {
    return
  }

  fetch("http://localhost:41322/api/grupe/" + id + "/korisnici-van-grupe")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Request failed. Status: " + response.status);
      }
      return response.json();
    })
    .then((korisnici) => renderKorisniciWithNoGrp(korisnici))
    .catch((error) => {
      console.error("Error:", error.message);

      let table = document.querySelector("#korisnici-table");
      if (table) {
        table.style.display = "none";
      }

      alert("Error occurred while loading the data. Please try again.");
    });
}

function renderKorisniciWithNoGrp(data) {
  let table = document.querySelector("#korisnici-bez-grupe-tbody");
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
    editBtn.textContent = "Dodaj u grupu";
    editBtn.addEventListener("click", function () {
      // funkcija za dodavanje korisnika u grupu
      let urlParams = new URLSearchParams(window.location.search);
      let grupaId = urlParams.get("id");
      UbaciUGrupu(grupaId, korisnik["id"]);
    });
    cell3.appendChild(editBtn);

    newRow.appendChild(cell1);
    newRow.appendChild(cell2);
    newRow.appendChild(cell3);
    table.appendChild(newRow);
  });
}

function UbaciUGrupu(grupaId, korisnikId) {
  fetch(`http://localhost:41322/api/grupe/${grupaId}/korisnici/${korisnikId}`, {
    method: "PUT",
  })
    .then((response) => {
      if (!response.ok) {
        const error = new Error("Request failed. Status: " + response.status);
        error.response = response;
        throw error;
      }
      alert("Korisnik je uspešno dodat u grupu.");
      getKorisnici();
      getWithNoGrp();
    })
    .catch((error) => {
      console.error("Error: " + error.message);
      if (error.response && error.response === 404) {
        alert("Korisnik nije u grupi");
      } else {
        alert("Doslo je do greske. Pokusajte ponovo.");
      }
    });
}

function izbaciIzGrupe(grupaId, korisnikId) {
  fetch(`http://localhost:41322/api/grupe/${grupaId}/korisnici/${korisnikId}`, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        const error = new Error("Request failed. Status: " + response.status);
        error.response = response;
        throw error;
      }
      alert("Korisnik je uspešno izbačen iz grupe.");
      getKorisnici();
      getWithNoGrp();
    })
    .catch((error) => {
      console.error("Error: " + error.message);
      if (error.response && error.response === 404) {
        alert("Korisnik nije u grupi");
      } else {
        alert("Doslo je do greske. Pokusajte ponovo.");
      }
    });
}

function init() {
  getKorisnici();
  getWithNoGrp();
}

document.addEventListener("DOMContentLoaded", init);
