using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

namespace DevExpress.VideoRent
{
   public enum AccountTypeEnum

    {
        other, Receivable, Payable, Liquidity
    };


    public class Account : VideoRentBaseObject
    {
        double balance;
        int Code;
        AccountTypeEnum type;
        string Name;
        Customer customer;

        /// <summary>
        /// 
        /// </summary>
        public string AccountName
        {
            get { return GetPropertyValue<string>("Name"); }
            set
            {
                SetPropertyValue<string>("Name", value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountTypeEnum AccountType
        {
            get { return GetPropertyValue<AccountTypeEnum>("type"); }
            set
            {
                SetPropertyValue<AccountTypeEnum>("type", value);
            }
        }

        public int AccountCode
        {
            get { return GetPropertyValue<int>("Code"); }
            set { SetPropertyValue<int>("Code", value); }
        }

        public double AccountBalance
        {
            get { return GetPropertyValue<double>("Balance"); }
            set { SetPropertyValue<double>("Balance", value); }
        }


        [Association("Customers-Account")]
        public Customer Customer
        {
            get { return customer; }
            set { SetPropertyValue<Customer>("Customer", ref customer, value); }
        }

        public Account(Session session) : base(session)
        {
           

        }
       


    }
}
