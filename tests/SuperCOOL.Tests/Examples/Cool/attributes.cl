class Main inherits IO {
   x : Int;
   y : Int <- ~10 * 2;
   z : String <- "casa";
   main(): SELF_TYPE {
      {
         out_int(x);
         out_int(y);
         out_string(z);
         x <- 5;
         y <- 3 + x;
         z <- "hello";
         out_int(x - y);
         out_int(y + x);
         let t:Int in {
            t <- x * y;
            out_int(t);
            x <- t;
            out_int(t);
         };
         let z:Int in {
            z <- 5;
            y <- z * x;
            out_int(z);
         };
         out_string(z);
         out_string(z.type_name());
      }
   };
};