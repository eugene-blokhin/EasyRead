using EasyRead.Common;

namespace EasyRead.Core.DataAccess
{
    public sealed class ForeignKeyOutOfRange : ExceptionData
    {
        public ForeignKeyOutOfRange(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}

