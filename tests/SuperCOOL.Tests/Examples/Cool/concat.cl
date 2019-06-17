class Main inherits IO {
    main(): SELF_TYPE {
        let x:String <- "JoseA".concat("132"), y:String <- in_string(), z:String <- in_string()
		in {
			out_string(x);
			out_string("\n");
			out_string(y.concat(z));
		}
    };
};