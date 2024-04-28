namespace CharacterAPI.Models
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class DataPage<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }


        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> Rows { get; set; }
    }
}
