using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lms.Data.Data;
using Lms.Api.Extensions;
using Lms.Core.Repositories;
using Lms.Data.Repositories;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<LmsApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LmsApiContext") ?? throw new InvalidOperationException("Connection string 'LmsApiContext' not found.")));


            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
            //                .AddNewtonsoftJson()
            //                .AddXmlDataContractSerializerFormatters();

            builder.Services.AddControllers(opt =>
            {
                opt.ReturnHttpNotAcceptable = true;
                opt.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

            builder.Services.AddAutoMapper(typeof(LmsMappings));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                //var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                //opt.IncludeXmlComments(xmlCommentsFullPath);
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => opt.IncludeXmlComments(xmlFile));
            });


            var app = builder.Build();

            app.SeedDataAsync().GetAwaiter().GetResult();





            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}