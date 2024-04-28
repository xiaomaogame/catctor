using CharacterAPI.Models;
using CharacterAPI.Models.Dto;
using CharacterAPI.Repo;
using CharacterAPI.Tables;
using CharacterAPI.Utils;
using Newtonsoft.Json;

namespace CharacterAPI.App
{
    public class MyApp
    {
        public static void AddImgJsonData(AddImgJsonData data)
        {
            if (ImgJsonRepo.ExitsType(data.Type))
            {
                throw new Exception("类型重复！");
            }


            if (!ImgRepo.ExitsImgByCode(data.Code))
            {
                throw new Exception("无对应项目！");
            }

            //插入一条json记录
            ImgJsonRepo.Insert(new ImgJsonTable
            {
                Code = data.Code,
                IconData = data.IconData,
                ImgUrl = data.ImgUrl,
                Type = data.Type,
                Sex = data.Sex,
                Desc = data.Desc
            });
        }

        public static List<AniInfo> GetAniInfo()
        {
            return ConfigurationHelper.GetDatabaseSettings<List<AniInfo>>("anis");
        }

        public static List<ImgTable> GetImgTabes()
        {
            return ImgRepo.GetImgTables();
        }

        public static List<ImgJsonDataRet> GetImgData()
        {
            return ImgRepo.GetImgData();
        }

        public static bool EditImgJson(EditImgJson imgJsonData)
        {
            var imgJson = ImgJsonRepo.GetImgJsonById(imgJsonData.Id);
            imgJson.Code = imgJsonData.Code;
            imgJson.Type = imgJsonData.Type;
            imgJson.Desc = imgJsonData.Desc;
            imgJson.Sex = imgJsonData.Sex;

            // 源文件路径
            string sourceFile = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{imgJson.ImgUrl}");

            // 目标文件路径
            string destinationFile = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{imgJson.Code}/{imgJson.Type}.png");


            if (File.Exists(sourceFile))
            {
                if (!File.Exists(destinationFile))
                {
                    // 移动文件
                    File.Move(sourceFile, destinationFile);
                    imgJson.ImgUrl = $"{imgJson.Code}/{imgJson.Type}.png";
                }
              
             
                var ret = ImgJsonRepo.Edit(imgJson);

            }
            else
            {
                throw new Exception("源文件不存在");
            }

            return true;
        }

        public static void DeleteImgJson(int id)
        {
            ImgJsonRepo.Delete(id);
        }

        public static string GetNameByCode(string code)
        {
            int count = SqlSugarHelper.Db.Queryable<ImgJsonTable>().Where(x => x.Code == code).Count() + 1;
            return code + count.ToString();
        }
    }

}
