using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{
    public class DonHang
    {
        [Key]
        public int DonHangID { get; set; }
        [ForeignKey("TrangThaiDonHang")]
        public int? TrangThaiDonHangID { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được bỏ trống")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        public string DiaChi { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [DataType(DataType.Text,ErrorMessage = "Số điện thoại phải là số")]
        [RegularExpression(@"^[0][0-9]{9}$",ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0")]
        public string SoDienThoai { get; set; }
        public DateTime NgayDatHang { get; set; }
        public string GhiChu { get; set; }
        [ForeignKey("HinhThucThanhToan")]
        public int? HinhThucThanhToanID { get; set; }
        public int TienShip { get; set; }
        public int TienKhuyenMai { get; set; }
        public int TongTien { get; set; }
        public float ThuTienPayPal { get; set; }
        public string TrangThaiThanhToan { get; set; }
        [ForeignKey("KhachHang")]
        public int? KhachHangID { get; set; }
        [ForeignKey("MaGiamGia")]
        public int? MaGiamGiaID { get; set; }
        public virtual HinhThucThanhToan HinhThucThanhToan { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        public virtual TrangThaiDonHang TrangThaiDonHang { get; set; }
        public virtual MaGiamGia MaGiamGia { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
    }
}
