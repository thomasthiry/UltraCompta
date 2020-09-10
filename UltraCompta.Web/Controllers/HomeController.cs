﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UltraCompta.Business;
using UltraCompta.Web.Models;

namespace UltraCompta.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CreateOrderCommand _createOrderCommand;

        public HomeController(IOrderSource orderSource, ICustomerSource customerSource, IInvoiceStorage invoiceStorage)
        {
            _createOrderCommand = new CreateOrderCommand(orderSource, customerSource, invoiceStorage);
        }

        public IActionResult Index()
        {
            IEnumerable<string> files = Directory.EnumerateFiles("C:/Dev/UltraCompta/InputFiles/");
            return View(files.Select(f => f.Replace("C:/Dev/UltraCompta/InputFiles/", "").Replace(".txt", "")));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GenerateInvoice(string orderReference)
        {
            var invoice = _createOrderCommand.Generate(orderReference);

            return Content(invoice, "text/html");
        }
    }
}
