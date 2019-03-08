using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using SuperCOOL.ANTLR;
using SuperCOOL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCOOL.SemanticCheck
{
    public class SuperCoolSemanticCheckVisitor : SuperCOOLBaseVisitor<SemanticCheckResult>
    {
        public CompilationUnit CompilationUnit { get; }
        public SuperCoolSemanticCheckVisitor(CompilationUnit compilationUnit)
        {
            this.CompilationUnit = compilationUnit;
        }

        public SemanticCheckResult VisitAdd([NotNull] SuperCOOLParser.AddContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitAssignment([NotNull] SuperCOOLParser.AssignmentContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitBlock([NotNull] SuperCOOLParser.BlockContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitBoolNot([NotNull] SuperCOOLParser.BoolNotContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitCase([NotNull] SuperCOOLParser.CaseContext context)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitClassDefine([NotNull] SuperCOOLParser.ClassDefineContext context)
        {
            SemanticCheckResult semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;

            string inherit=context.TYPEID(1).Symbol.Text;
            semanticCheckResult.Correct &= CompilationUnit.IsTypeDef(inherit);

            var listFeaturesResult = context.listFeature().Accept(this);
            semanticCheckResult.Correct &= listFeaturesResult.Correct;
            return semanticCheckResult;
        }

        public override  SemanticCheckResult VisitClasses([NotNull] SuperCOOLParser.ClassesContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            var clasesResult = context.classDefine().Accept(this);
            semanticCheckResult.Correct = clasesResult.Correct;

            var programBlocksResult = context.programBlocks().Accept(this);
            semanticCheckResult.Correct &= programBlocksResult.Correct;

            return semanticCheckResult;
        }

        public override SemanticCheckResult VisitMultipleListFeature([NotNull] SuperCOOLParser.MultipleListFeatureContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            var featureResult = context.feature().Accept(this);
            semanticCheckResult.Correct = featureResult.Correct;

            var programBlocksResult = context.listFeature().Accept(this);
            semanticCheckResult.Correct &= programBlocksResult.Correct;

            return semanticCheckResult;
        }

        public override SemanticCheckResult VisitSingleListFeature([NotNull] SuperCOOLParser.SingleListFeatureContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            var featureResult = context.feature().Accept(this);
            semanticCheckResult.Correct = featureResult.Correct;
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitDivision([NotNull] SuperCOOLParser.DivisionContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitEqual([NotNull] SuperCOOLParser.EqualContext context)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitFalse([NotNull] SuperCOOLParser.FalseContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.GetTypeIfDef("bool");
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitIf([NotNull] SuperCOOLParser.IfContext context)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitInt([NotNull] SuperCOOLParser.IntContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.GetTypeIfDef("int");
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitIsvoid([NotNull] SuperCOOLParser.IsvoidContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLessEqual([NotNull] SuperCOOLParser.LessEqualContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLessThan([NotNull] SuperCOOLParser.LessThanContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitLetIn([NotNull] SuperCOOLParser.LetInContext context)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitSingleListFormal([NotNull] SuperCOOLParser.SingleListFormalContext context)
        {
            return context.formal().Accept(this);
        }

        public override SemanticCheckResult VisitFormal([NotNull] SuperCOOLParser.FormalContext context)
        {
            var result = new SemanticCheckResult();
            result.Correct= CompilationUnit.IsTypeDef(context.TYPEID().Symbol.Text);
            return result;
        }

        public override SemanticCheckResult VisitMultipleListFormal([NotNull] SuperCOOLParser.MultipleListFormalContext context)
        {
            var result = new SemanticCheckResult();
            var formalResult= context.formal().Accept(this);
            result.Correct = formalResult.Correct;

            var listResult = context.listFormal().Accept(this);
            result.Correct &= listResult.Correct;

            return result;
        }

        public override SemanticCheckResult VisitMethod([NotNull] SuperCOOLParser.MethodContext context)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            var formalListResult = context.listFormal().Accept(this);
            result.Correct = formalListResult.Correct;

            var exprResult = context.expression().Accept(this);
            string returnTypeName = context.TYPEID().Symbol.Text;
            if (!CompilationUnit.IsTypeDef(returnTypeName))
            {
                result.Correct = false;
                return result;
            }
            var returnType = CompilationUnit.GetTypeIfDef(returnTypeName);
            result.Correct &= exprResult.Type.IsIt(returnType);

            return result; 
        }

        public SemanticCheckResult VisitMethodCall([NotNull] SuperCOOLParser.MethodCallContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitMinus([NotNull] SuperCOOLParser.MinusContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitMultiply([NotNull] SuperCOOLParser.MultiplyContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitNegative([NotNull] SuperCOOLParser.NegativeContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitNew([NotNull] SuperCOOLParser.NewContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitOwnMethodCall([NotNull] SuperCOOLParser.OwnMethodCallContext context)
        {
            throw new NotImplementedException();
        }

        public SemanticCheckResult VisitParentheses([NotNull] SuperCOOLParser.ParenthesesContext context)
        {
            throw new NotImplementedException();
        }

        public override SemanticCheckResult VisitProgram([NotNull] SuperCOOLParser.ProgramContext context)
        {
            SemanticCheckResult result = new SemanticCheckResult();
            var programBlockResult = context.programBlocks().Accept(this);
            result.Correct = programBlockResult.Correct && CompilationUnit.NotCyclicalInheritance() && CompilationUnit.HasEntryPoint();
            return result;
        }

        public override SemanticCheckResult VisitProperty([NotNull] SuperCOOLParser.PropertyContext context)
        {
            SemanticCheckResult semanticCheckResult = new SemanticCheckResult();
            var result=context.expression().Accept(this);
            string type = context.TYPEID().Symbol.Text;
            result.Correct &= CompilationUnit.IsTypeDef(type);
            if (result.Type == CompilationUnit.GetTypeIfDef(type))
                semanticCheckResult.Correct = true;
            return semanticCheckResult;
        }

        public override SemanticCheckResult VisitString([NotNull] SuperCOOLParser.StringContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.GetTypeIfDef("string");
            return semanticCheckResult;
        }

        public override SemanticCheckResult VisitTrue([NotNull] SuperCOOLParser.TrueContext context)
        {
            var semanticCheckResult = new SemanticCheckResult();
            semanticCheckResult.Correct = true;
            semanticCheckResult.Type = CompilationUnit.GetTypeIfDef("bool");
            return semanticCheckResult;
        }

        public SemanticCheckResult VisitWhile([NotNull] SuperCOOLParser.WhileContext context)
        {
            throw new NotImplementedException();
        }
    }
}
