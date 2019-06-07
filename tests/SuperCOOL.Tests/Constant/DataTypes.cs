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
            cu.TypeEnvironment.AddType("q");
            cu.TypeEnvironment.AddType("p");
            cu.TypeEnvironment.AddType("o");
            cu.TypeEnvironment.AddType("n");
            cu.TypeEnvironment.AddType("m");
            cu.TypeEnvironment.AddType("l");
            cu.TypeEnvironment.AddType("k");
            cu.TypeEnvironment.AddType("j");
            cu.TypeEnvironment.AddType("i");
            cu.TypeEnvironment.AddType("h");
            cu.TypeEnvironment.AddType("g");
            cu.TypeEnvironment.AddType("f");
            cu.TypeEnvironment.AddType("e");
            cu.TypeEnvironment.AddType("d");
            cu.TypeEnvironment.AddType("c");
            cu.TypeEnvironment.AddType("b");
            cu.TypeEnvironment.AddInheritance("q","n");
            cu.TypeEnvironment.AddInheritance("p","o");
            cu.TypeEnvironment.AddInheritance("o","n");
            cu.TypeEnvironment.AddInheritance("n","m");
            cu.TypeEnvironment.AddInheritance("m","c");
            cu.TypeEnvironment.AddInheritance("c","Object");
            cu.TypeEnvironment.AddInheritance("l", "f");
            cu.TypeEnvironment.AddInheritance("f", "Object");
            cu.TypeEnvironment.AddInheritance("e", "Object");
            cu.TypeEnvironment.AddInheritance("d", "Object");
            cu.TypeEnvironment.AddInheritance("b", "Object");
            cu.TypeEnvironment.AddInheritance("g", "b");
            cu.TypeEnvironment.AddInheritance("h", "g");
            cu.TypeEnvironment.AddInheritance("i", "h");
            cu.TypeEnvironment.AddInheritance("j", "i");
            cu.TypeEnvironment.AddInheritance("k", "h");

        }
    }
}
