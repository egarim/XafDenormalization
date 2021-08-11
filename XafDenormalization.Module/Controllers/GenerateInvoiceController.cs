using Bogus;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafDenormalization.Module.BusinessObjects;
using XafDenormalization.Module.BusinessObjects.Normalized;

namespace XafDenormalization.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GenerateInvoiceController : WindowController
    {
        SimpleAction GenerateData;
        public GenerateInvoiceController()
        {
            InitializeComponent();
            GenerateData = new SimpleAction(this, "GenerateData", "View");
            GenerateData.Execute += GenerateData_Execute;
            

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void GenerateData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var Os = this.Application.CreateObjectSpace();
            var Customers = Os.CreateCollection(typeof(Customer), null).Cast<Customer>().ToList();
            var Products = Os.CreateCollection(typeof(Product), null).Cast<Product>().ToList();
            //var PaymentTerms = Os.CreateCollection(typeof(PaymentTerms), null).Cast<PaymentTerms>().ToList();

            var CustomerFaker = new Faker<TempInvoice>()
                 .RuleFor(o => o.Customer, f => f.PickRandom<Customer>(Customers))
                 .RuleFor(o => o.Product, f => f.PickRandom<Product>(Products,30))
                 .RuleFor(o => o.Qty, f => f.Random.Int(0,500));

            List<TempInvoice> TempInvoices = CustomerFaker.Generate(10);
            var InvoiceDate = DateTime.Now;
            foreach (var CurrentInvoice in TempInvoices)
            {
                var NormalizedInvoice= Os.CreateObject<Invoice>();
                NormalizedInvoice.Date = InvoiceDate;
                NormalizedInvoice.Customer = CurrentInvoice.Customer;
              
                foreach (Product product in CurrentInvoice.Product)
                {
                    var Detail = Os.CreateObject<InvoiceDetail>();
                    Detail.Product = product;
                    Detail.Qty = CurrentInvoice.Qty;
                    NormalizedInvoice.InvoiceDetails.Add(Detail);
                }

            }
            Os.CommitChanges();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
     
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
