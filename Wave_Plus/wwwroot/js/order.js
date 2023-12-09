var dataTable;



$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }

        else {
            if (url.includes("ondelivery")) {
                loadDataTable("ondelivery");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    if (url.includes("shipped")) {
                        loadDataTable("shipped")
                    }
                    else {
                        if (url.includes("cancelled")) {
                            loadDataTable("cancelled")
                        } else {
                            loadDataTable("all");
                        }
                    }
                }
            }
        }

    }
});



function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'name', "width": "25%" },
            { data: 'applicationUser.email', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'orderStatus', "width": "20%" },
            { data: 'paymentStatus', "width": "20%" },
            { data: 'orderTotal', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-80 btn-group" role="group" >
                                <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2 rounded">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                            </div>`
                },
                "width": "10%"
            }
        ]

    });

}


