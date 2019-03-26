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
        double _balance;
        int _code;
        AccountTypeEnum _type;
        string _name;
        Customer _customer;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return GetPropertyValue<string>("Name"); }
            set
            {
                SetPropertyValue<string>("Name", ref _name,  value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountTypeEnum Type
        {
            get { return GetPropertyValue<AccountTypeEnum>("Type"); }
            set
            {
                SetPropertyValue<AccountTypeEnum>("Type", ref _type, value);
            }
        }

        public int Code
        {
            get { return GetPropertyValue<int>("Code"); }
            set { SetPropertyValue<int>("Code", ref _code, value); }
        }

        public double Balance
        {
            get { return GetPropertyValue<double>("Balance"); }
            set { SetPropertyValue<double>("Balance", ref _balance, value); }
        }


        [Persistent, Association("Customer-Accounts")]
        public Customer Customer
        {
            get { return _customer; }
            set { SetPropertyValue<Customer>("Customer", ref _customer, value); }
        }

        public Account(Session session) : base(session)
        {
           

        }
       


    }
}
