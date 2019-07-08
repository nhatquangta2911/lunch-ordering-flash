using CourseApi.Models;
using CourseApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourseApi
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
         services.Configure<AlbumstoreDatabaseSettings>(
             Configuration.GetSection(nameof(AlbumstoreDatabaseSettings)));

         services.AddSingleton<IAlbumstoreDatabaseSettings>(sp =>
             sp.GetRequiredService<IOptions<AlbumstoreDatabaseSettings>>().Value);

         services.AddSingleton<AlbumService>();
         
         services.AddMvc()
                 .AddJsonOptions(option => option.UseMemberCasing())
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }
         // TODO: Http 5000, Https 5001 (Auto redirect to 5001) 
         // app.UseHttpsRedirection();
         app.UseDefaultFiles();
         app.UseStaticFiles();
         app.UseMvc();
      }
   }
}
