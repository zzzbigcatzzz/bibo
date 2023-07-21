namespace BiboCareServices.Models.TinhNgayRungTrung
{
    public class DayContent
    {
        public long Id { get; set; }
       public DateTime Ngay { get; set; }
       public string? UserId { get; set; }
    }

    public class NgayTrongChuKy
    {
        public int Month { get; set; }
        public string DayName { get; set; }
        public int DayNumber { get; set; }
        public string ChuKy { get; set; }
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
