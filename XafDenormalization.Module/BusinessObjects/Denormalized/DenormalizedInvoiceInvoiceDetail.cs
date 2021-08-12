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
   
    [VisibleInReports()]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DenormalizedInvoiceInvoiceDetail : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DenormalizedInvoiceInvoiceDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        string invoiceNumber;
        DateTime invoiceDate;
        int paymentTermsDays;
        string paymentTermsName;
        string paymentTermsCode;
        decimal productUnitPrice;
        string productDescription;
        string productCode;
        string productName;
        DenormalizedInvoice invoice;
        decimal total;
        decimal unitPrice;
        int qty;
        string customerBillingAddress;
        string customerShippingAddress;
        string customerCode;
        string customerName;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
        }

        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
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

        [Association("DenormalizedInvoice-DenormalizedInvoiceInvoiceDetail")]
        public DenormalizedInvoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CustomerName
        {
            get => customerName;
            set => SetPropertyValue(nameof(CustomerName), ref customerName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CustomerCode
        {
            get => customerCode;
            set => SetPropertyValue(nameof(CustomerCode), ref customerCode, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string CustomerShippingAddress
        {
            get => customerShippingAddress;
            set => SetPropertyValue(nameof(CustomerShippingAddress), ref customerShippingAddress, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string CustomerBillingAddress
        {
            get => customerBillingAddress;
            set => SetPropertyValue(nameof(CustomerBillingAddress), ref customerBillingAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductName
        {
            get => productName;
            set => SetPropertyValue(nameof(ProductName), ref productName, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductCode
        {
            get => productCode;
            set => SetPropertyValue(nameof(ProductCode), ref productCode, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string ProductDescription
        {
            get => productDescription;
            set => SetPropertyValue(nameof(ProductDescription), ref productDescription, value);
        }


        public decimal ProductUnitPrice
        {
            get => productUnitPrice;
            set => SetPropertyValue(nameof(ProductUnitPrice), ref productUnitPrice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PaymentTermsCode
        {
            get => paymentTermsCode;
            set => SetPropertyValue(nameof(PaymentTermsCode), ref paymentTermsCode, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PaymentTermsName
        {
            get => paymentTermsName;
            set => SetPropertyValue(nameof(PaymentTermsName), ref paymentTermsName, value);
        }

        
        public int PaymentTermsDays
        {
            get => paymentTermsDays;
            set => SetPropertyValue(nameof(PaymentTermsDays), ref paymentTermsDays, value);
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (this.IsLoading)
                return;
        
            if (propertyName == nameof(Invoice))
            {
                if (Invoice == null)
                    return;
                this.InvoiceDate = Invoice.Date;
                this.CustomerCode = this.Invoice.Customer.Code;
                this.CustomerName = this.Invoice.Customer.Name;
                this.CustomerCode = this.Invoice.Customer.Code;
                this.CustomerShippingAddress = this.Invoice.Customer.ShippingAddress.FullAddress;
                this.CustomerBillingAddress = this.Invoice.Customer.BillingAddress.FullAddress;
                this.PaymentTermsCode = this.Invoice.Customer.PaymentTerms.Code;
                this.PaymentTermsName = this.Invoice.Customer.PaymentTerms.Name;
                this.paymentTermsDays = this.Invoice.Customer.PaymentTerms.Days;
            }

            if (propertyName == nameof(Product))
            {
                if (Product == null)
                    return;
                this.UnitPrice = this.Product.UnitPrice;
                this.ProductCode = this.Product.Code;
                this.ProductName = this.Product.Name;
                this.ProductDescription = this.Product.Description;
                this.ProductUnitPrice = this.Product.UnitPrice;
            }
            if (propertyName == nameof(Product))
            {
                if (Product == null)
                    return;
                this.UnitPrice = this.Product.UnitPrice;
                this.ProductCode = this.Product.Code;
                this.ProductName = this.Product.Name;
                this.ProductDescription = this.Product.Description;
                this.ProductUnitPrice = this.Product.UnitPrice;
            }
            if ((propertyName == nameof(Product)) || (propertyName == nameof(Qty)) || (propertyName == nameof(UnitPrice)))
            {
                this.Total = this.UnitPrice * this.Qty;
            }
        }




    }
}