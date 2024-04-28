
using CharacterAPI.Tables;
using CharacterAPI.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace CharacterAPI
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {

            // 创建一个新的配置生成器并从 appsettings.json 文件中加载配置
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConfigurationHelper.InitConfiguration(Configuration);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(option => { option.Filters.Add(new GlobalExceptionFilter()); }).AddNewtonsoftJson(o => {
                //修改属性名称的序列化方式，首字母小写
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //配置跨域处理，允许所有来源          
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("cors", builder =>
                {
                    builder
                    .WithOrigins("*") //允许任何来源的主机访问
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("any", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });

            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("any");
            app.UseStaticFiles();
            app.UseAuthorization();

        
            app.MapControllers();

            SqlSugarHelper.Db.DbMaintenance.CreateDatabase();
            //建表 （看文档迁移）
            SqlSugarHelper.Db.CodeFirst.InitTables<AnimationTable>(); //所有库都支持
            SqlSugarHelper.Db.CodeFirst.InitTables<ImgTable>(); //所有库都支持
            SqlSugarHelper.Db.CodeFirst.InitTables<ImgJsonTable>(); //所有库都支持
            app.Run();
        }
    }
}
