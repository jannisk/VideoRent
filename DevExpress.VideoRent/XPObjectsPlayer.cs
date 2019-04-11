using System;
using System.Drawing;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace DevExpress.VideoRent
{
    public class Player:Person, IComparable
    {
        private string _middleName;
        public string MiddleName
        {
            get { return _middleName; }
            set { SetPropertyValue<string>("MiddleName", ref _middleName, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetPropertyValue<string>("Email", ref _email, value); }
        }

        private string _comments;
        [Size(SizeAttribute.Unlimited)]
        public string Comments
        {
            get { return _comments; }
            set { SetPropertyValue<string>("Comments", ref _comments, value); }
        }

        Image _photo;
        private Customer _customer;

        [ValueConverter(typeof(ImageValueConverter))]
        public Image Photo
        {
            get { return _photo; }
            set { SetPropertyValue<Image>("Photo", ref _photo, value); }
        }

        public Player(Session session) : base(session)
        {
        }

        public Player(Session session, int selfId) : this(session)
        {
            SelfId = selfId;
        }

        public Player(Session session, int selfId, string firstName, string middleName, string lastName)
            : this(session, selfId) {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

       public Player(Session session, string firstName, string middleName, string lastName)
            : this(session) 
       {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public Player(Session session, Customer parent, string firstName, string middleName, string lastName, PersonGender personGender) : this(session, firstName, middleName, lastName)
        {
            Parent = parent;
            Gender = personGender;
        }

        [Association("Customer-Players")]
       public Customer Parent
       {
           get { return _customer; }
           set { SetPropertyValue<Customer>("Parent", ref _customer, value); }
       }

       public override void AfterConstruction()
       {
           base.AfterConstruction();
       }

    }
}
