using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringConcatNode : ASTCILFuncNode
    {
        public ASTCILStringConcatNode(CoolMethod method,ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.String, Functions.Concat),method, Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitStringConcat(this);
        }
    }
}
