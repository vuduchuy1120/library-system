using Library_System.Hubs;
using Library_System.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Library_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<LibrarySystemContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSession();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(400);
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/Forbidden";
                        options.LogoutPath = "/Logout";
                        options.LoginPath = "/Login";
                    });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("isAdmin", "True"));
            });
            builder.Services.AddSignalR();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapHub<SignalrHub>("/signalrHub");
            app.Run();
        }
    }
}