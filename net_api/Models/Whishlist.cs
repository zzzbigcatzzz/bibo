namespace BiboCareServices.Models
{
    public class Whishlist
    {
        public long ID { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public string source { get; set; }
        public string type { get; set; }
        public int status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }

    }

   public class RouteListWhishlist
    {
        public int status { set; get; }
        public List<Whishlist> message { set; get; }
    }

    public class WhishlistDetail
    {
        public long ID { get; set; }
        public string Wishlist_id { get; set; }
        public string product_id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
    public class RouteListWhishlistDetail
    {
        public int status { set; get; }
        public List<WhishlistDetail>? message { set; get; }
    }
}
