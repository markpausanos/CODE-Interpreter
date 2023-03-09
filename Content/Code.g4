grammar Code;

program: line* EOF;


line: statement | ifBlock | whileBlock;

statement: block | assignment | functionCall;

ifBlock: 'IF' expression block ('ELSE' elseIfBlock);
elseIfBlock: block | ifBlock;

whileBlock: 'WHILE' expression block;

assignment: DATA_TYPE IDENTIFIER ('=' expression)? (',' IDENTIFIER ('=' expression)?)*; 

functionCall: IDENTIFIER ':' expression;

expression
    : constant                                      #constantExpression
    | IDENTIFIER                                    #identifierExpression
    ;

begin: ('BEGIN ') BEGINNABLE;
end: ('END ') BEGINNABLE;

block: begin line* end;

constant: INT | CHAR | BOOL | FLOAT;

DATA_TYPE: 'INT' | 'CHAR' | 'BOOL' | 'FLOAT' | WORD;

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

INT: [0-9]+;
CHAR: '\'' ~ '\'' '\'';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
FLOAT: [0-9]+ ('.' [0-9]+)?;
Space: ' ' -> skip;
BEGINNABLE: 'CODE' | 'IF' | 'WHILE' ;
SYMBOL: ('!'|'?'|':'|'.'|'/'|'*')*;
WORD : ('a'..'z' | 'A'..'Z')+;
DIGIT: ('0'..'9')*;
NEWLINE : '\r'? '\n' | '\r';