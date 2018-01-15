namespace SentryDotNet
{
    public interface IUserSentryContext
    {
        string Id { get; set; }

        string Email { get; set; }

        string IpAddress { get; set; }

        string Username { get; set; }
    }
}