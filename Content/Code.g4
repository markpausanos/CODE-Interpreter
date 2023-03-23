﻿grammar Code;

program: begin_code declaration* line* end_code;

line
    : if_block 
    | statement
    ;

begin_code: NEWLINE? BEGIN CODE;
end_code: NEWLINE END CODE EOF;
BEGIN: 'BEGIN';
END: 'END';
CODE: 'CODE';   

declaration: NEWLINE data_type variable (',' variable)*;
variable: IDENTIFIER ('=' (expression))?;
data_type: INT_TEXT | CHAR_TEXT | BOOL_TEXT | FLOAT_TEXT;

statement
    : assignment
    | function_call 
    | if_block 
    | while_block
    ;

assignment: NEWLINE IDENTIFIER ('=' IDENTIFIER)* '=' expression;

function_call: NEWLINE (display | scan);
arguments : expression (',' expression)*; 

display: 'DISPLAY' ':' expression;
scan: 'SCAN' ':' IDENTIFIER (',' IDENTIFIER)*;

if_block: NEWLINE IF '(' expression ')' if_code_block else_if_block* else_block?;
else_if_block: NEWLINE ELSE IF '(' expression ')' if_code_block;
else_block: NEWLINE ELSE if_code_block;
begin_if: NEWLINE BEGIN IF;
end_if: NEWLINE END IF;
if_code_block: begin_if line* end_if;
IF: 'IF';
ELSE: 'ELSE';

while_block: NEWLINE WHILE '(' expression ')' while_code_block;
begin_while: NEWLINE BEGIN WHILE;
end_while: NEWLINE END WHILE;
break: NEWLINE BREAK;
continue: NEWLINE CONTINUE;
while_line
    : line
    | break
    | continue
    ;
while_code_block: begin_while while_line* end_while;
WHILE: 'WHILE';
BREAK: 'BREAK';
CONTINUE: 'CONTINUE';

expression
    : constant                                      #constantExpression
    | ESCAPE_CHAR                                   #escapeCharExpression
    | IDENTIFIER                                    #identifierExpression
    | '(' expression ')'                            #parenthesizedExpression
    | 'NOT' expression                              #notExpression
    | unary expression                              #unaryExpression
    | expression multiply_op expression             #multiplyExpression
    | expression add_op expression                  #addExpression
    | expression compare_op expression              #compareExpression
    | expression bool_op expression                 #boolExpression
    | expression concat_op expression               #concatExpression
    | newline_op                                    #newlineExpression
    ;

INT_TEXT: 'INT';
CHAR_TEXT: 'CHAR';
BOOL_TEXT: 'BOOL';
FLOAT_TEXT: 'FLOAT';
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
INT: [0-9]+;    
CHAR: '\'' ~ '\'' '\'';
ESCAPE_CHAR: '[' .? ']';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
STRING: '"' (~ '"')* '"';
FLOAT: [0-9]+ ('.' [0-9]+)?;

constant: INT | CHAR | BOOL | FLOAT | STRING;

unary: '+' | '-';
multiply_op: '*' | '/' | '%';
add_op: '+' | '-';
compare_op: '>' | '<' | '>=' | '<=' | '==' | '<>';
bool_op: 'AND' | 'OR';
concat_op: '&';
newline_op: '$';

COMMENT: NEWLINE? '#' ~[\r?\n]*-> channel(HIDDEN);
NEWLINE: ('\r'? '\n')+;
WS: [ \t]+ -> skip;