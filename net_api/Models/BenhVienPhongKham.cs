namespace BiboCareServices.Models
{
    public class BenhVienPhongKham
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
    }
    public class RouteListBenhVienPhongKham
    {
        public int status { set; get; }
        public List<BenhVienPhongKham> message { set; get; }
    }
    public class LichKham
    {
        public long Id { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Status { get; set; }
        public long BenhVienPhongKhamId { get; set; }
        public DateTime NgayKham { get; set; }
        public string GioKham { get; set; }
        public string NoiDungKham { get; set; }
        public string CreateBy { get; set; }
    }

    public class RouteListLichKham
    {
        public int status { set; get; }
        public List<LichKham> message { set; get; }
    }
}
