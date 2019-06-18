class Main inherits IO {
   main(): Object {
      let x:Object in 
      {
         x<-5;
         case x of
            x:Int=> out_int(x);
            x:Object=>x.abort();
         esac;

         case something(1,2) of
            x:Int=> out_int(x);
            x:Object=>x.abort();
         esac;

         if isvoid x then out_string("is void") else out_string("is not") fi ;
         if isvoid true then out_string("is void") else out_string("is not") fi ; 

      }
   };

   something(num1:Object,num2:Object):Object{
      {
         out_string(num1.type_name());
         out_string(num2.type_name());
         5;
      }
   };
};