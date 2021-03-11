using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQTask_TopicExhange_Web.RabbitMQ;

namespace RabbitMQTask_TopicExhange_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private List<string> messages = new List<string>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var name = Request.Form["name"];
            var email = Request.Form["email"];
            var bookOrCancel = Request.Form["bookorcancel"];

            List<string> message = new List<string>();

            message.Add(bookOrCancel);
            message.Add($"Name: {name}, Email: {email}");

            Publish.PublishMessage(message.ToArray());

        }


    }
}
