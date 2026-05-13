const uri = '/api/Comics';
let comics = [];

function getComics() {
    fetch(uri)
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося отримати комікси.');
            }
            return response.json();
        })
        .then(data => {
            comics = data;
            _displayComics(comics);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to get comics.', error);
        });
}

function addComic() {
    const comic = {
        title: document.getElementById('add-title').value.trim(),
        description: document.getElementById('add-description').value.trim(),
        price: parseFloat(document.getElementById('add-price').value),
        stockQuantity: parseInt(document.getElementById('add-stockQuantity').value),
        releaseYear: parseInt(document.getElementById('add-releaseYear').value),
        publisherId: parseInt(document.getElementById('add-publisherId').value),
        genreId: parseInt(document.getElementById('add-genreId').value),
        coverImagePath: document.getElementById('add-coverImagePath').value.trim() || null,
        isActive: document.getElementById('add-isActive').checked
    };

    if (!comic.title) {
        alert('Введи назву коміксу.');
        return;
    }

    if (isNaN(comic.price) || isNaN(comic.stockQuantity) || isNaN(comic.publisherId) || isNaN(comic.genreId)) {
        alert('Перевір числові поля. Для ціни використовуй крапку, наприклад 249.99');
        return;
    }

    fetch('/api/Comics', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(comic)
    })
        .then(async response => {
            const text = await response.text();

            if (!response.ok) {
                alert(text || 'Не вдалося додати комікс.');
                throw new Error(text || 'Не вдалося додати комікс.');
            }

            return text ? JSON.parse(text) : null;
        })
        .then(createdComic => {
            if (createdComic) {
                comics.push(createdComic);
                _displayComics(comics);
            } else {
                getComics();
            }

            clearAddForm();
        })
        .catch(error => {
            console.error('Unable to add comic.', error);
        });
}

function deleteComic(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося видалити комікс.');
            }

            comics = comics.filter(c => c.id !== id);
            _displayComics(comics);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to delete comic.', error);
        });
}

function displayEditForm(id) {
    const comic = comics.find(c => c.id === id);

    document.getElementById('edit-id').value = comic.id;
    document.getElementById('edit-title').value = comic.title ?? '';
    document.getElementById('edit-description').value = comic.description ?? '';
    document.getElementById('edit-price').value = comic.price ?? '';
    document.getElementById('edit-stockQuantity').value = comic.stockQuantity ?? '';
    document.getElementById('edit-releaseYear').value = comic.releaseYear ?? '';
    document.getElementById('edit-publisherId').value = comic.publisherId ?? '';
    document.getElementById('edit-genreId').value = comic.genreId ?? '';
    document.getElementById('edit-coverImagePath').value = comic.coverImagePath ?? '';
    document.getElementById('edit-isActive').checked = comic.isActive;

    document.getElementById('editComic').style.display = 'block';
}

function updateComic() {
    const comicId = document.getElementById('edit-id').value;

    const comic = {
        title: document.getElementById('edit-title').value.trim(),
        description: document.getElementById('edit-description').value.trim(),
        price: parseFloat(document.getElementById('edit-price').value),
        stockQuantity: parseInt(document.getElementById('edit-stockQuantity').value),
        releaseYear: parseInt(document.getElementById('edit-releaseYear').value),
        publisherId: parseInt(document.getElementById('edit-publisherId').value),
        genreId: parseInt(document.getElementById('edit-genreId').value),
        coverImagePath: document.getElementById('edit-coverImagePath').value.trim() || null,
        isActive: document.getElementById('edit-isActive').checked
    };

    if (!comic.title) {
        alert('Введи назву коміксу.');
        return false;
    }

    fetch(`${uri}/${comicId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(comic)
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося оновити комікс.');
            }

            getComics();
            closeInput();
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to update comic.', error);
        });

    return false;
}

function closeInput() {
    document.getElementById('editComic').style.display = 'none';
}

function clearAddForm() {
    document.getElementById('add-title').value = '';
    document.getElementById('add-description').value = '';
    document.getElementById('add-price').value = '';
    document.getElementById('add-stockQuantity').value = '';
    document.getElementById('add-releaseYear').value = '';
    document.getElementById('add-publisherId').value = '';
    document.getElementById('add-genreId').value = '';
    document.getElementById('add-coverImagePath').value = '';
    document.getElementById('add-isActive').checked = true;
}

function _displayComics(data) {
    const tBody = document.getElementById('comics');
    const counter = document.getElementById('counter');

    tBody.innerHTML = '';
    const button = document.createElement('button');

    data.forEach(comic => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${comic.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteComic(${comic.id})`);

        let tr = tBody.insertRow();

        tr.insertCell(0).appendChild(document.createTextNode(comic.title ?? ''));
        tr.insertCell(1).appendChild(document.createTextNode(comic.description ?? ''));
        tr.insertCell(2).appendChild(document.createTextNode(comic.price ?? ''));
        tr.insertCell(3).appendChild(document.createTextNode(comic.stockQuantity ?? ''));
        tr.insertCell(4).appendChild(document.createTextNode(comic.releaseYear ?? ''));
        tr.insertCell(5).appendChild(document.createTextNode(comic.genre ? comic.genre.name : comic.genreId));
        tr.insertCell(6).appendChild(document.createTextNode(comic.publisher ? comic.publisher.name : comic.publisherId));
        tr.insertCell(7).appendChild(document.createTextNode(comic.isActive ? 'Так' : 'Ні'));

        let tdEdit = tr.insertCell(8);
        tdEdit.appendChild(editButton);

        let tdDelete = tr.insertCell(9);
        tdDelete.appendChild(deleteButton);
    });

    counter.innerText = `Кількість коміксів: ${data.length}`;
}