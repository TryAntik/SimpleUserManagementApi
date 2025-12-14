namespace SimpleUserManagementApi.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string exc) : base(exc) { }
}