using System;
using System.IO;
using System.Reflection;
using CourseApi.Models;
using CourseApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

         services.Configure<CardstoreDatabaseSettings>(Configuration.GetSection(nameof(CardstoreDatabaseSettings)));
         services.AddSingleton<ICardstoreDatabaseSettings>(sp => 
            sp.GetRequiredService<IOptions<CardstoreDatabaseSettings>>().Value);

         services.AddSingleton<AlbumService>();
         services.AddSingleton<CardService>();
         
         services.AddMvc()
                 .AddJsonOptions(option => option.UseMemberCasing())
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
         services.AddDataProtection().SetApplicationName("Get to know ASP.NET Core");

         services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
               Version = "v1",
               Title = "My Very First API - ASP.NET Core",
               Description = "A simple ASP.NET Core Web API - preparing for the next project LunchOrder at Enclave",
               Contact = new OpenApiContact
               {
                  Name = "Quang (Shawn) N. TA",
                  Email = string.Empty,
                  Url = new System.Uri("https://github.com/nhatquangta2911")
               }
            });
            // Set the comments path for the Swagger JSON and UI.
            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath); 
         });
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
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }
         // TODO: Http 5000, Https 5001 (Auto redirect to 5001) 
         // app.UseHttpsRedirection();
         // app.UseDefaultFiles();
         // app.UseStaticFiles();
         app.UseCookiePolicy();
         app.UseAuthentication();
         // app.UseSession();
         app.UseSwagger();

         app.UseSwaggerUI(c => 
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Very First API - ASP.NET Core V1");
            c.RoutePrefix = string.Empty;
         });

         app.UseMvc();
      }
   }
}
