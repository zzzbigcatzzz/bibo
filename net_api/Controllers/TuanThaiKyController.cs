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
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BiboCareServices.Controllers
{
    [ApiController]
    [Route("Bibocare")]
    public class TuanThaiKyController : ControllerBase
    {
        #region Nhật ký thai kỳ
        [HttpPost]
        [Route("LuuTruNhatKy")]
        public async Task<IActionResult> LuuTruNhatKy(ObjNhatKy it)
        {
            try
            {
                string lsImage = string.Join(",", it.LsImage);

                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_insert_40tuan_NhatKy", it.Content, it.DateGhi, "B10926", lsImage);

                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Lưu ảnh thành công" });
                }
                else
                {
                    return Ok(new { status = 400, message = "Có lỗi xẩy ra, không lưu được ảnh" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "LuuTruNhatKy");
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Danh sách nhật ký thai kỳ
        [HttpGet]
        [Route("DanhSachThaiKy")]
        public IActionResult DanhSachThaiKy()
        {
            try
            {
                List<ObjNhatKy> danhSachThaiKy = new List<ObjNhatKy>();

                DataTable dt = SqlHelper.ExecuteDataTable(EnvMiddleware.GetValue("strConn"), "sp_get_40tuan_NhatKy");


                foreach (DataRow row in dt.Rows)
                {
                    ObjNhatKy nhatKy = new ObjNhatKy
                    {
                        Content = row["Content"].ToString(),
                        DateGhi = row["DateGhi"].ToString(),
                        LsImage = new List<string>()
                    };

                    danhSachThaiKy.Add(nhatKy);
                }

                return Ok(new { status = 200, data = danhSachThaiKy });
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhSachThaiKy");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Thực phẩm dinh dưỡng
        [HttpGet]
        [Route("GetListThucPhamDinhDuong")]
        public async Task<IActionResult> GetListThucPhamDinhDuong(string CateId, int PageIndex, int PageSize)
        {
            RouteListThucPhamDinhDuong res = new RouteListThucPhamDinhDuong();

            try
            {
                //var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                //if (rs != "SUCCESS")
                //    return Unauthorized(new { status = 401, message = rs });
                List<ThucPhamDinhDuong> lsItem = SqlHelper.ExecuteList<ThucPhamDinhDuong>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_getlist_thucphamdinhduong", CateId, PageIndex, PageSize);
                if (lsItem.Count() > 0)
                {

                    res.status = 200;
                    res.message = lsItem;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListThucPhamDinhDuong");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("DetailThucPhamDinhDuong")]
        public async Task<IActionResult> DetailThucPhamDinhDuong(int RowId)
        {
            RouteDetailThucPhamDinhDuong res = new RouteDetailThucPhamDinhDuong();
            try
            {
                ThucPhamDinhDuong item = SqlHelper.ExecuteList<ThucPhamDinhDuong>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_detail_thucphamdinhduong", RowId).FirstOrDefault();
                if (item != null)
                {
                    res.status = 200;
                    res.message = item;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch(Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DetailThucPhamDinhDuong");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
        #endregion

        #region Hoạt động
        [HttpGet]
        [Route("GetListHoatDong")]
        public async Task<IActionResult> GetListHoatDong(string CateId)
        {
            RouteListHoatDong res = new RouteListHoatDong();

            try
            {
                //var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                //if (rs != "SUCCESS")
                //    return Unauthorized(new { status = 401, message = rs });
                List<HoatDong> lsItem = SqlHelper.ExecuteList<HoatDong>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_getlist_hoatdong", CateId);
                if (lsItem.Count() > 0)
                {

                    res.status = 200;
                    res.message = lsItem;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListHoatDong");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
        #endregion

        #region Danh mục Hoạt động
        [HttpGet]
        [Route("GetListDanhMucHoatDong")]
        public async Task<IActionResult> GetListDanhMucHoatDong()
        {
            RouteListDanhMucHoatDong res = new RouteListDanhMucHoatDong();

            try
            {
                //var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                //if (rs != "SUCCESS")
                //    return Unauthorized(new { status = 401, message = rs });
                List<DanhMucHoatDong> lsItem = SqlHelper.ExecuteList<DanhMucHoatDong>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_getlist_danhmuc_hoatdong");
                if (lsItem.Count() > 0)
                {

                    res.status = 200;
                    res.message = lsItem;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListDanhMucHoatDong");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
        #endregion

        #region Danh mục tuần
        [HttpGet]
        [Route("GetListDanhMucTuan")]
        public async Task<IActionResult> GetListDanhMucTuan()
        {
            RouteListDanhMucTuan res = new RouteListDanhMucTuan();

            try
            {
                //var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                //if (rs != "SUCCESS")
                //    return Unauthorized(new { status = 401, message = rs });
                List<DanhMucTuan> lsItem = SqlHelper.ExecuteList<DanhMucTuan>(EnvMiddleware.GetValue("strConn"), "sp_getlist_40tuan_danhmuc_tuan");
                if (lsItem.Count() > 0)
                {

                    res.status = 200;
                    res.message = lsItem;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListDanhMucTuan");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
        #endregion

        #region Danh sách cần làm
        [HttpPost]
        [Route("DanhSachCanLam")]
        public async Task<IActionResult> ThemDanhSachCanLam(DanhSachCanLam item)
        {
            try
            {
                item.CreateBy = "";
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_40tuan_insert_DanhCanLam",item.CateWeekId, item.Content, item.SetNoti, item.DateJob, item.TimeJob, item.CreateBy);
                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Thêm thành công" });
                }
                else
                {
                    return Ok(new { status = 400, message = "Thêm thất bại! Có lỗi xảy ra" });
                }
            }
            catch(Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "LuuTruNhatKy");
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Week Content
        [HttpGet]
        [Route("GetWeekContent")]
        public async Task<IActionResult> GetWeekContent(string UserId)
        {

            const int SoTuanThaiKy = 40;
            DateTime ngayDuSinh = new DateTime();
            try
            {
                var it = SqlHelper.ExecuteList<obj_date_out>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_get_userlogbyid", UserId).FirstOrDefault();
                if (it.date_children == null)
                {
                    return BadRequest(new { status = 400, message = "Không tìm được ngày dự sinh" });
                }
                ngayDuSinh = (DateTime)it.date_children;
            }
            catch(Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetUserLogById");
                return BadRequest(ex.Message);
            }
            long tuan = (ngayDuSinh - DateTime.Today).Days / 7;
            RouteWeekContent res = new RouteWeekContent();
            
            try
            {
                var item = SqlHelper.ExecuteList<WeekContent>(EnvMiddleware.GetValue("strConn"), "sp_40tuan_getweekcontent", SoTuanThaiKy - tuan+1).FirstOrDefault();
                if (item != null)
                {
                    item.Month = item.Code / 4;
                    res.status = 200;
                    res.message = item;
                }
                else
                {
                    res.status = 400;
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetWeekContent");
                return BadRequest(ex.Message);
            }
            return Ok(res);

        }

        [HttpPost]
        [Route("AddUserLog")]
        public async Task<IActionResult> AddUserLog( DateTime NgayBatDauKinh, int SoNgayHanhkinh, string UserId,string NgayDuSinh = "" )
        {
            UserLog userLog= new UserLog();
            if (!String.IsNullOrEmpty(NgayDuSinh) )
            {
                userLog.NgayDuSinh = NgayDuSinh;
            }
            else
            {
                userLog.NgayDuSinh = NgayBatDauKinh.AddMonths(9).AddDays(SoNgayHanhkinh).ToString();
            }
            try
            {
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_40tuan_insert_userlog", UserId, userLog.NgayDuSinh);
                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Thêm thành công" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Thêm thất bại! Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "AddUserLog");
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
