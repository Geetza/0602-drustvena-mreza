function inicijalizujGrupe() {
  let addGrupaBtn = document.querySelector("#addGrupaBtn");
  addGrupaBtn.addEventListener("click", function () {
    window.location.href = "./grupeForma.html";
  });

  getGrupe();
}

function getGrupe() {
  fetch("http://localhost:41322/api/grupe")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Request failed. Status: " + response.status);
      }
      return response.json();
    })
    .then((grupe) => prikaziGrupe(grupe))
    .catch((error) => {
      console.error("Error: " + error.message);

      alert("An error occured while loading data. Please try again.");
    });
}

function prikaziGrupe(data) {
  let table = document.querySelector("#grupe-table tbody");

  table.innerHTML = "";

  data.forEach((grupa) => {
    let newRow = document.createElement("tr");

    let cell1 = document.createElement("td");
    cell1.textContent = grupa["ime"];
    newRow.appendChild(cell1);

    let cell2 = document.createElement("td");
    let datumOsnivanja = grupa["datumOsnivanja"];
    const datum = new Date(datumOsnivanja);
    const godina = datum.getFullYear();
    const mesec = String(datum.getMonth() + 1).padStart(2, "0");
    const dan = String(datum.getDate()).padStart(2, "0");
    const formatiranDatum = `${godina}-${mesec}-${dan}`;
    cell2.textContent = formatiranDatum;
    newRow.appendChild(cell2);

    let cell3 = document.createElement("td");
    let deleteButton = document.createElement("button");
    deleteButton.textContent = "Delete";
    deleteButton.addEventListener("click", function () {
      fetch("http://localhost:41322/api/grupe/" + grupa["id"], {
        method: "DELETE",
      })
        .then((response) => {
          if (!response.ok) {
            const error = new Error(
              "Request failed. Status: " + response.status
            );
            error.response = response;
            throw error;
          }
          getGrupe();
        })
        .catch((error) => {
          console.error("Error:" + error.message);
          if (error.response && error.response.status === 404) {
            alert("Group does not exist!");
          } else {
            alert("An error occurred while creating data. Please try again.");
          }
        });
    });
    cell3.appendChild(deleteButton);
    newRow.append(cell3);

    let cell4 = document.createElement("td");
    let viewButton = document.createElement("button");
    viewButton.textContent = "Korisnici";
    viewButton.addEventListener("click", function () {
      fetch("http://localhost:41322/api/grupe/" + grupa["id"] + "/korisnici")
        .then((response) => {
          if (!response.ok) {
            const error = new Error(
              "Request failed. Status: " + response.status
            );
            error.response = response;
            throw error;
          }
          return response.json();
        })
        .then((data) => {
          console.log(data);
          window.location.href =
            "../korisnici/korisniciGrupe.html?id=" + grupa["id"];
        })
        .catch((error) => {
          console.error("Error:" + error.message);
          if (error.response && error.response.status === 404) {
            alert("Group does not exist!");
          } else {
            alert("An error occurred while creating data. Please try again.");
          }
        });
    });

    cell4.appendChild(viewButton);
    newRow.append(cell4);

    table.appendChild(newRow);
  });
}

document.addEventListener("DOMContentLoaded", inicijalizujGrupe);
