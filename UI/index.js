const Cache = new Map();

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