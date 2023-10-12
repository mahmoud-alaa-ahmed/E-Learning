using DataLayer.Data;
using DataLayer.Services.Interfaces;
using DataLayer.Services.Reposatories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using System.Text;

namespace AuthModel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            //add automapper
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDbContext>(o => o.UseLazyLoadingProxies().
            UseSqlServer(builder.Configuration.GetConnectionString("defConnection")));
            //defaulttoken for create and reset password
            builder.Services.AddIdentity<CustomIdentityUser, IdentityRole>().
                AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IAuth,AuthenticationServices>();
            builder.Services.AddScoped(typeof(IEntity<>), typeof(EntityRepository<>));
            builder.Services.AddTransient<IUnit, UnitOfWork>();
            //create token with 1 hour time
            builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>o.TokenLifespan=TimeSpan.FromHours(1));
            // prevent domain conflicts and error access
            builder.Services.AddCors(o => o.AddPolicy("newPolicy", p =>
            {
                p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            //allow only Who have the token to access the api resources
            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(b =>
            {
                b.RequireHttpsMetadata = false;
                b.SaveToken = true;
                b.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:Key").Value)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    //ClockSkew=TimeSpan.Zero (refresh token replacement)
                };
            });
          




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("newPolicy");
            app.UseRouting();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Images")),
                RequestPath = "/image"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos")),
                RequestPath = "/video"
            });

            //is a must to secure ur api resources by token only send what is necessary and allow show resources if u have token
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}