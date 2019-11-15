grammar MicroC;

/*
 * Parser Rules
 */
 
parse : globalBlock EOF ;

globalBlock
    : LBRACE declaration* statement+ RBRACE
    ;
    
scopedBlock
    : LBRACE declaration* statement+ RBRACE
    ;
    
unscopedBlock
    : LBRACE statement+ RBRACE
    ;
    
declaration
    : INT IDENT SEMICOLON #intDecl
    | INT LBRACKET NUMBER RBRACKET IDENT SEMICOLON #arrayDecl
    | LBRACE fieldDeclaration (SEMICOLON fieldDeclaration)* RBRACE name=IDENT SEMICOLON #recDecl
    ;
    
fieldDeclaration
    : INT IDENT
    ;
    
statement
    : IDENT ASSIGN a_expr SEMICOLON #assignStmt
    | IDENT LBRACKET index=a_expr RBRACKET ASSIGN value=a_expr SEMICOLON #assignArrayStmt
    | name=IDENT DOT field=IDENT ASSIGN a_expr SEMICOLON #assignFieldStmt
    | IDENT ASSIGN LPAREN a_expr (COMMA a_expr)* RPAREN SEMICOLON #assignRecStmt
    | IF LPAREN b_expr RPAREN unscopedBlock #ifStmt
    | IF LPAREN b_expr RPAREN ifBody=unscopedBlock ELSE elseBody=unscopedBlock #ifElseStmt
    | WHILE LPAREN b_expr RPAREN unscopedBlock #whileStmt
    | READ IDENT SEMICOLON #readStmt
    | WRITE a_expr SEMICOLON #writeStmt
    ;
    
a_expr
    : LPAREN a_expr RPAREN #aexprParen
    | op=MINUS left=a_expr #aexprUnaryMinus
    | left=a_expr op=( MULT | DIV ) right=a_expr #aexprProduct
    | left=a_expr op=( PLUS | MINUS ) right=a_expr #aexprSum
    | IDENT #aexprVar
    | IDENT LBRACKET a_expr RBRACKET #aexprArrayAccess
    | name=IDENT DOT field=IDENT #aexprRecAccess
    | NUMBER #aexprLiteral
    ;
    
b_expr
    : LPAREN b_expr RPAREN #bexprParen
    | NOT b_expr #bexprNot
    | left=b_expr bop=( AND | OR ) right=b_expr #bexprBinop
    | left=a_expr rop=( LT | GT | LTEQ | GTEQ | EQ | NOTEQ ) right=a_expr #rexprBinop
    | value=(TRUE | FALSE) #bexprLit
    ;

/*
 * Lexer Rules
 */

INT : 'int' ; 
IF : 'if' ;
ELSE : 'else' ; 
WHILE : 'while' ;
READ : 'read' ;
WRITE : 'write' ;
TRUE : 'true' ;
FALSE : 'false' ;
NOT : 'not' ; 

EQ : '==' ;
NOTEQ : '!=' ;
LTEQ : '<=' ;
GTEQ : '>=' ;

ASSIGN : ':=';

LT : '<' ;
GT : '>' ;
OR : '|' ;
AND : '&' ;

PLUS : '+' ;
MINUS : '-' ;
MULT : '*' ;
DIV : '/' ;
MOD : '%' ;

LBRACE : '{' ;
RBRACE : '}' ;
LPAREN : '(' ;
RPAREN : ')' ;
LBRACKET : '[' ;
RBRACKET : ']' ;
SEMICOLON : ';' ;
COMMA : ',' ;
DOT : '.' ;

NUMBER : [0-9]+ ;

IDENT : [a-zA-Z]+ ;

WHITESPACE : [ \t\r\n\u000C] -> skip ;
