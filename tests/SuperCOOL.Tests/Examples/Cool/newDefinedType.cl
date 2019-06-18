class Main inherits IO {
   main(): SELF_TYPE {
        {
            out_string((new String).concat("asd"));
            if (new String) = ""
                then out_string("Ok")
                else out_string("error")
            fi;
            out_int((new Int) + 7 * 2 / 3);
            if (new Bool)
                then out_string("error")
                else out_string("Ok")
            fi;
        }
   };
};