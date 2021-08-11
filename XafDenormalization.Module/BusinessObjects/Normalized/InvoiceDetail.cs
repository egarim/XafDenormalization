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

namespace XafDenormalization.Module.BusinessObjects.Normalized
{
   
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InvoiceDetail : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvoiceDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

      
        Invoice invoice;
        decimal total;
        decimal unitPrice;
        int qty;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        public int Qty
        {
            get => qty;
            set => SetPropertyValue(nameof(Qty), ref qty, value);
        }


        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }


        public decimal Total
        {
            get => total;
            set => SetPropertyValue(nameof(Total), ref total, value);
        }

        [Association("Invoice-InvoiceDetails")]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (this.IsLoading)
                return;


            if(propertyName==nameof(Product))
            {
                this.UnitPrice = this.Product.UnitPrice;
            }
            if ((propertyName == nameof(Product))|| (propertyName == nameof(Qty)) || (propertyName == nameof(UnitPrice)))
            {
                this.Total = this.UnitPrice * this.Qty;
            }
        }




    }
}