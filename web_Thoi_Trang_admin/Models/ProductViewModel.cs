using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_Thoi_Trang_admin.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; } // mã sản phẩm
        public string NamePro { get; set; } // tên sản phẩm
        public string CategoryName { get; set; } // tên danh mục
        public decimal Price { get; set; } // đơn giá
        public string ImagePro { get; set; } // tên file ảnh trong Content/images
        public string ShortDescription { get; set; } // Thêm mô tả ngắn

    }
    public class ProductEditViewModel
    {
        public int MaSanPham { get; set; }
       
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string TenSanPham { get; set; }

        public string MoTa { get; set; }
        public HttpPostedFileBase HinhAnhDaiDien { get; set; }
        public int MaDanhMuc { get; set; }
        public bool TrangThai { get; set; }

        // Danh sách biến thể đi kèm sản phẩm
        public List<ProductVariantViewModel> BienThes { get; set; } = new List<ProductVariantViewModel>();
    }

    // 3. ViewModel chi tiết cho từng biến thể (Màu, Size, Giá, Tồn kho)
    public class ProductVariantViewModel
    {
        public int MaBienThe { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã SKU")]
        public string MaSKU { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kích cỡ")]
        public string KichCo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập màu sắc")]
        public string MauSac { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không được âm")]
        public decimal GiaBan { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
        public int SoLuongTon { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}