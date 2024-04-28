using SqlSugar;

namespace CharacterAPI.Tables
{
    public class ImgTable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Layer { get; set; }
    }
}
