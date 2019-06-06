using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL
{
    interface ILabelILGenerator
    {
        string GenerateVariable();
        string GenerateIf();
        (string varInit, string endOfCase) GenerateCase();
    }
}
