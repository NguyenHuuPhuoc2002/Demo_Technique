using System;
using System.Collections.Generic;

namespace Unit_Of_Work.Data;

public partial class SinhVien
{
    public string MaSv { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? NgaySinh { get; set; }

    public string? MaLh { get; set; }

    public virtual LopHoc? MaLhNavigation { get; set; }
}
