using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringSubStrNode : ASTCILFuncNode
    {
        public ASTCILStringSubStrNode(CoolMethod method,ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.String, Functions.Substr), method, Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitStringSubStr(this);
        }
    }
}
