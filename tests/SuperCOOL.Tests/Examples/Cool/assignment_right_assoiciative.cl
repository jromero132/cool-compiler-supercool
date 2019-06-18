class Main inherits IO {
   y:Int;
   x:Int;
   main(): SELF_TYPE {
      {
         x <- y <- 5;
         out_int(x);
         out_int(y);
         let z:Int, w:Int in
         {
            z <- w <- x * y;
            out_int(z);
            out_int(w);
         };
      }
   };
};