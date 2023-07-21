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
    public class NgheLamChaMeController : ControllerBase
    {

        #region lấy danh sách nội dung khóa học
        [HttpGet]
        [Route("DsNoiDungKhoaHoc")]
        public async Task<IActionResult> DsNoiDungKhoaHoc(string CourseId = "")
        {
            RouteVideoCourse ResPon = new RouteVideoCourse();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                List<ResVideoCourse> items = SqlHelper.ExecuteList<ResVideoCourse>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_LIST_VIDEO_COURSE", CourseId);

                List<lsContentCourse> lsContentCourse = new List<lsContentCourse>();
                if (items.Count() > 0)
                {
                    lsContentCourse = items.GroupBy(l => l.RowId).Select(cl => new lsContentCourse
                    {
                        RowId = cl.First().RowId,
                        Title = cl.First().Title,
                        TotalVideo = cl.Count(),
                        LsItemVideo = cl.Select(x1 => new ItemVideo
                        {
                            TitleVideo = x1.TitleVideo,
                            LinkVideo = x1.LinkVideo,
                            TimeVideo = x1.TimeVideo
                        }).ToList()
                    }).ToList();
                    ResPon.status = 200;
                    ResPon.message = lsContentCourse;
                }
                else
                {
                    ResPon.status = 400;
                }
                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DsNoiDungKhoaHoc");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region chi tiết khóa học
        [HttpGet]
        [Route("ChiTietKhoaHoc")]
        public async Task<IActionResult> ChiTietKhoaHoc(string RowId = "")
        {
            RouteDetailCourse ResPon = new RouteDetailCourse();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                DetailCourse DetailCourse = SqlHelper.ExecuteFirstItem<DetailCourse>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_DETAIL_COURSE", RowId);
                if (DetailCourse != null)
                {
                    ResPon.status = 200;
                    ResPon.message = DetailCourse;
                }
                else
                {
                    ResPon.status = 400;
                }
                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "ChiTietKhoaHoc");
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region danh sách khóa học
        [HttpGet]
        [Route("DanhSachKhoaHoc")]
        public async Task<IActionResult> DanhSachKhoaHoc(string CateId = "",int PageIndex = 1, int PageSize = 50)
        {
            RouteListCourse ResPon = new RouteListCourse();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                PageIndex = PageIndex <= 0 ? 1 : PageIndex;
                PageSize = PageSize <= 0 ? 50 : PageSize;
                var metadata = new metadata();

                List<ListCourse> ListCourse = SqlHelper.ExecuteList<ListCourse>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_COURSE", CateId, PageIndex, PageSize);
                if (ListCourse.Count() > 0)
                {
                    ResPon.message = ListCourse;
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = ListCourse[0].TotalRows;
                    metadata.total_page = ListCourse[0].NumOfPages;
                }
                else
                {
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = 0;
                    metadata.total_page = 0;
                }
                ResPon.status = 200;
                ResPon.metadata = metadata;

                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhSachKhoaHoc");
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
