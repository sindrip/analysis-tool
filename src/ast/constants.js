// Declarations
const INT_DECL = 'IntegerDeclaration';
const ARRAY_DECL = 'ArrayDeclaration';
const RECORD_DECL = 'RecordDeclaration';

// Expressions
// -- Literals
const BOOL_LITERAL = 'BooleanLiteral';
const INT_LITERAL = 'IntegerLiteral';
// The tuple literal is of the form (x_0, x_1, ..., x_n)
// For the kernel implementation it is always of the form (x_0, x_1)
const TUPLE_LITERAL = 'TupleLiteral';
// -- Value Access
const IDENTIFIER = 'Identifier';
const ARRAY_ACCESS = 'ArrayAccess';
const RECORD_ACCESS = 'RecordAccess';
// -- Binary Expressions
const A_BINARY_EXPR = 'ArithmeticBinaryExpression';
const B_BINARY_EXPR = 'BooleanBinaryExpression';
const R_BINARY_EXPR = 'RelationalBinaryExpression';
// -- Unary Expression
const B_UNARY_EXPR = 'BooleanUnaryExpression';

// Statements
const STMT_LIST = 'StatementList';
const ASSIGN_STMT = 'AssignStatement';
const IF_STMT = 'IfStatement'
const IF_ELSE_STMT = 'IfElseStatement';
const WHILE_STMT = 'WhileStatement';
const READ_STMT = 'ReadStatement';
const WRITE_STMT = 'WriteStatement';

module.exports = {
    INT_DECL,
    ARRAY_DECL,
    RECORD_DECL,
    BOOL_LITERAL,
    INT_LITERAL,
    TUPLE_LITERAL,
    IDENTIFIER,
    ARRAY_ACCESS,
    RECORD_ACCESS,
    A_BINARY_EXPR,
    B_BINARY_EXPR,
    R_BINARY_EXPR,
    B_UNARY_EXPR,
    STMT_LIST,      
    ASSIGN_STMT,
    IF_STMT,
    IF_ELSE_STMT,
    WHILE_STMT,
    READ_STMT,
    WRITE_STMT,
};