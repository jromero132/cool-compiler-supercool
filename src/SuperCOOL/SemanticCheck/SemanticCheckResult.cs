using SuperCOOL.Core;
using System;
using System.Collections.Generic;

namespace SuperCOOL.SemanticCheck
{
    public class SemanticCheckResult
    {
        public bool Correct => Errors.Count == 0;
        public CoolType Type { get; private set; }
        public List<string> Errors { get; internal set; }
        public SemanticCheckResult()
        {
            Errors = new List<string>();
            Type = new NullType();
        }

        public void Ensure(SemanticCheckResult semanticCheckResult)
        {
            if (!semanticCheckResult.Correct)
                Errors.AddRange(semanticCheckResult.Errors);
        }
        public void Ensure(SemanticCheckResult semanticCheckResult ,bool condition, string Message)
        {
            if (!semanticCheckResult.Correct)
                Errors.AddRange(semanticCheckResult.Errors);
            else if (!condition)
                Errors.Add(Message);
        }

        internal void EnsureReturnType(CoolType type)
        {
            if (Correct)
                Type = type;
        }

        internal void Ensure(bool condition, string Message)
        {
            if (!condition)
                Errors.Add(Message);
        }
    }
}