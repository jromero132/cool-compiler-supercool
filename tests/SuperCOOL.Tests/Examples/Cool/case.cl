class Main inherits IO {
   main(): SELF_TYPE {
      {
         case "hello" of
            x1:Int => out_int(x1);
            x2:String => out_string(x2);
            x2:Bool => out_int(3);
         esac;
         case self of
            x1:Int => out_int(x1);
            x2:String => out_string(x2);
            x2:Main => out_int(3);
         esac;
         case self of
            x1:Int => out_int(x1);
            x2:IO => out_string("x2");
            x2:Bool => out_int(3);
         esac;
         case self of
            x1:Object => out_int(123);
            x2:Int => out_string("x2");
            x2:Bool => out_int(3);
         esac;
         case self of
            x1:String => out_string(x1);
            x2:Int => out_int(x2);
            x2:Bool => out_int(3);
         esac;
      }
   };
};