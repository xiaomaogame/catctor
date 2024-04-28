namespace CharacterAPI.Models
{
    /// <summary>
    /// 一般返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T>
    {
        /// <summary>
        /// 返回码
        /// </summary>
        /// <example>20000</example>
        public int Code { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 成功/失败
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}
