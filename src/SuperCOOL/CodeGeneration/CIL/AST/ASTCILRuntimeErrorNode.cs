using System.Runtime.InteropServices.ComTypes;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILRuntimeErrorNode : ASTCILExpressionNode
    {
        public int Id { get; }
        public ASTCILRuntimeErrorNode(int id)
        {
            Id = id;
        }
    }
}
