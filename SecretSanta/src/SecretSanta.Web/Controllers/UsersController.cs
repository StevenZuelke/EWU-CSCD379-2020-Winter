using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        public UsersController(IHttpClientFactory clientFactory)
        {
            HttpClient httpClient = clientFactory?.CreateClient("SecretSantaApi") ?? throw new ArgumentNullException(nameof(clientFactory));
            Client = new UserClient(httpClient);
        }

        private UserClient Client { get; }

        //GET: User
        public async Task<ActionResult> Index()
        {
            ICollection<User> users = await Client.GetAllAsync();
            return View(users);
        }

        public async Task<ActionResult> GetId(int id)
        {
            User user = await Client.GetAsync(id);
            return View(user);
        }

        //CREATE: User
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserInput userInput)
        {
            var createdUser = await Client.PostAsync(userInput);

            return RedirectToAction(nameof(Index));
        }

        //UPDATE: User
        public async Task<ActionResult> Edit(int id)
        {

            var fetchedUser = await Client.GetAsync(id);

            return View(fetchedUser);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, UserInput userInput)
        {
            var updatedUser = await Client.PutAsync(id, userInput);

            return RedirectToAction(nameof(Index));
        }


        //DELETE: User

        public async Task<ActionResult> DeleteAsync(int id)
        {
            User user = await Client.GetAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(User user)
        {
            if (user != null)
            {
                await Client.DeleteAsync(user.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
