// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var placeholderElement = $('#add-edit-modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
            }
        });
    });
});


var selectedRows = [];
var deleteEmployeeId;
var employeeData;
$(document).ready(function () {
    employeeData = $('#EmployeeDataTable').DataTable(
        {
            ajax: {
                url: "/Employees/Index?handler=Employee",
                type: "get",
            },
            async: true,
            processing: true,
            serverSide: true,
            filter: true,
            order: [[1, 'asc']],
            columns: [
                {
                    "data": "select",
                    render: function (data, type, row) {
                        var html = `<div class="form-check">
                        <input class="form-check-input SelectedRow" type="checkbox" value="" id="chkSelectRow">
                        <label class="form-check-label" for="chkSelectRow">
                         </label>
                            </div>
                                `;
                        return html;
                    }

                },
                {
                    "data": "name" , name:"Name"

                },
                { "data": "departmentName", name: "DepartmentName"},
                {
                    "data": "dateOfBirth", name: "DateOfBirth",
                    "render": function (data) {
                        // Assuming "dateOfBirth" is the property in your data containing the date
                        // Format the date in "dd/mm/yyyy" format
                        var date = new Date(data);
                        var day = date.getDate().toString().padStart(2, '0');
                        var month = (date.getMonth() + 1).toString().padStart(2, '0');
                        var year = date.getFullYear();
                        return month + '/' + day + '/' + year;
                    }
                },
                {
                    "data": "actions",
                    render: function (data, type, row) {
                        var html = `
                                                <input type="button" class="btn btn-primary btn-sm" id="btnEdit" value="Edit"  />
                                         
                                        <input type="button" class="btn btn-danger btn-sm" id="btnDelete" value="Delete"  />
                            `;
                        return html;
                    }
                }

            ]
        }
    );

    $('#EmployeeDataTable tbody').on('click', '#btnEdit', function (event) {
        var dataRow = getSelectedRow($(this));
        console.log(dataRow);

        $.ajax({
            url: "/Employees/Index?handler=AddEditEmployeePartial",
            type: "get",
            // beforeSend: function (xhr) {
            //     xhr.setRequestHeader("XSRF-TOKEN",
            //         $('input:hidden[name="__RequestVerificationToken"]').val());
            // },
            data: { "id": dataRow.id },
            success: function (response) {
                $("#add-employee").remove();
                $("#add-edit-modal-placeholder").html('');
                $("#add-edit-modal-placeholder").html(response);
                var dateData = getDate(dataRow.dateOfBirth);
                $("#addEditEmployeeTitle").text("Edit Employee");
                $("#add-employee").modal('show');
                $("#txtDOB").val(dateData);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });



    });

    function getDate(dateStr) {
        const originalDate = new Date(dateStr);
        const formattedDate = originalDate.getFullYear() + '-' +
            (originalDate.getMonth() + 1).toString().padStart(2, '0') + '-' +
            originalDate.getDate().toString().padStart(2, '0');

        return formattedDate;
    }

    $('#EmployeeDataTable tbody').on('click', '#btnDelete', function () {

        var dataRow = getSelectedRow($(this));
        deleteEmployeeId = dataRow.id;
        $("#deleteConfirmationModal").modal("show");
    });

    $('#btnShowSelectedRow').click(function () {
        var rowsHtml = "";

        if (selectedRows.length > 0) {
            selectedRows.map((x) => {
                rowsHtml += `<tr><td>${x.name}</td><td>${x.departmentName}</td></tr>`;
            })
        } else {
            rowsHtml = "<tr><td colspan='2'>No selected rows</td></tr>";
        }
        $("#tableSelectedRowsData").html(rowsHtml);
        $("#showSelectedRowModal").modal("show");
    });

    $('#EmployeeDataTable tbody').on('change', '.SelectedRow', function () {
        var isChecked = $(this).prop('checked');

        var dataRow = getSelectedRow($(this));
        var index = selectedRows.findIndex(x => x.id == dataRow.id);

        if (isChecked) selectedRows.push(dataRow);
        else {
            if (index > -1) {
                selectedRows.splice(index);
            }
        }
    });


    const getSelectedRow = (ele) => {
        return employeeData.row($(ele).closest('tr')).data();
    }



});

const deleteEmployee = () => {
    $.ajax({
        url: "/Employees/Index?handler=Delete",
        type: "get",
        async: true,
        data: { id: deleteEmployeeId },
        success: (data) => {
            employeeData.draw();
            $("#deleteConfirmationModal").modal("hide");

        },
        error: () => {

        }

    });
}


$(document).on("submit", "#form-Employee", function (e) {
    
    e.preventDefault();
    $.ajax({
        url: "/Employees/Index?handler=AddEditEmployeePartial",
        type: "POST",
        async: true,
        data: $("#form-Employee").serializeArray(),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: (data) => {
            employeeData.draw();
            $("#add-employee").modal('hide');

        },
        error: () => {

        }

    });
});


