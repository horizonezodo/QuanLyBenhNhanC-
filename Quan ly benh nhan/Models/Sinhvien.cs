using System;
using System.Collections.Generic;

#nullable disable

namespace TestAppWithDB.Models
{
    public partial class Sinhvien
    {
        public int Masv { get; set; }
        public string Hoten { get; set; }
        public string Diachi { get; set; }
        public byte? Diem { get; set; }
        public int? Malop { get; set; }
        public string Anh { get; set; }

        public virtual Lophoc MalopNavigation { get; set; }
    }
}
