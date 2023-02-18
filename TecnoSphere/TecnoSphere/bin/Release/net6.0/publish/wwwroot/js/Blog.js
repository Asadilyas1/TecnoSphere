window.addEventListener('DOMContentLoaded', function () {
    fetch('../RecentBlogs')
        .then(async (response) => {

            // get json response here
            let data = await response.text();
            this.document.getElementById("recent-blogs").innerHTML = data;

        })
        .catch((err) => {
            console.log(err);
        });

    fetch('../BlogCategories')
        .then(async (response) => {

            // get json response here
            let data = await response.text();
            this.document.getElementById("blog-categories").innerHTML = data;

        })
        .catch((err) => {
            console.log(err);
        });
});

document.getElementById('livesearchtags').addEventListener('keyup', function (e) {
    //Run LiveSearch on ever key up
    LiveSearch()
});

function LiveSearch() {
    //Get the input value
    let value = document.getElementById('livesearchtags').value

    $.ajax({
        type: "POST",
        url: "../LiveTagSearch",
        data: { search: value },
        datatype: "html",
        success: function (data) {
            // Insert the returned search results html into the result element
            $('#result').html(data);
        }
    });
}

// New timeout variable
let timeout = null;

document.getElementById('livesearchtags').addEventListener('keyup', function (e) {
    // Clear existing timeout      
    clearTimeout(timeout);

    // Reset the timeout to start again
    timeout = setTimeout(function () {
        LiveSearch()
    }, 800);
});  
