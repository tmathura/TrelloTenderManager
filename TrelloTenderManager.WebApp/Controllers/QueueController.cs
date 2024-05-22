using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Abstractions;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.WebApp.Models;
using TrelloTenderManager.WebApp.Services;

namespace TrelloTenderManager.WebApp.Controllers;

/// <summary>
/// Controller for managing the queue of CSV files.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="QueueController"/> class.
/// </remarks>
/// <param name="fileSystem">The file system.</param>
/// <param name="webAppClient">The web app client.</param>
public class QueueController(IFileSystem fileSystem, IWebAppClient webAppClient) : Controller
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Displays the view for uploading a CSV file to the queue.
    /// </summary>
    /// <returns>The view for uploading a CSV file.</returns>
    public IActionResult QueueCsv()
    {
        var model = new FileUploadModel();
        return View(model);
    }

    /// <summary>
    /// Handles the HTTP POST request for queuing a CSV file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <returns>The view for uploading a CSV file.</returns>
    [HttpPost]
    public async Task<IActionResult> QueueCsv(FileUploadModel model)
    {
        model.Success = false;

        try
        {
            var fileLines = new List<string?>();
            if (ModelState.IsValid)
            {
                IsFileCsv(model.UploadFile.FileName);

                fileLines = await ReadFileLines(model);
            }

            if (fileLines.Count > 0)
            {
                await ProcessFileData(model, model.UploadFile.FileName, fileLines);
            }
        }
        catch (Exception exception)
        {
            ModelState.AddModelError("post-error", exception.Message);

            _logger.Error($"{exception.Message} - {exception.StackTrace}");
        }

        return View("QueueCsv", model);
    }

    /// <summary>
    /// Displays the view for showing the queue.
    /// </summary>
    /// <returns>The view for showing the queue.</returns>
    public async Task<IActionResult> ShowQueue()
    {
        var model = new ShowQueueModel();

        try
        {
            await GetQueueData(model);
        }
        catch (Exception exception)
        {
            ModelState.AddModelError("post-error", exception.Message);

            _logger.Error($"{exception.Message} - {exception.StackTrace}");
        }

        return View(model);
    }

    /// <summary>
    /// Handles the HTTP GET request for displaying an error.
    /// </summary>
    /// <returns>The view for displaying an error.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Checks if the file is a CSV file.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    private void IsFileCsv(string fileName)
    {
        var fileInfo = fileSystem.FileInfo.New(fileName);

        if (fileInfo.Extension.ToLower() != ".csv")
        {
            throw new Exception("Cannot queue a non csv file.");
        }
    }

    /// <summary>
    /// Reads the lines of the uploaded CSV file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <returns>A list of file lines.</returns>
    private static async Task<List<string?>> ReadFileLines(FileUploadModel model)
    {
        var fileLines = new List<string?>();
        await using var stream = model.UploadFile.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync() is { } line)
        {
            fileLines.Add(line);
        }

        return fileLines;
    }

    /// <summary>
    /// Processes the data of the uploaded CSV file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <param name="filename">The name of the file.</param>
    /// <param name="fileLines">The lines of the file.</param>
    private async Task ProcessFileData(FileUploadModel model, string filename, List<string?> fileLines)
    {
        var fileContent = string.Join(Environment.NewLine, fileLines);
        var request = new QueueFromCsvRequest { Filename = filename, FileContent = fileContent };
        await webAppClient.QueueService.QueueCsv(request);

        model.Success = true;
        model.SuccessMessage = $"The csv file '{filename}' was queued successfully.";
    }

    /// <summary>
    /// Gets the data of the queue.
    /// </summary>
    /// <param name="model">The show queue model.</param>
    private async Task GetQueueData(ShowQueueModel model)
    {
        var csvQueueItems = await webAppClient.QueueService.GetQueue();

        model.CsvQueueItems = csvQueueItems;
    }
}
