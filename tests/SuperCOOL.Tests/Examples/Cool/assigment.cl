class Main inherits IO {
   main(): SELF_TYPE {
         let x:Int <- 5, y:Int <- 10, z:Int <- 2
         in 
         {
            x <- x + y - z * 2 + 19 + y / 4;
            x <- x / z * 3 + x - z + y;
            y <- x + z * y - x / 2;
            out_int(x);
            out_int(y);
            out_int(z);
         }
   };
};