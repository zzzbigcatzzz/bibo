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
    public class HoiDapBacSiController : ControllerBase
    {
        #region Câu hỏi liên quan
        [HttpGet]
        [Route("CauHoiLienQuan")]
        public async Task<IActionResult> CauHoiLienQuan(string RowId = "")
        {
            RouteListQuestionLienQuan ResPon = new RouteListQuestionLienQuan();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                List<ItemQuestion> lsItem = SqlHelper.ExecuteList<ItemQuestion>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_QUESTION_LIENQUAN", RowId);
                SqlHelper.CommandTimeout = 3000;

                List<LsQuestion> LsQuestion = new List<LsQuestion>();
                if (lsItem.Count() > 0)
                {
                    LsQuestion = lsItem.GroupBy(l => l.RowId).Select(cl => new LsQuestion
                    {
                        RowId = cl.First().RowId,
                        Content = cl.First().Content,
                        LinkImageAuthor = cl.First().LinkImageAuthor,
                        AuthorName = cl.First().AuthorName,
                        TotalComment = cl.First().TotalComment,
                        TotalLike = cl.First().TotalLike,
                        createdDate = cl.First().createdDate,
                        lsCate = cl.Select(x1 => new objCombox
                        {
                            Code = x1.CateCode,
                            Name = x1.CateName
                        }).ToList()
                    }).ToList();

                    ResPon.status = 200;
                    ResPon.message = LsQuestion;
                }
                else
                {
                    ResPon.status = 400;
                }
                return Ok(ResPon);

            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "CauHoiLienQuan");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region chi tiết câu hỏi
        [HttpGet]
        [Route("ChiTietCauHoi")]
        public async Task<IActionResult> ChiTietCauHoi(string RowId = "")
        {
            RouteDetailQuestion ResPon = new RouteDetailQuestion();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                LsQuestion LsQuestion = new LsQuestion();
                objDetailQuestion objDetailQuestion = new objDetailQuestion();
                if (RowId != "")
                {
                    List<ItemQuestion> Item = SqlHelper.ExecuteList<ItemQuestion>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_DETAIL_QUESTION", RowId);
                    if (Item != null)
                    {
                        LsQuestion = Item.GroupBy(l => l.RowId).Select(cl => new LsQuestion
                        {
                            RowId = cl.First().RowId,
                            Content = cl.First().Content,
                            LinkImageAuthor = cl.First().LinkImageAuthor,
                            AuthorName = cl.First().AuthorName,
                            TotalComment = cl.First().TotalComment,
                            TotalLike = cl.First().TotalLike,
                            createdDate = cl.First().createdDate,
                            lsCate = cl.Select(x1 => new objCombox
                            {
                                Code = x1.CateCode,
                                Name = x1.CateName
                            }).ToList()
                        }).First();

                        List<lsAnswer> lsAnswer = SqlHelper.ExecuteList<lsAnswer>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_ANSWER", RowId);

                        objDetailQuestion.LsQuestion = LsQuestion;
                        objDetailQuestion.lsAnswer = lsAnswer;

                        ResPon.status = 200;
                        ResPon.message = objDetailQuestion;
                    }
                    else
                    {
                        return Ok(new {status = 400,message = "Không lấy được thông tin câu hỏi"});
                    }
                }
                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "ChiTietCauHoi");
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Danh sách chuyên mục
        [HttpGet]
        [Route("DanhsachChuyenMuc")]
        public async Task<IActionResult> DanhsachChuyenMuc()
        {
            RouteListCate ResPon = new RouteListCate();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                List<objCombox> LsCate = SqlHelper.ExecuteList<objCombox>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_CATE");
                if (LsCate.Count() > 0)
                {
                    ResPon.status = 200;
                    ResPon.message = LsCate;
                }
                else
                {
                    ResPon.status = 400;
                }
                return Ok(ResPon);

            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhsachChuyenMuc");
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Danh sách câu hỏi đáp
        [HttpGet]
        [Route("DanhsachHoiDap")]
        public async Task<IActionResult> DanhsachHoiDap(string DocterId = "", string CateId = "", int PageIndex = 1, int PageSize = 50)
        {
            RouteListQuestion ResPon = new RouteListQuestion();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                PageIndex = PageIndex <= 0 ? 1 : PageIndex;
                PageSize = PageSize <= 0 ? 50 : PageSize;


                List<ItemQuestion> lsItem = SqlHelper.ExecuteList<ItemQuestion>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_QUESTION", DocterId, CateId, PageIndex, PageSize);
                SqlHelper.CommandTimeout = 3000;

                var metadata = new metadata();
                var objQuestion = new objQuestion();
                List<LsQuestion> LsQuestion = new List<LsQuestion>();
                List<objCombox> lsCate = new List<objCombox>();
                if (lsItem.Count() > 0)
                {
                    LsQuestion = lsItem.GroupBy(l => l.RowId).Select(cl => new LsQuestion
                    {
                        RowId = cl.First().RowId,
                        Content = cl.First().Content,
                        LinkImageAuthor = cl.First().LinkImageAuthor,
                        AuthorName = cl.First().AuthorName,
                        TotalComment = cl.First().TotalComment,
                        TotalLike = cl.First().TotalLike,
                        createdDate = cl.First().createdDate,
                        lsCate = cl.Select(x1 => new objCombox
                        {
                            Code = x1.CateCode,
                            Name = x1.CateName
                        }).ToList()
                    }).ToList();

                    List<objCombox> LsCate = SqlHelper.ExecuteList<objCombox>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_CATE");

                    objQuestion.LsQuestion = LsQuestion;
                    objQuestion.lsCate = LsCate;
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = LsQuestion.Count();
                    metadata.total_page = lsItem[0].NumOfPages;

                }
                else
                {
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = 0;
                    metadata.total_page = 0;
                }

                ResPon.status = 200;
                ResPon.message = objQuestion;
                ResPon.metadata = metadata;
                return Ok(ResPon);

            }
            catch(Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhsachHoiDap");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region gửi câu hỏi cho bác sĩ
        [HttpPost]
        [Route("GuiCauHoiChoBacSi")]
        public async Task<IActionResult> GuiCauHoiChoBacSi(ObjGuiCauHoi it)
        {
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                if (it.content == "")
                {
                    return Ok(new { status = 400, message = "Nội dung gửi không được để trống" });
                }
                if (it.createdBy == "")
                {
                    return Ok(new { status = 400, message = "Mã gửi người không được để trống" });
                }
                if (it.createdByName == "")
                {
                    return Ok(new { status = 400, message = "Tên người gửi không được để trống" });
                }
                if (it.cateId.Count() == 0)
                {
                    return Ok(new { status = 400, message = "Bạn vui lòng chọn chuyên mục" });
                }

                string cateIds = string.Join(",", it.cateId);

                int res = SqlHelper.ExecuteNonQuery(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_INSERT_QUESTION", it.content, it.LinkImageUser, it.createdBy, it.createdByName, cateIds);

                if (res > 0)
                {
                    return Ok(new { status = 200, message = "Gửi câu hỏi thành công" });
                }
                else
                {
                    return Ok(new { status = 400, message = "Có lỗi không gửi được câu hỏi" });
                }
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GuiCauHoiChoBacSi");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region danh sách bài viết liên quan
        [HttpGet]
        [Route("BaiVietLienQuan")]
        public async Task<IActionResult> BaiVietLienQuan(string CateId = "", string PostId = "", int PageIndex = 1, int PageSize = 50)
        {
            RouteListPost ResPon = new RouteListPost();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                PageIndex = PageIndex <= 0 ? 1 : PageIndex;
                PageSize = PageSize <= 0 ? 50 : PageSize;
                var metadata = new metadata();

                List<ListPost> LsPost = SqlHelper.ExecuteList<ListPost>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_RELATED_POSTS", CateId, PostId, PageIndex, PageSize);
                
                if (LsPost.Count() > 0)
                {
                    ResPon.message = LsPost;
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = LsPost[0].TotalRows;
                    metadata.total_page = LsPost[0].NumOfPages;
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "BaiVietLienQuan");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region chi tiết bài viết 
        [HttpGet]
        [Route("ChiTietBaiViet")]
        public async Task<IActionResult> ChiTietBaiViet(string RowId = "")
        {
            RouteDetailPost ResPon = new RouteDetailPost();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                if (RowId != "")
                {
                    DetailPost item = SqlHelper.ExecuteFirstItem<DetailPost>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_DETAIL_POST", RowId);
                    if (item.Title != null)
                    {
                        ResPon.status = 200;
                        ResPon.message = item;
                    }
                    else
                    {
                        ResPon.status = 400;
                        ResPon.message = null;
                    }
                }
                else
                {
                    ResPon.status = 400;
                    ResPon.message = null;
                }
                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "ChiTietBaiViet");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region danh sách bài viết bác sĩ chia sẻ
        [HttpGet]
        [Route("DanhSachBaiViet")]
        public async Task<IActionResult> DanhSachBaiViet(string CateId = "", string authorId = "", int PageIndex = 1, int PageSize = 50)
        {
            RouteListPost ResPon = new RouteListPost();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                PageIndex = PageIndex <= 0 ? 1 : PageIndex;
                PageSize = PageSize <= 0 ? 50 : PageSize;
                var metadata = new metadata();

                List<ListPost> LsPost = SqlHelper.ExecuteList<ListPost>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_POST", CateId, authorId, PageIndex, PageSize);
                if (LsPost.Count() > 0)
                {
                    ResPon.message = LsPost;
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = LsPost[0].TotalRows;
                    metadata.total_page = LsPost[0].NumOfPages;
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
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhSachBaiViet");
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region API lấy chi tiết thông tin bác sĩ
        [HttpGet]
        [Route("ChiTietBacSi")]
        public async Task<IActionResult> ChiTietBacSi(string RowId = "")
        {
            RouteDetailDoctor ResPon = new RouteDetailDoctor();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });
                if (RowId != "")
                {
                    DetailDoctor item = SqlHelper.ExecuteFirstItem<DetailDoctor>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_DETAIL_DOCTOR", RowId);
                    if (item.FullName != null)
                    {
                        ResPon.status = 200;
                        ResPon.message = item;
                    }
                    else
                    {
                        ResPon.status = 400;
                        ResPon.message = null;
                    }
                }
                else
                {
                    ResPon.status = 400;
                    ResPon.message = null;
                }
                return Ok(ResPon);
            }
            catch(Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "ChiTietBacSi");
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region API lấy danh bác sĩ
        [HttpGet]
        [Route("DanhSachBacSi")]
        public async Task<IActionResult> DanhSachBacSi(string CateId = "", int PageIndex = 1, int PageSize = 50)
        {
            RouteInfoDoctor ResPon = new RouteInfoDoctor();
            try
            {
                var rs = Common.checkAuth(Request.Headers["Authorization"].ToString());
                if (rs != "SUCCESS")
                    return Unauthorized(new { status = 401, message = rs });

                PageIndex = PageIndex <= 0 ? 1 : PageIndex;
                PageSize = PageSize <= 0 ? 50 : PageSize;

                List<LsDoctor> LsDoctor = SqlHelper.ExecuteList<LsDoctor>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_DOCTOR", CateId, PageIndex, PageSize);
                SqlHelper.CommandTimeout = 3000;

                var metadata = new metadata();
                var objMessage = new objMessage();
                List<ItemDoctor> ItemDoctor = new List<ItemDoctor>();
                List<objCombox> lsCate = new List<objCombox>();
                if (LsDoctor.Count() > 0)
                {
                    ItemDoctor = LsDoctor.GroupBy(l => l.RowId).Select(cl => new ItemDoctor
                    {
                        RowId = cl.First().RowId,
                        FullName = cl.First().FullName,
                        LinkImage = cl.First().LinkImage,
                        Position = cl.First().Position,
                        NumberLike = cl.First().NumberLike,
                        lsCate = cl.Select(x1 => new objCombox
                        {
                            Code = x1.CateCode,
                            Name = x1.CateName
                        }).ToList()
                    }).ToList();

                    List<objCombox> LsCate = SqlHelper.ExecuteList<objCombox>(EnvMiddleware.GetValue("strConn"), "APP_BIBOCARE_GETLIST_CATE");

                    objMessage.ItemDoctor = ItemDoctor;
                    objMessage.lsCate = LsCate;
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = ItemDoctor.Count();
                    metadata.total_page = LsDoctor[0].NumOfPages;
                }
                else
                {
                    metadata.current_page = PageIndex;
                    metadata.per_page = PageSize;
                    metadata.total_item = 0;
                    metadata.total_page = 0;
                }
                ResPon.status = 200;
                ResPon.message = objMessage;
                ResPon.metadata = metadata;

                return Ok(ResPon);
            }
            catch (Exception ex)
            {
                DataAccess.LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DanhSachBacSi");
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
