// Call the dataTables jQuery plugin
$(document).ready(function () {
    $('#albumDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/Album/LoadData",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [4],
                "render": function (data, type, full) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "targets": [8],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [1],
                "render": function (data, type, full) {
                    return (data == null) ? "no image" : '<img style="width:100%" class="img-thumbnail" src="/Contents/images/album/' + data + '"/>';
                }
            }],

        "columns": [
            { "data": "AlbumID", "name": "AlbumID", "autoWidth": true },
            { "data": "HinhAnh", "name": "HinhAnh", "autoWidth": true },
            { "data": "TenAlbum", "name": "TenAlbum", "autoWidth": true },
            { "data": "GiaBan", "name": "GiaBan", "autoWidth": true },
            { "data": "NgayPhatHanh", "name": "NgayPhatHanh ", "autoWidth": true },
            { "data": "SoLuong", "name": "SoLuong", "autoWidth": true },
            { "data": "DaBan", "name": "DaBan", "autoWidth": true },
            { "data": "XuatXu", "name": "XuatXu", "autoWidth": true },
            { "data": "PhuKien", "name": "PhuKien", "autoWidth": true },
            { "data": "TheLoai", "name": "TheLoai", "autoWidth": true },
            { "data": "TacGia", "name": "TacGia", "autoWidth": true },
            { "data": "QuocGia", "name": "QuocGia", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-primary" href="/Admin/Album/Details/' + full.AlbumID + '">Details</a>'; }, "orderable": false
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/Album/Edit/' + full.AlbumID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.AlbumID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#donHangDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/DonHang/LoadData",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [12],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [5],
                "render": function (data, type, full) {
                    return moment(data).format('DD/MM/YYYY hh:mm:ss');
                }
            }],

        "columns": [
            { "data": "DonHangID", "name": "DonHangID", "autoWidth": true },
            { "data": "TenKhachHang", "name": "TenKhachHang", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "DiaChi", "name": "DiaChi", "autoWidth": true },
            { "data": "SoDienThoai", "name": "SoDienThoai", "autoWidth": true },
            { "data": "NgayDatHang", "name": "NgayDatHang", "autoWidth": true },
            { "data": "GhiChu", "name": "GhiChu", "autoWidth": true },
            { "data": "TenNguoiDatHang", "name": "TenNguoiDatHang", "autoWidth": true },
            { "data": "TrangThaiDonHang", "name": "TrangThaiDonHang", "autoWidth": true },
            { "data": "TrangThaiThanhToan", "name": "TrangThaiThanhToan", "autoWidth": true },
            { "data": "HinhThucThanhToan", "name": "HinhThucThanhToan", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-primary" href="/Admin/DonHang/Details/' + full.DonHangID + '">Detail</a>'; }, "orderable": false
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/DonHang/Edit/' + full.DonHangID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.DonHangID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    var table = $('#donHangDataTable').DataTable();

    $('.btnstatus').on('click', function () {
        $('.btnstatus').removeClass('active')
        $('.btn01').removeClass('active')
        $(this).addClass('active')
        table
            .search($(this).text())
            .draw();
    });
    $('.btn01').on('click', function () {
        $('.btnstatus').removeClass('active')
        $(this).addClass('active')
        table
            .search('')
            .draw();
    });
    $('#khachHangDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/KhachHang/LoadData",
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
            { "data": "KhachHangID", "name": "KhachHangID", "autoWidth": true },
            { "data": "TenKhachHang", "name": "TenKhachHang", "autoWidth": true },
            { "data": "EmailKhachHang", "name": "EmailKhachHang", "autoWidth": true },
            { "data": "DiaChi", "name": "DiaChi", "autoWidth": true },
            { "data": "SDT", "name": "SDT", "autoWidth": true },
            { "data": "DiemKhachHang", "name": "DiemKhachHang", "autoWidth": true },

            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/KhachHang/Edit/' + full.KhachHangID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.KhachHangID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#theLoaiDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/TheLoai/LoadData",
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
            { "data": "TheLoaiID", "name": "TheLoaiID", "autoWidth": true },
            { "data": "TenTheLoai", "name": "TenTheLoai", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/TheLoai/Edit/' + full.TheLoaiID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.TheLoaiID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#baiHatDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/BaiHat/LoadData",
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
            { "data": "BaiHatID", "name": "BaiHatID", "autoWidth": true },
            { "data": "TenBaiHat", "name": "TenBaiHat", "autoWidth": true },
            { "data": "ThoiLuong", "name": "ThoiLuong", "autoWidth": true },
            { "data": "Album", "name": "Album", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-primary" href="/Admin/BaiHat/Details/' + full.BaiHatID + '">Detail</a>'; }, "orderable": false
            },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/BaiHat/Edit/' + full.BaiHatID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.BaiHatID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#tacGiaDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/TacGia/LoadData",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [2],
                "render": function (data, type, full) {
                    return (data == null) ? "no image" : '<img style="width:100%" class="img-thumbnail" src="/Contents/images/Artist/' + data + '"/>';
                }
            }],

        "columns": [
            { "data": "TacGiaID", "name": "TacGiaID", "autoWidth": true },
            { "data": "TenTacGia", "name": "TenTacGia", "autoWidth": true },
            { "data": "HinhAnh", "name": "HinhAnh", "autoWidth": true },
            { "data": "GioiThieu", "name": "GioiThieu", "autoWidth": true },
            { "data": "NamSinh", "name": "NamSinh", "autoWidth": true },
            { "data": "NoiSinh", "name": "NoiSinh", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/TacGia/Edit/' + full.TacGiaID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.TacGiaID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#quocGiaDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/QuocGia/LoadData",
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
            { "data": "QuocGiaID", "name": "QuocGiaID", "autoWidth": true },
            { "data": "TenQuocGia", "name": "TenQuocGia", "autoWidth": true },
            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/QuocGia/Edit/' + full.QuocGiaID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.QuocGiaID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $('#nhanVienDataTable').DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/Admin/NhanVien/LoadData",
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
            { "data": "NhanVienID", "name": "NhanVienID", "autoWidth": true },
            { "data": "TenNhanVien", "name": "TenNhanVien", "autoWidth": true },
            { "data": "EmailNhanVien", "name": "EmailNhanVien", "autoWidth": true },
            { "data": "MatKhauNhanVien", "name": "MatKhauNhanVien", "autoWidth": true },

            {
                "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Admin/NhanVien/Edit/' + full.NhanVienID + '">Edit</a>'; }, "orderable": false
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.NhanVienID + "'); >Delete</a>";
                }, "orderable": false
            },

        ]
    });
    $("body").toggleClass("sidebar-toggled"),
        $(".sidebar").toggleClass("toggled"),
        $(".sidebar").hasClass("toggled") && $(".sidebar .collapse").collapse("hide")
});
