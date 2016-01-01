using EasyRead.Common;

namespace EasyRead.Core.DataAccess
{
    public sealed class RequiredFieldNotInitialized : ExceptionData
    {
        public RequiredFieldNotInitialized(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}

