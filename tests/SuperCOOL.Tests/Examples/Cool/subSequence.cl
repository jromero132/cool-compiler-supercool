class Main inherits IO {
   main(): Object {
      {
         let s:String <- in_string(), i:Int <- 0, j:Int
         in {
            while i < s.length() 
            loop {
               j <- i;
               while j < s.length()
               loop {
                  let s2:String <- s.substr(i, j - i + 1), s3:String
                  in {
                     out_int(s2.length()).out_string("\n").out_int(s3.length()).out_string("\n").out_string(s2.concat("\n".concat(s3.concat("\n"))));
                     s3 <- s2;
                  };
                  j <- j + 1;
               } pool;
            i <- i + 1;
            } pool;
         };
      }
   };
};