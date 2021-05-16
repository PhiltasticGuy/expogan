using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExpoGAN.Web.Data;
using ExpoGAN.Web.Services;

namespace ExpoGAN.Web
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
            //services.AddCors();
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddRazorPages();

            // Add PostgresSQL support.
            var connectionString = Configuration["DATABASE_URL"];
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(
                    options => options.UseNpgsql(connectionString)
                )
                .AddTransient<IArtPieceRepository, ArtPieceRepository>();

            services.AddHttpClient<EmojisService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Puisque HTTPS nécessite l'installation et l'autorisation préalable
            // de certificats SSL sur la machine de développement, nous l'avons
            // temporairement désactivé tant que les étapes ne sont pas décrites
            // dans le README. La fonctionnalité a été testée sous Windows 10 avec
            // l'utilisation implicite des certificats SSL d'ASP.NET Core.
            // https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https
            //TODO: Activer cette ligne une fois que HTTPS sera configuré sur Linux chez Quentin.
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            // Activer l'authentification et l'autorisation dans l'application web.
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseCors(
            //    builder =>
            //    {
            //        builder
            //            .AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader();
            //    }
            //);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
