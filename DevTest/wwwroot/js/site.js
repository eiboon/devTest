//Single site so all js is dumped in here, its a complete mess.
//TODO: refactor and make readable and supportable

function SetPink() {
    var searchButton = document.getElementById("search-button");
    //TODO: remove debug code
    searchButton.style.borderColor = "#ff0055";
    searchButton.style.borderStyle = "Solid";

}
function FlushPink() {
    var searchButton = document.getElementById("search-button");
    //console.log(searchButton);
    searchButton.style.borderColor = "#3f3f3f";
    searchButton.style.borderStyle = "none";
}

function CreateEmployeeElement(json) {
    const newEmployee = document.createElement("div");
    //Add our employee object
    newEmployee.innerHTML =
        `<div class="employee-container">
        <div class="row">
        <label class="employee-name">${json.firstName} ${json.lastName}</label>
        </div>
        <div class="row">
                        <label class="employee-role">${json.role}</label>
        </div>
        <div class="row">
                        <label class="employee-number">${json.phone}</label>
        </div>
        <div class="row">
                        <label class="employee-email">${json.email}</label>
        </div>
        </div>`;
    return newEmployee;
}
function AppendEmployeeToEmployees(employee) {
    const employees = document.getElementById("Employees");
    employees.appendChild(employee);
}
function ClearEmployees() {
    $("#Employees").empty();
    
}
function serializeFromId(id) {
    const obj = $(`#${id}`)

    return obj.serialize();
}


function SubmitEmployee(event) {

    $("#responseMessage").html("");
    event.preventDefault();

    //MAke sure the warnigns are hidden
    $("#firstname-warning").addClass("hidden");
    $("#lastname-warning").addClass("hidden");
    $("#role-warning").addClass("hidden");
    $("#phone-warning").addClass("hidden");
    $("#email-warning").addClass("hidden");

    $("#firstname").removeClass("warning");
    $("#lastname").removeClass("warning");
    $("#role").removeClass("warning");
    $("#phone").removeClass("warning");
    $("#email").removeClass("warning");

    $.ajax({
        url: "/Home/EmployeeFormAjax",
        type: "POST",
        data: serializeFromId("EmployeeForm"), // Serializes form data
        success: function (response) {

            $("#EmployeeForm")[0].reset();
            $("#EmployeeFormContainer").fadeOut(25);
            ShowPopup("New user added!");


        },
        error: function (response) {
            //The response is a collection of errors,
            //field and error
            const errorResponses = response.responseJSON
            
            errorResponses.forEach(function (error) {

                //This allows for the backend to control the message displayed to user
                $(`#${error.field}-warning`)[0].innerText = error.error;
                console.log($(`#${error.field}-warning`));

                $(`#${error.field}-warning`).removeClass("hidden");
                $(`#${error.field}`).addClass("warning")
            });
            if (false) {
                //TODO
            }
            else {
                //unhandled error
                $("#responseMessage").html("<p>Error submitting form.</p>");
            }
        }
    });
}
function ShowEmployeeForm() {
    $("#EmployeeFormContainer").fadeIn(25);
}

function ShowPopup(Text) { 
   
    const close = document.querySelector(".close");
    $("#popup-text").text = text;


    // Show popup after 3 seconds
    $("#popup").fadeIn(500);

    // Close popup when clicking the "×"
    close.addEventListener("click", () => {
        popup.style.display = "none";
    });
    //close after 10 seconds
    setTimeout(() => {
        $("#popup").fadeOut(5000);;
    }, 5000);
}
function HighlightCharacters(text, chars) {
    let regex = new RegExp(`${chars}`, "gi");
    return text.replace(regex, match => `<span class="highlight">${match}</span>`);
}

//Eventhandlers for buttons and input boxes
$("#search-bar").on("blur", function () {
    FlushPink();
});
$("#search-bar").on("focus", function () {
    SetPink();
});

$("#search-bar").on("focus", function () {
    $("#suggestions").empty();
});

$("#create-user").on("click", function (event) {
    SubmitEmployee(event)
})

//Add EventListner to check for text on our searchbar
$("#search-bar").on("input", function () {

    if ($(this).val().length > 1) {
        console.log("Text changed:", $(this).val());
        //JSON the data we are sending so it can interact with MVC
        const data = { text: $(this).val() }

        //AJAX to get some data from server
        $.ajax({
            url: "/Home/EmployeeQuickSearch",
            type: "POST",
            data: data,
            success: function (response) {
                console.log(response);
                if (response) {
                    
                    response.forEach(function (name) {
                        console.log(name);
                        $("#suggestions").append(`<li>${HighlightCharacters(name, data.text)}</li>`);
                        $("#suggestions").removeClass("hidden");
                        //add eventhandler for clicking the dropdown
                        $("#suggestions li").on("click", function () {
                            $("#search-bar").val($(this).text()); // Set input value
                            $("#suggestions").addClass("hidden"); // Hide dropdown
                        });
                        $("#suggestions li").on("blur", function () {
                            $("#suggestions").addClass("hidden"); // Hide dropdown
                        });
                    });
                }
            },
            error: function (response) {
                //TODO error handling
            }
        });
    }
});


$("#search-button").on("click", function () {

    console.log("Search clicked");
    ClearEmployees();
    const data = { text: $("#search-bar").val() };
    console.log(data);
    //AJAX to get some data from server
    $.ajax({
        url: "/Home/EmployeeSearch",
        type: "POST",
        data: data,
        success: function (response) {
            //TODO populate search results
            response.forEach(function (json) {
                const employee = CreateEmployeeElement(json);
                AppendEmployeeToEmployees(employee);
            });
        },
        error: function (response) {
            //TODO error handling
        }
    });

});
