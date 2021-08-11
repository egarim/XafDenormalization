using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using XafDenormalization.Module.BusinessObjects;
using Bogus;
using System.Collections.Generic;

namespace XafDenormalization.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FirstOrDefault<DomainObject1>(u => u.Name == name);
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            List<PaymentTerms> paymentTerms = new List<PaymentTerms>();
            if (ObjectSpace.GetObjectsCount(typeof(PaymentTerms), null) == 0)
            {
                PaymentTerms Cash = ObjectSpace.CreateObject<PaymentTerms>();
                Cash.Code = "001";
                Cash.Name = "Cash";

                PaymentTerms Days30 = ObjectSpace.CreateObject<PaymentTerms>();
                Days30.Code = "002";
                Days30.Name = "30 days";

                PaymentTerms Days60 = ObjectSpace.CreateObject<PaymentTerms>();
                Days60.Code = "003";
                Days60.Name = "60 days";

                PaymentTerms Days90 = ObjectSpace.CreateObject<PaymentTerms>();
                Days90.Code = "004";
                Days90.Name = "90 days";
                paymentTerms.Add(Cash);
                paymentTerms.Add(Days30);
                paymentTerms.Add(Days60);
                paymentTerms.Add(Days90);
            }

            var ProductCode = 0;
            var CustomerIds = 0;
            var Session = ((XPObjectSpace)this.ObjectSpace).Session;
            if (ObjectSpace.GetObjectsCount(typeof(Customer), null) == 0)
            {
                var CustomerFaker = new Faker<Customer>()
                    .CustomInstantiator(c => this.ObjectSpace.CreateObject<Customer>())
                    .RuleFor(o => o.Name, f => f.Person.FullName)
                       .RuleFor(o => o.PaymentTerms, f => f.PickRandom<PaymentTerms>(paymentTerms))
                    .RuleFor(o => o.Code, f => (CustomerIds++).ToString("D8"))
                    .RuleFor(o => o.ShippingAddress, f => new AddressBase(Session, f.Person.Address.City, f.Person.Address.Street, f.Person.Address.Suite, f.Person.Address.State, f.Person.Address.ZipCode))
                    .RuleFor(o => o.BillingAddress, f => new AddressBase(Session, f.Person.Address.City, f.Person.Address.Street, f.Person.Address.Suite, f.Person.Address.State, f.Person.Address.ZipCode));
                //.RuleFor(o => o.ShippingAddress, f => $"{f.Person.Address.City}{System.Environment.NewLine}{f.Person.Address.Street}{System.Environment.NewLine}{f.Person.Address.Suite}{System.Environment.NewLine}{f.Person.Address.State}{System.Environment.NewLine}{f.Person.Address.ZipCode}")
                //.RuleFor(o => o.BillingAddress, f => $"{f.Person.Address.City}{System.Environment.NewLine}{f.Person.Address.Street}{System.Environment.NewLine}{f.Person.Address.Suite}{System.Environment.NewLine}{f.Person.Address.State}{System.Environment.NewLine}{f.Person.Address.ZipCode}");
                var Customers = CustomerFaker.Generate(100);
            }
            if (ObjectSpace.GetObjectsCount(typeof(Product), null) == 0)
            {
                var CustomerFaker = new Faker<Product>()
                    .CustomInstantiator(c => this.ObjectSpace.CreateObject<Product>())
                    .RuleFor(o => o.Name, f => f.Commerce.Product())
                     .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
                     .RuleFor(o => o.UnitPrice, f => Decimal.Parse(f.Commerce.Price(1,100,2,"")))
                    .RuleFor(o => o.Code, f => (ProductCode++).ToString("D8"));
                var Customers = CustomerFaker.Generate(100);
            }


            ApplicationUser sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
                sampleUser.UserName = "User";
                // Set a password if the standard authentication type is used
                sampleUser.SetPassword("");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
            }
            // If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }
    }
}
