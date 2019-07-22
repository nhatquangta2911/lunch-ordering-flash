using System.Text;
using CourseApi.Helpers;
using CourseApi.Models;
using CourseApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AutoMapper;
using CourseApi.Services.Users;
using CourseApi.Services.Dishes;
using CourseApi.Services.Menus;
using CourseApi.Services.DailyChoices;
using CourseApi.Services.Orders;
using CourseApi.Entities;

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
         services.AddCors();
         services.AddMvc()
                 .AddJsonOptions(option => option.UseMemberCasing())
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
         services.AddAutoMapper(typeof(Startup));

         // For secret information
         var appSettingsSection = Configuration.GetSection("AppSettings");
         services.Configure<AppSettings>(appSettingsSection);

         // Configure jwt authentication
         var appSettings = appSettingsSection.Get<AppSettings>();
         var key = Encoding.ASCII.GetBytes(appSettings.Secret);
         services.AddAuthentication(x => 
         {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
         .AddJwtBearer(x => 
         {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(key),
               ValidateIssuer = false,
               ValidateAudience = false
            };
         });

         // configure DI for application services

         services.Configure<OrderstoreDatabaseSettings>(Configuration.GetSection(nameof(OrderstoreDatabaseSettings)));
         services.AddSingleton<IOrderstoreDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<OrderstoreDatabaseSettings>>().Value);

         services.Configure<DailyChoicestoreDatabaseSettings>(Configuration.GetSection(nameof(DailyChoicestoreDatabaseSettings)));
         services.AddSingleton<IDailyChoicestoreDatabaseSettings>(sp => 
            sp.GetRequiredService<IOptions<DailyChoicestoreDatabaseSettings>>().Value);

         services.Configure<MenustoreDatabaseSettings>(Configuration.GetSection(nameof(MenustoreDatabaseSettings)));
         services.AddSingleton<IMenustoreDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<MenustoreDatabaseSettings>>().Value);

         services.Configure<DishstoreDatabaseSettings>(Configuration.GetSection(nameof(DishstoreDatabaseSettings)));

         services.AddSingleton<IDishstoreDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<DishstoreDatabaseSettings>>().Value);

         services.Configure<UserstoreDatabaseSettings>(
            Configuration.GetSection(nameof(UserstoreDatabaseSettings))
         );
         services.AddSingleton<IUserstoreDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<UserstoreDatabaseSettings>>().Value);

         services.AddSingleton<UserService>();
         services.AddSingleton<DishService>();
         services.AddSingleton<MenuService>();
         services.AddSingleton<DailyChoiceService>();
         services.AddSingleton<OrderService>();
         
         services.AddDataProtection().SetApplicationName("Get to know ASP.NET Core");

         services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
               Version = "v1",
               Title = "Lunch Ordering API - ASP.NET Core",
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
         app.UseDefaultFiles();
         app.UseStaticFiles();
         // app.UseCookiePolicy();

         app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

         app.UseAuthentication();
         // app.UseSession();
         app.UseSwagger();

         app.UseSwaggerUI(c => 
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lunch Ordering API - ASP.NET Core V1");
         });

         app.UseMvc();
      }
   }
}
