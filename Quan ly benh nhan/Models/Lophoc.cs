using System;
using System.Collections.Generic;

#nullable disable

namespace TestAppWithDB.Models
{
    public partial class Lophoc
    {
        public Lophoc()
        {
            Sinhviens = new HashSet<Sinhvien>();
        }

        public int Malop { get; set; }
        public string Tenlop { get; set; }
        public string Giangvien { get; set; }

        public virtual ICollection<Sinhvien> Sinhviens { get; set; }
    }
}
