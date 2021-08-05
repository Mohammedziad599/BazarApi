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
    let serverPath = getDeployment("order");
    if (Cache.get("o") !== undefined) {
        showOrder(Cache.get("o"), true);
        return;
    }
    axios.get(`https://${serverPath}/purchase/list`).then(response => {
        const data = response.data;
        Cache.set("o", data);
        showOrder(data, true);
    });
}

function clearOrderTable() {
    const orderTable = document.getElementById("orderTable");
    while (orderTable.lastElementChild) {
        orderTable.removeChild(orderTable.lastElementChild);
    }
}