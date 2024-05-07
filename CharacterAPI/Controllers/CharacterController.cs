using CharacterAPI.App;
using CharacterAPI.Models;
using CharacterAPI.Models.Dto;
using CharacterAPI.Tables;
using CharacterAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
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

            string localPath = $"wwwroot/resources/{code}";

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
                file.CopyTo(stream);
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

        [HttpPost]
        public DataResult<string> ExportImgPackage(List<ExportImg> data)
        {
            string guid = Guid.NewGuid().ToString();
            string yasuoguid = Guid.NewGuid().ToString();

            for (int i = 0; i < data.Count; i++)
            {
                string imageData = data[i].Data.Substring(data[i].Data.IndexOf(',') + 1); // 将"data:image/png;base64,"这部分去除
                byte[] imageBytes = Convert.FromBase64String(imageData);
                string aniName = data[i].Name.Split('_')[1];
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), guid, aniName, data[i].Name + "_" + data[i].Index + ".png");

                if (!Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));

                System.IO.File.WriteAllBytes(filePath, imageBytes);
            }

            string startPath = Path.Combine(Directory.GetCurrentDirectory(), guid);
            string zipPath = Path.Combine(Directory.GetCurrentDirectory(), yasuoguid, "catgame_" + DateTime.Now.ToString("yyyyMMdd") + ".zip");

            if (!Directory.Exists(System.IO.Path.GetDirectoryName(zipPath)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(zipPath));

            ZipFile.CreateFromDirectory(startPath, zipPath);

            Directory.Delete(startPath, recursive: true);

            return Success(zipPath, "");

            //byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);

            //Directory.Delete(System.IO.Path.GetDirectoryName(zipPath), recursive: true);

            //return File(fileBytes, "application/zip", "catgame_" + DateTime.Now.ToString("yyyyMMdd")+".zip");


          
        }

        [HttpGet]
        public IActionResult DownloadFile(string zipPath)
        {
         
            // 确认文件存在
            if (!System.IO.File.Exists(zipPath))
            {
                return NotFound();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);
            string name = System.IO.Path.GetFileName(zipPath);

            Directory.Delete(System.IO.Path.GetDirectoryName(zipPath), recursive: true);

            return File(fileBytes, "application/zip", name);
        }

        [HttpPost]
        public DataResult<string> GetCredits()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "CREDITS/CREDITS.TXT");
            string data = System.IO.File.ReadAllText(path);
            return Success(data, "");
        }
    }


}
