

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using kurs.Context;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add DbContext
        builder.Services.AddDbContext<MagazinminContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
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

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
    ///{id?}
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    services.AddControllersWithViews();
                    services.AddDbContext<MagazinminContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
                });

                webBuilder.Configure((context, app) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
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

                    app.UseRouting();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllers();
                    });
                });
            });
}





//using kurs.Context;
//using Microsoft.EntityFrameworkCore;


//namespace kurs
//{
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        builder.Services.AddControllersWithViews();




//        builder.Services.AddDbContext<MagazinminContext>(options =>

//            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));




//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        if (!app.Environment.IsDevelopment())
//        {
//            app.UseExceptionHandler("/Home/Error");
//            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//            app.UseHsts();
//        }

//        app.UseHttpsRedirection();
//        app.UseStaticFiles();

//        app.UseRouting();

//        app.UseAuthorization();

//        app.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Pokupatel}/{action=Index}");

//        //    app.UseEndpoints(endpoints =>
//        //    {
//        //        endpoints.MapControllerRoute(
//        //name: "search",
//        //pattern: "",
//        //defaults: new { controller = "Search", action = "Index" });
//        //    });
//        app.Run();
//    }
//}
//public class Program
//{
//    public static void Main(string[] args)
//    {


//        var builder = WebApplication.CreateBuilder(args);

//        CreateHostBuilder(args).Build().Run();

//        // Add services to the container.
//        builder.Services.AddControllersWithViews();




//        builder.Services.AddDbContext<MagazinminContext>(options =>

//            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//    }

//    public static IHostBuilder CreateHostBuilder(string[] args)
//    {
//        return Host.CreateDefaultBuilder(args)
//                   .ConfigureWebHostDefaults(webBuilder =>
//                   {
//                       webBuilder.ConfigureServices((context, services) =>
//                       {
//                           services.AddControllersWithViews();
//                       });

//                       webBuilder.Configure(app =>
//                       {
//                           app.UseRouting();

//                           app.UseEndpoints(endpoints =>
//                           {
//                               endpoints.MapControllerRoute(
//                                    name: "default",
//                                    pattern: "{controller=Pokupatel}/{action=Index}"
//                                );

//                               endpoints.MapControllers();
//                           });
//                       });
//                   });
//    }
//}
//}