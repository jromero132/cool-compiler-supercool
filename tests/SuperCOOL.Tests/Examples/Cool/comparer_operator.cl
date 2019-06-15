class Main inherits IO {
   main(): SELF_TYPE {
      {
         if 11 < 10 then out_string("Ok") else out_string("error") fi;
         if 5 < 10 then out_string("Ok") else out_string("error") fi;
         if 10 < 10 then out_string("Ok") else out_string("error") fi;
         if 10 = 10 then out_string("Ok") else out_string("error") fi;
         if 10 = 13 then out_string("Ok") else out_string("error") fi;
         if 3 <= 2 then out_string("Ok") else out_string("error") fi;
         if 1 <= 10 then out_string("Ok") else out_string("error") fi;
         if 5 <= 5 then out_string("Ok") else out_string("error") fi;
      }
   };
};