using System.Reflection;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.WebApi.Filters;

namespace TrelloTenderManager.WebApi;

public class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<ICsvHelperWrapper, CsvHelperWrapper>();
        builder.Services.AddSingleton<ITenderCsvParser, TenderCsvParser>();
        builder.Services.AddSingleton<ITrelloDotNetWrapper, TrelloDotNetWrapper>();
        builder.Services.AddSingleton<IListOnBoardManager, ListOnBoardManager>();
        builder.Services.AddSingleton<ICustomFieldOnBoardManager, CustomFieldOnBoardManager>();
        builder.Services.AddSingleton<ICustomFieldManager, CustomFieldManager>();
        builder.Services.AddSingleton<IBoardManager, BoardManager>();
        builder.Services.AddSingleton<ICardManager, CardManager>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<HttpResponseExceptionFilter>();
        });

        builder.Services.AddRouting(routeOptions => routeOptions.LowercaseUrls = true);

        var repository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
        var fileInfo = new FileInfo("log4net.config");
        log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

        var app = builder.Build();

        app.Services.GetRequiredService<IBoardManager>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}