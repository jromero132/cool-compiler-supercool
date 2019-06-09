namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILGetAttrNode : ASTCILExpressionNode
    {
        public string TypeName { get; }
        public string AttributeName { get; }

        public ASTCILGetAttrNode( string typeName, string attributeName )
        {
            TypeName = typeName;
            AttributeName = attributeName;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitGetAttr( this );
    }
}
