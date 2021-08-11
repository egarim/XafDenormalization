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

namespace XafDenormalization.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("FullAddress")]
    public class AddressBase : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AddressBase(Session session)
            : base(session)
        {
        }
        public AddressBase(Session session, string City, string Street, string Number, string State, string ZipCode)
           : base(session)
        {
            this.FullAddress = $"{City}{Environment.NewLine}{Street}{Environment.NewLine}{Number}{Environment.NewLine}{State}{Environment.NewLine}{ZipCode}";
        }
        public override void AfterConstruction()
        {

            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string fullAddress;

        [Size(SizeAttribute.Unlimited)]
        public string FullAddress
        {
            get => fullAddress;
            set => SetPropertyValue(nameof(FullAddress), ref fullAddress, value);
        }
    }
    //[DefaultClassOptions]
    //[DefaultProperty("FullAddress")]
    //public class ShippingAddress : AddressBase
    //{ // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
    //    public ShippingAddress(Session session)
    //        : base(session)
    //    {
    //    }

    //    public ShippingAddress(Session session, string City, string Street, string Number, string State, string ZipCode) : base(session, City, Street, Number, State, ZipCode)
    //    {

    //    }
    //}
    //[DefaultClassOptions]
    //[DefaultProperty("FullAddress")]
    //public class BillingAddress : AddressBase
    //{ // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
    //    public BillingAddress(Session session)
    //        : base(session)
    //    {
    //    }

    //    public BillingAddress(Session session, string City, string Street, string Number, string State, string ZipCode) : base(session, City, Street, Number, State, ZipCode)
    //    {

    //    }
    //}
}