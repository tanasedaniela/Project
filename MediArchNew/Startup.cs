using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediArch.Data;
using MediArch.Models;
using MediArch.Services;
using Data.Persistence;
using Data.Domain.Interfaces;
using BusinessRep.Services;
using Data.Domain.Interfaces.ServiceInterfaces;
using MediArch.Services.Interfaces;
using MediArch.Services.Services;
using BusinessRep.Repositories;

namespace MediArch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            // Search => Services => SQL SERVER (SQLEXPRESS) -> start
        
            services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("BusinessConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Add application services.
            
            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<IMedicineRepository, MedicineRepository>();
            services.AddTransient<IConsultRepository, ConsultRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IAnswerRepository, AnswerRepository>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IConsultService, ConsultService>();
            services.AddTransient<IMedicineService, MedicineService>();
            services.AddTransient<IRecordService, RecordService>();
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            MyIdentityDataInitializer.SeedData(roleManager, userManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    "Not_Found",
                    "{*url}",
                    new { controller = "Home", action = "Not_Found" }
                );
                
            });
        }
    }
}
