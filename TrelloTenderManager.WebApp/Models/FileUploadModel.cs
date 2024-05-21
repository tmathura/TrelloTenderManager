using System.ComponentModel.DataAnnotations;

namespace TrelloTenderManager.WebApp.Models;

/// <summary>
/// Represents a file upload model.
/// </summary>
public class FileUploadModel
{
    /// <summary>
    /// Gets or sets the uploaded file.
    /// </summary>
    [Required(ErrorMessage = "Please select file")]
    public IFormFile UploadFile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the upload was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the success message.
    /// </summary>
    public string? SuccessMessage { get; set; }
}
