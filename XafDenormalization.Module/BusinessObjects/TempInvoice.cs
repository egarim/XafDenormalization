using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafDenormalization.Module.BusinessObjects
{
    public class TempInvoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }

       
        public IEnumerable<Product> Product { get; set; }
        public int Qty { get; set; }


    }
}
