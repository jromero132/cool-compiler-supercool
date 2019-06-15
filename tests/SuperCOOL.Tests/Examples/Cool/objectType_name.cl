class Main inherits IO {
   main(): SELF_TYPE {
      {
         out_string(type_name());
         let x:String,y:Object in
         {
             y <- x;
            out_string(x.type_name());
            out_string(y.type_name());
         };
      }
   };
};