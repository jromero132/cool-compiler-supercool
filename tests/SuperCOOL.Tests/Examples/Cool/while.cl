class Main inherits IO {
   main(): SELF_TYPE {
      {
         let x:Int <- 5 in 
         while 0 < x 
         loop 
         {
            out_string("OK");
            x <- x - 1;
         } pool;
         let x:Int <- 0 in 
         while 0 < x 
         loop 
         {
            out_string("error");
            x <- x / 0;
         } pool;
         let x:Int <- 0 in 
         while 0 < x 
         loop 
         {
            out_string("error");
            x <- x / 2;
         } pool;
         self;
      }
   };
};