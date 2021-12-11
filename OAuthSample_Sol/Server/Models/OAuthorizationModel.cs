namespace Server.Models
{
    public class OAuthorizationModel
    {
        public string Name { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
    }
}
