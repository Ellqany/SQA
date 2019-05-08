using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQA.Abstraction;
using SQA.Concreate;
using SQA.Models;
using SQA.Models.FacultyContext;
using SQA.Models.Identity;
using SQA.Services.Abstraction;
using SQA.Services.Concreate;

namespace SQA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<IPasswordValidator<User>, PasswordValidator<User>>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IGenericServices, GenericServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration["ConnectionStrings:ConnectionString"]));

            services.AddDbContext<FacultyDBContext>(options =>
            options.UseSqlServer(
                Configuration["ConnectionStrings:DefaultConnection"]));
            //Seading database with identity data 
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                Configuration["ConnectionStrings:DefaultConnection"]));
            //Password strength and validation
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
            //the pass to login in case of unauthorized user try to access the  resources
            services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Student/Registration");
            //This allow url to type is lower case
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddMemoryCache();
            services.AddSession();
            services.AddSignalR();
            //Compatibility version of MVC
            services.AddMvc().
                SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<Chat>("/Chat");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}/{uname?}",
                    defaults: new { area = "", controller = "Student", action = "Index" }
                    );
            });
            // IdentitySeedData.EnsurePopulated(app);
        }

    }
}
