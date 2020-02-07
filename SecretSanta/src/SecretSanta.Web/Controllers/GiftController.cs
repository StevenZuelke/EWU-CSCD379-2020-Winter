using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Controllers
{
    public class GiftController : Controller
    {
       
        IHttpClientFactory ClientFactory { get; set; }
        public GiftController(IHttpClientFactory clientFactory)
        {

            if (clientFactory is null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }
            ClientFactory = clientFactory;
        }
        //Get: Gift
        public async Task<IActionResult> Index()
        {
            HttpClient client = ClientFactory.CreateClient("SecretSantaApi");
            var giftClient = new GiftClient(client);
            var gifts = await giftClient.GetAllAsync();
            return View(gifts);
        }
        
    }

}