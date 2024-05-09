using CharacterAPI.Tables;
using CharacterAPI.Utils;
using static Dm.net.buffer.ByteArrayBuffer;

namespace CharacterAPI.Task
{
    public class Restore
    {

 

        static List<ImgJsonTable> JsonTable;
        static List<ImgTable> LayerTable;

        private static Timer _timer;

      
        public static void Init()
        {
            SaveDb();
            SaveFile();
            _timer = new Timer(async _ => await Run(), null, TimeSpan.Zero, TimeSpan.FromHours(3));
        }

        public static async System.Threading.Tasks.Task Run()
        {
            
            RestoreDb();
            RestoreFile();
        }



        static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resources");
        static string desPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/temp_resources");


        public static void SaveDb()
        {
            JsonTable = SqlSugarHelper.Db.Queryable<ImgJsonTable>().ToList();
            LayerTable = SqlSugarHelper.Db.Queryable<ImgTable>().ToList();
        }

        public static void SaveFile()
        {
            DeleteFolderAndContents(desPath);
            CopyDirectory(filePath, desPath);
        }
        public static void RestoreDb()
        {
            SqlSugarHelper.Db.Deleteable<ImgJsonTable>().ExecuteCommand();
            SqlSugarHelper.Db.Deleteable<ImgTable>().ExecuteCommand();

            SqlSugarHelper.Db.Insertable(JsonTable).OffIdentity().ExecuteCommand();
            SqlSugarHelper.Db.Insertable(LayerTable).OffIdentity().ExecuteCommand();
        }

        public static void RestoreFile()
        {
            DeleteFolderAndContents(filePath);
            CopyDirectory(desPath, filePath);
        }

        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDir);
            DirectoryInfo diTarget = new DirectoryInfo(targetDir);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // 检查目标目录是否存在，如果不存在，则创建一个
            Directory.CreateDirectory(target.FullName);

            // 复制所有文件到新的文件夹
            foreach (FileInfo fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
            }

            // 复制每个子目录并使用递归
            foreach (DirectoryInfo sourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(sourceSubDir.Name);
                CopyAll(sourceSubDir, nextTargetSubDir);
            }
        }

        public static void DeleteFolderAndContents(string folderPath)
        {
            // 检查文件夹是否存在，如果存在，则删除
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            else
            {
                Console.WriteLine("Directory not found: " + folderPath);
            }
        }
    }
}
