using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILObjectTypeNameNode : ASTCILFuncNode
    {
        public ASTCILObjectTypeNameNode(CoolMethod method,ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Type_Name),method, Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitObjectTypeName(this);
        }
    }
}
