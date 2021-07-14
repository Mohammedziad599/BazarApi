let bname;
let btopic;
let bquantity;
let bprice;
let clickall = false;
let idbook;
let buylist = [];

function info(e) {
    if (e.id == "b11") {
        axios.get("http://localhost:5000/book/1").then(response => {
            const data = response.data;
            console.log(data);
            bname = data.name;
            btopic = data.topic;
            bquantity = data.quantity;
            bprice = data.price;
            showinfo();
        });

    } else if (e.id == "b22") {
        axios.get("http://localhost:5000/book/2").then(response => {
            const data = response.data;
            console.log(data);
            bname = data.name;
            btopic = data.topic;
            bquantity = data.quantity;
            bprice = data.price;
            showinfo();
        });
    } else if (e.id == "b33") {
        axios.get("http://localhost:5000/book/3").then(response => {
            const data = response.data;
            console.log(data);
            bname = data.name;
            btopic = data.topic;
            bquantity = data.quantity;
            bprice = data.price;
            showinfo();
        });
    } else if (e.id == "b44") {
        axios.get("http://localhost:5000/book/4").then(response => {
            const data = response.data;
            console.log(data);
            bname = data.name;
            btopic = data.topic;
            bquantity = data.quantity;
            bprice = data.price;
            showinfo();
        });
    }
}

function showinfo() {
    let tbodyRef = document.getElementById('myTable');
    if (clickall) {
        debugger;
        for (let j = 2; j < 5; j++) {
            tbodyRef.deleteRow(2);
        }
        clickall = false;
    }
    document.getElementById("bn").innerText = bname;
    document.getElementById("bt").innerText = btopic;
    document.getElementById("bq").innerText = bquantity;
    document.getElementById("bp").innerText = bprice;
}

function infoall() {
    clickall = true;
    axios.get("http://localhost:5000/book").then(response => {
        const data = response.data;
        console.log(data);
        let tbodyRef = document.getElementById('myTable');
        document.getElementById("bn").innerText = data[0].name;
        document.getElementById("bt").innerText = data[0].topic;
        document.getElementById("bq").innerText = data[0].quantity;
        document.getElementById("bp").innerText = data[0].price;
        for (let i = 1; i < 4; i++) {
            let row = tbodyRef.insertRow(i + 1);
            let cell1 = row.insertCell(0);
            let cell2 = row.insertCell(1);
            let cell3 = row.insertCell(2);
            let cell4 = row.insertCell(3);
            cell1.innerHTML = data[i].name;
            cell2.innerHTML = data[i].topic;
            cell3.innerHTML = data[i].quantity;
            cell4.innerHTML = data[i].price;
        }


    });
}

function Search() {
    debugger;
    let booktopic = document.getElementById("myInput").value;

    let modal1 = document.getElementById("myModal");
    modal1.style.displays = "block";

    if (booktopic == "distributed systems" || booktopic == "undergraduate school") {
        axios.get("https://localhost:5001/book/search/" + booktopic).then(response => {
            const data = response.data;
            console.log(data);
            document.getElementById("p1").innerHTML = "The Books in topic " + booktopic + " are : <br> &nbsp 1- " + data[0].name + "<br> &nbsp 2- " + data[1].name;
        });

        modal1.style.display = 'block';

    } else {
        alert("please check the topic");
    }
}

function modalgo() {
    let modal = document.getElementById("myModal");
    modal.style.display = "none";
}

function getbookn() {
    axios.get("http://localhost:5000/book").then(response => {
        const data = response.data;
        document.getElementById("n1").innerText = data[0].name;
        document.getElementById("n2").innerText = data[1].name;
        document.getElementById("n3").innerText = data[2].name;
        document.getElementById("n4").innerText = data[3].name;
    });
}

function buy(e) {
    if (e.id == "b1") {
        idbook = 1;

    } else if (e.id == "b2") {
        idbook = 2;
    } else if (e.id == "b3") {
        idbook = 3;
    } else if (e.id == "b4") {
        idbook = 4;
    }
    alert("the purchase is done ");
    axios.post("https://localhost:5003/order/" + idbook).then(response => {
        const data = response.data;
    });
}

function listbook() {
    let text = "The Books sold are :";
    let modal2 = document.getElementById("myModal");
    axios.get("https://localhost:5003/order").then(response => {
        const data = response.data;
        for (let i = 0; i < data.length; i++) {
            let str = data[i].time;
            text = text + " <br> &nbsp  order ID:  " + data[i].id + " &nbsp Book ID: " + data[i].bookId + "  &nbsp Time: " + str.split(".", 1) + "<br>";
        }
        document.getElementById("p1").innerHTML = text;
        modal2.style.display = 'block';
    });

}