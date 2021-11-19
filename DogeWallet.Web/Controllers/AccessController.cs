using DogeWallet.Wallet.Wallets.Doge;
using DogeWallet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeWallet.Web.Controllers
{
    public class AccessController : Controller

    {
        public IActionResult Login()
        {
            var password = HttpContext.Session.GetString("Password");
            if (password != null) return RedirectToAction("Wallet","Wallet");
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserSecret model)
        {
            if (!ModelState.IsValid) return View(model);

            var password = model.Password + model.Pin;
                                                                       
            HttpContext.Session.SetString("Password", password);

            return RedirectToAction("Wallet", "Wallet");
        }
    }
}
