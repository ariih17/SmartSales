var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblOrderHeader').DataTable({
        "ajax": {
                    url: '/order/getall',
                    dataSrc: 'data.$values',
                },
        "columns": [
            {
                data: 'sO_ORDER_ID',
                "render": function (data) {
                    return `<div class="w-75 btn group" role="group"> 
                        <a href="/order/edit?id=${data}" class="btn btn-primary mx-2 rounded"><i class="bi bi-pencil-square"></i> Edit </a>
                        <a onClick=Delete('/order/delete/${data}') class="btn btn-danger mx-2 rounded"><i class="bi bi-trash-fill"></i> Delete </a>
                    </div>`
                },
                "width": "25%"
            },
            { data: 'ordeR_NO', "width": "15%" },
            { data: 'ordeR_DATE', "width": "10%" },
            { data: 'customeR_NAME', "width": "15%" },
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}