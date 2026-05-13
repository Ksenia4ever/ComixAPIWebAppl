const uri = '/api/Genres';
let genres = [];

function getGenres() {
    fetch(uri)
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося отримати жанри.');
            }
            return response.json();
        })
        .then(data => {
            genres = data;
            _displayGenres(genres);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to get genres.', error);
        });
}

function addGenre() {
    const addNameTextbox = document.getElementById('add-name');

    const genre = {
        name: addNameTextbox.value.trim()
    };

    if (!genre.name) {
        alert('Введи назву жанру.');
        return;
    }

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(genre)
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося додати жанр.');
            }
            return response.json();
        })
        .then(createdGenre => {
            genres.push(createdGenre);
            _displayGenres(genres);
            addNameTextbox.value = '';
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to add genre.', error);
        });
}

function deleteGenre(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося видалити жанр.');
            }

            genres = genres.filter(g => g.id !== id);
            _displayGenres(genres);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to delete genre.', error);
        });
}

function displayEditForm(id) {
    const genre = genres.find(g => g.id === id);

    document.getElementById('edit-id').value = genre.id;
    document.getElementById('edit-name').value = genre.name;
    document.getElementById('editGenre').style.display = 'block';
}

function updateGenre() {
    const genreId = document.getElementById('edit-id').value;

    const genre = {
        name: document.getElementById('edit-name').value.trim()
    };

    if (!genre.name) {
        alert('Введи назву жанру.');
        return false;
    }

    fetch(`${uri}/${genreId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(genre)
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося оновити жанр.');
            }

            getGenres();
            closeInput();
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to update genre.', error);
        });

    return false;
}

function closeInput() {
    document.getElementById('editGenre').style.display = 'none';
}

function _displayGenres(data) {
    const tBody = document.getElementById('genres');
    const counter = document.getElementById('counter');

    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(genre => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${genre.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteGenre(${genre.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(genre.name));

        let td2 = tr.insertCell(1);
        td2.appendChild(editButton);

        let td3 = tr.insertCell(2);
        td3.appendChild(deleteButton);
    });

    counter.innerText = `Кількість жанрів: ${data.length}`;
}