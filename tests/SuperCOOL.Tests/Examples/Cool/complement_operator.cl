class Main inherits IO {
   main(): SELF_TYPE {
      {
         let x:Int <- 5 in
         out_int(~x);
         let x:Bool <- true in 
         if not true then out_string("error") else out_string("Ok") fi;
      }
   };
};