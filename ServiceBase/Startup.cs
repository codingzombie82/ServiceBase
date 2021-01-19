using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ServiceBase.Controllers;
using ServiceBase.Data;
using ServiceBase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            JWTAuthentication(services); //jWT ���� ��� �Լ�


            services.AddControllers(); // MVC�� ����ϱ� ����

            //������ ���
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //������ ���

            services.AddSingleton<WeatherForecastService>(); //���� ����

           
            //[DI] ������ ����(Dependency Injection)
            DependencyInjectionContainer(services);

            //swagger ���  
            services.AddSwaggerGen();//Swagger �߰�
            //swagger ���  

            // CORS ��� ���
            AddCors(services);
            // CORS ��� ���
        }

        private void AddCors(IServiceCollection services) {

            #region CORS
            //[CORS][1] CORS ��� ���
            //[CORS][1][1] �⺻: ��� ���
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAnyOrigin",
            //        builder => builder
            //        .AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});
            ////[CORS][1][2] ����: ��� ���
            //services.AddCors(o => o.AddPolicy("AllowAllPolicy", options =>
            //{
            //    options.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader();
            //}));
            //[CORS][1][3] ����: Ư�� �����θ� ���
            //services.AddCors(o => o.AddPolicy("AllowSpecific", options =>
            //        options.WithOrigins("https://localhost:44371")
            //               .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
            //               .WithHeaders("accept", "content-type", "origin", "X-TotalRecordCount")));
            #endregion
        }


        //jWT ���� ��� �Լ�
        private void JWTAuthentication(IServiceCollection services) {
            //JWT ���� ���
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = true;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("placeholder-key-that-is-long-enough-for-sha256")),
                    ValidateAudience = false, //��ū ��ȿ�� �˻� �߿� ����� ��ȿ���� �˻����� ����
                    ValidateIssuer = false, //��ū ��ȿ�� �˻� �߿� �߱��ڰ� ��ȿ���� �˻����� ����
                    ValidateLifetime = false, //��ū ��ȿ�� �˻� �߿� ������ ��ȿ���� �˻����� ����
                    RequireExpirationTime = false, //��ū�� ����ð� �Ӽ��� �ʿ����� ����
                    ClockSkew = TimeSpan.Zero, //�ð��� ��ȿ���� �˻� �� �� ���� 
                    ValidateIssuerSigningKey = true //securityToken �� ���� �� SecurityKey�� ��ȿ�� �˻� �� ȣ�� �Ǵ��� ����
                };
            });
            services.AddScoped<ITokenBuilder, TokenBuilder>(); // ��ū ����
            //JWT ���� ���
        }


        /// <summary>
        /// ������ ���� ���� �ڵ常 ���� ��Ƽ� ����
        /// - �������丮 ���
        /// </summary>
        private void DependencyInjectionContainer(IServiceCollection services)
        { 
            //�׽�Ʈ��
            services.AddTransient<IPointRepository, PointRepositoryInMemory>(); // ��-�޸� ���
            services.AddTransient<IPointLogRepository, PointLogRepository>();
            // Exams: Questions, ... 
            services.AddTransient<IQuestionRepository, QuestionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); //wwwroot ����ϱ� ����

            app.UseRouting();

            app.UseAuthentication(); // ��ū ���� �� Api ��� ������ ���� ó�� [Authorize]
            app.UseAuthorization(); //���� 

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //swagger ���  
            app.UseSwagger(); //swagger ���                             
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            //swagger ���  

            // CORS ��� ���
            //            app.UseCors("AllowAnyOrigin");
            app.UseCors();
            // CORS ��� ���


            app.UseEndpoints(endpoints =>
            {
                //MVC �����
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //MVC �����

                //������ �⺻
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapRazorPages();
                //������ �⺻


                // ��Ʈ ������ �ε����ڸ��� Ư�� URL�� �̵��ϰ��� �� �� 
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/Home");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
