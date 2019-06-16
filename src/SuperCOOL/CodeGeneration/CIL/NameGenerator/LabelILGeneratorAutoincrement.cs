namespace SuperCOOL.NameGenerator
{
    public class LabelILGeneratorAutoincrement : ILabelILGenerator
    {
        private int ifIndex;
        private int caseIndex;
        private int variableIndex;
        private int stringData;

        public LabelILGeneratorAutoincrement() => this.ifIndex = this.caseIndex = this.variableIndex = this.stringData = 1;

        public string GenerateVariable() => $"_var_{ variableIndex++ }";

        public (string end, string @else, string init) GenerateIf() => ($"_end_if_{ this.ifIndex }", $"_else_if_{ this.ifIndex }", $"_if_{ ifIndex++ }");

        public (string varInit, string endOfCase) GenerateCase() => (GenerateVariable(), $"_caseEnd_{ this.caseIndex++ }");

        public string GenerateFunc( string className, string methodName ) => $"{ className }_{ methodName }";

        public string GenerateInit( string classTypeName ) => $"{ classTypeName }__init";

        public (string @object, string value) GenerateStringData() => ($"_string_obj_{ stringData }", $"_string_val_{ stringData++ }");

        public (string @object, string value) GenerateEmptyStringData() => ("_string_obj_empty", "_string_val_empty");
        public string GenerateVoid() => "_void";

        public (string @object,string value) GenerateLabelTypeName( string name ) => ($"__{ name }_obj", $"__{ name }_val");

        public string GenerateLabelTypeInfo( string name ) => $"___type_info_{ name }";

        public string GenerateLabelVirtualTable( string name ) => $"____virtual_table_{ name }";

        public string GetException( int id ) => $"____exception{ id }";
        public string GetNewLine() => "_NewLine";
    }
}
