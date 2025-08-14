namespace Quiz.Shared.Exceptions;

public class CustomNotFoundException(string errorMessage) : Exception(errorMessage)
{ }