/**
 * send purchase book request to the server.
 * @param id the id of the book.
 * @returns {Promise<*>} axios response.
 */
async function order(id) {
    let serverPath = getDeployment("order");
    return await axios.post(`https://${serverPath}/purchase/${id}`, {});
}

/**
 * purchase a book and show the order details in the order table.
 * @param element the button that the user clicked.
 * @returns {Promise<void>} nothing.
 */
async function purchase(element) {
    let id = parseInt(element.getAttribute("data-id"));
    try {
        let response = await order(id);
        let modalBody = document.getElementById("successBody");
        modalBody.innerText = `Success you have purchased book that has id = ${id} see info in the order table.`;
        successModal = new bootstrap.Modal(document.getElementById("successModal"), {
            keyboard: false
        });
        successModal.show();
        showOrder(response.data, false);
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
        clearOrderTable();
    }
}

/**
 * adds orders info to the orders table.
 * @param data the array or object that contains the order details.
 * @param isArray if the data is array or object.
 */
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

/**
 * get all orders info from the server.
 * @returns {Promise<void>} nothing.
 */
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

/**
 * will get the value from the user and then return the order details in the order table.
 * @returns {Promise<void>} nothing.
 */
async function getOrderById() {
    let orderId = parseInt(document.getElementById("orderID").value);
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
        let modalBody = document.getElementById("modalBody");
        modalBody.innerText = `Order id should be integer.`;
        errorModal = new bootstrap.Modal(document.getElementById("modal"), {
            keyboard: false
        });
        errorModal.show();
    }
}

/**
 * clear the order tabke.
 */
function clearOrderTable() {
    const orderTable = document.getElementById("orderTable");
    while (orderTable.lastElementChild) {
        orderTable.removeChild(orderTable.lastElementChild);
    }
}