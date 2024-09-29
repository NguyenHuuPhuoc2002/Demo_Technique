using System;
using System.Collections.Generic;

namespace Unit_Of_Work.Data;

public partial class LopHoc
{
    public string MaLh { get; set; } = null!;

    public string? TenLh { get; set; }

    public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
}
