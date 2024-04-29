using SqlSugar;

namespace CharacterAPI.Tables
{
    public class ImgJsonTable
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ImgUrl { get; set; }
        public string IconData { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public string Sex { get; set; }
        public string Pos { get; set; }
        public int PreId { get; set; }
    }
}
