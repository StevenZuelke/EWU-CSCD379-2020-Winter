using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Web.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class GiftsController : Controller
    {
        public GiftsController(IHttpClientFactory clientFactory)
        {
            HttpClient httpClient = clientFactory?.CreateClient("SecretSantaApi") ?? throw new ArgumentNullException(nameof(clientFactory));
            Client = new GiftClient(httpClient);
        }

        private GiftClient Client { get; }

        //GET: Gift
        public async Task<ActionResult> Index()
        {
            ICollection<Gift> gifts = await Client.GetAllAsync();
            return View(gifts);
        }

        public async Task<ActionResult> GetId(int id)
        {
            Gift gift = await Client.GetAsync(id);
            return View(gift);
        }

        //CREATE: Gift
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GiftInput giftInput)
        {
            ActionResult result = View(giftInput);

            if (ModelState.IsValid)
            {
                var createdGift = await Client.PostAsync(giftInput);
                result = RedirectToAction(nameof(Index));
            }

            return result;
        }

        //UPDATE: Gift
        public async Task<ActionResult> Edit(int id)
        {

            var fetchedGift = await Client.GetAsync(id);

            return View(fetchedGift);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(int id, GiftInput giftInput)
        {
            ActionResult result = View(giftInput);
            if (ModelState.IsValid)
            {
                var updatedGift = await Client.PutAsync(id, giftInput);

                result = RedirectToAction(nameof(Index));
            }
            return result;
        }

        
        //DELETE: Gift
        
        public async Task<ActionResult> DeleteAsync(int id)
        {
            Gift gift = await Client.GetAsync(id);
            return View(gift);
        }
        
        [HttpPost]
        public async Task<ActionResult> Delete(Gift gift)
        {
            ActionResult result = View(gift);
            try
            {
                if(gift != null)
                {
                    if (await Client.GetAsync(gift.Id) != null)
                    {
                        await Client.DeleteAsync(gift.Id);
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
