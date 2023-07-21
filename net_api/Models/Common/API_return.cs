using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class objCombox
{
    public string Code { set; get; }
    public string Name { set; get; }
}
public class objComboxName
{
    public string Name { set; get; }
}
public class Pagination
{
    public bool codeReturn { set; get; }
    public string orderNo { set; get; }
    public string No { set; get; }
    public string message { set; get; }
    public string ValidationPeriodID { set; get; }
    public string Items { set; get; }
}

public class API_returnAPI
{
    public bool StatusCode { set; get; }
    public string Message { set; get; }
    public string Items { set; get; }
}
public class API_Return_v2
{
    public int status { get; set; }
    public string success { get; set; }
    public string   message { get; set; }

}
public class API_returnAPI_Call<T>
{
    public bool StatusCode { set; get; }
    public string Message { set; get; }
    public List<T> Items { set; get; }
}
