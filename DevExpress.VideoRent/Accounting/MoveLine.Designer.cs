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

    [Persistent(@"move_line")]
    public partial class MoveLine : XPLiteObject
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
        Move fMoveId;
        [Size(38)]
        [Persistent(@"move_id")]
        [Association(@"MoveLineReferencesMove")]
        public Move MoveId
        {
            get { return fMoveId; }
            set { SetPropertyValue<Move>("MoveId", ref fMoveId, value); }
        }
        string fAccountId;
        [Indexed(Name = @"FK_Move_Line_Account_idx")]
        [Size(38)]
        [Persistent(@"account_id")]
        public string AccountId
        {
            get { return fAccountId; }
            set { SetPropertyValue<string>("AccountId", ref fAccountId, value); }
        }
        DateTime fDataPosted;
        [Persistent(@"data_posted")]
        public DateTime DataPosted
        {
            get { return fDataPosted; }
            set { SetPropertyValue<DateTime>("DataPosted", ref fDataPosted, value); }
        }
        bool fReconciled;
        [Persistent(@"reconciled")]
        public bool Reconciled
        {
            get { return fReconciled; }
            set { SetPropertyValue<bool>("Reconciled", ref fReconciled, value); }
        }
        Journal fJournal;
        [Size(38)]
        [Persistent(@"journal")]
        [Association(@"MoveLineReferencesJournal")]
        public Journal Journal
        {
            get { return fJournal; }
            set { SetPropertyValue<Journal>("Journal", ref fJournal, value); }
        }
    }

}
