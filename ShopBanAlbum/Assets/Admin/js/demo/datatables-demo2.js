// Call the dataTables jQuery plugin
$(document).ready(function() {
    $('#donHangDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "DonHang/LoadData",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }],

        "columns": [
            { "data": "DonHangID", "name": "DonHangID", "autoWidth": true },
            { "data": "TenKhachHang", "name": "TenKhachHang", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "DiaChi", "name": "DiaChi", "autoWidth": true },
            { "data": "DienThoai", "name": "DienThoai", "autoWidth": true },
            { "data": "NgayDatHang", "name": "NgayDatHang", "autoWidth": true },
            { "data": "GhiChu", "name": "GhiChu", "autoWidth": true },
            { "data": "TenNguoiDatHang", "name": "TenNguoiDatHang", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="DonHang/Edit/' + full.DonHangID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.DonHangID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
});
