using System.Collections.Generic;
using SuperCOOL.Core;

namespace SuperCOOL.Tests.Constant
{
    internal static class DataTypes
    {
        internal static readonly CoolType type17 = new CoolType( "q", type14 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type16 = new CoolType( "p", type15 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type15 = new CoolType( "o", type14 )
        {
            Childs = new List<CoolType>()
            {
                type16
            }
        };

        internal static readonly CoolType type14 = new CoolType( "n", type13 )
        {
            Childs = new List<CoolType>()
            {
                type15,
                type17
            }
        };

        internal static readonly CoolType type13 = new CoolType( "m", type3 )
        {
            Childs = new List<CoolType>()
            {
                type14
            }
        };

        internal static readonly CoolType type12 = new CoolType( "l", type6 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type11 = new CoolType( "k", type8 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type10 = new CoolType( "j", type9 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type9 = new CoolType( "i", type8 )
        {
            Childs = new List<CoolType>()
            {
                type10
            }
        };

        internal static readonly CoolType type8 = new CoolType( "h", type7 )
        {
            Childs = new List<CoolType>()
            {
                type9,
                type11
            }
        };

        internal static readonly CoolType type7 = new CoolType( "g", type2 )
        {
            Childs = new List<CoolType>()
            {
                type8
            }
        };

        internal static readonly CoolType type6 = new CoolType( "f", type1 )
        {
            Childs = new List<CoolType>()
            {
                type12
            }
        };

        internal static readonly CoolType type5 = new CoolType( "e", type1 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type4 = new CoolType( "d", type1 )
        {
            Childs = new List<CoolType>()
        };

        internal static readonly CoolType type3 = new CoolType( "c", type1 )
        {
            Childs = new List<CoolType>()
            {
                type13
            }
        };

        internal static readonly CoolType type2 = new CoolType( "b", type1 )
        {
            Childs = new List<CoolType>()
            {
                type7
            }
        };

        internal static readonly CoolType type1 = new CoolType( "object" )
        {
            Childs = new List<CoolType>()
            {
                type2,
                type3,
                type4,
                type5,
                type6
            }
        };

        internal static readonly CompilationUnit cu = new CompilationUnit
        {
            Types = new HashSet<CoolType>()
            {
                type1,
                type2,
                type3,
                type4,
                type5,
                type6,
                type7,
                type8,
                type9,
                type10,
                type11,
                type12,
                type13,
                type14,
                type15,
                type16,
                type17
            }
        };
    }
}
