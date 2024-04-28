using SqlSugar;

namespace CharacterAPI.Tables
{
    public class AnimationTable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string aniName { get; set; }

        public string framePos { get; set;}
    }
}
