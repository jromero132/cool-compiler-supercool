using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILObjectTypeNameNode : ASTCILFuncNode
    {
        public ASTCILObjectTypeNameNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Type_Name), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitObjectTypeName(this);
        }
    }
}
