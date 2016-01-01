using EasyReader.Common;

namespace EnglishEasyRead.Core.DataAccess
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