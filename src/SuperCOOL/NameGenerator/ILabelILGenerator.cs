using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Core;

namespace SuperCOOL
{
    interface ILabelILGenerator
    {
        string GenerateVariable();
        string GenerateIf();
        (string varInit, string endOfCase) GenerateCase();
        string GenerateFunc(string className,string methodName);
        string GenerateInit(string classTypeName);
    }
}
