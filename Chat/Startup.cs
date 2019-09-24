using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Chat.DAL.EF;
using Chat.Models.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Chat.BLL.Automapper;
using System.Reflection;
using Chat.AutoMapper;
using Chat.BLL.Infrastructure;
using Chat.BLL.Services;
using Chat.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Chat.DAL.Interfaces;
using Chat.DAL.Repositories;
using Chat.Hubs;
using File = Chat.Models.Entities.File;

namespace Chat
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChatContext>(options =>
    options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>
               (opts =>
            {
               opts.Password.RequiredLength = 5;   // минимальная длина
               opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
               opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
               opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
               opts.Password.RequireDigit = false; // требуются ли цифры
               opts.User.RequireUniqueEmail=true;
            })
            .AddEntityFrameworkStores<ChatContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
 
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/Account/Login";

            //});


            services.AddCors();
            services.Configure<EmailConfig>(Configuration.GetSection("SettingEmailAccount"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IRepositoryBase<Dialog>, RepositoryBase<Dialog>>();
            services.AddTransient<IRepositoryBase<Message>, RepositoryBase<Message>>();
            services.AddTransient<IRepositoryBase<File>, RepositoryBase<File>>();
            services.AddTransient<IRepository<Contact>, ContactRepository>();
            services.AddTransient<IRepository<BlackList>, BlackListRepository>();
            services.AddTransient<IEmailSender,EmailSender>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IChatHub, ChatHub>();
            services.AddTransient<IBlackListService, BlackListService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IContactService, ContactService>();


            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly, typeof(MapperProfile).GetTypeInfo().Assembly);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR(o=>o.EnableDetailedErrors = true);
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
                app.UseHsts();
            }

            app.UseCors(builder =>
            builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });
        }
    }
}
