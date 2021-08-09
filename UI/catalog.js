async function getCatalogData(bookId) {
    let serverPath = getDeployment("catalog");
    return await axios.get(`http://${serverPath}/book/${bookId}`);
}

async function info(element) {
    let id = parseInt(element.getAttribute("data-id"));
    let cacheResponse = await getCacheValue(`b-${id}`);
    if (cacheResponse !== undefined) {
        return cacheResponse;
    }
    try {
        let response = await getCatalogData(id);
        await setCacheValue(`b-${id}`, response.data);
        return response.data;
    } catch (error) {
        if (error.response && error.response.status === 404) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `There is no book with id = "${id}"`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        } else if (error.response && error.response.status === 400) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `Bad Request to retrive the book book id should be an integer`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        }
        return undefined;
    }

}

function showInfo(data, isArray) {
    if (data != null) {
        const infoTable = document.getElementById("infoTable");
        clearInfoTable();
        if (isArray) {
            for (let i = 0; i < data.length; i++) {
                let tableRow = document.createElement("tr");
                let itemThead = document.createElement("th");
                itemThead.setAttribute("scope", "row");
                itemThead.innerText = data[i].id;
                let name = document.createElement("td");
                name.innerText = data[i].name;
                let topic = document.createElement("td");
                topic.innerText = data[i].topic;
                let price = document.createElement("td");
                price.innerText = data[i].price;
                tableRow.appendChild(itemThead);
                tableRow.appendChild(name);
                tableRow.appendChild(topic);
                tableRow.appendChild(price);
                infoTable.appendChild(tableRow);
            }
        } else {
            let tableRow = document.createElement("tr");
            let itemThead = document.createElement("th");
            itemThead.setAttribute("scope", "row");
            itemThead.innerText = data.id;
            let name = document.createElement("td");
            name.innerText = data.name;
            let topic = document.createElement("td");
            topic.innerText = data.topic;
            let price = document.createElement("td");
            price.innerText = data.price;
            tableRow.appendChild(itemThead);
            tableRow.appendChild(name);
            tableRow.appendChild(topic);
            tableRow.appendChild(price);
            infoTable.appendChild(tableRow);
        }
    }
}

async function showAll() {
    let cacheResponse = await getCacheValue("books");
    if (cacheResponse !== undefined) {
        showInfo(cacheResponse, true);
        return;
    }
    let serverPath = getDeployment("catalog");
    axios.get(`http://${serverPath}/book`).then(async response => {
        const data = response.data;
        showInfo(data, true);
        await setCacheArray("books", data);
    }).catch(error => {
        if (error.request && error.request.status === 404) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `No Books to show please visit us later.`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        }
    });
}

function getSearchMethod() {
    let element = document.getElementsByName('search_method');
    for (let i = 0; i < element.length; i++) {
        if (element[i].checked) {
            return element[i].value;
        }
    }
    return null;
}

async function searchInfo() {
    let serverPath = getDeployment("catalog");
    if (getSearchMethod() === "Topic") {
        let topic = document.getElementById("search").value;
        let cacheResponse = await getCacheValue(`s-topic-${topic}`);
        if (cacheResponse !== undefined) {
            showInfo(cacheResponse, true);
            return;
        }
        axios.get(`http://${serverPath}/book/topic/search/${topic}`).then(async response => {
            const data = response.data;
            showInfo(data, true);
            await setCacheArray(`s-topic-${topic}`, data);
        }).catch(error => {
            clearInfoTable();
            let modalBody = document.getElementById("modalBody");
            if (error.request && error.request.status === 400) {
                modalBody.innerText = `Write something in the search textbox it should not be empty.`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            } else if (error.request && error.request.status === 404) {
                modalBody.innerText = `No Books Found`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            }
        });
    } else {
        let name = document.getElementById("search").value;
        let cacheResponse = await getCacheValue(`s-name-${name}`);
        if (cacheResponse !== undefined) {
            showInfo(cacheResponse, true);
            return;
        }
        axios.get(`http://${serverPath}/book/name/search/${name}`).then(async response => {
            const data = response.data;
            showInfo(data, true);
            await setCacheArray(`s-name-${name}`, data)
        }).catch(() => {
            clearInfoTable();
            let modalBody = document.getElementById("modalBody");
            if (error.request && error.request.status === 400) {
                modalBody.innerText = `Write something in the search textbox it should not be empty.`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            } else if (error.request && error.request.status === 404) {
                modalBody.innerText = `No Books Found`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            }
        });
    }
}

function showBooks(booksData) {
    const books = document.getElementById("books");
    while (books.lastElementChild) {
        books.removeChild(books.lastElementChild);
    }
    let currentRow = null;
    for (let i = 0; i < booksData.length; i++) {
        if ((i % 2) === 0) {
            let row = document.createElement("div");
            row.className = "row";
            currentRow = row;
            let column = document.createElement("div");
            column.className = "col-lg-6 col-md-12 mb-2 mt-2";
            let card = document.createElement("div");
            card.className = "card";
            let cardBody = document.createElement("div");
            cardBody.className = "card-body";
            let cardHeader = document.createElement("h5");
            cardHeader.className = "card-title";
            cardHeader.innerText = booksData[i].name;
            let purchaseButton = document.createElement("a");
            purchaseButton.className = "btn btn-indigo m-1";
            purchaseButton.innerText = "Purchase";
            purchaseButton.onclick = function () {
                purchase(this).then(response => {
                    showOrder(response, false);
                });
            }
            purchaseButton.href = "#";
            purchaseButton.setAttribute("data-id", booksData[i].id);
            let infoButton = document.createElement("a");
            infoButton.className = "btn btn-indigo m-1";
            infoButton.innerText = "Info";
            infoButton.onclick = function () {
                info(this).then(response => {
                    if (response !== undefined) {
                        showInfo(response, false);
                    }
                });
            }
            infoButton.href = "#";
            infoButton.setAttribute("data-id", booksData[i].id);
            cardBody.appendChild(cardHeader);
            cardBody.appendChild(purchaseButton);
            cardBody.appendChild(infoButton);
            card.appendChild(cardBody);
            column.appendChild(card);
            row.appendChild(column);
            books.append(row);
        } else {
            let column = document.createElement("div");
            column.className = "col-lg-6 col-md-12 mb-2 mt-2";
            let card = document.createElement("div");
            card.className = "card";
            let cardBody = document.createElement("div");
            cardBody.className = "card-body";
            let cardHeader = document.createElement("h5");
            cardHeader.className = "card-title";
            cardHeader.innerText = booksData[i].name;
            let purchaseButton = document.createElement("a");
            purchaseButton.className = "btn btn-indigo m-1";
            purchaseButton.innerText = "Purchase";
            purchaseButton.onclick = function () {
                purchase(this).then(response => {
                    showOrder(response, false);
                });
            }
            purchaseButton.href = "#";
            purchaseButton.setAttribute("data-id", booksData[i].id);
            let infoButton = document.createElement("a");
            infoButton.className = "btn btn-indigo m-1";
            infoButton.innerText = "Info";
            infoButton.onclick = function () {
                info(this).then(response => {
                    if (response !== undefined) {
                        showInfo(response, false);
                    }
                });
            }
            infoButton.href = "#";
            infoButton.setAttribute("data-id", booksData[i].id);
            cardBody.appendChild(cardHeader);
            cardBody.appendChild(purchaseButton);
            cardBody.appendChild(infoButton);
            card.appendChild(cardBody);
            column.appendChild(card);
            currentRow.appendChild(column);
        }
    }
}

async function getBooks() {
    let serverPath = getDeployment("catalog");
    let cacheResponse = await getCacheValue("books");
    if (cacheResponse !== undefined) {
        showBooks(cacheResponse);
        return;
    }
    axios.get(`http://${serverPath}/book`).then(async response => {
        const data = response.data;
        showBooks(data);
        await setCacheArray("books", data);
    }).catch(error => {
        if (error.response && error.response.status === 404) {
            document.getElementById("waitingMessage").innerText = "No Books found";
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `No Books to show please visit us later.`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        }
    })
}

function clearInfoTable() {
    const infoTable = document.getElementById("infoTable");
    while (infoTable.lastElementChild) {
        infoTable.removeChild(infoTable.lastElementChild);
    }
}