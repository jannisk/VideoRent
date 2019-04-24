using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace DevExpress.VideoRent
{
    public class Move:VideoRentBaseObject
    {
        protected override string GeneratedIdType { get { return "Move"; } }

        public Move(Session session) : base(session)
        {
            _datePosted = DateTime.Now;
        }

        public Move(Session session, int selfId) : this(session)
        {
            SelfId = selfId;
        }

        public override void AfterConstruction() { base.AfterConstruction(); }


        [Persistent, Indexed(Unique = true)]
        public int MoveId
        {
            get { return GetPropertyValue<int>("MoveId"); }
            set { SetPropertyValue<int>("MoveId", value); }
        }

        string _employee;
        [Persistent(@"employee")]
        public string Employee
        {
            get { return _employee; }
            set { SetPropertyValue<string>("Employee", ref _employee, value); }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue<string>("Name", ref _name, value); }
        }

        string _reference;
        public string Reference
        {
            get { return _reference; }
            set { SetPropertyValue<string>("Reference", ref _reference, value); }
        }

        DateTime _datePosted;
        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { SetPropertyValue<DateTime>("DatePosted", ref _datePosted, value); }
        }

        [Association(@"MoveLineReferencesMove", typeof(Moveline))]
        public XPCollection<Moveline> MoveLines { get { return GetCollection<Moveline>("MoveLines"); } }

        protected override void OnSavingOverride()
        {
            base.OnSavingOverride();
            Name = "Move" + MoveId;
        }
    }
}
