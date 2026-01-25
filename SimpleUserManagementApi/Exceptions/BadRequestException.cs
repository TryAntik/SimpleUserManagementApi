namespace SimpleUserManagementApi.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string exc) : base(exc) { }
}