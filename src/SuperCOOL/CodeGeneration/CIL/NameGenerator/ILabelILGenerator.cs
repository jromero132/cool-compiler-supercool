namespace SuperCOOL
{
    public interface ILabelILGenerator
    {
        string GenerateVariable();
        (string end, string @else, string init) GenerateIf();
        (string varInit, string endOfCase) GenerateCase();
        string GenerateFunc(string className, string methodName);
        string GenerateInit(string classTypeName);
        (string @object, string value) GenerateStringData();
        (string @object, string value) GenerateEmptyStringData();
        string GenerateVoid();
        (string @object, string value) GenerateLabelTypeName(string name);
        string GenerateLabelTypeInfo(string name);
        string GenerateLabelVirtualTable(string name);
        string GetException( int id );
        string GetNewLine();
    }
}
