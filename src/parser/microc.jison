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
        /*{ console.log( JSON.stringify( { vars: $2, statements: $3 }, undefined, 2 )); }*/
        /*{ console.log({ vars: $2, statements: $3 }); }*/
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
        { $$ = { 'type': 'IntDecl', name: $2 }; }
    | INT '[' NUMBER ']' NAME ';'
        { $$ = { 'type': 'ArrayDecl', name: $5 }; }
    | '{' INT FST ';' INT SND '}' NAME ';'
        { $$ = { 'type': 'RecordDecl', name: $8 }; }
    ;

statement_list
    : statement_list statement
        { $$ = $1.concat($2); }
    | statement
        { $$ = [$1]; }
    ;

statement
    /*: variable ASSIGN a_expr ';'
        { $$ = {'token': 'ASSIGN', left: $1, right: $3 }; }
    | NAME ASSIGN '(' a_expr ';' a_expr ')' ';'
        { $$ = { 'token': 'RECORD_ASSIGN', 'name': $1, fields: [ $1, $2] }; }
    */
    : NAME ASSIGN a_expr ';'
        { $$ = {'token': 'ASSIGN', left: { token: 'VAR', name: $1 }, right: $3 }; }
    | NAME '[' a_expr ']' ASSIGN a_expr ';'
        { $$ = {'token': 'ASSIGN', left: { token: 'ARRAY', name: $1, index: $3 }, right: $6 }; }
    | NAME '.' FST ASSIGN a_expr ';'
        { $$ = {'token': 'ASSIGN', left: { token: 'RECORD_FIELD', field: 'fst' }, right: $5 }; }
    | NAME '.' SND ASSIGN a_expr ';'
        { $$ = {'token': 'ASSIGN', left: { token: 'RECORD_FIELD', field: 'snd' }, right: $5 }; }
    | NAME ASSIGN '(' a_expr ';' a_expr ')' ';'
        { $$ = { 'token': 'RECORD_ASSIGN', 'name': $1, fields: [ $1, $2] }; }

    | IF '(' b_expr ')' '{' statement_list '}'
        { $$ = { 'token': 'IF', 'condition': $3, 'body': $6 }; }
    | IF '(' b_expr ')' '{' statement_list '}' ELSE '{' statement_list '}'
        { $$ = { 'token': 'IFELSE', 'condition': $3, 'ifbody': $6, 'elsebody': $10 }; }
    | WHILE '(' b_expr ')' '{' statement_list '}'
        { $$ = { 'token': 'WHILE', 'condition': $3, 'body': $6 };}
    | READ variable ';'
        { $$ = { 'token': 'READ', value: $2 }; }
    | WRITE a_expr ';'
        { $$ = { 'token': 'WRITE', value: $2 }; }
    ;

b_expr
    : TRUE
        { $$ = { token: 'BOOLEAN', value: true }; }
    | FALSE
        { $$ = { token: 'BOOLEAN', value: false }; }
    | NOT b_expr
        { $$ = { token: 'NOT', value: $2 }; }
    | b_expr AND b_expr
        { $$ = { token: 'AND', left: $1, right: $3}; }
    | b_expr OR b_expr
        { $$ = { token: 'OR', left: $1, right: $3}; }
    | a_expr op_r a_expr
        { $$ = { token: $2, left: $1, right: $3}; }
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
        { $$ = { token: 'INTEGER', value: $1 }; }
    | variable
        { $$ = $1 }
    | a_expr '+' a_expr
        { $$ = { token: 'PLUS', left: $1, right: $3 }; }
    | a_expr '-' a_expr
        { $$ = { token: 'MINUS', left: $1, right: $3 }; }
    | a_expr '*' a_expr
        { $$ = { token: 'MULT', left: $1, right: $3 }; }
    | a_expr '/' a_expr
        { $$ = { token: 'DIV', left: $1, right: $3 }; }
    ;

variable
    : NAME
        { $$ = { token: 'VAR', name: $1 }; }
    | NAME '[' a_expr ']'
        { $$ = { token: 'ARRAY', name: $1, index: $3 }; }
    | NAME '.' FST
        { $$ = { token: 'RECORD_FIELD', field: 'fst' }; }
    | NAME '.' SND
        { $$ = { token: 'RECORD_FIELD', field: 'snd' }; }
    ;