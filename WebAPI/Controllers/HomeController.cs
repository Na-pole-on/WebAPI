using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Net.Http.Json;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        static List<Person> users = new List<Person>
        {
            new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
            new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
            new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
        };

        public IActionResult Index() => View();

        [HttpGet("/api/users")]
        public IActionResult Users() => Json(users);

        [HttpGet("/api/users/{id}")]
        public IActionResult GetById(string id)
        {
            Person? user = users
                .FirstOrDefault(u => u.Id == id);

            if (user == null) 
                return NotFound(new { message = "Пользователь не найден" });

            return Json(user);
        }

        [HttpDelete("/api/users/{id}")]
        public IActionResult DeleteById(string id)
        {
            Person? user = users
                .FirstOrDefault(u => u.Id == id);

            if (user == null) 
                return NotFound(new { message = "Пользователь не найден" });

            users.Remove(user);
            return Json(user);
        }

        [HttpPost("/api/users")]
        public async Task<IActionResult> AddUser()
        {
            Person? user = await Request.ReadFromJsonAsync<Person>();

            user.Id = Guid.NewGuid().ToString();
            users.Add(user);
            return Json(user);
        }

        [HttpPut("/api/users")]
        public async Task<IActionResult> UpdateUser()
        {
            Person? userData = await Request
                .ReadFromJsonAsync<Person>();

            var user = users
                .FirstOrDefault(u => u.Id == userData.Id);
            
            if (user == null) 
                return NotFound(new { message = "Пользователь не найден" });

            user.Age = userData.Age;
            user.Name = userData.Name;
            return Json(user);
        }
    }
}
