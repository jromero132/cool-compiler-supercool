using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncNode : ASTCILNode
    {
        public string Tag { get; }
        public CoolMethod Method { get; }
        public IReadOnlyList<ASTCILExpressionNode> Body { get; }
        public bool Boxing { get; }
        public CoolType BoxingType { get; }
        public ASTCILFuncNode(string Tag, CoolMethod method, IEnumerable<ASTCILExpressionNode> body,bool boxing,CoolType boxingType) : this(Tag,method,body)
        {
            this.Boxing=boxing;
            this.BoxingType = boxingType;
        }

        public ASTCILFuncNode(string Tag, CoolMethod method, IEnumerable<ASTCILExpressionNode> body) : base()
        {
            this.Tag = Tag;
            Method = method;
            Body = body.ToList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFunc( this );
    }
}