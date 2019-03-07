using System;
using System.Data;
using DevExpress.Xpo.DB;

namespace DevExpress.VideoRent.Helpers
{
    public class MyMySqlConnectionProvider : MySqlConnectionProvider
    {
        public MyMySqlConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption)
        {
        }
        protected override object ConvertToDbParameter(object clientValue, TypeCode clientValueTypeCode)
        {
            if (clientValueTypeCode == TypeCode.Object && clientValue is Guid)
            {
                var guid_s =  ((Guid) clientValue).ToString();
                return "{" + guid_s + "}";
            }

            var convertToDbParameter = base.ConvertToDbParameter(clientValue, clientValueTypeCode);
            return convertToDbParameter;
        }
        //protected override string GetSqlCreateColumnTypeForGuid(DBTable table, DBColumn column)
        //{
        //    return "binary(16)";
        //}
    }
}
