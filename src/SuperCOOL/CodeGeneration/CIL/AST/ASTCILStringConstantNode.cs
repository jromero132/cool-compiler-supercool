using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILStringConstantNode : ASTCILExpressionNode
    {
        public string Value { get; }

        public ASTCILStringConstantNode(string value) : base(Types.String)
        {
            Value = value;
        }
    }
}
