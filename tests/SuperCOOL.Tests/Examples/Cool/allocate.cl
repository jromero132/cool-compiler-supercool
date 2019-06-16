class Main inherits IO
{
    a:A;
    main():Object
    {{
        a<-new A;
        a.print();
    }};
};
class A
{
    a:Int<-4;
    b:Int;
    io:IO <- new IO;
    print():IO
    {{
        io.out_int(a);
        io.out_int(b);
    }};
};