using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafDenormalization.Module.BusinessObjects
{
    public class TempInvoice
    {
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }

        public PaymentTerms PaymentTerms { get; set; }
        public Product Product { get; set; }


    }
}
