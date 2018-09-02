namespace Blog.Web
{
    using AutoMapper;
    using Blog.Common.Infrastructure;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin;
    using Blog.Services.Admin.Interfaces;
    using Blog.Services.Author;
    using Blog.Services.Author.Interfaces;
    using Blog.Services.Category;
    using Blog.Services.Category.Interfaces;
    using Blog.Services.User;
    using Blog.Services.User.Interfaces;
    using Blog.Web.Areas.Identity.Services;
    using Blog.Web.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BlogDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<BlogDbContext>();

            RegisterServiceLayer(services);

            services.AddAuthentication()
                .AddFacebook(options =>
                    {
                        options.AppId = this.Configuration.GetSection("Facebook:AppId").Value;
                        options.AppSecret = this.Configuration.GetSection("Facebook:AppSecret").Value;
                    })
                .AddGoogle(options =>
                    {
                        options.ClientId = this.Configuration.GetSection("Google:ClientId").Value;
                        options.ClientSecret = this.Configuration.GetSection("Google:ClientSecret").Value;
                    });

            services.Configure<IdentityOptions>(options =>
                {
                    options.Password = new PasswordOptions()
                    {
                        RequireDigit = false,
                        RequiredLength = Constants.PasswordMinimumLength,
                        RequiredUniqueChars = 1,
                        RequireLowercase = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false
                    };

                    //options.SignIn.RequireConfirmedEmail = true;
                });

            services.AddSingleton<IEmailSender, SendGridEmailSender>();
            services.Configure<SendGridOptions>(this.Configuration.GetSection("EmailSettings"));

            services.AddAutoMapper();

            services.AddMvc(
                    options =>
                        {
                            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                        })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticResources((HostingEnvironment)env);
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                ApplicationBuilderAuthExtensions.SeedDatabase(app);
                ApplicationBuilderCategoriesExtensions.SeedDatabase(app);
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAdminCategoriesService, AdminCategoriesService>();
            services.AddScoped<IAdminPostsService, AdminPostsService>();
            services.AddScoped<IAdminTagsService, AdminTagsService>();
            services.AddScoped<IAdminCommentsService, AdminCommentsService>();
            services.AddScoped<IAdminRepliesService, AdminRepliesService>();
            services.AddScoped<IAdminUsersService, AdminUsersService>();
            services.AddScoped<IAuthorPostsService, AuthorPostsService>();
            services.AddScoped<ICategoryPostsService, CategoryPostsService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
