using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILObjectCopyNode : ASTCILFuncNode
    {
        public ASTCILObjectCopyNode(CoolMethod method,ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Copy),method, Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitObjectCopy(this);
        }
    }
}
