//varibles
const cartBtn = document.querySelector(".cart-btn");
const closeCartBtn = document.querySelector(".close-cart");
const cartDOM = document.querySelector(".cart");
const cartOverlay = document.querySelector(".cart-overlay");
const cartItems = document.querySelector(".cart-items");
const cartTotal = document.querySelector(".cart-total");
const cartContent = document.querySelector(".cart-content");
const productsDOM = document.querySelector(".products-center");
const catagoriesDOM = document.querySelector("#categories");
const btns = document.querySelector(".bag-btn");
var globalTimeout = null;
//console.log(btns);


//cart
let cart = [];
//buttons
let buttonsDOM = [];

// login
var modal = document.getElementById('login');

// When the user clicks anywhere outside, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

//Signup
var modal = document.getElementById('SignUp');

// When the user clicks anywhere outside, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}



//getting the products
class Api {
    async getList(dom, url) {
        var result = await this.get(dom, url);
        return Array.from(result);
    }
    async get(dom, url) {
        try {
            $(dom).LoadingOverlay("show")
            var user = Storage.getUser();
            var user = Storage.getUser();

            let result = await fetch(url, {
                headers: (user != undefined) ? {
                    "Authorization": `Bearer ${user.token}`
                } : {}
            });
            let data = await result.json();
            $(dom).LoadingOverlay("hide")
            if (result.status == 200)
                return data.data;
            else {
                if (data.error != undefined)
                    throw data.error;
                if (data.errors != undefined)
                    throw data.errors[Object.keys(data.errors)[0]][0];
                if (data.title != undefined)
                    throw data.title;
                throw "Unknown error";
            }
        } catch (error) {
            $(dom).LoadingOverlay("hide")
            Snackbar.show({ text: error, pos: 'top-center', actionText: 'Got it!' });
            throw error;
        }
    }



    async post(dom, url, body) {
        try {
            $(dom).LoadingOverlay("show")
            var user = Storage.getUser();
            var headers = {
                'Content-Type': 'application/json'
            };
            if (user != undefined) {
                headers["Authorization"] = `Bearer ${user.token}`
            }
            let result = await fetch(url, {
                method: "Post",
                body: JSON.stringify(body),
                headers: headers
            });
            let data = await result.json();
            $(dom).LoadingOverlay("hide")
            if (result.status == 200)
                return data.data;
            else {
                if (data.error != undefined)
                    throw data.error;
                if (data.errors != undefined)
                    throw data.errors[Object.keys(data.errors)[0]][0];
                if (data.title != undefined)
                    throw data.title;
                throw "Unknown error";
            }
        } catch (error) {
            $(dom).LoadingOverlay("hide")
            Snackbar.show({ text: error, pos: 'top-center', actionText: 'Got it!' });
            throw error;
        }
    }


    async multipart(dom, url, body) {
        try {
            $(dom).LoadingOverlay("show")
            var user = Storage.getUser();
            var headers = {
            };
            if (user != undefined) {
                headers["Authorization"] = `Bearer ${user.token}`
            }
            const formData = new FormData();
            for (const name in body) {
                formData.append(name, body[name]);
            }
            let result = await fetch(url, {
                method: "Post",
                body: formData,
                headers: headers
            });
            let data = await result.json();
            $(dom).LoadingOverlay("hide")
            if (result.status == 200)
                return data.data;
            else {
                if (data.error != undefined)
                    throw data.error;
                if (data.errors != undefined)
                    throw data.errors[Object.keys(data.errors)[0]][0];
                if (data.title != undefined)
                    throw data.title;
                throw "Unknown error";
            }
        } catch (error) {
            $(dom).LoadingOverlay("hide")
            Snackbar.show({ text: error, pos: 'top-center', actionText: 'Got it!' });
            throw error;
        }
    }

    async put(dom, url, body) {
        try {
            $(dom).LoadingOverlay("show")
            var user = Storage.getUser();
            var headers = {
                'Content-Type': 'application/json'
            };
            if (user != undefined) {
                headers["Authorization"] = `Bearer ${user.token}`
            }
            let result = await fetch(url, {
                method: "Put",
                body: JSON.stringify(body),
                headers: headers,
            });
            let data = await result.json();

            $(dom).LoadingOverlay("hide")
            if (result.status == 200)
                return data.data;
            else {
                if (data.error != undefined)
                    throw data.error;
                if (data.errors != undefined)
                    throw data.errors[Object.keys(data.errors)[0]][0];
                if (data.title != undefined)
                    throw data.title;
                throw "Unknown error";
            }
        } catch (error) {
            $(dom).LoadingOverlay("hide")
            Snackbar.show({ text: error, pos: 'top-center', actionText: 'Got it!' });
            throw error;
        }
    }
};


//getting the products
class Tools {
    async loadMainTools(categoryId, search) {
        const api = new Api();
        var url = `/Tools/Tools/${categoryId}`;
        if (search != undefined && search.length > 0) {
            url += `?q=${search}`;
        }
        let result = await api.getList($(`#tools-content-${categoryId}`), url);
        result = result.map(item => {
            const { id, name, image, description, dayPrice, categoryId, userId } = item;
            return { id, name, image, description, dayPrice, categoryId, userId };
        });
        return result;

    }

    async loadMyTools(search) {
        const api = new Api();
        var url = `/Tools/MyTools`;
        if (search != undefined && search.length > 0) {
            url += `?q=${search}`;
        }
        let result = await api.getList($(`#my-products`), url);
        result = result.map(item => {
            const { id, name, image, description, dayPrice, categoryId, userId } = item;
            return { id, name, image, description, dayPrice, categoryId, userId };
        });
        return result;

    }
};



//getting the products
class Categories {
    async load() {
        const api = new Api();
        let result = await api.getList(catagoriesDOM, "/Categories/List");
        result = result.map(item => {
            const { id, name, toolsCount } = item;
            return { id, name, toolsCount };
        });
        return result;

    }
};



//display products
class UI {
    addItemToCart(button, item) {
        let inCart = cart.find(_item => _item.id === item.id);
        if (inCart) {
            item.amount = inCart.amount;
        } else {
            item.amount = 1;
        }
        button.text("In Cart");
        button.prop("disabled", true);
        cart = [...cart.filter(_item => _item.id !== item.id), item];
        //save cart in local storage
        Storage.saveCart(cart);
        //set cart values
        this.setCartValues(cart);
        //show the cart

    }
    showTools(categoryId, tools) {
        $("#products").find(".tabcontent").css('display', 'none');
        $(`.tablinks`).removeClass('active')

        var content = $(`#product-list-${categoryId}`);
        $(`#tools-content-${categoryId}`).css('display', 'block');
        $(`#tools-button-${categoryId}`).addClass('active');
        content.children().remove();
        var user = Storage.getUser();
        tools.forEach(tool => {
            var child = $(`
                <article class="product">
                    <div class="img-container">
                        <img
                         src=${tool.image}
                         alt="product" 
                         class="product-img">
                        <button class="bag-btn" data-user="${tool.userId}" id="bag-btn-${tool.id}">
                             <i class="fas fa-shopping-cart"></i>
                             add to cart
                        </button>
                    </div>
                    <h3>${tool.name}</h3>
                    <h5>${tool.description}</h5>
                    <h4>$${tool.dayPrice}</h4>
                </article>
            `);
            content.append(child);
            var button = child.find(`#bag-btn-${tool.id}`);
            let id = tool.id;
            let inCart = cart.find(item => item.id === id);
            if (inCart) {

                if (user != undefined && user.id == tool.userId) {
                    this.removeItem(id);
                } else {
                    this.addItemToCart(button, tool);
                    this.updateCartItem(tool);
                }
            }
            button.on("click", () => {
                this.addItemToCart(button, tool);
                //display cart item
                this.addCartItem(tool);
                this.showCart();
            });
        });
        if (user != undefined) {
            $(`button[data-user="${user.id}"]`).hide();
        }

    }

    displayCategories(categories) {
        $("#categories").empty();
        $("#products").find(".tabcontent").empty();
        const tools = new Tools();
        $("#add-tool-category").empty();
        categories.forEach(category => {

            $("#add-tool-category").append($(`<option value='${category.id}'>${category.name}</option>`));
            var button = $(` <button class="tablinks" id="tools-button-${category.id}">${category.name}</button>`);
            var content = $(`
                 <div id="tools-content-${category.id}" class="tabcontent">
                 <div class="input-wrapper">
                     <input type="text" name="" id="search-item-${category.id}" placeholder="     Search products">
                     <label for="search-item" class="fa fa-search input-icon"></label>
                 </div>
     
                 <div class="products-center" id="product-list-${category.id}">
                 </div>
                 </div>
            `);
            var input = content.find(`#search-item-${category.id}`);
            var obj = this;
            input.on("keyup", () => {
                if (globalTimeout != null) {
                    clearTimeout(globalTimeout);
                }
                globalTimeout = setTimeout(function () {
                    globalTimeout = null;
                    tools.loadMainTools(category.id, input.val()).then(items => obj.showTools(category.id, items));
                }, 700);

            })
            button.on("click", () => {
                tools.loadMainTools(category.id).then(items => obj.showTools(category.id, items));
            });
            $("#categories").append(button);
            $("#tools-clearfix").before(content);
        });
        if (categories.length > 0)
            tools.loadMainTools(categories[0].id).then(items => this.showTools(categories[0].id, items));

    }


    displayMyTool(tool) {
        var dom = $(`#my-tool-${tool.id}`);
        if (dom.length == 0) {
            var child = $(`
            <article class="product" id="my-tool-${tool.id}">
                <div class="img-container">
                    <img
                     id="my-tool-image-${tool.id}"
                     src=${tool.image}
                     alt="product" 
                     class="product-img">
                     <button class="bag-btn"  id="edit-btn-${tool.id}">
                     <i class="fas fa-edit"></i>
                     Edit
                </button>
                </div>
                <h3  id="my-tool-name-${tool.id}">${tool.name}</h3>
                <h5  id="my-tool-description-${tool.id}">${tool.description}</h5>
                <h4  id="my-tool-day-price-${tool.id}">$${tool.dayPrice}</h4>
            </article>
        `);
            var button = child.find(`#edit-btn-${tool.id}`);
            button.on("click", () => {
                UI.showAddTool(tool);
            });
            $(`#my-tools-list`).prepend(child);
        } else {
            $(`#my-tool-day-price-${tool.id}`).text(tool.dayPrice);
            $(`#my-tool-description-${tool.id}`).text(tool.description);
            $(`#my-tool-name-${tool.id}`).text(tool.name);
            $(`#my-tool-image-${tool.id}`).prop("src", tool.image);
        }
    }
    displayMyTools(tools) {

        $(`#my-tools-list`).empty();
        tools.forEach(tool => {
            this.displayMyTool(tool);
        });
    }


    setCartValues(cart) {
        let tempTotal = 0;
        let itemsTotal = 0;
        cart.map(item => {
            tempTotal += item.dayPrice * item.amount;
            itemsTotal += item.amount;
        });
        cartTotal.innerText = parseFloat(tempTotal.toFixed(2));
        cartItems.innerText = itemsTotal;
        //console.log(cartTotal, cartItems);
    }
    addCartItem(item) {
        $(`cart-item-${item.id}`).remove();
        const div = document.createElement("div");
        div.classList.add("cart-item");
        div.id = `cart-item-${item.id}`;
        div.innerHTML = `
        <img id="cart-item-image-${item.id}" src=${item.image} alt="product">
                <div>
                    <h4 id="cart-item-name-${item.id}">${item.name} </h4>
                    <h5 id="cart-item-price-${item.id}">$${item.dayPrice * item.amount}</h5>
                    <span class="remove-item" data-id=${item.id}>remove</span>
                </div>
                <div>
                    <i class="fas fa-chevron-up" data-id=${item.id}></i>
                    <p class="item-amount" id="cart-item-amount-${item.id}">${item.amount}</p>
                    <i class="fas fa-chevron-down" data-id=${item.id}></i>
                </div>`;
        cartContent.appendChild(div);
    }

    updateCartItem(item) {
        $(`#cart-item-image-${item.id}`).attr("src", item.image);
        $(`#cart-item-name-${item.id}`).text(item.name);
        $(`#cart-item-price-${item.id}`).text(item.dayPrice * item.amount);
        $(`#cart-item-amount-${item.id}`).text(item.amount);
    }
    showCart() {
        cartOverlay.classList.add("transparentBcg");
        cartDOM.classList.add("showCart");
    }
    setupAPP() {
        cart = Storage.getCart();
        this.setCartValues(cart);
        this.populateCart(cart);
        cartBtn.addEventListener("click", this.showCart);
        closeCartBtn.addEventListener("click", this.hideCart);
        UI.handleLoginButton();
        $("#login-form").on("submit", this.login);
        $("#register-form").on("submit", this.register);
        $("#tool-form").on("submit", this.submitToolForm);

    }

    login(event) {
        event.preventDefault();
        var email = $("#login-email").val();
        var password = $("#login-password").val();
        var checked = $("#login-check").prop("checked");
        var api = new Api();
        var obj = this;
        api.post($("#login-form"), "/Auth/Login", {
            "email": email,
            "password": password,
        }).then((data) => {
            Storage.saveUser(data, checked);
            UI.handleLoginButton();
            $(`button[data-user="${data.id}"]`).hide();
        });
    }

    register(event) {
        event.preventDefault();
        var name = $("#register-name").val();
        var email = $("#register-email").val();
        var password = $("#register-password").val();
        var confirmPassword = $("#register-confirm-password").val();
        var bankAccount = $("#register-bank-account").val();
        var checked = $("#register-check").prop("checked");

        var api = new Api();
        api.post($("#register-form"), "/Auth/Register", {
            "name": name,
            "email": email,
            "bankAccount": bankAccount,
            "password": password,
            "confirmPassword": confirmPassword,
        }).then((data) => {
            Storage.saveUser(data, checked);
            UI.handleLoginButton();
            $(`button[data-user="${data.id}"]`).hide();
        });
    }

    static handleLoginButton() {
        $("#login").css({ "display": "none" });
        $("#SignUp").css({ "display": "none" });
        var user = Storage.getUser();
        if (user != undefined) {
            $("#login-menu-item").hide();
            $("#logout-menu-item").show();
            $("#logout-menu-item-link").text(`Logout (${user.name})`);
            $("#my-products-menu-item").show();
            $("#my-tools-list").empty();
            var input = $(`#my-tools-search`);
            const tools = new Tools();
            const ui = new UI();
            tools.loadMyTools(input.val()).then(items => ui.displayMyTools(items));
            input.on("keyup", () => {
                if (globalTimeout != null) {
                    clearTimeout(globalTimeout);
                }
                globalTimeout = setTimeout(function () {
                    globalTimeout = null;
                    tools.loadMyTools(input.val()).then(items => ui.displayMyTools(items));
                }, 700);

            })
        } else {
            $("#login-menu-item").show();
            $("#logout-menu-item").hide();
            $("#my-products-menu-item").hide();
            $("#my-tools-list").empty();
            UI.showProducts();

        }
    }

    static logOut() {
        var user = Storage.getUser();
        Storage.removeUser();
        UI.handleLoginButton();
        $(`button[data-user="${user.id}"]`).show();
        $("#my-tools-list").empty();
        UI.showProducts();
    }


    populateCart(cart) {
        cart.forEach(item => this.addCartItem(item));
    }
    hideCart() {
        cartOverlay.classList.remove("transparentBcg");
        cartDOM.classList.remove("showCart");
    }
    cartLogic() {
        //clear cart button
        $(".clear-cart").on("click", () => {
            this.clearCart();
        });
        //cart functionality
        cartContent.addEventListener("click", event => {
            if (event.target.classList.contains("remove-item")) {
                let removeItem = event.target;
                let id = parseInt(removeItem.dataset.id);
                this.removeItem(id);
            }
            else if (event.target.classList.contains("fa-chevron-up")) {
                let addAmount = event.target;
                let id = parseInt(addAmount.dataset.id);
                let tempItem = cart.find(item => item.id === id);
                tempItem.amount = tempItem.amount + 1;
                Storage.saveCart(cart);
                this.setCartValues(cart);
                addAmount.nextElementSibling.innerText =
                    tempItem.amount;
            }
            else if (event.target.classList.contains
                ("fa-chevron-down")) {
                let lowerAmount = event.target;
                let id = parseInt(lowerAmount.dataset.id);
                let tempItem = cart.find(item => item.id === id);
                tempItem.amount = tempItem.amount - 1;
                if (tempItem.amount > 0) {
                    Storage.saveCart(cart);
                    lowerAmount.previousElementSibling.innerText
                        = tempItem.amount;
                } else {
                    this.removeItem(id);
                }
            }
        });
    }
    clearCart() {
        let cartItems = cart.map(item => item.id);
        cartItems.forEach(id => this.removeItem(id));
        this.hideCart();
    }
    removeItem(id) {
        cart = cart.filter(item => item.id !== id);
        this.setCartValues(cart);
        Storage.saveCart(cart);
        let button = $(`#bag-btn-${id}`);
        button.html(`<i class="fas fa-shopping-cart"></i>add to cart`);
        button.prop("disabled", false);
        $(`#cart-item-${id}`).remove();
    }
    getSingleButton(id) {
        return buttonsDOM.find(button => button.dataset.id === id);
    }

    static showProducts() {
        $(`#navebar-n`).find('.active').removeClass("active");
        $(`#products-menu-item`).find('a').addClass("active");
        $(`#products`).show();
        $(`#my-products`).hide();
    }
    static showMyProducts() {
        window.location.href = "/#products";
        $(`#navebar-n`).find('.active').removeClass("active");
        $(`#my-products-menu-item`).find('a').addClass("active")
        $(`#products`).hide();
        $(`#my-products`).show();
    }


    static showAddTool(tool) {
        $("#AddTool").css({ "display": "block" });
        if (tool !== undefined) {
            $("#AddTool").attr("update-id", tool.id);
            $("#add-tool-name").val(tool.name);
            $("#add-tool-description").val(tool.description);
            $("#add-tool-category").val(tool.categoryId);
            $("#add-tool-price").val(tool.dayPrice);
            $("#add-tool-addOrEditButton").text("Edit");
        } else {
            $("#add-tool-name").val("");
            $("#add-tool-description").val("");
            $("#add-tool-category").val("");
            $("#add-tool-price").val("");
            $("#add-tool-addOrEditButton").text("Create");
        }
        $("#add-tool-image").val("");

    }

    submitToolForm(event) {
        event.preventDefault();
        var body = {
            "name": $("#add-tool-name").val(),
            "description": $("#add-tool-description").val(),
            "categoryId": $("#add-tool-category").val(),
            "dayPrice": $("#add-tool-price").val(),
        };
        var files = document.getElementById("add-tool-image").files;
        if (files.length > 0) {
            body["image"] = files[0];
        }
        var updateId = $("#AddTool").attr("update-id");
        var api = new Api();
        if (updateId === undefined) {
            api.multipart($("#tool-form"), "/Tools/Create", body).then((data) => {
                (new UI()).displayMyTool(data);
                $("#AddTool").css({ "display": "none" });
            });
        } else {
            api.multipart($("#tool-form"), `/Tools/Update/${updateId}`, body).then((data) => {
                (new UI()).displayMyTool(data);
                $("#AddTool").css({ "display": "none" });
            });
        }

    }


    static completeOrder() {
        var cart = Storage.getCart();
        var user = Storage.getUser();
        if (user == undefined) {
            Snackbar.show({
                text: "Sorry! you need to login first",
                pos: 'top-center',
                actionText: 'Got it!',
            });
            return;
        }
        var items = Array.from(cart).map((item) => {
            return {
                "toolId": item.id,
                "days": item.amount
            };
        });
        if (cart.length == 0) {
            Snackbar.show({
                text: "Sorry! you need to add at least one item",
                pos: 'top-center',
                actionText: 'Got it!',
            });
        }

        var api = new Api();
        api.post($("#cart-loading"), "/Orders/Create", {
            "items": items
        }).then((data) => {
            var ui = new UI();
            ui.clearCart();
            ui.hideCart();
            var message = [
                "<h3>Your order:</h3>"
            ];
            var total = 0;
            cart.forEach(element => {
                total += element.amount * element.dayPrice;
                message.push(`<h5><span>${element.name}: </span>  <span style="color:gold;">${element.amount} days</span>  <span style="color:green;">${element.amount * element.dayPrice} EGP</span></h5>`);
            });
            message.push(`<h4><span>Total: </span> <span style="color:green;">${total} EGP</span></h4>`);

            Snackbar.show({
                text: message.join("<br/>"),
                pos: 'top-center',
                duration: 50000,
                actionText: 'Thanks!',
            });
        });

    }

}
//local storage
class Storage {

    static getProduct(id) {
        let products = JSON.parse(localStorage.getItem("products"));
        return products.find(product => product.id === id);
    };
    static saveCart(cart) {
        localStorage.setItem("cart", JSON.stringify(cart));
    };
    static getCart() {
        return localStorage.getItem('cart')
            ? JSON.parse(localStorage.getItem('cart')) : [];
    };

    static saveUser(user, rememberMe) {
        if (rememberMe == true || rememberMe == "true") {
            localStorage.setItem("user", JSON.stringify(user));
        } else {
            sessionStorage.setItem("user", JSON.stringify(user));
        }
    };
    static removeUser(user, rememberMe) {
        localStorage.removeItem("user");
        sessionStorage.removeItem("user");
    };
    static getUser() {
        var user = localStorage.getItem('user') ?? sessionStorage.getItem('user');
        if (user != undefined && user != "undefined") {
            return JSON.parse(user);
        }
    };
};

document.addEventListener("DOMContentLoaded", () => {
    $(`#my-products`).hide();
    const ui = new UI();
    const categories = new Categories();
    //setup app
    ui.setupAPP();
    //get all products
    categories.load().then(items => {
        ui.displayCategories(items);
    }).then(() => {
        ui.cartLogic();
    });
});

