using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using SuperCOOL.Core;
using SuperCOOL.Tests.Constant;
using System.IO;

namespace SuperCOOL.Tests.CoolTest
{
    public class FrontEndTest
    {
        private void Check(string[] files)
        {
            var output = Compiler.Compile(files);
            Helper.RunCoolCompilerFrontend(files, "semantic.txt");
            string out_file = File.ReadAllText("semantic.txt");
            Helper.DeleteFile("semantic.txt");
            Assert.True(Compare(out_file, output));
        }

        private bool Compare(string out_file, List<Error> output)
        {
            return out_file == "" && output.Count == 0;
        }

        [Fact]
        public void Arith()
        {
            var files = new string[] { "Examples/Cool/arith.cl"};
            Check(files);
        }


        [Fact]
        public void Atoi()
        {
            var files = new string[] { "Examples/Cool/atoi_test.cl", "Examples/Cool/atoi.cl" };
            Check(files);
        }


        [Fact]
        public void BookList()
        {
            var files = new string[] { "Examples/Cool/book_list.cl" };
            Check(files);
        }

        //TODO:: LIke the others when new cool compiler arrives
        [Fact]
        public void Cells()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/cells.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Complex()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/complex.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Cool()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/cool.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Graph()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/graph.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void HairScary()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/hair_scary.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void HelloWorld()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/hello_world.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Io()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/io.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Lam()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/lam.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Live()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/live.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void List()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/list.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void NewComplex()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/new_complex.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Palindrome()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/palindrome.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void Primes()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/palindrome.cl" });
            Assert.Equal(new List<Error>(), output);
        }

        [Fact]
        public void SortList()
        {
            var output = Compiler.Compile(new string[] { "Examples/Cool/sort_list.cl" });
            Assert.Equal(new List<Error>(), output);
        }

    }
}