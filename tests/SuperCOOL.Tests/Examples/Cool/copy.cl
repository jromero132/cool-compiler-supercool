class Main inherits IO {
   x:String <- "temp";
   main(): Object {
      {
         out_int(x.copy().length());
         let y:String <- x.copy(), z:String 
         in{
            z <- y.concat(x);
            out_string(z);
            out_string(y);
            out_string(x.substr(2,1));
            out_string(x.copy().concat(z.copy().substr(1,3)));
            y <- "test";
            out_string(y);
            out_string(x);
         };
      }
   };
};