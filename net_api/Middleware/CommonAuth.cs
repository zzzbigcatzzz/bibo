using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using BiboCareServices.Middleware;
using Newtonsoft.Json;
using net_api.Models.AuthStaffDev;

public class Common
{
    private static string Token = EnvMiddleware.GetValue("Token");

    public static string checkAuth(string re)
    {
        var result = "SUCCESS";
        var headers = re;
        string f_token = re;
        if (!headers.Contains("Bearer"))
        {
            result = "Authorization không đúng định dạng";
        }
        if (Token != f_token)
        {
            result = "Token ko đúng Authorization Bearer";
        }
        return result;
    }


    public static async Task<ReadTokenInfo> CheckAuth_StaffDevAsync(string authkey)
    {
        try
        {
            Console.WriteLine("CheckAuth_StaffDevAsync authkey: " + JsonConvert.SerializeObject(authkey));

            var it = new ReadTokenInfo();
            if (String.IsNullOrEmpty(authkey))
            {
                it.status = (int)HttpStatusCode.Unauthorized;
                it.message = "token lost!";
                return it;
            }
            if (!authkey.Contains("Bearer"))
            {
                it.status = (int)HttpStatusCode.Unauthorized;
                it.message = "Authorization wrong format";
                return it;
            }
            var authkeyjwt = authkey.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(authkeyjwt);
            var MaNV = jwtSecurityToken.Payload.ElementAt(0).Value.ToString(); // Mã nhân viên
            DateTime Time = jwtSecurityToken.ValidTo.ToLocalTime(); // Thời gian hết hạn

            Console.WriteLine("LocalTime: " + DateTime.Now.ToString());
            Console.WriteLine("Time: " + Time.ToString());



            if (DateTime.Now > Time)
            {
                it.status = (int)HttpStatusCode.Unauthorized;
                it.message = "Token quá hạn!";
                it.uId = MaNV;
                return it;
            }

            var isAccount = await BaseApiClient.GetAsync_API_v2("staffdev_APIDomain", authkey, "account/authorization?CodeEmp=" + MaNV);
            Console.WriteLine("CheckAuth_StaffDevAsync isAccount: " + JsonConvert.SerializeObject(isAccount));

            if (isAccount.StatusCode == true)
            {
                it.status = (int)HttpStatusCode.OK;
                it.message = "SUCCESS";
                it.uId = MaNV;
            }
            return it;
        }
        catch (Exception ex)
        {
            // Console.WriteLine("controller ex: " + JsonConvert.SerializeObject(ex));
            Console.WriteLine("CheckAuth_StaffDevAsync ex: " + JsonConvert.SerializeObject(ex));
            return null;
        }
    }
}
