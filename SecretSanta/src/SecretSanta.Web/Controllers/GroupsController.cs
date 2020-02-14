﻿using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        public GroupsController(IHttpClientFactory clientFactory)
        {
            HttpClient httpClient = clientFactory?.CreateClient("SecretSantaApi") ?? throw new ArgumentNullException(nameof(clientFactory));
            Client = new GroupClient(httpClient);
        }

        private GroupClient Client { get; }

        //GET: Group
        public async Task<ActionResult> Index()
        {
            ICollection<Group> groups = await Client.GetAllAsync();
            return View(groups);
        }

        public async Task<ActionResult> GetId(int id)
        {
            Group group = await Client.GetAsync(id);
            return View(group);
        }

        //CREATE: Group
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GroupInput groupInput)
        {
            ActionResult result = View(groupInput);
            if (ModelState.IsValid)
            {
                var createdGroup = await Client.PostAsync(groupInput);

                result = RedirectToAction(nameof(Index));
            }
            return result;
        }

        //UPDATE: Group
        public async Task<ActionResult> Edit(int id)
        {

            var fetchedGroup = await Client.GetAsync(id);

            return View(fetchedGroup);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, GroupInput groupInput)
        {
            ActionResult result = View(groupInput);
            if (ModelState.IsValid)
            {
                var updatedGroup = await Client.PutAsync(id, groupInput);

                result = RedirectToAction(nameof(Index));
            }
            return result;
        }


        //DELETE: Group

        public async Task<ActionResult> DeleteAsync(int id)
        {
            Group group = await Client.GetAsync(id);
            return View(group);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Group group)
        {
            ActionResult result = View(group);
            try
            {
                if (group != null)
                {
                    if (await Client.GetAsync(group.Id) != null)
                    {
                        await Client.DeleteAsync(group.Id);
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
