const uri = '/api/Publishers';
let publishers = [];

function getPublishers() {
    fetch(uri)
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося отримати видавництва.');
            }
            return response.json();
        })
        .then(data => {
            publishers = data;
            _displayPublishers(publishers);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to get publishers.', error);
        });
}

function addPublisher() {
    const publisher = {
        name: document.getElementById('add-name').value.trim(),
        country: document.getElementById('add-country').value.trim()
    };

    if (!publisher.name) {
        alert('Введи назву видавництва.');
        return;
    }

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(publisher)
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося додати видавництво.');
            }
            return response.json();
        })
        .then(createdPublisher => {
            publishers.push(createdPublisher);
            _displayPublishers(publishers);
            clearAddForm();
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to add publisher.', error);
        });
}

function deletePublisher(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося видалити видавництво.');
            }

            publishers = publishers.filter(p => p.id !== id);
            _displayPublishers(publishers);
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to delete publisher.', error);
        });
}

function displayEditForm(id) {
    const publisher = publishers.find(p => p.id === id);

    document.getElementById('edit-id').value = publisher.id;
    document.getElementById('edit-name').value = publisher.name ?? '';
    document.getElementById('edit-country').value = publisher.country ?? '';
    document.getElementById('editPublisher').style.display = 'block';
}

function updatePublisher() {
    const publisherId = document.getElementById('edit-id').value;

    const publisher = {
        id: parseInt(publisherId, 10),
        name: document.getElementById('edit-name').value.trim(),
        country: document.getElementById('edit-country').value.trim()
    };

    if (!publisher.name) {
        alert('Введи назву видавництва.');
        return false;
    }

    fetch(`${uri}/${publisherId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(publisher)
    })
        .then(async response => {
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Не вдалося оновити видавництво.');
            }

            getPublishers();
            closeInput();
        })
        .catch(error => {
            alert(error.message);
            console.error('Unable to update publisher.', error);
        });

    return false;
}

function closeInput() {
    document.getElementById('editPublisher').style.display = 'none';
}

function clearAddForm() {
    document.getElementById('add-name').value = '';
    document.getElementById('add-country').value = '';
}

function _displayPublishers(data) {
    const tBody = document.getElementById('publishers');
    const counter = document.getElementById('counter');

    tBody.innerHTML = '';
    const button = document.createElement('button');

    data.forEach(publisher => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${publisher.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deletePublisher(${publisher.id})`);

        let tr = tBody.insertRow();

        tr.insertCell(0).appendChild(document.createTextNode(publisher.name ?? ''));
        tr.insertCell(1).appendChild(document.createTextNode(publisher.country ?? ''));

        let tdEdit = tr.insertCell(2);
        tdEdit.appendChild(editButton);

        let tdDelete = tr.insertCell(3);
        tdDelete.appendChild(deleteButton);
    });

    counter.innerText = `Кількість видавництв: ${data.length}`;
}