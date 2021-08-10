let errorModal;
let successModal;

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
            if (element[i].value === "Docker") {
                if (server === "order") {
                    return "localhost:6001";
                } else if (server === "catalog") {
                    return "localhost:5000";
                } else if (server === "cache") {
                    return "localhost:3000";
                }
            } else {
                if (server === "order") {
                    return "192.168.50.101";
                } else if (server === "catalog") {
                    return "192.168.50.100";
                } else if (server === "cache") {
                    return "192.168.50.102";
                }
            }
        }
    }
}

/**
 * this will send a request to the cache to cache the key and the value in the cache server.
 * @param key the key as a string that represent the value in the cache server
 * @param value the value that i want to cache in the server
 * @returns {Promise<void>} nothing
 */
async function setCacheValue(key, value) {
    let cacheServerPath = getDeployment("cache");
    try {
        await axios.post(`http://${cacheServerPath}/cache/${key}`, value);
    } catch (error) {
    }
}

/**
 * this will send a request to the cache to cache the key and the array in the cache server.
 * @param key the key as a string that represent the array in the cache server
 * @param arrayOfValues the array that i want to cache in the server
 * @returns {Promise<void>} nothing
 */
async function setCacheArray(key, arrayOfValues) {
    let cacheServerPath = getDeployment("cache");
    try {
        await axios.post(`http://${cacheServerPath}/cache/array/${key}`, arrayOfValues);
    } catch (error) {
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