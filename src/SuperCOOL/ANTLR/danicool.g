grammar SuperCool;
options
{
	 output = AST;
	 language = CSharp7;
	 k =1;
}

tokens
{
  PROGRAM;
  TYPE;
  FUNCTION;
  ATRIBUTTE;
  FORMAL;
  BLOCK;
  VARREQ;
  TCALL;
  DCALL;
  LCALL;
  PARAMS;
  ARGS;
  DEC;
  CASEUT;
}


@lexer::namespace {AST.ANTLR}
@lexer::header
{
	using System;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using AST.Reports;
}
@lexer::modifier { public }

@lexer::ctorModifier { public }

@parser::namespace {AST.ANTLR}
@parser::header
{
	using System;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using AST.Reports;
}
@parser::modifier { public }

@parser::ctorModifier { public }

@parser::members 
{ 
    public bool HasError{get; private set;}
    
    public int FileIndex{get; set;}
    
    public Dictionary<int,int> Mapping{get; set;}
    
    public string[] Files{get; set;}
    
	public CompilationReport Errors{ get; private set; }

	public override void ReportError(RecognitionException ex) 
	{
		if(this.Errors == null)
			this.Errors = new CompilationReport(Mapping, Files);
		this.Errors.AddError(ex.Line, ex.CharPositionInLine + 1, GetErrorMessage(ex, TokenNames), "_Parser",FileIndex);
		HasError = true;
	} 
}

//Fragments are used to replace on REGEX on Lexer and Parser Rules
fragment DIGIT	: '0' .. '9';
fragment LETTER	: ('a' .. 'z' | 'A' .. 'Z');

fragment ESCSEQ			: '\\' ('n' | 't' | 'r' | '"' | ((WS)*'\\') | (ASCII));
fragment ASCII		    : ('0' DIGIT DIGIT)| ('1'('0'..'1''0'..'9'|'2''0'..'7'));
fragment PRINTCHAR		: ' '|'!'|'#' .. '[' | ']' .. '~';

CLASS		: 'class';
INHERITS	: 'inherits';

NEW 		: 'new';
LET 		: 'let';
IN 		: 'in';
CASE 		: 'case';
OF 		: 'of';
ESAC 		: 'esac';
WHILE 		: 'while';
POOL 		: 'pool';
LOOP 		: 'loop';
IF 		: 'if';
THEN 		: 'then';
ELSE 		: 'else';
FI 		: 'fi';

COM		: ',';
TWODOT		: ':';
SEMICOLON	: ';';
LPARENT		: '(';
RPARENT		: ')';
LKEY		: '{';
RKEY		: '}';
ARR		: '@';
DOT		: '.';
PLUS		: '+';
MINUS		: '-';
MULT		: '*';
DIV		: '/';
EQ		: '=';
LT		: '<';
LTE		: '<=';
CA 		: '=>';

ASSIG		: '<-';
NOT 		: 'not';
VOID		: 'isvoid';
NBIT		: '~';


WS		: ('\r' | '\n' | '\t' | ' ' ) { $channel = Hidden; } ;

TRUE 		: 'true';
FALSE 		: 'false';
INTEGER		: (DIGIT)+;
STRING		: '"' ((PRINTCHAR | ESCSEQ))* '"';
IDCLASS		: 'A' .. 'Z' (LETTER | DIGIT | '_')*;
ID 		: 'a' .. 'z' (LETTER | DIGIT | '_')*;

COMMENT		:   '(*' .* (COMMENT .*)*  '*)' { $channel = Hidden; } ;


/*Parser stuff*/


public program 	: listclass? EOF -> ^(PROGRAM listclass?);

listclass	: (class SEMICOLON)+ -> class+;

class 		: CLASS type = IDCLASS(INHERITS inh = IDCLASS)? LKEY listfeature? RKEY -> ^(CLASS $type $inh? listfeature?);

listfeature 	: (feature SEMICOLON)+ -> feature+;

feature 	: name = ID LPARENT  listformal? RPARENT TWODOT type = IDCLASS LKEY expr RKEY -> ^(FUNCTION ^(FORMAL $name $type) ^(PARAMS listformal?) expr)
		| formal (ASSIG expr)? -> ^(ATRIBUTTE formal expr?);
		
listformal	: formal(COM formal)* -> formal+;

formal		: name = ID TWODOT type = IDCLASS -> ^(FORMAL $name $type);

expr 		: ID ASSIG expr -> ^(ASSIG ID expr)
		| not ;
		
not 		: NOT opr = not -> ^(NOT $opr)
		| comparer;

comparer	: (plusminus -> plusminus) ( EQ opr = plusminus -> ^(EQ $comparer $opr)| LT  opr = plusminus -> ^(LT $comparer $opr) | LTE opr = plusminus -> ^(LTE $comparer $opr) )?;

plusminus 	: (mult -> mult) (PLUS opr = mult -> ^(PLUS $plusminus $opr)| MINUS opr = mult -> ^(MINUS $plusminus $opr))*;

mult 		: (void -> void) (MULT opr = void -> ^(MULT $mult $opr)| DIV opr = void -> ^(DIV $mult $opr))*;

void 		: VOID opr = void -> ^(VOID $opr)
		| nbit;
		
nbit 		: NBIT opr = nbit -> ^(NBIT $opr)
		| callty;
		
callty 		: (call -> call) (ARR type = IDCLASS DOT method = ID LPARENT args = arguments RPARENT -> ^(TCALL $method $args $callty $type ))* ;

call 		: (other -> other)(DOT ID LPARENT args = arguments RPARENT -> ^(DCALL ID  $args $call))* ;

arguments :	(expr (COM expr)*)? ->  ^(ARGS (expr)*);

other		: ID -> ^(VARREQ ID)
		| INTEGER 
		| STRING
		| TRUE 
		| FALSE 
		| ID LPARENT args = arguments RPARENT -> ^(LCALL ID $args)
		| IF cnd = expr THEN th = expr ELSE els = expr FI -> ^(IF $cnd $th $els)
		| WHILE cnd = expr LOOP body = expr POOL -> ^(WHILE $cnd $ body)
		| LKEY expr_seq RKEY -> ^(BLOCK expr_seq) 
		| NEW IDCLASS -> ^(NEW IDCLASS) 
		| LPARENT expr RPARENT -> expr
		| LET listlet IN expr -> ^(LET listlet expr)
		| CASE expr OF listcase ESAC -> ^(CASE expr listcase);

expr_seq	: (expr SEMICOLON)+  -> expr+;

listlet		: let (COM let)* -> let+;
let		: formal (ASSIG expr)? -> ^(DEC formal expr?);

listcase	: (case SEMICOLON)+ -> case+;
case		: formal CA expr -> ^(CASEUT formal expr);