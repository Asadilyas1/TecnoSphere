var t = "";
$(document).ready(function () {
    let i = 1;
    t = $("#SkillsDatatable").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,  
        "ajax": {
            "url": "/Dashboard/GetSkills",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [ 
            {
                "render": function (data, type, full, meta) {
                    return i++;
                }
            },
            { "data": "name", "name": "Name", "autoWidth": false },
            {
                data: null, render: function (data, type, row) {
                    return `<a href="javascript:void(0)" onclick="Edit('${row.id}')" class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></a> ` + 
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
    $('#modal-md').modal({ backdrop: 'static', keyboard: false }, 'show');
});

$("#form").submit(function (e) {
    e.preventDefault();
    $("#nameErr").text('');
    if ($("[name='Name']").val() == "") {
        $("#nameErr").text('Name is required!');
    } else {
        let url = "/Dashboard/SaveSkill";
        if ($("[name='Id']").val() != "") {
            url = "/Dashboard/UpdateSkill";
        }
        $.ajax({
            url: url,
            method: "POST",
            data: $("#form").serialize(),
            datatype: "JSON",
            success: function (r) {
                alert(r);
                $("#form")[0].reset();
                $('#modal-md').modal("hide");
                reload_table();
            },
            error: function (e) {
                alert(e);
            }
        });
    }
});

function Edit(id) {
    $.ajax({
        url: "/Dashboard/EditSkill",
        method: "POST",
        data: {
            id: id
        },
        datatype: "JSON",
        success: function (r) {
            if (r != null) {
                $("[name='Id']").val(r.id);
                $("[name='Name']").val(r.name);
                $("[name='Order']").val(r.ord);
                if (r.visibility == 1) {
                    $("#Visibility").val(1);
                    $("#is_visibile").prop("checked", true);
                }
                else if (r.isFeatured == 0) {
                    $("#Visibility").val(0);
                    $("#is_visibile").prop("checked", false);
                }
                $("#btnSave").text('Update Skill');
                $("#btnSave").css({
                    backgroundColor: "#17a2b8"
                });
                $('#modal-md').modal({ backdrop: 'static', keyboard: false }, 'show');
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
                    url: "/Blog/DeleteSkill",
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



