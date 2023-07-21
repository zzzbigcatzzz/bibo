
using BiboCareServices.DataAccess;
using BiboCareServices.Middleware;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Text;
using Lib.Utils.Package;
using System.Data;
using System.Xml.Linq;

public class BaseConnectionSql
{
    public static DataTable ExecuteDataTable<Tkey>(string strCon, string Stored, Tkey item)
    {
        DataSet ds = new DataSet();
        StringBuilder str = new StringBuilder();
        try
        {
            var strCons = EnvMiddleware.GetValue(strCon);
            using (var con = new SqlConnection(strCons))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Stored, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                PropertyInfo[] props = typeof(Tkey).GetProperties();
                object value1;
                foreach (PropertyInfo prop in props)
                {
                    value1 = prop.GetValue(item, null);
                    if (value1 != null)
                    {
                        var p = new SqlParameter
                        {
                            ParameterName = prop.Name,
                            Value = value1
                        };
                        if (prop.PropertyType == typeof(DateTime) && (DateTime)value1 == DateTime.MinValue)
                        {
                            //ignore
                        }
                        else
                        {
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + prop.Name + ", " + value1 + "));");
                        }
                    }
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds);
                }
                con.Close();
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
            return ds.Tables[0];
        }
    }


    public static List<TValue> ExecuteList<Tkey, TValue>(string strCon, string Stored, Tkey item)
    {
        var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        var kq = new List<TValue>();
        StringBuilder str = new StringBuilder();
        try
        {
            try
            {
                using (con)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(Stored, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Parameters

                    PropertyInfo[] props = typeof(Tkey).GetProperties();
                    object value1;
                    foreach (PropertyInfo prop in props)
                    {
                        value1 = prop.GetValue(item, null);
                        if (value1 != null)
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = prop.Name,
                                Value = value1
                            };
                            if (prop.PropertyType == typeof(DateTime) && (DateTime)value1 == DateTime.MinValue)
                            {
                                //ignore
                            }
                            else
                            {
                                cmd.Parameters.Add(p);
                                str.Append("cmd.Parameters.Add(new SqlParameter(" + prop.Name + ", " + value1 + "));");
                            }
                        }
                    }

                    //while
                    var reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Type type = typeof(TValue);
                            TValue obj = (TValue)Activator.CreateInstance(type);
                            PropertyInfo[] properties = type.GetProperties();

                            foreach (PropertyInfo property in properties)
                            {
                                try
                                {
                                    var value = reader[property.Name];
                                    if (value != null)
                                        property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
                                }
                                catch { }
                            }
                            kq.Add(obj);
                        }
                    }
                    catch (Exception ex1) {
                        throw ex1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
            return kq;
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
            return kq;
        }
    }
    public static bool Execute_Update_Insert<TValue>(string strCon, string Stored, TValue item)
    {
        StringBuilder str = new StringBuilder();
        var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        var kq = new List<TValue>();
        try
        {
            using (con)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Stored, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                PropertyInfo[] props = typeof(TValue).GetProperties();
                object value1;
                foreach (PropertyInfo prop in props)
                {
                    value1 = prop.GetValue(item, null);
                    if (value1 != null)
                    {
                        var p = new SqlParameter
                        {
                            ParameterName = prop.Name,
                            Value = value1
                        };
                        if (prop.PropertyType == typeof(DateTime) && (DateTime)value1 == DateTime.MinValue)
                        {
                            //ignore
                        }
                        else
                        {
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + prop.Name + ", " + value1 + "));");
                        }
                    }
                }
                var reader = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        catch (Exception ex)
        {
            con.Close();
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), Stored);
            return false;
        }
    }
    public static List<TValue> ExecuteList_V1<TValue>(string strCon, string Stored, params object[] parameterValues)
    {
        var con = new SqlConnection(EnvMiddleware.GetValue(strCon));
        var kq = new List<TValue>();
        StringBuilder str = new StringBuilder();
        try
        {
            try
            {
                using (con)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(Stored, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Parameters

                    if ((parameterValues != null) && (parameterValues.Length > 0))
                    {
                        var commandParameters = SqlHelperParameterCache.GetSpParameterSet(EnvMiddleware.GetValue(strCon), Stored);

                        //Số lượng tham số không khớp với số lượng Giá trị tham số
                        if (commandParameters.Length != parameterValues.Length)
                        {
                            throw new ArgumentException("Số lượng tham số không khớp với số lượng Giá trị tham số.");
                        }
                        // Lặp lại thông qua SqlParameters, gán các giá trị từ vị trí tương ứng trong
                        for (int i = 0, j = commandParameters.Length; i < j; i++)
                        {
                            // Nếu giá trị mảng hiện tại xuất phát từ IDbDataParameter, thì hãy gán thuộc tính Giá trị của nó
                            if (parameterValues[i] is IDbDataParameter value)
                            {
                                var paramInstance = value;
                                var p = new SqlParameter
                                {
                                    ParameterName = commandParameters[i].ToString(),
                                    Value = paramInstance.Value ?? DBNull.Value
                                };
                                cmd.Parameters.Add(p);
                                str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + paramInstance.Value ?? DBNull.Value + "));");

                            }
                            else if (parameterValues[i] == null)
                            {
                                var p = new SqlParameter
                                {
                                    ParameterName = commandParameters[i].ToString(),
                                    Value = DBNull.Value
                                };
                                cmd.Parameters.Add(p);
                                str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + DBNull.Value + "));");
                            }
                            else
                            {
                                var p = new SqlParameter
                                {
                                    ParameterName = commandParameters[i].ToString(),
                                    Value = parameterValues[i]
                                };
                                cmd.Parameters.Add(p);
                                str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + parameterValues[i] + "));");
                            }
                        }
                    }
                    //while
                    var reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Type type = typeof(TValue);
                            TValue obj = (TValue)Activator.CreateInstance(type);
                            PropertyInfo[] properties = type.GetProperties();

                            foreach (PropertyInfo property in properties)
                            {
                                try
                                {
                                    var value = reader[property.Name];
                                    if (value != null)
                                        property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
                                }
                                catch { }
                            }
                            kq.Add(obj);
                        }
                    }
                    catch (Exception ex1)
                    {
                        throw ex1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            con.Close();
            return kq;
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
            return kq;
        }
    }

    public static List<string> Execute_List_String(string strCon, string Stored, params object[] parameterValues)
    {
        List<string> its = new List<string>();
        StringBuilder str = new StringBuilder();

        using (var con = new SqlConnection(EnvMiddleware.GetValue(strCon)))
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Stored, con);
                cmd.CommandTimeout = 30000;
                cmd.CommandType = CommandType.StoredProcedure;

                if ((parameterValues != null) && (parameterValues.Length > 0))
                {
                    var commandParameters = SqlHelperParameterCache.GetSpParameterSet(EnvMiddleware.GetValue(strCon), Stored);

                    //Số lượng tham số không khớp với số lượng Giá trị tham số
                    if (commandParameters.Length != parameterValues.Length)
                    {
                        throw new ArgumentException("Số lượng tham số không khớp với số lượng Giá trị tham số.");
                    }
                    // Lặp lại thông qua SqlParameters, gán các giá trị từ vị trí tương ứng trong
                    for (int i = 0, j = commandParameters.Length; i < j; i++)
                    {
                        // Nếu giá trị mảng hiện tại xuất phát từ IDbDataParameter, thì hãy gán thuộc tính Giá trị của nó
                        if (parameterValues[i] is IDbDataParameter value)
                        {
                            var paramInstance = value;
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = paramInstance.Value ?? DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + paramInstance.Value ?? DBNull.Value + "));");

                        }
                        else if (parameterValues[i] == null)
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + DBNull.Value + "));");
                        }
                        else
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = parameterValues[i]
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + parameterValues[i] + "));");
                        }
                    }
                }
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string ren = reader["Name"].ToString();//Name_DB chính là trong trên thủ tục trả ra , select 'Abc' as Name 
                    its.Add(ren);
                }
                con.Close();
                return its;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
                LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
                return its;
            }
        }
    }
    public static bool Execute_Update_Insert_V1(string strCon, string Stored, params object[] parameterValues)
    {
        StringBuilder str = new StringBuilder();
        var con = new SqlConnection(EnvMiddleware.GetValue(strCon));

        try
        {
            using (con)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Stored, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;

                if ((parameterValues != null) && (parameterValues.Length > 0))
                {
                    var commandParameters = SqlHelperParameterCache.GetSpParameterSet(EnvMiddleware.GetValue(strCon), Stored);

                    //Số lượng tham số không khớp với số lượng Giá trị tham số
                    if (commandParameters.Length != parameterValues.Length)
                    {
                        throw new ArgumentException("Số lượng tham số không khớp với số lượng Giá trị tham số.");
                    }
                    // Lặp lại thông qua SqlParameters, gán các giá trị từ vị trí tương ứng trong
                    for (int i = 0, j = commandParameters.Length; i < j; i++)
                    {
                        // Nếu giá trị mảng hiện tại xuất phát từ IDbDataParameter, thì hãy gán thuộc tính Giá trị của nó
                        if (parameterValues[i] is IDbDataParameter value)
                        {
                            var paramInstance = value;
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = paramInstance.Value ?? DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + paramInstance.Value ?? DBNull.Value + "));");

                        }
                        else if (parameterValues[i] == null)
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + DBNull.Value + "));");
                        }
                        else
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = parameterValues[i]
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + parameterValues[i] + "));");
                        }
                    }
                }

                var reader = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        catch (Exception ex)
        {
            con.Close();
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), Stored);
            return false;
        }
    }
    public static DataTable ExecuteDataTable_V1(string strCon, string Stored, params object[] parameterValues)
    {
        DataSet ds = new DataSet();
        StringBuilder str = new StringBuilder();
        try
        {
            var strCons = EnvMiddleware.GetValue(strCon);
            using (var con = new SqlConnection(strCons))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(Stored, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;

                if ((parameterValues != null) && (parameterValues.Length > 0))
                {
                    var commandParameters = SqlHelperParameterCache.GetSpParameterSet(EnvMiddleware.GetValue(strCon), Stored);

                    //Số lượng tham số không khớp với số lượng Giá trị tham số
                    if (commandParameters.Length != parameterValues.Length)
                    {
                        throw new ArgumentException("Số lượng tham số không khớp với số lượng Giá trị tham số.");
                    }
                    // Lặp lại thông qua SqlParameters, gán các giá trị từ vị trí tương ứng trong
                    for (int i = 0, j = commandParameters.Length; i < j; i++)
                    {
                        // Nếu giá trị mảng hiện tại xuất phát từ IDbDataParameter, thì hãy gán thuộc tính Giá trị của nó
                        if (parameterValues[i] is IDbDataParameter value)
                        {
                            var paramInstance = value;
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = paramInstance.Value ?? DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + paramInstance.Value ?? DBNull.Value + "));");

                        }
                        else if (parameterValues[i] == null)
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = DBNull.Value
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + DBNull.Value + "));");
                        }
                        else
                        {
                            var p = new SqlParameter
                            {
                                ParameterName = commandParameters[i].ToString(),
                                Value = parameterValues[i]
                            };
                            cmd.Parameters.Add(p);
                            str.Append("cmd.Parameters.Add(new SqlParameter(" + commandParameters[i].ToString() + ", " + parameterValues[i] + "));");
                        }
                    }
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds);
                }
                con.Close();
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            LogBuild.CreateLogger(str.ToString(), Stored + "_Param");
            LogBuild.CreateLogger("Error: " + ex.ToString(), Stored);
            return ds.Tables[0];
        }
    }

    //  Helper
    public static List<T> ExecuteList_Helper<T>(string strCon, string Stored, params object[] parameterValues) where T : new()
    {
        var dt = SqlHelper.ExecuteTable(EnvMiddleware.GetValue(strCon), Stored, parameterValues);
        return dt.HasRow() ? dt.ToList<T>() : new List<T>();
    }
    public static DataTable ExecuteTable_Helper(string strCon, string Stored, params object[] parameterValues)
    {
        var ds = SqlHelper.ExecuteDataset(EnvMiddleware.GetValue(strCon), Stored, parameterValues);
        return ds.HasItem() ? ds.Tables[0] : new DataTable();
    }

}
