using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WebAuth.Models;

namespace WebAuth.Services
{
    public class UserService
    {
        private readonly string _db;
        public UserService(IWebHostEnvironment env)
        {
            _db = Path.Combine(env.ContentRootPath, "user.json");
        }

        public void MockUsers()
        {
            var users = new List<User>()
            {
                new User() { Username = "alice", Email = "alice@notmail.com", Role = "User"},
                new User() { Username = "bob", Email = "bob@notmail.com", Role = "User"},
                new User() { Username = "carl", Email = "carl@notmail.com", Role = "Admin"},
            };

            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(_db, json);
        }

        public User GetUser(string username)
        {
            var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_db));
            return users.FirstOrDefault(x => x.Username == username);
        }
    }
}
