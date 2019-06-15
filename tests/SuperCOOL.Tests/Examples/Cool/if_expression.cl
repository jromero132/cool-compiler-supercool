class Main inherits IO {
   main(): SELF_TYPE {
        {
            if true then out_string("true") else out_string("false") fi;
            if false then out_string("true") else out_string("false") fi;
        }
   };
};