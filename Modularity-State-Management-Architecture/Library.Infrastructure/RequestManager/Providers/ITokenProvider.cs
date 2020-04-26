namespace StateBuilder.RequestManager.Providers
{
    public interface ITokenProvider
    {
        IToken GetAuthenticationToken();
        IToken GetIafToken();
    }
}
