class A inherits IO
{
    print():IO
    {
        out_string("i am of type A\n")
    };
};
class B inherits A
{
    print():IO
    {
        out_string("i am of type B\n")
    };
};
class C inherits B
{
    print():IO
    {
        out_string("i am of type C\n")
    };
};
class Main
{
    main():Object
    {{
        let x:C<-new C in
        {
            x.print();
            x@B.print();
            x@A.print();
        };
    }};
};