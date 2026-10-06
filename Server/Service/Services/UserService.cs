using Infrastructure;
using Service.DTOs;

namespace Service.Services;

public class UserService(RamRanchDatabase db)
{
    public UserResponse GetById(int id)
    {
        var u = db.Users.Single(x => x.Id == id);
        return new UserResponse
        {
            UserId = u.Id,
            UserName = u.Username
        };
    }
}   