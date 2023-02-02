namespace webapi.Services;

public interface IUserService
{
    bool UserExists(string name);
}

public class UserService : IUserService
{
    public bool UserExists(string name)
    {
        return true;
    }
}