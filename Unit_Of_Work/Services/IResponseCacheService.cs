namespace Unit_Of_Work.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheReponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCacheResponseAsync(string cacheKey);
        Task RemoveCacheResponseAsync(string pattern);
    }
}
