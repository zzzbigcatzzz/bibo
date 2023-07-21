namespace BiboCareServices.Models.TinhNgayRungTrung
{
    public class ObjDayContent
    {
        public long Id { get; set; }
       public DateTime Ngay { get; set; }
       public string? UserLogId { get; set; }
        public int DoDaiChuKy { get; set;}
        public DateTime NgayKinhGanNhat { get; set;}
        public int SoNgayHanhKinh { get; set;}
    }

    public class DayContent
    {
        public DateTime Ngay { get; set; }
        public string? UserLogId { get; set; }
    }

    public class GiaiDoanObj
    {
        public string GiaiDoan { get; set; }
        public int Ngay { get; set; }

    }
    public class NgayTrongChuKy
    {
        public int Month { get; set; }
        public string DayName { get; set; }
        public int DayNumber { get; set; }
        public string GiaiDoan { get; set; }
        public int ThuTuNgayTrongGd { get; set; }
        public int KhaNangMT { get; set; }
        public List<TrieuChungHoatDong> lstTrieuChungHoatDong { get; set; }
    }
    public class TrieuChungHoatDong
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
    }

    public class RouteListNgayTrongChuKy
    {
        public int status { set; get; }
        public List<NgayTrongChuKy> message { set; get; }
    }
}
