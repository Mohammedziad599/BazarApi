function getDeployment(server) {
    let element = document.getElementsByName('deployment_location');
    for (let i = 0; i < element.length; i++) {
        if (element[i].checked) {
            if (element[i].value === "Docker") {
                if (server === "order") {
                    return "localhost:6001";
                } else {
                    return "localhost:5000";
                }
            } else {
                if (server === "order") {
                    return "192.168.50.101";
                } else {
                    return "192.168.50.100";
                }
            }
        }
    }
}

async function getCatalogData(bookId) {
    let serverPath = getDeployment("catalog");
    return await axios.get(`http://${serverPath}/book/${bookId}`);
}

async function info(element) {
    let id = parseInt(element.getAttribute("data-id"));
    let data = await getCatalogData(id);
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
    axios.get(`http://${serverPath}/book`).then(response => {
        const data = response.data;
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
        axios.get(`http://${serverPath}/book/topic/search/${topic}`).then(response => {
            const data = response.data;
            showInfo(data, true);
        }).catch(() => {
            clearInfoTable();
            alert("No data found");
        });
    } else {
        let name = document.getElementById("search").value;
        axios.get(`http://${serverPath}/book/name/search/${name}`).then(response => {
            const data = response.data;
            showInfo(data, true);
        }).catch(() => {
            clearInfoTable();
            alert("No data found");
        });
    }
}

function getBooks() {
    let serverPath = getDeployment("catalog");
    axios.get(`http://${serverPath}/book`).then(response => {
        if (response.status === 200) {
            const data = response.data;
            const books = document.getElementById("books");
            while (books.lastElementChild) {
                books.removeChild(books.lastElementChild);
            }
            let currentRow = null;
            for (let i = 0; i < data.length; i++) {
                if ((i % 2) === 0) {
                    let row = document.createElement("div");
                    row.className = "row mb-2";
                    currentRow = row;
                    let column = document.createElement("div");
                    column.className = "col-lg-6 col-md-12";
                    let card = document.createElement("div");
                    card.className = "card";
                    let cardBody = document.createElement("div");
                    cardBody.className = "card-body";
                    let cardHeader = document.createElement("h5");
                    cardHeader.className = "card-title";
                    cardHeader.innerText = data[i].name;
                    let purchaseButton = document.createElement("a");
                    purchaseButton.className = "btn btn-indigo";
                    purchaseButton.innerText = "Purchase";
                    purchaseButton.onclick = function () {
                        purchase(this).then(response => {
                            showOrder(response, false);
                        });
                    }
                    purchaseButton.href = "#";
                    purchaseButton.setAttribute("data-id", data[i].id);
                    let infoButton = document.createElement("a");
                    infoButton.className = "btn btn-indigo";
                    infoButton.innerText = "Info";
                    infoButton.onclick = function () {
                        info(this).then(response => {
                            showInfo(response, false);
                        });
                    }
                    infoButton.href = "#";
                    infoButton.setAttribute("data-id", data[i].id);
                    cardBody.appendChild(cardHeader);
                    cardBody.appendChild(purchaseButton);
                    cardBody.appendChild(infoButton);
                    card.appendChild(cardBody);
                    column.appendChild(card);
                    row.appendChild(column);
                    books.append(row);
                } else {
                    let column = document.createElement("div");
                    column.className = "col-lg-6 col-md-12";
                    let card = document.createElement("div");
                    card.className = "card";
                    let cardBody = document.createElement("div");
                    cardBody.className = "card-body";
                    let cardHeader = document.createElement("h5");
                    cardHeader.className = "card-title";
                    cardHeader.innerText = data[i].name;
                    let purchaseButton = document.createElement("a");
                    purchaseButton.className = "btn btn-indigo";
                    purchaseButton.innerText = "Purchase";
                    purchaseButton.onclick = function () {
                        purchase(this).then(response => {
                            showOrder(response, false);
                        });
                    }
                    purchaseButton.href = "#";
                    purchaseButton.setAttribute("data-id", data[i].id);
                    let infoButton = document.createElement("a");
                    infoButton.className = "btn btn-indigo";
                    infoButton.innerText = "Info";
                    infoButton.onclick = function () {
                        info(this).then(response => {
                            showInfo(response, false);
                        });
                    }
                    infoButton.href = "#";
                    infoButton.setAttribute("data-id", data[i].id);
                    cardBody.appendChild(cardHeader);
                    cardBody.appendChild(purchaseButton);
                    cardBody.appendChild(infoButton);
                    card.appendChild(cardBody);
                    column.appendChild(card);
                    currentRow.appendChild(column);
                }
            }
            console.log("Refresh Books Success");
        } else {
            console.log("Failed");
        }
    })
}

async function order(id) {
    let serverPath = getDeployment("order");
    return await axios.post(`https://${serverPath}/purchase/${id}`, {});
}

async function purchase(element) {
    let id = parseInt(element.getAttribute("data-id"));
    let data = await order(id);
    return data.data;
}

function showOrder(data, isArray) {
    if (data != null) {
        const orderTable = document.getElementById("orderTable");
        clearOrderTable();
        if (isArray) {
            for (let i = 0; i < data.length; i++) {
                let tableRow = document.createElement("tr");
                let itemThead = document.createElement("th");
                itemThead.setAttribute("scope", "row");
                itemThead.innerText = data[i].id;
                let bookId = document.createElement("td");
                bookId.innerText = data[i].bookId;
                let time = document.createElement("td");
                time.innerText = data[i].time;
                tableRow.appendChild(itemThead);
                tableRow.appendChild(bookId);
                tableRow.appendChild(time);
                orderTable.appendChild(tableRow);
            }
        } else {
            let tableRow = document.createElement("tr");
            let itemThead = document.createElement("th");
            itemThead.setAttribute("scope", "row");
            itemThead.innerText = data.id;
            let bookId = document.createElement("td");
            bookId.innerText = data.bookId;
            let time = document.createElement("td");
            time.innerText = data.time;
            tableRow.appendChild(itemThead);
            tableRow.appendChild(bookId);
            tableRow.appendChild(time);
            orderTable.appendChild(tableRow);
        }
    }
}

function listOrders() {
    let serverPath = getDeployment("order")
    axios.get("https://" + serverPath + "/purchase/list").then(response => {
        const data = response.data;
        showOrder(data, true);
    });
}

function clearInfoTable() {
    const infoTable = document.getElementById("infoTable");
    while (infoTable.lastElementChild) {
        infoTable.removeChild(infoTable.lastElementChild);
    }
}

function clearOrderTable() {
    const orderTable = document.getElementById("orderTable");
    while (orderTable.lastElementChild) {
        orderTable.removeChild(orderTable.lastElementChild);
    }
}
