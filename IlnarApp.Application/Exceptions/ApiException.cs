namespace IlnarApp.Application.Exceptions;


public class ApiException(string? message) : Exception(message);