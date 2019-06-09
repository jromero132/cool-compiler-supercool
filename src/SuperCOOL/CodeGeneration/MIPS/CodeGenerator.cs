using SuperCOOL.CodeGeneration.CIL;
using SuperCOOL.CodeGeneration.CIL.AST;
using SuperCOOL.CodeGeneration.MIPS.Registers;
using System;
using System.Collections.Generic;
using System.Text;
namespace SuperCOOL.CodeGeneration.MIPS
{
    class CodeGenerator : ICILVisitor<MipsProgram>
    {
        public MipsProgram VisitAddConstantVariable(ASTCILAddConstantVariableNode AddConstantVariable)
        {
            var rigth = AddConstantVariable.Right.Accept(this);
            var add = new MipsProgram();
            add.SectionCode.Append(MipsGenerationHelper.NewScript().Add(MipsRegisterSet.a0,AddConstantVariable.Left,MipsRegisterSet.a0));
            return rigth + add;
        }

        public MipsProgram VisitAddTwoConstant(ASTCILAddTwoConstantNode AddTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitAddTwoVariables(ASTCILAddTwoVariablesNode AddTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitAddVariableConstant(ASTCILAddVariableConstantNode AddVariableConstant)
        {
         
        }

        public MipsProgram VisitAllocate(ASTCILAllocateNode Allocate)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitAssignment(ASTCILAssignmentNode Assignment)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBlock(ASTCILBlockNode Block)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolConstant(ASTCILBoolConstantNode BoolConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolNot(ASTCILBoolNotNode BoolNot)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrConstantVariable(ASTCILBoolOrConstantVariableNode BoolOrConstantVariable)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrTwoConstant(ASTCILBoolOrTwoConstantNode BoolOrTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrTwoVariables(ASTCILBoolOrTwoVariablesNode BoolOrTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitBoolOrVariableConstant(ASTCILBoolOrVariableConstantNode BoolOrVariableConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideConstantVariable(ASTCILDivideConstantVariableNode DivideConstantVariable)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideTwoConstant(ASTCILDivideTwoConstantNode DivideTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideTwoVariables(ASTCILDivideTwoVariablesNode DivideTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitDivideVariableConstant(ASTCILDivideVariableConstantNode DivideVariableConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitExpression(ASTCILExpressionNode Expression)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFunc(ASTCILFuncNode Func)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFuncStaticCall(ASTCILFuncStaticCallNode FuncStaticCall)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitFuncVirtualCall(ASTCILFuncVirtualCallNode FuncVirtualCall)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitGetAttr(ASTCILGetAttrNode GetAttr)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitGoto(ASTCILGotoNode Goto)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitId(ASTCILIdNode Id)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIf(ASTCILIfNode If)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIntConstant(ASTCILIntConstantNode IntConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOInInt(ASTCILIOInIntNode IOInInt)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOInString(ASTCILIOInStringNode IOInString)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOOutInt(ASTCILIOOutIntNode IOOutInt)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIOOutString(ASTCILIOOutStringNode IOOutString)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitIsVoid(ASTCILIsVoidNode IsVoid)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanConstantVariable(ASTCILLessThanConstantVariableNode LessThanConstantVariable)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanTwoConstant(ASTCILLessThanTwoConstantNode LessThanTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanTwoVariables(ASTCILLessThanTwoVariablesNode LessThanTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLessThanVariableConstant(ASTCILLessThanVariableConstantNode LessThanVariableConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitLocal(ASTCILLocalNode Local)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMinusConstantVariable(ASTCILMinusConstantVariableNode MinusConstantVariable)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMinusTwoConstant(ASTCILMinusTwoConstantNode MinusTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMinusTwoVariables(ASTCILMinusTwoVariablesNode MinusTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMinusVariableConstant(ASTCILMinusVariableConstantNode MinusVariableConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMultiplyConstantVariable(ASTCILMultiplyConstantVariableNode MultiplyConstantVariable)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMultiplyTwoConstant(ASTCILMultiplyTwoConstantNode MultiplyTwoConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMultiplyTwoVariables(ASTCILMultiplyTwoVariablesNode MultiplyTwoVariables)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitMultiplyVariableConstant(ASTCILMultiplyVariableConstantNode MultiplyVariableConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitNew(ASTCILNewNode New)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitNode(ASTCILNode Node)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitParam(ASTCILParamNode Param)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitProgram(ASTCILProgramNode Program)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitRuntimeError(ASTCILRuntimeErrorNode RuntimeError)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitSelf(ASTCILSelfNode Self)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitSetAttribute(ASTCILSetAttributeNode SetAttribute)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitStringConstant(ASTCILStringConstantNode StringConstant)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitType(ASTCILTypeNode Type)
        {
            throw new NotImplementedException();
        }

        public MipsProgram VisitVoid(ASTCILVoidNode Void)
        {
            throw new NotImplementedException();
        }
    }
}
