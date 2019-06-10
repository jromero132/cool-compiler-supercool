namespace SuperCOOL
{
    public interface ILabelILGenerator
    {
        string GenerateVariable();
        (string end, string @else) GenerateIf();
        (string varInit, string endOfCase) GenerateCase();
        string GenerateFunc(string className, string methodName);
        string GenerateInit(string classTypeName);
        string GenerateStringData();
        string GenerateEmptyStringData();
        string GenerateLabelTypeName(string name);
        string GenerateLabelTypeInfo(string name);
        string GenerateLabelVirtualTable(string name);
        string GetBuffer();
        string GetException();
    }
}
