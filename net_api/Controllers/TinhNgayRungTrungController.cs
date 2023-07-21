using BiboCareServices.Middleware;
using BiboCareServices.Models.TinhNgayRungTrung;
using Lib.Utils.Package;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BiboCareServices.Controllers
{
    [Route("Bibocare")]
    [ApiController]
    public class TinhNgayRungTrungController : ControllerBase
    {
        [HttpPost]
        [Route("AddUserLog")]
        public async Task<IActionResult> AddUserLog(DateTime NgayKinhGanNhat, int SoNgayHanhKinh, string UserId, int DoDaiChuKy )
        {
            try
            {
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_insert_tinhngayrungtrung_userlog", UserId, DoDaiChuKy, NgayKinhGanNhat, SoNgayHanhKinh);
                if (rs > 0)
                {
                    List<DayContent> lstNgayTrongChuKy = new List<DayContent>() 
                    {
                        new DayContent { Ngay = NgayKinhGanNhat, UserId = UserId }
                    };
                    for(int i = 1; i < DoDaiChuKy; i++)
                    {
                        lstNgayTrongChuKy.Add(
                            new DayContent
                            {
                                Ngay = NgayKinhGanNhat.AddDays(i),
                                UserId = UserId
                            }
                       );
                    }
                    SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "sp_insert_daycontent",lstNgayTrongChuKy);
                    return Ok(new { status = 200, message = "Thêm thành công" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Thêm thất bại! Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "AddNgayRungTrungAddUserLog");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDayContent")]
        public async Task<IActionResult> GetDayContent(string UserId)
        {
            RouteListNgayTrongChuKy res = new RouteListNgayTrongChuKy();

            try
            {
                List<DayContent> lsItem = SqlHelper.ExecuteList<DayContent>(EnvMiddleware.GetValue("strConn"), "sp_getlist_daycontent", UserId);
                var result = new List<NgayTrongChuKy>();
                foreach(DayContent item in lsItem)
                {
                    result.Add(new NgayTrongChuKy
                    {
                        Month = item.Ngay.Month,
                        DayNumber = item.Ngay.Day,
                        DayName = (int)item.Ngay.DayOfWeek ==0 ? "Chủ Nhật" : "Thứ" + ((int)item.Ngay.DayOfWeek+1).ToString(),
                        KhaNangMT = 100,

                    });
                }
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
    }
}
