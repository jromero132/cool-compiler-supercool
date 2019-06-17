Class Calc
{
    n:Int;
    init(i:Int):Calc
    {
        {
            n<-i;
            self;
        }    
    };
    fact():Int
    {
        if n=1 then 1 else n*(new Calc).init(n-1).fact() fi
    };
};
Class Main inherits IO {
    main() : Object {
        out_int((new Calc).init(15).fact())
    };
};