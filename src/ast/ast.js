// Declarations
const INT_DECL = 'IntegerDeclaration';
const ARRAY_DECL = 'ArrayDeclaration';
const RECORD_DECL = 'RecordDeclaration';
// Expressions
// -- Literals
const BOOL_LITERAL = 'BooleanLiteral';
const INT_LITERAL = 'IntegerLiteral';
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
//const RECORD_ASSIGN_STMT = ''
const IF_STMT = 'IfStatement'
const IF_ELSE_STMT = 'IfElseStatement';
const WHILE_STMT = 'WhileStatement';
const READ_STMT = 'ReadStatement';
const WRITE_STMT = 'WriteStatement';


class Declaration {
    constructor(identifier, nodeKind) {
        this.nodeKind = nodeKind;
        this.name = identifier;
    }
}

class IntDecl extends Declaration {
    constructor(identifier) {
        super(identifier, INT_DECL);
    }

    print() {
        return 'int ' + this.name;
    }
}

class ArrayDecl extends Declaration {
    constructor(identifier, size) {
        super(identifier, ARRAY_DECL);
        this.size = size;
    }

    print() {
        return 'int[' + this.size + '] ' + this.name;
    }
}

class RecordDecl extends Declaration{
    constructor(identifier, fields) {
        super(identifier, RECORD_DECL);
        this.fields = fields;
    }

    print() {
        return '{ int fst; int snd; } ' + this.name;
    }
}

class Literal {
    constructor(nodeKind) {
        this.nodeKind = nodeKind;
    }
}

class BoolLit extends Literal {
    constructor(value) {
        super(BOOL_LITERAL);
        this.value = value;
    }
    print() {
        return this.value.toString();
    }
}

class IntLit extends Literal {
    constructor(value) {
        super(INT_LITERAL);
        this.value = value;
    }

    print() {
        return this.value.toString();
    }
}

class VarAccess {
    constructor(nodeKind) {
        this.nodeKind = nodeKind;
    }
}

class Identifier extends VarAccess {
    constructor(name) {
        super(IDENTIFIER);
        this.name = name;
    }

    print() {
        return this.name;
    }
}

class ArrayAccess extends VarAccess {
    constructor(name, expr) {
        super(ARRAY_ACCESS);
        this.name = name;
        this.index = expr;
    }

    print() {
        return this.name + '[' + this.index.print() + ']'
    }
}

class RecordAccess extends VarAccess {
    constructor(name, expr) {
        super(RECORD_ACCESS);
        this.name = name;
        this.field = expr;
    }

    print() {
        return this.name + '.' + this.field;
    }
}

class BinaryExpr {
    constructor(left, right, operator, nodeKind) {
        this.nodeKind = nodeKind;
        this.left = left;
        this.right = right;
        this.operator = operator;
    }

    print() {
        return this.left.print() + ' ' + operator + ' ' + this.right.print();
    }
}

class ABinaryExpr extends BinaryExpr {
    constructor(left, right, operator) {
        super(left, right, operator, A_BINARY_EXPR);
    }
}

class BBinaryExpr extends BinaryExpr {
    constructor(left, right, operator) {
        super(left, right, operator, B_BINARY_EXPR);
    }
}

class RBinaryExpr extends BinaryExpr {
    constructor(left, right, operator) {
        super(left, right, operator, R_BINARY_EXPR);
    }
}

class UnaryExpr {
    constructor(left, operator, nodeKind) {
        this.nodeKind = nodeKind;
        this.left = left;
        this.operator = operator;
    }

    print() {
        return this.operator + ' ' + this.left;
    }
}

class BUnaryExpr extends UnaryExpr {
    constructor(left, operator) {
        super(left, operator, B_UNARY_EXPR)
    }
}

class Statement {
    constructor(nodeKind) {
        this.nodeKind = nodeKind;
    }
}

class StatementList extends Statement {
    constructor(statements) {
        super(STMT_LIST)
        this.statements = statements;
    }
}

class AssignStmt extends Statement {
    constructor(identifier, expr) {
        super(ASSIGN_STMT);
        this.identifier = identifier;
        this.value = expr;
    }

    print() {
        return this.identifier.print() + ' := ' + this.value.print();
    }
}

class IfStmt extends Statement{
    constructor(bexpr, body) {
        super(IF_STMT);
        this.condition = bexpr;
        this.body = body;
    }

    print() {
        return 'if (' + this.condition.print() + ') {' + this.body.print() + '}';
    }
}

class IfElseStmt extends Statement {
    constructor(bexpr, ifbody, elsebody) {
        super(IF_ELSE_STMT);
        this.condition = bexpr;
        this.ifbody = ifbody;
        this.elsebody = elsebody;
    }

    print() {
        return 'if (' + this.condition.print() + ') {' + this.ifbody.print() + '} else {' + this.elsebody.print() + '}'
    }
}

class WhileStmt extends Statement{
    constructor(condition, body) {
        super(WHILE_STMT);
        this.condition = condition;
        this.body = body;
    }

    print() {
        return 'while (' + this.condition.print() + ') {' + this.body.print() + '}';
    }
}

class ReadStmt extends Statement {
    constructor(identifier) {
        super(READ_STMT);
        this.name = identifier;
    }

    print() {
        return 'Read ' + this.identifier.print();
    }
}

class WriteStmt extends Statement {
    constructor(aexpr) {
        super(WRITE_STMT);
        this.value = aexpr;
    }

    print() {
        return 'Write '+ this.identifier.print();
    }
}

module.exports = {
    constants: {
        INT_DECL,
        ARRAY_DECL,
        RECORD_DECL,
        BOOL_LITERAL,
        INT_LITERAL,
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
    },
    nodes: {
        IntDecl,
        ArrayDecl,
        RecordDecl,
        BoolLit,
        IntLit,
        Identifier,
        ArrayAccess,
        RecordAccess,
        ABinaryExpr,
        BBinaryExpr,
        RBinaryExpr,
        BUnaryExpr,

        StatementList,
        AssignStmt,
        //RecordAssignStmt,
        IfStmt,
        IfElseStmt,
        WhileStmt,
        ReadStmt,
        WriteStmt,
    }
};

const getBlocks = (program_star) => {
    const b = [];

    const helper = (program) => {
        switch(program.nodeKind) {
            case STMT_LIST:
                program.statements.forEach((e) => {
                    helper(e);
                });
            break;
            case ASSIGN_STMT:
                b.push(program);
            break;
            case IF_STMT:
                b.push(program.condition);
                helper(program.body);
            break;
            case IF_ELSE_STMT:
                b.push(program.condition);
                helper(program.ifbody);
                helper(program.elsebody);
            break;
            case WHILE_STMT:
                b.push(program.condition);
                helper(program.body);
            break;
            case READ_STMT:
                b.push(program);
            break;
            case WRITE_STMT:
                b.push(program);
            break;
            case INT_DECL:
                b.push(program);
            break;
            case ARRAY_DECL:
                b.push(program);
            break;
            case RECORD_DECL:
                b.push(program);
            break;
        }
    }

    helper(program_star);
    return b;
};

const getInit = (program) => {
    switch(program.nodeKind) {
        case STMT_LIST:
            return getInit(program.statements[0]);
        break;
        case IF_STMT:
        case IF_ELSE_STMT:
        case WHILE_STMT:
            return program.condition.label; 
        break;
        case ASSIGN_STMT:
        case READ_STMT:
        case WRITE_STMT:
        case INT_DECL:
        case ARRAY_DECL:
        case RECORD_DECL:
            return program.label;
        break;
    }
};

const getFinal = (p) => {

    const s = [];

    const helper = (program) => {
        switch(program.nodeKind) {
            case STMT_LIST:
                helper(program.statements[program.statements.length - 1]);
            break;
            case IF_STMT:
                helper(program.body);
            break;
            case IF_ELSE_STMT:
                helper(program.ifbody);
                helper(program.elsebody);
            break;
            case WHILE_STMT:
                s.push(program.condition.label); 
            break;
            case ASSIGN_STMT:
            case READ_STMT:
            case WRITE_STMT:
            case INT_DECL:
            case ARRAY_DECL:
            case RECORD_DECL:
                s.push(program.label);
            break;
        }
    }

    helper(p);
    return s;
};

// NOTE: this function mutates the argument
const labelBlocks = (blocks) => {
   let labelCount = 0; 
   blocks.forEach((b) => {
       //b.label = ++labelCount
       b.label = ++labelCount;
       //console.log(b);
   });
};

const getFlow = (p) => {
    const createNode = (src, dest) => {
        return { src, dest };
    }
    const flow = [];

    const helper = (program) => {
        switch(program.nodeKind) {
            case STMT_LIST:
                const s1 = program.statements[0];
                if (program.statements.length === 1) {
                    helper(s1);
                    break;
                }

                const s2 = new StatementList(program.statements.slice(1));
                const init_s2 = getInit(s2);
                const final_s1 = getFinal(s1);

                helper(s1);
                helper(s2);
                final_s1.forEach((lab) => {
                    flow.push(createNode(lab, init_s2))
                });
            break;
            case IF_STMT:
                helper(program.body);
                flow.push(createNode(program.condition.label, getInit(program.body)));
            break;
            case IF_ELSE_STMT:
                helper(program.ifbody);
                helper(program.elsebody)
                flow.push(createNode(program.condition.label, getInit(program.ifbody)));
                flow.push(createNode(program.condition.label, getInit(program.elsebody)));
            break;
            case WHILE_STMT:
                const init_s0 = getInit(program.body);
                const final_s0 = getFinal(program.body);

                helper(program.body);
                flow.push(createNode(program.condition.label, init_s0))
                final_s0.forEach((lab) => {
                    console.log(createNode(lab, program.condition.label))
                    flow.push(createNode(lab, program.condition.label))
                });
            break;
            case ASSIGN_STMT:
            case READ_STMT:
            case WRITE_STMT:
            case INT_DECL:
            case ARRAY_DECL:
            case RECORD_DECL:
            break;
        }
    };

    helper(p);
    return flow;
};

//console.log(JSON.stringify(program, undefined, 2));

const program = new StatementList([
    new IntDecl('x'),
    new ArrayDecl('A,', 10),
    new RecordDecl('R', ['fst', 'snd']),
    new AssignStmt(new Identifier('x'), new IntLit(3)),
    new AssignStmt(new ArrayAccess('A', new IntLit(3)), new IntLit(3)),
]);

const blocks = getBlocks(program);
labelBlocks(blocks);
console.log(blocks);

// This works becuase labelBlocks modified the pointers to the statements
console.log(getInit(program));
console.log(getFinal(program));
console.log(getFlow(program));

//===========================================
const p2 = new StatementList([
    new IntDecl('x'),
    new IntDecl('z'),
    new AssignStmt(new Identifier('x'), new IntLit(5)),
    new AssignStmt(new Identifier('z'), new IntLit(1)),
    new WhileStmt(
        new BBinaryExpr(new Identifier('x'), new IntLit(0), '>'),
        new StatementList([
            new AssignStmt(
                new Identifier('z'),
                new BBinaryExpr(new Identifier('z'), new Identifier('y'), '*')
            ),
            new AssignStmt(
                new Identifier('x'),
                new BBinaryExpr(new Identifier('x'), new IntLit(1), '-')
            ),
        ])
    ),
    new AssignStmt(new Identifier('x'), new IntLit(0)),
    new IfStmt(new BoolLit(true), new StatementList([
        new AssignStmt(new Identifier('x'), new IntLit(0)),
    ])),
    new AssignStmt(new Identifier('x'), new IntLit(0)),

    new IfElseStmt(
        new BoolLit(true),
        new StatementList([
            new AssignStmt(new Identifier('x'), new IntLit(1)),
        ]),
        new StatementList([
            new AssignStmt(new Identifier('z'), new IntLit(2)),
        ])
    ),

    new AssignStmt(new Identifier('y'), new IntLit(0)),
]);

const blocks2 = getBlocks(p2);
labelBlocks(blocks2);
console.log(blocks2);

console.log(getInit(p2));
console.log(getFinal(p2));
console.log(getFlow(p2));

const t = new AssignStmt(new Identifier('y'), new IntLit(0));

//console.log(program.print())

blocks.forEach((e) => console.log(e.print()));