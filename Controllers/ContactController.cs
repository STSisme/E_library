using Microsoft.AspNetCore.Mvc;
using E_Library.Dtos;  // Make sure the ContactFormDto is in your Dtos folder
using System.Threading.Tasks;


namespace E_Library.Controllers
{

    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContactForm(ContactFormDto model)
        {
            if (ModelState.IsValid)
            {
                // Simulate sending an email or storing the contact form data in a database
                // This can be done by using an email service or saving the message to a database.

                TempData["Message"] = "Thank you for contacting us. We will get back to you soon!";
                return RedirectToAction("Contact"); // Redirect back to the contact page after submission
            }

            // If the form is invalid, redisplay the contact page with the errors
            return View("Contact", model);
        }
    }

}
