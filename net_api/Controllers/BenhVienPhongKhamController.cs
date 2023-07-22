using BiboCareServices.Middleware;
using BiboCareServices.Models;
using Lib.Utils.Package;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BiboCareServices.Controllers
{
    [ApiController]
    [Route("Bibocare")]
    public class BenhVienPhongKhamController : ControllerBase
    {
        [HttpGet]
        [Route("GetListBenhVienPhongKham")]
        public async Task<IActionResult> GetListBenhVienPhongKham()
        {
            RouteListBenhVienPhongKham res = new RouteListBenhVienPhongKham();

            try
            {
                List<BenhVienPhongKham> lsItem = SqlHelper.ExecuteList<BenhVienPhongKham>(EnvMiddleware.GetValue("strConn"), "sp_getlist_phongkhambenhvien");
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListBenhVienPhongKham");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("GetListLichKham")]
        public async Task<IActionResult> GetListLichKham(string UserId)
        {
            RouteListLichKham res = new RouteListLichKham();

            try
            {
                List<LichKham> lsItem = SqlHelper.ExecuteList<LichKham>(EnvMiddleware.GetValue("strConn"), "sp_getlist_lichkham", UserId);
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetListLichKham");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("HenLichKham")]
        public async Task<IActionResult> HenLichKham(LichKham lichKham)
        {
            try
            {
                lichKham.CreateBy = "test";
                lichKham.Status = "Chờ xác nhận";
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_insert_lichkham",
                    lichKham.HoTen,
                    lichKham.SoDienThoai,
                    lichKham.BenhVienPhongKhamId,
                    lichKham.NgayKham,
                    lichKham.GioKham,
                    lichKham.NoiDungKham,
                    lichKham.CreateBy,
                    lichKham.Status
                    );
                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Đặt lịch thành công" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Đặt lịch thất bại! Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "HenLichKham");
                return BadRequest(ex.Message);
            }
        }

    }
}
