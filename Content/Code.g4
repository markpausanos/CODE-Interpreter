grammar Code;

program: begin_code line* end_code;

line: if_block | statement;

begin_code: 'BEGIN' CODE;
end_code: NEWLINE 'END' CODE EOF;
// code_block: begin_code (line (NEWLINE line)* NEWLINE?)? end_code;
CODE: 'CODE';   

if_block: 'IF' '(' expression ')' else_block ('ELSE' elseIfBlock)*;
begin_if: NEWLINE 'BEGIN' IF;
end_if: NEWLINE 'END' IF;
else_block: begin_if line* end_if;
IF: 'IF';

elseIfBlock: if_block | else_block;

statement: declaration | assignment | function_call;

declaration: NEWLINE IDENTIFIER variable (',' variable)*;
variable: IDENTIFIER ('=' (expression))?;
assignment: NEWLINE IDENTIFIER ('=' IDENTIFIER)* '=' expression;
function_call: NEWLINE (display | scan);
arguments : expression (',' expression)*; 

display: 'DISPLAY' ':' expression;
scan: 'SCAN' ':' IDENTIFIER (',' IDENTIFIER)*;

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
INT: [0-9]+;    
CHAR: '\'' ~ '\'' '\'' | '[' .? ']';
STRING: '"' .* '"';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
FLOAT: [0-9]+ '.' [0-9]+?;

constant: INT | CHAR | BOOL | FLOAT | STRING;
expression
    : constant                                      #constantExpression
    | IDENTIFIER                                    #identifierExpression
    | '(' expression ')'                            #parenthesizedExpression
    | 'NOT' expression                              #notExpression
    | expression multiply_op expression             #multiplyExpression
    | expression add_op expression                  #addExpression
    | expression compare_op expression              #compareExpression
    | expression bool_op expression                 #boolExpression
    | unary expression                              #unaryExpression
    | expression concat_op expression               #concatExpression
    | NEWLINE                                       #newlineExpression
    ;

unary: '+' | '-';
multiply_op: '*' | '/' | '%';
add_op: '+' | '-';
compare_op: '>' | '<' | '>=' | '<=' | '==' | '<>';
bool_op: 'AND' | 'OR';
concat_op: '&';

COMMENT: NEWLINE? '#' ~[\r?\n]*-> channel(HIDDEN);
NEWLINE: '\r'? '\n' | '$';
WS: [ \t]+ -> skip;