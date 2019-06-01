using System.Collections.Generic;
using SuperCOOL.Core;

namespace SuperCOOL.Tests.Constant
{
    internal static class DataTypes
    {
        internal static readonly CompilationUnit cu;

        static DataTypes()
        {
            cu = new CompilationUnit();
            cu.AddType("q");
            cu.AddType("p");
            cu.AddType("o");
            cu.AddType("n");
            cu.AddType("m");
            cu.AddType("l");
            cu.AddType("k");
            cu.AddType("j");
            cu.AddType("i");
            cu.AddType("h");
            cu.AddType("g");
            cu.AddType("f");
            cu.AddType("e");
            cu.AddType("d");
            cu.AddType("c");
            cu.AddType("b");
            cu.AddInheritance("q","n");
            cu.AddInheritance("p","o");
            cu.AddInheritance("o","n");
            cu.AddInheritance("n","m");
            cu.AddInheritance("m","c");
            cu.AddInheritance("c","Object");
            cu.AddInheritance("l", "f");
            cu.AddInheritance("f", "Object");
            cu.AddInheritance("e", "Object");
            cu.AddInheritance("d", "Object");
            cu.AddInheritance("b", "Object");
            cu.AddInheritance("g", "b");
            cu.AddInheritance("h", "g");
            cu.AddInheritance("i", "h");
            cu.AddInheritance("j", "i");
            cu.AddInheritance("k", "h");

        }
    }
}
