using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILObjectCopyNode : ASTCILFuncNode
    {
        public ASTCILObjectCopyNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Copy), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitObjectCopy(this);
        }
    }
}
