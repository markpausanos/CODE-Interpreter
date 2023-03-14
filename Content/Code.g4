﻿grammar Code;

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

statement: declaration | assignment | function_call;

declaration: DATA_TYPE variable (',' variable)*;
variable: IDENTIFIER? ('=' (expression))?;
assignment: variable (',' variable)* ('=' expression);
function_call: IDENTIFIER ':' expression;

DATA_TYPE: 'INT' | 'CHAR' | 'BOOL' | 'FLOAT';
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
INT: ('-')?[0-9]+;    
CHAR: '\'' ~ '\'' '\'';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
FLOAT: ('-')?[0-9]+ '.' [0-9]+?;
constant: INT | CHAR | BOOL | FLOAT;
expression
    : constant                                      #constantExpression
    | IDENTIFIER                                    #identifierExpression
//  | function_call                                 #function_callExpression
    | 'NOT' expression                              #notExpression
    | '(' expression ')'                            #parenthesizedExpression
    | expression multiply_op expression             #multiplyExpression
    | expression add_op expression                  #addExpression
    | expression compare_op expression              #compareExpression
    | expression bool_op expression                 #boolExpression
    ;

multiply_op: '*' | '/' | '%';
add_op: '+' | '-';
compare_op: '>' | '<' | '>=' | '<=' | '==' | '<>';
bool_op: 'AND' | 'OR';

COMMENT: '#' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;