let errorModal;
let successModal;
let CatalogReplica = 0;
let OrderReplica = 0;

/**
 * this function will used to get the server ip based on user choise in the ui
 * if the user choosed docker this function will return the ip of the choosed server "catalog" or "order"
 * on the docker deployment.
 * @param server the server that i need it's ip
 * @returns {string} this return the server ip
 */
function getDeployment(server) {
    let element = document.getElementsByName('deployment_location');
    for (let i = 0; i < element.length; i++) {
        if (element[i].checked) {
            if (server === "order") {
                return (OrderReplica++ % 2) === 0 ? "localhost:6001" : "localhost:6003";
            } else if (server === "catalog") {
                return (CatalogReplica++ % 2) === 0 ? "localhost:5000" : "localhost:5002";
            } else if (server === "cache") {
                return "localhost:3000";
            }
        } else {
            if (server === "order") {
                return (OrderReplica++ % 2) === 0 ? "192.168.50.101" : "192.168.50.201";
            } else if (server === "catalog") {
                return (CatalogReplica++ % 2) === 0 ? "192.168.50.100" : "192.168.50.200";
            } else if (server === "cache") {
                return "192.168.50.102";
            }
            if (element[i].value === "Docker") {
                if (server === "order") {
                    return (OrderReplica++ % 2) === 0 ? "localhost:6001" : "localhost:6003";
                } else if (server === "catalog") {
                    return (CatalogReplica++ % 2) === 0 ? "localhost:5000" : "localhost:5002";
                } else if (server === "cache") {
                    return "localhost:3000";
                }
            } else {
                if (server === "order") {
                    return (OrderReplica++ % 2) === 0 ? "192.168.50.101" : "192.168.50.201";
                } else if (server === "catalog") {
                    return (CatalogReplica++ % 2) === 0 ? "192.168.50.100" : "192.168.50.200";
                } else if (server === "cache") {
                    return "192.168.50.102";
                }
            }
        }
    }
}

/**
 * this function return the value of this key from the cache server
 * @param key the key as a string that represent the value in the cache server
 * @returns {Promise<undefined|*>} either undifined if an error happend or it will return an array of books or a book
 */
async function getCacheValue(key) {
    let cacheServerPath = getDeployment("cache");
    try {
        let response = await axios.get(`http://${cacheServerPath}/cache/${key}`);
        return response.data;
    } catch (error) {
        return undefined;
    }
}