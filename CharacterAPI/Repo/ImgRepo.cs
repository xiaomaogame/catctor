using CharacterAPI.Models.Dto;
using CharacterAPI.Tables;
using CharacterAPI.Utils;
using static Dm.net.buffer.ByteArrayBuffer;

namespace CharacterAPI.Repo
{
    public class ImgRepo
    {
        public static List<ImgJsonDataRet> GetImgData()
        {
            List<ImgTable> imgs = SqlSugarHelper.Db.Queryable<ImgTable>().OrderBy(x=>x.Layer).ToList();
            List<ImgJsonTable> jsons = SqlSugarHelper.Db.Queryable<ImgJsonTable>().ToList();

            List<ImgJsonDataRet> rets = new List<ImgJsonDataRet>();

            foreach (var item in imgs)
            {
                ImgJsonDataRet ret = new ImgJsonDataRet
                {
                    Code = item.Code,
                    Name = item.Name,
                    Layer = item.Layer,
                    Imgs = jsons.Where(x => x.Code == item.Code).ToList()
                };

                rets.Add(ret);
            }

            return rets;
        }

        public static bool ExitsLayer(int layer)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgTable>().First(x => x.Layer == layer);
            if (ret != null)
            {
                return true;
            }
            else
                return false;
        }

        public static bool ExitsImgByCode(string code)
        {
            var ret = SqlSugarHelper.Db.Queryable<ImgTable>().Where(x => x.Code == code);
            if (ret == null || !ret.Any())
                return false;
            return true;
        }
        public static List<ImgTable> GetImgTables()
        {
            return SqlSugarHelper.Db.Queryable<ImgTable>().OrderBy(x=>x.Layer).ToList();
        }

        public static ImgTable? GetImgByCode(string code)
        {
            return SqlSugarHelper.Db.Queryable<ImgTable>()?.First(x => x.Code == code);
        }
    
        public static bool Update(ImgTable imgdata)
        {
            var ret = SqlSugarHelper.Db.Updateable<ImgTable>(imgdata).ExecuteCommand();
            return ret > 0;
        }

        public static bool Delete(int id)
        {
            var ret = SqlSugarHelper.Db.Deleteable<ImgTable>(id).ExecuteCommand();
            return ret > 0;
        }

        public static bool Insert(ImgTable imgJsons)
        {
            var ret = SqlSugarHelper.Db.Insertable<ImgTable>(imgJsons).ExecuteCommand();
            return ret > 0;
        }

       


    }
}
