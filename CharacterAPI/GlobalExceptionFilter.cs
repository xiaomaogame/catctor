using CharacterAPI.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using SqlSugar;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CharacterAPI.Models;

namespace CharacterAPI
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = ExceptionResult(context.Exception);
        
            base.OnException(context);
        }

        /// <summary>
        /// 包装处理异常格式
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ObjectResult ExceptionResult(Exception ex)
        {

            DataResult<string> erroResult = new DataResult<string>();
            erroResult.Message = ex.Message.ToString();
            erroResult.Code = ((int)ErrorCodeEnum.Fail);
            erroResult.Success = false;
            erroResult.Data = null;

            return new ObjectResult(erroResult);
        }
    }
}
