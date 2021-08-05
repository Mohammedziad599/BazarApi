async function getCatalogData(bookId) {
    let serverPath = getDeployment("catalog");
    return await axios.get(`http://${serverPath}/book/${bookId}`);
}

async function info(element) {
    let id = parseInt(element.getAttribute("data-id"));
    if (Cache.get(`c${id}`)) {
        return Cache.get(`c${id}`);
    }
    let data = await getCatalogData(id);
    Cache.set(`c${id}`, data.data);
    return data.data;
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

function showAll() {
    let serverPath = getDeployment("catalog");
    if (Cache.get("books") !== undefined) {
        showInfo(Cache.get("books"), true);
        return;
    }
    axios.get(`http://${serverPath}/book`).then(response => {
        const data = response.data;
        Cache.set("books", data);
        showInfo(data, true);
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

function searchInfo() {
    let serverPath = getDeployment("catalog");
    if (getSearchMethod() === "Topic") {
        let topic = document.getElementById("search").value;
        if (Cache.get(`topic/${topic}`) !== undefined) {
            showInfo(Cache.get(`topic/${topic}`), true);
            return;
        }
        axios.get(`http://${serverPath}/book/topic/search/${topic}`).then(response => {
            const data = response.data;
            Cache.set(`topic/${topic}`, data);
            showInfo(data, true);
        }).catch(() => {
            clearInfoTable();
            alert("No data found");
        });
    } else {
        let name = document.getElementById("search").value;
        if (Cache.get(`name/${name}`) !== undefined) {
            showInfo(Cache.get(`name/${name}`), true);
            return;
        }
        axios.get(`http://${serverPath}/book/name/search/${name}`).then(response => {
            const data = response.data;
            Cache.set(`name/${name}`, data);
            showInfo(data, true);
        }).catch(() => {
            clearInfoTable();
            alert("No data found");
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
                    showInfo(response, false);
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
                    showInfo(response, false);
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

function getBooks() {
    let serverPath = getDeployment("catalog");
    if (Cache.get("books") !== undefined) {
        showBooks(Cache.get("books"));
        return;
    }
    axios.get(`http://${serverPath}/book`).then(response => {
        const data = response.data;
        Cache.set("books", data);
        showBooks(data);
    }).catch(reason => {
        alert(reason);
    })
}

function clearInfoTable() {
    const infoTable = document.getElementById("infoTable");
    while (infoTable.lastElementChild) {
        infoTable.removeChild(infoTable.lastElementChild);
    }
}