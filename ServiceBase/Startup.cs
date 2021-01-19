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

            //JWT 인증 사용
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
                      ValidateAudience = false, //토큰 유효성 검사 중에 대상의 유효성을 검사할지 여부
                      ValidateIssuer = false, //토큰 유효성 검사 중에 발급자가 유효성을 검사할지 여부
                      ValidateLifetime = false, //토큰 유효성 검사 중에 수명의 유효성을 검사할지 여부
                      RequireExpirationTime = false, //토큰에 만료시간 속성이 필요한지 적용
                      ClockSkew = TimeSpan.Zero, //시간의 유효성을 검사 할 때 적용 
                      ValidateIssuerSigningKey = true //securityToken 에 서명 한 SecurityKey의 유효성 검사 가 호출 되는지 여부
                 };
            });
            services.AddScoped<ITokenBuilder, TokenBuilder>(); // 토큰 생성
            //JWT 인증 사용


            services.AddControllers(); // MVC를 사용하기 위해

            //블레이저 사용
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //블레이저 사용

            services.AddSingleton<WeatherForecastService>();

           
            //[DI] 의존성 주입(Dependency Injection)
            DependencyInjectionContainer(services);
        }

        /// <summary>
        /// 의존성 주입 관련 코드만 따로 모아서 관리
        /// - 리포지토리 등록
        /// </summary>
        private void DependencyInjectionContainer(IServiceCollection services)
        { 
            //테스트용
            services.AddTransient<IPointRepository, PointRepositoryInMemory>(); // 인-메모리 사용
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // 토큰 인증 후 Api 사용 인증에 대한 처리 [Authorize]
            app.UseAuthorization(); //인증 

            app.UseEndpoints(endpoints =>
            {
                //MVC 라우터
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //MVC 라우터

                //블레이저 기본
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapRazorPages();
                //블레이저 기본


                // 루트 페이지 로드하자마자 특정 URL로 이동하고자 할 때 
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/Home");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
