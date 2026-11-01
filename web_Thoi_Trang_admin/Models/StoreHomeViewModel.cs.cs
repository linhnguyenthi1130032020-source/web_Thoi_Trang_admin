using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_Thoi_Trang_admin.Models
{
    public class StoreHomeViewModel
    {
        // Danh sách danh mục lấy trực tiếp từ bảng DanhMuc trong CSDL của bạn
        public List<DanhMuc> Categories { get; set; }

        // Danh sách sản phẩm đang bán để Trang Chủ tiếp tục hiển thị.
        public List<ProductViewModel> Products { get; set; }

    }
}