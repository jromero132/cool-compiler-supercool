using SuperCOOL.Core;
using System.Collections.Generic;

namespace SuperCOOL.SemanticCheck
{
    public class SemanticCheckResult
    {
        public bool Correct { get; internal set; }
        public CoolType Type { get; internal set; }
        public List<string> Errors { get; internal set; }
        public SemanticCheckResult()
        {
            Errors = new List<string>();
        }
    }
}