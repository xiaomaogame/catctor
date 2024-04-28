using SqlSugar;

namespace CharacterAPI.Utils
{
    public class SqlSugarHelper //不能是泛型类
    {

        //多库情况下使用说明：
        //如果是固定多库可以传 new SqlSugarScope(List<ConnectionConfig>,db=>{}) 文档：多租户
        //如果是不固定多库 可以看文档Saas分库
        //用单例模式
        public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = "Data Source=ani.db",//连接符字串
            DbType = DbType.Sqlite,//数据库类型
            IsAutoCloseConnection = true //不设成true要手动close
        },
    db =>
    {
        //(A)全局生效配置点，一般AOP和程序启动的配置扔这里面 ，所有上下文生效
        //调试SQL事件，可以删掉
        db.Aop.OnLogExecuting = (sql, pars) =>
        {

            //获取原生SQL推荐 5.1.4.63  性能OK
            //Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));

            //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
            //Console.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer,sql,pars))

        };

        //多个配置就写下面
        //db.Ado.IsDisableMasterSlaveSeparation=true;

        //注意多租户 有几个设置几个
        //db.GetConnection(i).Aop
    });
    }
}
