using System.IO.Abstractions;
using System.Reflection;
using TrelloTenderManager.WebApp.Services;
using TrelloTenderManager.WebApp.Services.Wrappers.Implementations;
using TrelloTenderManager.WebApp.Services.Wrappers.Interfaces;

namespace TrelloTenderManager.WebApp;

/// <summary>
/// The entry point class for the web application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method that is executed when the application starts.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddSingleton<IRestClientWrapper, RestClientWrapper>();
        builder.Services.AddSingleton<IWebAppClient, WebAppClient>();
        builder.Services.AddSingleton<IFileSystem, FileSystem>();

        var repository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
        var fileInfo = new FileInfo("log4net.config");
        log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
