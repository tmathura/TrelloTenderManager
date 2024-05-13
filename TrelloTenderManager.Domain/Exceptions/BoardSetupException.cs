namespace TrelloTenderManager.Domain.Exceptions;

public class BoardSetupException(string message, Exception inner) : Exception(message, inner);