using DogeWallet.Wallet.Wallets.Doge;
using DogeWallet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeWallet.Web.Controllers
{
    public class WalletController : Controller
    {
        private readonly IConfiguration _config;
        public WalletController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Wallet()
        {
            var password = HttpContext.Session.GetString("Password");
            if (password == null) return RedirectToAction("Index", "Home");

            var dogeWallet = new DogeTestnetWallet(password, NBitcoin.ScriptPubKeyType.Legacy);

            var walletModel = new WalletModel()
            {
                Address = dogeWallet.GetAddress()
            };

            return View(walletModel);
        }

        [HttpPost]
        public async Task<IActionResult> Wallet(WalletModel model)
        {
            var password = HttpContext.Session.GetString("Password");
           if (password == null) return RedirectToAction("Index", "Home");

            var donations = new DonationModel();
            _config.Bind("Donations", donations);

            var dogeWallet = new DogeTestnetWallet(password, NBitcoin.ScriptPubKeyType.Legacy);
            var txResult = await dogeWallet.PushRawTxAsync(model.TransactionModel.ToAddress, model.TransactionModel.Amount,
                                                           model.TransactionModel.Fee, model.TransactionModel.IncludeDonation,
                                                           donations.DonationAddress,
                                                           donations.DonationAmount);
            model.TransactionModel.TransactionResult = txResult;
            return RedirectToAction("TxInfo","TxInfo",model.TransactionModel);
        }
    }
}
