const Cache = new Map();
const ws = new WebSocket("ws://localhost:5000/ws");


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