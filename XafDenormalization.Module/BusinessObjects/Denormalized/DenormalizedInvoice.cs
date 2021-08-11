using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XafDenormalization.Module.BusinessObjects.Normalized;

namespace XafDenormalization.Module.BusinessObjects.Denormalized
{
    [DefaultClassOptions]
    [NavigationItem("Normalized")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DenormalizedInvoice : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DenormalizedInvoice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        PaymentTerms paymentTerms;
        Customer customer;
        DateTime date;

        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }
        


        public PaymentTerms PaymentTerms
        {
            get => paymentTerms;
            set => SetPropertyValue(nameof(PaymentTerms), ref paymentTerms, value);
        }
        [Association("DenormalizedInvoice-DenormalizedInvoiceInvoiceDetail")]
        public XPCollection<DenormalizedInvoiceInvoiceDetail> InvoiceDetails
        {
            get
            {
                return GetCollection<DenormalizedInvoiceInvoiceDetail>(nameof(InvoiceDetails));
            }
        }
    }
}