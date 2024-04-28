using CharacterAPI.Tables;

namespace CharacterAPI.Models.Dto
{
    public class ImgJsonDataRet
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Layer { get; set; }
        public List<ImgJsonTable> Imgs { get; set; }
    }

}
