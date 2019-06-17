class Main inherits IO {
   main(): SELF_TYPE {
      let x:Object in
         case x of
            x1:Int => out_int(x1);
            x2:String => out_string(x2);
            x2:Bool => out_int(3);
         esac
   };
};