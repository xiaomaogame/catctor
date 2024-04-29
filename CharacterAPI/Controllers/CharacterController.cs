using CharacterAPI.App;
using CharacterAPI.Models;
using CharacterAPI.Models.Dto;
using CharacterAPI.Tables;
using CharacterAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CharacterAPI.Controllers
{

    public class CharacterController : MyController
    {

        private readonly ILogger<CharacterController> _logger;


        public CharacterController(ILogger<CharacterController> logger)
        {
            _logger = logger;
            string connectionString = "Data Source=ani.db;"; // 这里的 myDatabase.db 就是你的 SQLite 数据库文件，路径可以是相对路径也可以是绝对路径
        }

        [HttpPost]
        public DataResult<List<AniInfo>> GetAniInfo()
        {
            var ret = MyApp.GetAniInfo();
            return Success(ret);
        }


        [HttpPost]
        public DataResult<List<ImgJsonDataRet>> GetImgData()
        {
            var ret = MyApp.GetImgData();
            return Success(ret);
        }



        [HttpPost]
        public DataResult<List<ImgTable>> GetImgTables()
        {
            var ret = MyApp.GetImgTabes();
            return Success(ret);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public DataResult<string> UploadImage(IFormFile file)
        {
            string code = HttpContext.Request.Form["code"];
            string name = HttpContext.Request.Form["name"];
            string iconData = HttpContext.Request.Form["iconData"];
            string desc = HttpContext.Request.Form["desc"];
            string sex = HttpContext.Request.Form["sex"];
            string pos = HttpContext.Request.Form["pos"];

            if (file == null || file.Length == 0)
            {
                return Fail("文件大小为空！");
            }

            string localPath = $"wwwroot/{code}";

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), localPath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fileName = code + "_" + name.Trim().Replace(".png", "") + ".png";
            string imgUrl = code + "/" + fileName;

            var path = Path.Combine(Directory.GetCurrentDirectory(), localPath, fileName);

            MyApp.AddImgJsonData(new AddImgJsonData
            {
                Code = code,
                Name = name,
                IconData = iconData,
                ImgUrl = imgUrl,
                Type = fileName.Replace(".png", ""),
                Desc = desc,
                Sex = sex,
                Pos = pos
            });

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            return Success();
        }


        [HttpPost]
        public DataResult<string> EditIcon(EditImgJson imgJson)
        {
            MyApp.EditImgJson(imgJson);
            return Success();
        }


        [HttpPost]
        public DataResult<string> DelImgJson(DelJson delJson)
        {
            MyApp.DeleteImgJson(delJson.Id);
            return Success();
        }

        [HttpPost]
        public DataResult<string> GetName(GetName getName)
        {
            string name = MyApp.GetNameByCode(getName.Code);
            return Success(name, "");
        }

    }


}
