var t = "";
$(document).ready(function () {
    let i = 1;
    t = $("#MemberDatatable").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,  
        "ajax": {
            "url": "/api/member",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                targets: ['_all'],
                className: 'mdc-data-table__cell',
            },
            {
                "targets": [6],
                "type": "date",
                "render": function (data) {
                    if (data !== null) {
                        var d = new Date(data),
                            month = '' + (d.getMonth() + 1),
                            day = '' + d.getDate(),
                            year = d.getFullYear();

                        if (month.length < 2)
                            month = '0' + month;
                        if (day.length < 2)
                            day = '0' + day;

                        return [day, month, year].join('-');
                    } else {
                        return "";
                    }
                }
            }
        ],
        "columns": [
            {
                "render": function (data, type, full, meta) {
                    return i++;
                }
            },
            {
                data: null, render: function (data, type, full, meta) {
                    let phone = "";
                    let fax = "";
                    if (full.phoneNumber != null) {
                        phone = "<span>Mob.: " + full.phoneNumber + "</span><br>";
                    }
                    if (full.faxNo != null) {
                        fax = "<span>Fax.: " + full.faxNo + "</span>";
                    }
                    return "<span>" + full.name + "</span><br>" +
                        "<span>Res.: " + full.correspondenceAddress + "</span><br>" +
                        `${phone}` + `${fax}`;
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<span>Ofc.: " + row.offNo + "</span> ," +
                        "<span>" + row.resAddress + "</span>";
                }
            },  
            { "data": "email", "name": "Email", "autoWidth": false },
            { "data": "barCouncil", "name": "BarCouncil", "autoWidth": false },
            { "data": "memberNo", "name": "MemberNo", "autoWidth": false },
            { "data": "dateOfBirth", "name": "DateofBirth", "autoWidth": false },
            {
                data: null, render: function (data, type, row) {
                    return `<a href="javascript:void(0)" onclick="Edit('${row.id}')" class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></a>` + 
                        `<a href="javascript:void(0)" onclick="Delete('${row.id}')" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></a>`;
                }
            }
        ]
    });
}); 

function reload_table() {
    t.ajax.reload(null, false); //reload datatable ajax 
}

$("#btnNew").click(function () {
    $('#modal-xl').modal({ backdrop: 'static', keyboard: false }, 'show');
});

$("#form").submit(function (e) {
    e.preventDefault();
    $("#nameErr").text('');
    $("#mobileErr").text('');
    $("#corresErr").text('');
    $("#resErr").text('');
    $("#offErr").text('');
    $("#barErr").text('');
    $("#memErr").text('');
    $("#dobErr").text('');
    $("#emailErr").text('');
    $("#passErr").text('');
    if ($("[name='Name']").val() == "") {
        $("#nameErr").text('Name is required!');
    }
    else if ($("[name='Mobile']").val() == "") {
        $("#mobileErr").text('Mobile # is required!');
    }
    else if ($("[name='CorrespondenceAddress']").val() == "") {
        $("#corresErr").text('Correspondence Address is required!');
    }
    else if ($("[name='ResAddress']").val() == "") {
        $("#resErr").text('Res Address is required!');
    }
    else if ($("[name='OffNo']").val() == "") {
        $("#offErr").text('Office No is required!');
    }
    else if ($("[name='BarCouncil']").val() == "") {
        $("#barErr").text('Bar Council is required!');
    }
    else if ($("[name='MemberNo']").val() == "") {
        $("#memErr").text('Member No is required!');
    }
    else if ($("[name='DateOfBirth']").val() == "") {
        $("#dobErr").text('Date Of Birth is required!');
    }
    else if ($("[name='Email']").val() == "") {
        $("#emailErr").text('Email is required!');
    }
    else if ($("[name='Password']").val() == "") {
        $("#passErr").text('Password is required!');
    } else {
        let url = "/Dashboard/Save";
        if ($("[name='Id']").val() != "") {
            url = "/Dashboard/Update";
        }
        $.ajax({
            url: url,
            method: "POST",
            data: $("#form").serialize(),
            datatype: "JSON",
            success: function (r) {
                alert(r);
                $("#form")[0].reset();
                $('#modal-xl').modal("hide");
                reload_table();
            },
            error: function (e) {
                alert(e);
            }
        });
    }
});

function genPassword() {
    var chars = "0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var passwordLength = 12;
    var password = "";
    for (var i = 0; i <= passwordLength; i++) {
        var randomNumber = Math.floor(Math.random() * chars.length);
        password += chars.substring(randomNumber, randomNumber + 1);
    }
    document.getElementById("inputPassword").value = password;
}
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

function Edit(id) {
    $.ajax({
        url: "/Dashboard/Edit",
        method: "POST",
        data: {
            id: id
        },
        datatype: "JSON",
        success: function (r) {
            if (r != null) {
                $("[name='Id']").val(r.id);
                $("[name='Name']").val(r.name);
                $("[name='PhoneNumber']").val(r.phoneNumber);
                $("[name='Fax']").val(r.faxNo);
                $("[name='CorrespondenceAddress']").val(r.correspondenceAddress);
                $("[name='ResAddress']").val(r.resAddress);
                $("[name='OffNo']").val(r.offNo);
                $("[name='BarCouncil']").val(r.barCouncil);
                $("[name='MemberNo']").val(r.memberNo);
                $("[name='DateOfBirth']").val(formatDate(r.dateOfBirth));
                $("[name='Email']").val(r.email);
                $("[name='Password']").val(r.passwordHash);
                $("#btnSave").text('Update Changes');
                $("#btnSave").css({
                    backgroundColor: "#17a2b8"
                });
                $("#Email").hide();
                $("#Password").hide();
                $('#modal-xl').modal({ backdrop: 'static', keyboard: false }, 'show');
            }
        },
        error: function (e) {
            alert(e);
        }
    });
}

function Delete(id) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Dashboard/Delete",
                    type: "POST",
                    data: {
                        id: id
                    },
                    dataType: "json",
                    success: function () {
                        swal("Done!", "It was succesfully deleted!", "success");
                        reload_table();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        swal("Error deleting!", "Please try again", "error");
                    }
                });
            } else {
                swal("Your imaginary file is safe!");
            }
        });
}



