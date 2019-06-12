using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public ASTCILIOOutIntNode(CoolMethod method,ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.IO, Functions.OutInt), method,
            Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor) => Visitor.VisitIOOutInt(this);
    }
}
