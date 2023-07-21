using System;

public class ObjDangKy
{
    public string FullName { set; get; }
    public string Phone { set; get; }
    public string Email { set; get; }
    public string NgayDuSinh { set; get; }
    public string Address { set; get; }
    public string NoiDung { set; get; }
    public string NoiDungChiaSe { set; get; }
}

public class ThucPhamDinhDuong
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public string ShortDes { set; get; }
    public string Content { set; get; }
    public string LinkImage { set; get; }
}
public class HoatDong
{
    public string Title { set; get; }
    public string LinkImage { set; get; }
    public bool Check { set; get; }
    public string CateId { set; get; }
}

public class DanhMucHoatDong
{
    public string Code { set; get; }
    public string Name { set; get; }
}
public class obj_date_out
{
    public DateTime? date_children { set; get; }
}
public class WeekContent
{
    public string Name { set; get; }
    public string Lenght { set; get; }
    public string Weight { set; get; }
    public string Content { set; get; }
    public string LinkImage { set; get; }
    public long Code { set; get; }
    public long Month { set; get; }
}
public class DanhMucTuan
{
    public string Code { set; get; }
    public string Name { set; get; }
}

public class RouteWeekContent
{
    public int status { set; get; }
    public WeekContent message { set; get; }
}
public class RouteListDanhMucTuan
{
    public int status { set; get; }
    public List<DanhMucTuan> message { set; get; }
}

public class DanhSachCanLam
{
    public int CateWeekId { set; get; }
    public string Content { set; get; }
    public bool SetNoti { set; get; }
    public DateTime DateJob { set; get; }
    public string TimeJob { set; get; }
    public string CreateBy { set; get; }
}

public class UserLog
{
    public int UserId { set; get; }
    public string NgayDuSinh { set; get; }
}
public class RouteListHoatDong
{
    public int status { set; get; }
    public List<HoatDong> message { set; get; }
}

public class RouteListDanhMucHoatDong
{
    public int status { set; get; }
    public List<DanhMucHoatDong> message { set; get; }
}
public class RouteListThucPhamDinhDuong
{
    public int status { set; get; }
    public List<ThucPhamDinhDuong> message { set; get; }
}

public class RouteDetailThucPhamDinhDuong
{
    public int status { set; get; }
    public ThucPhamDinhDuong message { set; get; }
}

