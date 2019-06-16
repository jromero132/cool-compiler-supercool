class Main inherits IO {
   main(): SELF_TYPE {
	let x:String <- in_string(), y:String <- in_string() in out_string(x.concat(y))
   };
};