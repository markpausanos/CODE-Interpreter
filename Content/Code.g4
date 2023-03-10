grammar Code;

program: line* EOF;

line: code_block | if_block | statement;

begin_code: 'BEGIN' CODE;
end_code: 'END' CODE;
code_block: begin_code line* end_code;
CODE: 'CODE';

if_block: 'IF' '(' expression ')' else_block ('ELSE' elseIfBlock)*;
begin_if: 'BEGIN' IF;
end_if: 'END' IF;
else_block: begin_if line* end_if;
IF: 'IF';

elseIfBlock: if_block | else_block;

statement: assignment | function_call;

assignment: DATA_TYPE variable (',' variable)*; 
variable: INT? IDENTIFIER? ('=' (expression))?;

DATA_TYPE: 'INT' | 'CHAR' | 'BOOL' | 'FLOAT';
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
INT: [0-9]+;    
CHAR: '\'' ~ '\'' '\'';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
FLOAT: [0-9]+ '.' [0-9]+?;
constant: INT | CHAR | BOOL | FLOAT;
expression
    : constant              #constantExpression
    | '(' expression ')'    #parenthesizedExpression
    ;

function_call: IDENTIFIER ':' expression;

COMMENT: '#' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;