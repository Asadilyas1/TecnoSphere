function PagerClick(index) {
    document.getElementById("hfCurrentPageIndex").value = index;
    document.forms[0].submit();
}
//Load Services
const sectionServices = document.querySelector("#LoadServices");
//Load Portfolio
const sectionPortfolio = document.querySelector("#LoadPortfolio");
//Load News
const sectionNews = document.querySelector("#LoadNews");
window.addEventListener("DOMContentLoaded", function () {
    //Load Services
    fetch('api/load-services/')
        .then((data) => data.json())
        .then((res) => {
            let displayMenu = res.map(function (item) {
                return `<div class="col-12 col-md-6 col-lg-4 item">
                    <div class="card featured">
                        <i class="${item.icon}"></i>
                        <h4>${item.title}</h4>
                        <p>${item.description}</p>
                        <a href="${item.slug}"><i class="btn-icon icon-arrow-right-circle"></i></a>
                    </div>
                </div>`;
            });
            displayMenu = displayMenu.join("");
            sectionServices.innerHTML = displayMenu;
        });
    //Load Portfolio
    fetch('api/load-portfolio/')
        .then((data) => data.json())
        .then((res) => {
            let displayMenu = res.map(function (item) {
                return `<div class="col-12 col-md-6 col-lg-4 item filter-item">
                    <div class="row card p-0 text-center">
                        <div class="gallery">
                            <a href="data:image/png;${item.image}" class="image-over">
                                <img src="data:image/png;${item.image}" alt="Lorem ipsum">
                            </a>
                        </div>
                        <div class="card-caption col-12 p-0">
                            <div class="card-body">
                                <h4 class="m-0">${item.title}</h4>
                            </div>
                            <div class="card-footer d-lg-flex align-items-center justify-content-center">
                                <p>${item.description} </p>
                            </div>
                        </div>
                    </div>
                </div>`;
            });
            displayMenu = displayMenu.join("");
            sectionPortfolio.innerHTML = displayMenu;
        });
    //Load News
    fetch('api/load-news/')
        .then((data) => data.json())
        .then((res) => {
            let displayMenu = res.map(function (item) {
                return `<div class="swiper-slide slide-center item" style="width: 360px; margin-right: 30px;">
                        <div class="row card p-0 text-center">
                            <div class="image-over">
                                <img src="data:image/png${item.blogImage}" alt="Lorem ipsum">
                            </div>
                            <div class="card-caption col-12 p-0">
                                <div class="card-body">
                                    <a href="page-single-post-1.html">
                                        <h4 class="m-0">${item.title}</h4>
                                    </a>
                                </div>
                                <div class="card-footer d-lg-flex align-items-center justify-content-center">
                                    <a href="#" class="d-lg-flex align-items-center"><i class="icon-user"></i>${item.user.firstName} ${item.user.lastName}</a>
                                    <a href="#" class="d-lg-flex align-items-center"><i class="icon-clock"></i>${GetDays(item.date)} Days Ago</a>
                                </div>
                            </div>
                        </div>
                    </div>`;
            });
            displayMenu = displayMenu.join("");
            sectionNews.innerHTML = displayMenu;
        });
});

function GetDays(blogDate) {
    let date = new Date();
    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();
    let currentDate = `${month}/${day}/${year}`;
    var date1 = new Date(blogDate);
    var date2 = new Date(currentDate);
    // To calculate the time difference of two dates
    var Difference_In_Time = date2.getTime() - date1.getTime();

    // To calculate the no. of days between two dates
    var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);

    //To display the final no. of days (result)
    return Difference_In_Days;
}