using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BiboCareServices.Middleware;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using AutoMapper;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.WebSockets;
using Lib.Utils.Package;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Data;
using BiboCareServices.DataAccess;
using System.Security.Cryptography;
using System.IO.Pipelines;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Numerics;
using AutoMapper.Configuration.Conventions;
using System.Collections;
using Azure;
using System.ComponentModel;

namespace BiboCareServices.Controllers
{
    [ApiController]
    [Route("Bibocare")]
    public class TuanDieuKyController : ControllerBase
    {
        #region Đăng ký tư vấn
        [HttpPost]
        [Route("DangKyTuVan")]
        public async Task<IActionResult> DangKyTuVan(ObjDangKy it)
        {
            try
            {
                var res = await Common.CheckAuth_StaffDevAsync(Request.Headers["Authorization"].ToString());
                if (res.message != "SUCCESS")
                    return BadRequest(new { status = 401, message = res });


                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_insert_DangKy_web40tuan", it.FullName, it.Phone, it.Email, it.NgayDuSinh, it.Address,it.NoiDung,it.NoiDungChiaSe, res.uId);

                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Đăng ký tư vấn thành công" });
                }
                else
                {
                    return Ok(new { status = 400, message = "Có lỗi xẩy ra, không gửi được thông tin" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DangKyTuVan");
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
