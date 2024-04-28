using CharacterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.ComponentModel;

namespace CharacterAPI.Utils
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MyController : ControllerBase
    {


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        public DataResult<T> Success<T>(T data, string msg = null)
        {
            if (msg == null)
            {
                msg = ErrorCodeEnum.Success.GetDescription();
            }

            return new DataResult<T>()
            {
                Message = msg,
                Success = true,
                Code = (int)ErrorCodeEnum.Success,
                Data = data
            };
        }

        [NonAction]
        public DataResult<string> Success(string msg = null)
        {
            if (msg == null)
            {
                msg = ErrorCodeEnum.Success.GetDescription();
            }

            return new DataResult<string>()
            {
                Message = msg,
                Success = true,
                Code = (int)ErrorCodeEnum.Success
            };
        }

        [NonAction]
        public DataResult<string> UploadSuccess(string msg = null)
        {
            if (msg == null)
            {
                msg = ErrorCodeEnum.UpLoadSuccess.GetDescription();
            }

            return new DataResult<string>()
            {
                Message = msg,
                Success = true,
                Code = (int)ErrorCodeEnum.UpLoadSuccess
            };
        }




        /// <summary>
        /// 返回成功（分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        public PageResult<T> SuccessPage<T>(List<T> data, int total, string msg = null)
        {
            if (msg == null)
            {
                msg = ErrorCodeEnum.Success.GetDescription();
            }

            return new PageResult<T>()
            {
                Code = (int)ErrorCodeEnum.Success,
                Message = msg,
                Success = true,
                Data = new DataPage<T>()
                {
                    Total = total,
                    Rows = data
                }
            };
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        public DataResult<string> Fail(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = ErrorCodeEnum.Fail.GetDescription();
            }

            return new DataResult<string>()
            {
                Message = msg,
                Success = false,
                Code = (int)ErrorCodeEnum.Fail
            };
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        public DataResult<T> Fail<T>(T data, string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = ErrorCodeEnum.Fail.GetDescription();
            }

            return new DataResult<T>()
            {
                Message = msg,
                Success = false,
                Code = (int)ErrorCodeEnum.Fail,
                Data = data
            };
        }

        [NonAction]
        public PageResult<T> FailPage<T>(List<T> data, int total, string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = ErrorCodeEnum.Fail.GetDescription();
            }

            return new PageResult<T>()
            {
                Code = (int)ErrorCodeEnum.Fail,
                Message = msg,
                Success = false,
                Data = new DataPage<T>()
                {
                    Total = total,
                    Rows = data
                }
            };
        }
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 20000,

        /// <summary>
        /// 上传成功
        /// </summary>
        [Description("上传成功")]
        UpLoadSuccess = 20001,


        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 40000,

        /// <summary>
        /// Token认证失败
        /// </summary>
        [Description("Token认证失败")]
        Unauthorized = 40100,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("授权失败")]
        Forbidden = 40300,
    }
}
