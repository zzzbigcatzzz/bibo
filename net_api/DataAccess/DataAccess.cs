
using BiboCareServices.DataAccess;
using BiboCareServices.Middleware;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Text;
using Lib.Utils.Package;
using System.Data;
using System.Xml.Linq;
using BiboCareServices.Models.TinhNgayRungTrung;

namespace BiboCareServices.DataAccess
{
    public static class DataAccess
    {

        public static GiaiDoanObj XacDinhChuKy(ObjDayContent dayContent)
        {
            if (dayContent.Ngay >= dayContent.NgayKinhGanNhat &&
               dayContent.Ngay < dayContent.NgayKinhGanNhat.AddDays(dayContent.SoNgayHanhKinh))
            {
                return new GiaiDoanObj
                {
                    GiaiDoan = "Kì kinh",
                    Ngay= (dayContent.Ngay- dayContent.NgayKinhGanNhat).Days +1
                };
            }
            else if (dayContent.Ngay < dayContent.NgayKinhGanNhat.AddDays(dayContent.DoDaiChuKy)
                 && dayContent.Ngay >= dayContent.NgayKinhGanNhat.AddDays(dayContent.DoDaiChuKy - 13))
            {
                return new GiaiDoanObj
                {
                    GiaiDoan = "Ngày bình thường",
                    Ngay = (dayContent.Ngay - dayContent.NgayKinhGanNhat.AddDays(dayContent.DoDaiChuKy - 13)).Days +1
                };
                
            }
            else
            {
                return new GiaiDoanObj
                {
                    GiaiDoan = "Kì rụng trứng",
                    Ngay = (dayContent.Ngay - dayContent.NgayKinhGanNhat.AddDays(dayContent.SoNgayHanhKinh)).Days + 1
                };
            }
        }
        //public static DataTable CreateTableOrder(List<ItemOrder> ItemOrder)
        //{
        //    OrderItemtype obj = new OrderItemtype();
        //    List<OrderitemDetail> order_detail = new List<OrderitemDetail>();
        //    foreach (var x in ItemOrder)
        //    {
        //        OrderitemDetail sub = new OrderitemDetail()
        //        {
        //            ItemCode = x.ItemCode,
        //            Price = x.DiscountPrice == 0 ? x.Price : x.DiscountPrice,
        //            Quantity = x.Quantity
        //        };
        //        order_detail.Add(sub);
        //    }
        //    obj.order_detail = order_detail;
        //    var order = obj.CreateOrderDetailTable();
        //    return order;
        //}

        //public static List<TValue> ExecuteList_V1<TValue>(string strCon, string Stored, string PhamVi, List<ItemOrder> ItemOrder)
        //{
        //    var order = CreateTableOrder(ItemOrder);
        //    var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        //    var kq = new List<TValue>();
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        try
        //        {
        //            using (con)
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(Stored, con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("PhamVi", PhamVi));
        //                cmd.Parameters.Add(new SqlParameter("order", order));
        //                //while
        //                var reader = cmd.ExecuteReader();
        //                try
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Type type = typeof(TValue);
        //                        TValue obj = (TValue)Activator.CreateInstance(type);
        //                        PropertyInfo[] properties = type.GetProperties();

        //                        foreach (PropertyInfo property in properties)
        //                        {
        //                            try
        //                            {
        //                                var value = reader[property.Name];
        //                                if (value != null)
        //                                    property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
        //                            }
        //                            catch { }
        //                        }
        //                        kq.Add(obj);
        //                    }
        //                }
        //                catch (Exception ex1)
        //                {
        //                    throw ex1;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        con.Close();
        //        return kq;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //        LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
        //        LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
        //        return kq;
        //    }
        //}

        //public static DataTable CreateOrderDetailTableV2(List<ItemOrder_V1> ItemOrder)
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("ItemCode", typeof(string));
        //    table.Columns.Add("Price", typeof(decimal));
        //    table.Columns.Add("Quantity", typeof(int));
        //    foreach (var row in ItemOrder)
        //    {
        //        table.Rows.Add(row.ItemCode, row.Price, row.Quantity);
        //    }
        //    return table;
        //}
        //public static List<TValue> ExecuteList_V2<TValue>(string strCon, string Stored, string PhamVi, List<ItemOrder_V1> ItemOrder)
        //{
        //    var order = CreateOrderDetailTableV2(ItemOrder);
        //    var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        //    var kq = new List<TValue>();
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        try
        //        {
        //            using (con)
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(Stored, con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("PhamVi", PhamVi));
        //                cmd.Parameters.Add(new SqlParameter("order", order));
        //                //while
        //                var reader = cmd.ExecuteReader();
        //                try
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Type type = typeof(TValue);
        //                        TValue obj = (TValue)Activator.CreateInstance(type);
        //                        PropertyInfo[] properties = type.GetProperties();

        //                        foreach (PropertyInfo property in properties)
        //                        {
        //                            try
        //                            {
        //                                var value = reader[property.Name];
        //                                if (value != null)
        //                                    property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
        //                            }
        //                            catch { }
        //                        }
        //                        kq.Add(obj);
        //                    }
        //                }
        //                catch (Exception ex1)
        //                {
        //                    throw ex1;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        con.Close();
        //        return kq;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //        LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
        //        LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
        //        return kq;
        //    }
        //}

        //public static List<TValue> ExecuteList_V3<TValue>(string strCon, string Stored, string MaCT, List<ItemOrder_V1> ItemOrder)
        //{
        //    var order = CreateOrderDetailTableV2(ItemOrder);
        //    var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        //    var kq = new List<TValue>();
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        try
        //        {
        //            using (con)
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(Stored, con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("MaCT", MaCT));
        //                cmd.Parameters.Add(new SqlParameter("order", order));
        //                //while
        //                var reader = cmd.ExecuteReader();
        //                try
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Type type = typeof(TValue);
        //                        TValue obj = (TValue)Activator.CreateInstance(type);
        //                        PropertyInfo[] properties = type.GetProperties();

        //                        foreach (PropertyInfo property in properties)
        //                        {
        //                            try
        //                            {
        //                                var value = reader[property.Name];
        //                                if (value != null)
        //                                    property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
        //                            }
        //                            catch { }
        //                        }
        //                        kq.Add(obj);
        //                    }
        //                }
        //                catch (Exception ex1)
        //                {
        //                    throw ex1;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        con.Close();
        //        return kq;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //        LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
        //        LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
        //        return kq;
        //    }
        //}

        //public static List<TValue> ExecuteList_V4<TValue>(string strCon, string Stored, string MaCT, decimal StepAmount, string Type, List<ItemOrder_V1> ItemOrder)
        //{
        //    var order = CreateOrderDetailTableV2(ItemOrder);
        //    var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        //    var kq = new List<TValue>();
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        try
        //        {
        //            using (con)
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(Stored, con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("MaCT", MaCT));
        //                cmd.Parameters.Add(new SqlParameter("StepAmount", StepAmount));
        //                cmd.Parameters.Add(new SqlParameter("Type", Type));
        //                cmd.Parameters.Add(new SqlParameter("order", order));
        //                //while
        //                var reader = cmd.ExecuteReader();
        //                try
        //                {
        //                    while (reader.Read())
        //                    {
        //                        Type type = typeof(TValue);
        //                        TValue obj = (TValue)Activator.CreateInstance(type);
        //                        PropertyInfo[] properties = type.GetProperties();

        //                        foreach (PropertyInfo property in properties)
        //                        {
        //                            try
        //                            {
        //                                var value = reader[property.Name];
        //                                if (value != null)
        //                                    property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
        //                            }
        //                            catch { }
        //                        }
        //                        kq.Add(obj);
        //                    }
        //                }
        //                catch (Exception ex1)
        //                {
        //                    throw ex1;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        con.Close();
        //        return kq;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //        LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
        //        LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
        //        return kq;
        //    }
        //}

    }
}
