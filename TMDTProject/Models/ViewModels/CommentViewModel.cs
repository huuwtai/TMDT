using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDTProject.Models.ViewModels
{
    public class CommentViewModel
    {
        public int MaBinhLuan { get; set; }
        public int? MaNguoiDung { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}