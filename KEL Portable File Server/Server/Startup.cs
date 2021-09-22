using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using KELPortableFileServer.Server.Data;
using KELPortableFileServer.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace KELPortableFileServer.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(new JsonConfigurationSource { Path = "appsettings.json" });
            IConfiguration configurationRoot = configurationBuilder.Build();
            var configurationSection = configurationRoot.GetSection("Configuration");

            services
                .AddOptions<MemoryCacheOptions>()
                .Configure(options => {
                    //options.
                });


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var builder = services.AddIdentityServer(options =>
            {
                //options.Cors.CorsPaths.Add()
            }).AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options => {
                var x509 = LoadCert();
                var rsa = x509.GetRSAPrivateKey();
                var key = new RsaSecurityKey(rsa);
                options.SigningCredential = new SigningCredentials(key, SecurityAlgorithms.RsaSha512);
            });


            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var x509 = LoadCert();
                builder
                    .AddSigningCredential(x509)
                    .AddValidationKey(x509);
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();


            services.AddControllersWithViews(options =>
            {
                options.RequireHttpsPermanent = true;
                options.SuppressAsyncSuffixInActionNames = true;
            });
            services.AddRazorPages(options =>
            {
                //options.RootDirectory
            });

        }

        X509Certificate2 LoadCert()
        {
            var tempFolder = Path.Combine(Environment.ContentRootPath, "Cert");
            var certFile = Path.Combine(tempFolder, "cert.pfx");
            var password = "p@ssw0rd!";

            //var rsa = RSA.Create();
            //rsa.ImportFromEncryptedPem(File.ReadAllText(certFile), password.AsSpan());

            var x509 = new X509Certificate2(
                File.ReadAllBytes(certFile),
                password
                );
            return x509;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions { });

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
