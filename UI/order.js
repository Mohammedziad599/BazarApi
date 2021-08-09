async function order(id) {
    let serverPath = getDeployment("order");
    return await axios.post(`https://${serverPath}/purchase/${id}`, {});
}

async function purchase(element) {
    let id = parseInt(element.getAttribute("data-id"));
    try {
        let response = await order(id);
        await setCacheValue(`o-${id}`, response.data);
        return response.data;
    } catch (error) {
        if (error.response && error.response.status === 400) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `Bad Request when purchasing book with id = ${id}.`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        } else if (error.response && error.response.status === 404) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = error.response.data.detail;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        }
        return undefined;
    }
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

async function listOrders() {
    let cacheResponse = await getCacheValue("orders");
    if (cacheResponse !== undefined) {
        showOrder(cacheResponse, true);
        return;
    }
    let serverPath = getDeployment("order");
    axios.get(`https://${serverPath}/purchase/list`).then(async response => {
        const data = response.data;
        showOrder(data, true);
        await setCacheArray("orders", data);
    }).catch(error => {
        if (error.response && error.response.status === 404) {
            let modalBody = document.getElementById("modalBody");
            modalBody.innerText = `No Orders Found.`;
            errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                keyboard: false
            });
            errorModal.show();
        }
    });
}

async function getOrderById() {
    let orderId = document.getElementById("orderID").value;
    if (typeof orderId === 'number' && orderId % 1 === 0) {
        let cacheResponse = await getCacheValue(`o-${orderId}`);
        if (cacheResponse !== undefined) {
            showOrder(cacheResponse, false);
            return;
        }
        let serverPath = getDeployment("order");
        axios.get(`https://${serverPath}/purchase/${orderId}`).then(async response => {
            let data = response.data;
            showOrder(data, false);
            await setCacheValue(`o-${orderId}`, data);
        }).catch(error => {
            if (error.response && error.response.status === 400) {
                let modalBody = document.getElementById("modalBody");
                modalBody.innerText = `Bad Request Order id should be integer.`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            } else if (error.response && error.response.status === 404) {
                let modalBody = document.getElementById("modalBody");
                modalBody.innerText = `Order not found with id=${orderId}.`;
                errorModal = new bootstrap.Modal(document.getElementById("modal"), {
                    keyboard: false
                });
                errorModal.show();
            }
        });
    } else {
    }
}

function clearOrderTable() {
    const orderTable = document.getElementById("orderTable");
    while (orderTable.lastElementChild) {
        orderTable.removeChild(orderTable.lastElementChild);
    }
}