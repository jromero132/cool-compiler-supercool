using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILGetAttrNode : ASTCILExpressionNode
    {
        public string TypeName { get; }
        public string AttributeName { get; }

        public ASTCILGetAttrNode( string typeName, string attributeName,ISymbolTable symbolTable):base(symbolTable)
        {
            TypeName = typeName;
            AttributeName = attributeName;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitGetAttr( this );
    }
}
