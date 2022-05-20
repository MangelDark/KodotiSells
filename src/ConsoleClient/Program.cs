using Models;
using Service;
using System;
using System.Collections.Generic;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            var orderService  =  new InvoiceService();
            //var result = orderService.Get(1);
            var invoice = new Invoice
            {
                Id = 16,
                ClientId = 1,
                Detail = new List<InvoiceDetail>
                {
                    new InvoiceDetail
                    {
                        ProductoId = 1,
                        Quantity = 5,
                        Price = 15000
                    },
                    new InvoiceDetail
                    {
                        ProductoId = 8,
                        Quantity = 15,
                        Price = 12005
                    }
                }

            };

            //orderService.Create(invoice);
            orderService.Update(invoice);
            Console.ReadLine();
          


            
        }
    }
}
