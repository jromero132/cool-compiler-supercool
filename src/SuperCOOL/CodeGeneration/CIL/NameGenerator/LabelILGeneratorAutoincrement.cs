namespace SuperCOOL.NameGenerator
{
    public class LabelILGeneratorAutoincrement : ILabelILGenerator
    {
        private int ifIndex;
        private int exceptionIndex;
        private int caseIndex;
        private int variableIndex;
        private int stringData;

        public LabelILGeneratorAutoincrement() => this.ifIndex = this.caseIndex = this.variableIndex = this.stringData = 1;

        public string GenerateVariable() => $"_var_{ variableIndex++ }";

        public (string end, string @else) GenerateIf() => ($"_end_if_{ this.ifIndex }", $"_else_if_{ this.ifIndex++ }");

        public (string varInit, string endOfCase) GenerateCase() => (GenerateVariable(), $"_caseEnd_{ this.caseIndex++ }");

        public string GenerateFunc( string className, string methodName ) => $"{ className }_{ methodName }";

        public string GenerateInit( string classTypeName ) => $"{ classTypeName }__init";

        public string GenerateStringData() => $"_string_{ stringData++ }";

        public string GenerateEmptyStringData() => $"_string_empty";

        public string GenerateLabelTypeName( string name ) => $"__{ name }";

        public string GenerateLabelTypeInfo( string name ) => $"___type_info_{ name }";

        public string GenerateLabelVirtualTable( string name ) => $"____virtual_table_{ name }";

        public string GetBuffer() => $"_____buffer";

        public string GetException() => $"____exception{ exceptionIndex++ }";
    }
}
