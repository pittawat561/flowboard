namespace FlowBoard.Domain.Common;

/// <summary>Raised when a domain rule (invariant) is broken.</summary>
public sealed class DomainException(string message) : Exception(message);
