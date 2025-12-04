using Microsoft.AspNetCore.Http;

namespace MyPass.Core.Session
{
    public class WebSessionManager
    {
        private readonly IHttpContextAccessor _http;

        private const string KEY = "CurrentUserId";

        public WebSessionManager(IHttpContextAccessor http)
        {
            _http = http;
        }

        public int? CurrentUserId
        {
            get => _http.HttpContext!.Session.GetInt32(KEY);
        }

        public bool IsAuthenticated => CurrentUserId != null;

        public void SetCurrentUser(int id)
        {
            _http.HttpContext!.Session.SetInt32(KEY, id);
        }

        public void Clear()
        {
            _http.HttpContext!.Session.Remove(KEY);
        }
    }
}


