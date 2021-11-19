using DogeWallet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeWallet.Web.Controllers
{
    public class TxInfoController : Controller
    {
        public IActionResult TxInfo(TransactionModel model)
        {
            var password = HttpContext.Session.GetString("Password");
            if (password == null) return RedirectToAction("Index", "Home");
            return View(model);
        }
    }
}
