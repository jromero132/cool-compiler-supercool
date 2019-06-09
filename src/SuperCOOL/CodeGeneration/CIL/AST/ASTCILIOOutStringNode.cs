using System;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public ASTCILIOOutStringNode(ILabelILGenerator labelIlGenerator,ASTCILExpressionNode expresion) : base(labelIlGenerator.GenerateFunc(Types.IO,Functions.OutString), 
            new[] {expresion})
        {
        }
    }
}
