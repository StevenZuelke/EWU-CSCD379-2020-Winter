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
            ActionResult result = View(userInput);
            if (ModelState.IsValid)
            {
                var createdUser = await Client.PostAsync(userInput);

                result = RedirectToAction(nameof(Index));
            }
            return result;
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
            ActionResult result = View(userInput);
            if (ModelState.IsValid)
            {
                var updatedUser = await Client.PutAsync(id, userInput);

                result = RedirectToAction(nameof(Index));
            }
            return result;
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
            ActionResult result = View(user);
            try
            {
                if (user != null)
                {
                    if (await Client.GetAsync(user.Id) != null)
                    {
                        await Client.DeleteAsync(user.Id);
                        result = RedirectToAction(nameof(Index));
                        ModelState.AddModelError("Id", "Not Correct Id");
                    }
                }

            }
            catch (NullReferenceException e) { Console.WriteLine(e.Message); }
            return result;
        }
    }
}
