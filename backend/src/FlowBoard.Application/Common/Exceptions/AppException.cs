namespace FlowBoard.Application.Common.Exceptions;

/// <summary>
/// Base for expected application errors. The API layer maps each subtype to an HTTP status.
/// </summary>
public abstract class AppException(string message) : Exception(message);

public sealed class ConflictException(string message) : AppException(message);

public sealed class NotFoundException(string message) : AppException(message);

public sealed class InvalidCredentialsException() : AppException("Email or password is incorrect.");
