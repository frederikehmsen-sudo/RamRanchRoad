namespace Api;

public interface ICurrentUser
{
    int Id { get; }
}

public class HeaderCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private const int DefaultUserId = 1;

    public int Id =>
        int.TryParse(accessor.HttpContext?.Request.Headers["X-User-Id"], out var id)
            ? id
            : DefaultUserId;
}