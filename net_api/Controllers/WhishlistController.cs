using BiboCareServices.Middleware;
using BiboCareServices.Models;
using Lib.Utils.Package;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BiboCareServices.Controllers
{
    [Route("Bibocare")]
    [ApiController]
    public class WhishlistController : ControllerBase
    {
        [HttpPost]
        [Route("create_wishlist")]
        public async Task<IActionResult> create_wishlist(Whishlist item)
        {
            try
            {
                item.CreatedBy = "";
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConnOMS"), "sp_insert_whishlist",
                    item.code,
                    item.name,
                    item.user_id,
                    item.source,
                    item.type,
                    item.status, 
                    item.CreatedBy);
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "create_wishlist");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get_list_wishlist")]
        public async Task<IActionResult> get_list_wishlist(string user_id)
        {
            RouteListWhishlist res = new RouteListWhishlist();

            try
            {
                //var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                //if (rs != "SUCCESS")
                //    return Unauthorized(new { status = 401, message = rs });
                List<Whishlist> lsItem = SqlHelper.ExecuteList<Whishlist>(EnvMiddleware.GetValue("strConnOMS"), "sp_getlist_whishlist");
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "get_list_wishlist");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }

        [HttpDelete]
        [Route("delete_wishlist")]
        public async Task<IActionResult> delete_wishlist(long wishlist_id)
        {
            try
            {
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConnOMS"), "sp_delete_whishlist", wishlist_id);
                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Xóa thành công" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Xóa thất bại! Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "delete_wishlist");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add_product_to_wishlist")]
        public async Task<IActionResult> add_product_to_wishlist(string wishlist_id,string user_id,string product_id)
        {
            try
            {
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConnOMS"), "sp_add_product_to_wishlist",
                    wishlist_id,
                    product_id,
                    user_id);
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "create_wishlist");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("remove_product_from_wishlist")]
        public async Task<IActionResult> remove_product_from_wishlist(string wishlist_id, string user_id, string product_id)
        {
            try
            {
                int rs = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConnOMS"), "sp_remove_product_from_wishlist",
                    wishlist_id,
                    product_id,
                    user_id);
                if (rs > 0)
                {
                    return Ok(new { status = 200, message = "Xóa thành công" });
                }
                else
                {
                    return BadRequest(new { status = 400, message = "Xóa thất bại! Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "remove_product_from_wishlist");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get_wishlist_detail")]
        public async Task<IActionResult> get_wishlist_detail(string user_id, string wishlist_id)
        {
            RouteListWhishlistDetail res = new RouteListWhishlistDetail();

            try
            {
                var lsItem = SqlHelper.ExecuteList<WhishlistDetail>(EnvMiddleware.GetValue("strConnOMS"), "sp_get_wishlist_detail", wishlist_id, user_id);
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "get_wishlist_detail");
                return BadRequest(ex.Message);
            }
            return Ok(res);
        }
    }
}
