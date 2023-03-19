grammar Code;

program: begin_code line* end_code;

line: if_block | statement;

begin_code: 'BEGIN' CODE NEWLINE;
end_code: 'END' CODE EOF;
// code_block: begin_code (line (NEWLINE line)* NEWLINE?)? end_code;
CODE: 'CODE';   

if_block: 'IF' '(' expression ')' else_block ('ELSE' elseIfBlock)*;
begin_if: 'BEGIN' IF NEWLINE;
end_if: 'END' IF NEWLINE;
else_block: begin_if line* end_if;
IF: 'IF';

elseIfBlock: if_block | else_block;

statement: declaration | assignment | function_call;

declaration: DATA_TYPE variable (',' variable)* NEWLINE;
variable: IDENTIFIER ('=' (expression))?;
assignment: IDENTIFIER ('=' IDENTIFIER)* '=' expression NEWLINE;
function_call: IDENTIFIER ':' expression NEWLINE;

DATA_TYPE: 'INT' | 'CHAR' | 'BOOL' | 'FLOAT';
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
INT: [0-9]+;    
CHAR: '\'' ~ '\'' '\'';
STRING: '"' . '"';
BOOL: ('"' 'TRUE' '"') | ('"' 'FALSE' '"');
FLOAT: [0-9]+ '.' [0-9]+?;
UNARY: '+' | '-';

constant: INT | CHAR | BOOL | FLOAT | STRING;
expression
    : constant                                      #constantExpression
    | IDENTIFIER                                    #identifierExpression
    | 'NOT' expression                              #notExpression
    | '(' expression ')'                            #parenthesizedExpression
    | '[' expression ']'                            #bracketizedExpression
    | expression multiply_op expression             #multiplyExpression
    | expression add_op expression                  #addExpression
    | expression compare_op expression              #compareExpression
    | expression bool_op expression                 #boolExpression
    | UNARY expression                              #unaryExpression
    ;

multiply_op: '*' | '/' | '%';
add_op: '+' | '-';
compare_op: '>' | '<' | '>=' | '<=' | '==' | '<>';
bool_op: 'AND' | 'OR';

COMMENT: '#' ~[\r\n]* NEWLINE -> skip;
NEWLINE   : '\r'? '\n';
WS: [ \t]+ -> skip;