function initializeForm() {

    let submitBtn = document.querySelector('#submitBtn')
    submitBtn.addEventListener('click', submit)

    let cancelBtn = document.querySelector('#cancelBtn')
    cancelBtn.addEventListener('click', function () {
        window.location.href = 'grupe.html'
    })



}



function submit() {
    const form = document.querySelector('#form')
    const formData = new FormData(form)

    const reqBody = {
        ime: formData.get('ime'),
        datumOsnivanja: formData.get('datumOsnivanja')
    }


    const nameErrorMessage = document.querySelector('#imeError')
    nameErrorMessage.textContent = ''
    const dateErrorMessage = document.querySelector('#datumError')
    dateErrorMessage.textContent = ''

    if (reqBody.ime.trim() === '') {
        nameErrorMessage.textContent = 'Name field is required'
        return
    }

    if (reqBody.datumOsnivanja === null || reqBody.datumOsnivanja === undefined) {
        dateErrorMessage.textContent = 'Date field is required'
        return
    }

    console.log(ime)
    console.log(datumOsnivanja)

    let method = 'POST'
    let url = 'http://localhost:41322/api/grupe'

    const urlParams = new URLSearchParams(window.location.search)
    const id = urlParams.get('id')

    if (id) {
        method = 'PUT'
        url = url + "/" + id
    }

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(reqBody)
    })
        .then(response => {
            if (!response.ok) {
                const error = new Error('Request failed. Status: ' + response.status)
                error.response = response
                throw error
            }
            return response.json()
        })
        .then(data => {
            window.location.href = 'grupe.html'
        })
        .catch(error => {
            console.error('Error: ' + error.message)
            if (error.response && error.response.status === 404) {
                alert('Book does not exist!')
            }
            else if (error.response && error.response.status === 400) {
                alert('Data is invalid!')
            }
            else {
                alert('An error occurred while updating data. Please try again.')
            }
        })
}



document.addEventListener('DOMContentLoaded', initializeForm)