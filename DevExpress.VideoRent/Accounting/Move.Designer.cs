//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
namespace DevExpress.VideoRent.videorent
{

    [Persistent(@"move")]
    public partial class Move : XPLiteObject
    {
        string fOid;
        [Key]
        [Size(38)]
        public string Oid
        {
            get { return fOid; }
            set { SetPropertyValue<string>("Oid", ref fOid, value); }
        }
        int fId;
        [Indexed(Name = @"id_UNIQUE", Unique = true)]
        [Persistent(@"id")]
        public int Id
        {
            get { return fId; }
            set { SetPropertyValue<int>("Id", ref fId, value); }
        }
        string fEmployee;
        [Indexed(Name = @"FK_Move_Employee")]
        [Size(38)]
        [Persistent(@"employee")]
        public string Employee
        {
            get { return fEmployee; }
            set { SetPropertyValue<string>("Employee", ref fEmployee, value); }
        }
        string fName;
        [Size(64)]
        [Persistent(@"name")]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>("Name", ref fName, value); }
        }
        string fReference;
        [Size(64)]
        [Persistent(@"reference")]
        public string Reference
        {
            get { return fReference; }
            set { SetPropertyValue<string>("Reference", ref fReference, value); }
        }
        DateTime fDataPosted;
        [Persistent(@"data_posted")]
        public DateTime DataPosted
        {
            get { return fDataPosted; }
            set { SetPropertyValue<DateTime>("DataPosted", ref fDataPosted, value); }
        }
        [Association(@"MoveLineReferencesMove", typeof(MoveLine))]
        public XPCollection<MoveLine> MoveLines { get { return GetCollection<MoveLine>("MoveLines"); } }
    }

}