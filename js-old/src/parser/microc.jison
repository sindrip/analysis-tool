/* const ast = require('./../ast/nodes'); */
/* lexical grammar */
%lex

%%
\s+                   /* skip whitespace */

/* keywords */
"int"                 return 'INT';
"fst"                 return 'FST';
"snd"                 return 'SND';
"if"                  return 'IF';
"else"                return 'ELSE';
"while"               return 'WHILE';
"read"                return 'READ';
"write"               return 'WRITE';
"true"                return 'TRUE';
"false"               return 'FALSE';
"not"                 return 'NOT';

[0-9]+                return 'NUMBER';
[A-Za-z]+             return 'NAME';

"*"                   return '*';
"/"                   return '/';
"%"                   return '%';
"-"                   return '-';
"+"                   return '+';

"<="                  return 'LE';
">="                  return 'GE';
"<"                   return 'LT';
">"                   return 'GT';
"=="                  return 'EQ';
"!="                  return 'NOT_EQ';

"&"                   return 'AND';
"|"                   return 'OR';

":="                  return 'ASSIGN';

/* Delimiters */
"("                   return '(';
")"                   return ')';
"{"                   return '{';
"}"                   return '}';
"["                   return '[';
"]"                   return ']';
";"                   return ';';
"."                   return '.';

<<EOF>>               return 'EOF';
.                     return 'INVALID';

/lex

/* operator associations and precedence */

%left '+' '-'
%left '*' '/' '%'

%left OR
%left AND
%left NOT

%start program

%% /* language grammar */

program
    : '{' declaration_list statement_list '}' EOF
        { return { vars: $2, statements: $3 }}
    ;

declaration_list
    : declaration_list declaration
        { $$ = $1.concat($2); }
    | /* empty */
        { $$ = []; }
    ;

declaration
    : INT NAME ';'
        { $$ = yy.intDeclaration($2); }
    | INT '[' NUMBER ']' NAME ';'
        { $$ = yy.arrayDeclaration($5, $3); }
    | '{' INT FST ';' INT SND '}' NAME ';'
        { $$ = yy.recordDeclaration($8); }
    ;

statement_list
    : statement_list statement
        { $$ = $1.concat($2); }
    | statement
        { $$ = [$1]; }
    ;

statement
    : NAME ASSIGN a_expr ';'
        { $$ = yy.assignmentNode(yy.varIdentifier($1), $3); }
    | NAME '[' a_expr ']' ASSIGN a_expr ';'
        { $$ = yy.assignmentNode(yy.arrayIdentifier($1, $3), $6); }
    | NAME '.' FST ASSIGN a_expr ';'
        { $$ = yy.assignmentNode(yy.recordIdentifier($1, 'fst'), $5); }
    | NAME '.' SND ASSIGN a_expr ';'
        { $$ = yy.assignmentNode(yy.recordIdentifier($1, 'snd'), $5); }
    | NAME ASSIGN '(' a_expr ';' a_expr ')' ';'
        { $$ = { 'token': 'RECORD_ASSIGN', 'name': $1, fields: [ $1, $2] }; }

    | IF '(' b_expr ')' '{' statement_list '}'
        { $$ = yy.ifNode($3, $6); }
    | IF '(' b_expr ')' '{' statement_list '}' ELSE '{' statement_list '}'
        { $$ = yy.ifElseNode($3, $6, $10); }
    | WHILE '(' b_expr ')' '{' statement_list '}'
        { $$ = yy.whileNode($3, $6); }
    | READ variable ';'
        { $$ = yy.readNode($2); }
    | WRITE a_expr ';'
        { $$ = yy.writeNode($2); }
    ;

b_expr
    : TRUE
        { $$ = yy.booleanLiteral(true); }
    | FALSE
        { $$ = yy.booleanLiteral(false); }
    | NOT b_expr
        { $$ = { token: 'NOT', value: $2 }; }
    | b_expr AND b_expr
        { $$ = yy.booleanBinopExpression($1, '&', $3); }
    | b_expr OR b_expr
        { $$ = yy.booleanBinopExpression($1, '|', $3); }
    | a_expr op_r a_expr
        { $$ = yy.relationalExpression($1, $2, $3); }
    ;

op_r
    : LT
        { $$ = 'LT'; }
    | GT
        { $$ = 'GT'; }
    | LE
        { $$ = 'LE'; }
    | GE
        { $$ = 'GE'; }
    | EQ
        { $$ = 'EQ'; }
    | NOT_EQ
        { $$ = 'NOT_EQ'; }
    ;

a_expr
    : NUMBER
        { $$ = yy.integerLiteral($1); }
    | variable
        { $$ = $1 }
    | a_expr '+' a_expr
        { $$ = yy.arithmeticBinopExpression($1, '+', $3); }
    | a_expr '-' a_expr
        { $$ = yy.arithmeticBinopExpression($1, '-', $3); }
    | a_expr '*' a_expr
        { $$ = yy.arithmeticBinopExpression($1, '*', $3); }
    | a_expr '/' a_expr
        { $$ = yy.arithmeticBinopExpression($1, '/', $3); }
    ;

variable
    : NAME
        { $$ = yy.varIdentifier($1); }
    | NAME '[' a_expr ']'
        { $$ = yy.arrayIdentifier($1, $3); }
    | NAME '.' FST
        { $$ = yy.recordIdentifier($1, 'fst'); }
    | NAME '.' SND
        { $$ = yy.recordIdentifier($1, 'snd'); }
    ;