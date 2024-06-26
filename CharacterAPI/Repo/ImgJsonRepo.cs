﻿using CharacterAPI.Tables;
using CharacterAPI.Utils;

namespace CharacterAPI.Repo
{
    public class ImgJsonRepo
    {
        public static bool Insert(ImgJsonTable data)
        {
            var ret = SqlSugarHelper.Db.Insertable(data).ExecuteCommand();
            return ret > 0;
        }

        public static bool ExitsType(string type)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgJsonTable>().First(x => x.Type == type);
            if (ret != null)
            {
                return true;
            }
            else
                return false;
        }

        public static bool ExitsTypeNotId(string type,int id)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgJsonTable>().Where(x=>x.Id!=id).First(x => x.Type == type);
            if (ret != null)
            {
                return true;
            }
            else
                return false;
        }

        public static bool ExitsCode(string code)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgJsonTable>().First(x => x.Code == code);
            if (ret != null)
            {
                return true;
            }
            else
                return false;
        }

    


        public static bool Edit(ImgJsonTable imgJson)
        {
            var ret = SqlSugarHelper.Db.Updateable(imgJson).ExecuteCommand();
            return ret > 0;
        }

        public static ImgJsonTable GetImgJsonById(int id)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgJsonTable>().First(x => x.Id == id);
            return ret;
        }


        public static bool Delete(int id)
        {
            var data = GetImgJsonById(id);
            //删除文件
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/resources", data.ImgUrl);
            if (File.Exists(filePath))
            { 
                File.Delete(filePath);
            }
            return SqlSugarHelper.Db.Deleteable(data).ExecuteCommand() > 0;
        }
    }
}
