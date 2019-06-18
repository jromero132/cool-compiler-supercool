class Main inherits IO {
   b:B<-new B;
   
   main(): Object {
         out_int(b.geta()).out_string(b.getb()).out_string(b.getc()).out_string(b.getd()).out_int(b.geth())
   };
   
};
class A inherits IO {
   a:Int<-13;
   b:String<-"Hola";
   c:String<-"Jose";
   geta():Int{a};
   getb():String{b};
   getc():String{c};
};
class B inherits A {
     d:String <-"Ariel";
     h : Int <- 2;
     getd():String{d};
     geth():Int{h};
};