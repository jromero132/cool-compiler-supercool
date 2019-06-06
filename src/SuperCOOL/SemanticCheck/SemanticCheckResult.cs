using SuperCOOL.Core;
using System;
using System.Collections.Generic;

namespace SuperCOOL.SemanticCheck
{
    public class SemanticCheckResult
    {
        public bool Correct => Errors.Count == 0;
        public CoolType Type { get; private set; }
        public List<Error> Errors { get; internal set; }
        public SemanticCheckResult()
        {
            Errors = new List<Error>();
            Type = new NullType();
        }

        public void Ensure(SemanticCheckResult semanticCheckResult)
        {
            if (!semanticCheckResult.Correct)
                Errors.AddRange(semanticCheckResult.Errors);
        }
        public void Ensure(SemanticCheckResult semanticCheckResult ,bool condition, Error error)
        {
            if (!semanticCheckResult.Correct)
                Errors.AddRange(semanticCheckResult.Errors);
            else if (!condition)
                Errors.Add(error);
        }

        internal void EnsureReturnType(CoolType type)
        {
            if (Correct)
                Type = type;
        }

        internal void Ensure(bool condition, Error error)
        {
            if (!condition)
                Errors.Add(error);
        }
    }
}