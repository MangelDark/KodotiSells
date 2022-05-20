using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Product Product { get; set; }
        public Invoice Invoice { get; set; }    
        public int InvouceId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Iva { get; set; }
        public decimal SubTotal { get; set; }
        public decimal  Total { get; set; }



    }
}
