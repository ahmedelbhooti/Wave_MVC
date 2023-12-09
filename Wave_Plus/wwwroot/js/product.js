﻿var dataTable;



$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
   dataTable =  $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'name', "width": "32%" },
            { data: 'price', "width": "15%" },
            { data: 'category.name', "width": "15%" },
            { data: 'productAvailbality', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-80 btn-group" role="group" >
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2 rounded">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a onClick=Delete('/admin/product/Delelte?id=${data}') class="btn btn-danger mx-2 rounded">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>`
                },
                "width": "25%"
            }
        ]

    });

}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
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
    })
}

$(document).ready(function () {
    $('.btn-plus, .btn-minus').click(function () {
        var btn = $(this);
        var input = btn.closest('.input-group').find('input');
        var qty = parseInt(input.val());

        if (btn.hasClass('btn-plus')) {
            qty++;
        } else {
            qty--;
            if (qty < 1) qty = 1;
        }

        input.val(qty);
    });
});