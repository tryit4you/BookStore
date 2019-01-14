using BookStore.Data;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.Data.Repositories;
using BookStore.Infrastructures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace BookStore
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
            services.AddEntityFrameworkSqlServer();
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.AddIdentity<ApplicationUser, IdentityRole>(
                opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    //  opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequiredLength = 8;
                    opts.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<BookStoreDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/bk-ebook/account/AccessDenied";
                options.LoginPath = "/login";
            });

            #region Transient

            services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreConnectionString")));
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IBookTypeRepository, BookTypeRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<IDownloadFormatRepository, DownloadFormatRepository>();
            services.AddTransient<IEmailRegisterRepository, EmailRegisterRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ICourseCategoryRepository, CourseCategoryRepository>();

            #endregion Transient

            services.Configure<CookiePolicyOptions>(options =>

            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddResponseCompression();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddMvc()
         .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
         .AddSessionStateTempDataProvider();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/images"
            });
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseResponseCompression();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                      name: "areas",
                      template: "{area:exists}/{controller=Home}/{action=Index}"
                ); routes.MapRoute(
                      name: "UploadEbook",
                      template: "/upload/{userId}",
                      defaults: new { controller = "books", action = "upload" }
                ); routes.MapRoute(
                           name: "UpdateEbook",
                           template: "/cap-nhat/{userId}&{id}",
                           defaults: new { controller = "Books", action = "PutAsync" }
                     ); routes.MapRoute(
                           name: "Manager",
                           template: "/quan-ly/{userId}",
                           defaults: new { controller = "books", action = "GetManagers" }
                     ); routes.MapRoute(
                            name: "EbookByUser",
                            template: "/e-books/{userId}",
                            defaults: new { controller = "books", action = "GetBookByUser" }
                      );
                routes.MapRoute(
                      name: "ebookDetail",
                      template: "v/{meta}/{id}",
                      defaults: new { controller = "books", action = "detail" }
                ); routes.MapRoute(
                      name: "category",
                      template: "c/{meta}/{id}",
                      defaults: new { controller = "Books", action = "LoadByCategory" }
                );
                routes.MapRoute(
                      name: "booktype",
                      template: "t/{meta}/{id}",
                      defaults: new { controller = "Books", action = "LoadByType" }
                ); routes.MapRoute(
                      name: "google",
                      template: "/signin-google",
                      defaults: new { controller = "UserAccount", action = "GoogleSignIn" }
                ); routes.MapRoute(
                      name: "khoahoc",
                      template: "/khoa-hoc",
                      defaults: new { controller = "Course", action = "Index" }
                ); routes.MapRoute(
                       name: "xemkhoahoc",
                       template: "/khoa-hoc/{meta}/{id}",
                       defaults: new { controller = "Course", action = "Detail" }
                 );
                routes.MapRoute(
                     name: "profile",
                     template: "/tai-khoan/{id?}",
                     defaults: new { controller = "UserAccount", action = "Profile" }
               ); routes.MapRoute(
                     name: "logout",
                     template: "/logout/{id?}",
                     defaults: new { controller = "UserAccount", action = "LogOut" }
               ); routes.MapRoute(
                          name: "login",
                          template: "/login",
                          defaults: new { controller = "UserAccount", action = "Login" }
                    ); routes.MapRoute(
                           name: "loginrequest",
                           template: "/login/request",
                           defaults: new { controller = "UserAccount", action = "LoginPage" }
                     );
                routes.MapRoute(
                   name: "about",
                   template: "p/thong-tin.html",
                   defaults: new { controller = "About", action = "Index" }
             ); routes.MapRoute(
                   name: "contact",
                   template: "p/lien-he.html",
                   defaults: new { controller = "Contact", action = "Index" }
             );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=books}/{action=Index}/{id?}");
            });
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                dbContext.Database.Migrate();
                DbSeeder.Seed(dbContext, roleManager, userManager);
            }
            BookStoreDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }
    }
}