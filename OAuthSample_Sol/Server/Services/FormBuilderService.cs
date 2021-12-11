using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class FormBuilderService
    {
        private readonly string _formPath;
        public FormBuilderService(IWebHostEnvironment env)
        {
            _formPath = Path.Combine(env.WebRootPath, "oauthForm.html");
        }

        public async Task<string> BuildForm(string redirectUri, string state, string code)
        {
            var formTemplate = await File.ReadAllTextAsync(_formPath);
            
            var form = formTemplate
                .Replace("{_redirectUri}", redirectUri)
                .Replace("{_state}", state)
                .Replace("{_code}", code);

            return form;
        }
    }
}
