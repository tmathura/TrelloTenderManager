using log4net;
using Microsoft.AspNetCore.Mvc;
using System.IO.Abstractions;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.WebApp.Models;
using TrelloTenderManager.WebApp.Services;

namespace TrelloTenderManager.WebApp.Controllers;

/// <summary>
/// Controller for managing card operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CardController"/> class.
/// </remarks>
/// <param name="fileSystem">The file system.</param>
/// <param name="webAppClient">The web app client.</param>
public class CardController(IFileSystem fileSystem, IWebAppClient webAppClient) : Controller
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Displays the view for processing CSV files.
    /// </summary>
    /// <returns>The view for processing CSV files.</returns>
    public IActionResult ProcessCsv()
    {
        var model = new FileUploadModel();
        return View(model);
    }

    /// <summary>
    /// Processes the uploaded CSV file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <returns>The result of processing the CSV file.</returns>
    [HttpPost]
    public async Task<IActionResult> ProcessCsv(FileUploadModel model)
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
            ModelState.AddModelError("error", exception.Message);

            _logger.Error($"{exception.Message} - {exception.StackTrace}");
        }

        return View("ProcessCsv", model);
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
            throw new Exception("Cannot process a non csv file.");
        }
    }

    /// <summary>
    /// Reads the lines of the uploaded file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <returns>The lines of the uploaded file.</returns>
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
    /// Processes the data from the uploaded file.
    /// </summary>
    /// <param name="model">The file upload model.</param>
    /// <param name="filename">The name of the uploaded file.</param>
    /// <param name="fileLines">The lines of the uploaded file.</param>
    private async Task ProcessFileData(FileUploadModel model, string filename, List<string?> fileLines)
    {
        var request = new ProcessFromCsvRequest { FileContent = fileLines.ToArray() };
        var response = await webAppClient.CardService.ProcessCsv(request);

        //if (!string.IsNullOrWhiteSpace(response.AccumulatedData.InvalidAccumulatedData))
        //{
        //    ModelState.AddModelError("error", response.AccumulatedData.InvalidAccumulatedData);
        //}

        model.Success = true;
        model.SuccessMessage = $"The csv file '{filename}' was processed successfully.";
    }
}
