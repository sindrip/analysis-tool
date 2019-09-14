const constants = require('./constants');

class Declaration {
    constructor(identifier, nodeKind) {
        this.nodeKind = nodeKind;
        this.name = identifier;
    }
}

class IntDecl extends Declaration {
    constructor(identifier) {
        super(identifier, constants.INT_DECL);
    }

    print() {
        return 'int ' + this.name;
    }
}

class ArrayDecl extends Declaration {
    constructor(identifier, size) {
        super(identifier, constants.ARRAY_DECL);
        this.size = size;
    }

    print() {
        return 'int[' + this.size + '] ' + this.name;
    }
}

class RecordDecl extends Declaration{
    constructor(identifier, fields) {
        super(identifier, constants.RECORD_DECL);
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
        super(constants.BOOL_LITERAL);
        this.value = value;
    }
    print() {
        return this.value.toString();
    }
}

class IntLit extends Literal {
    constructor(value) {
        super(constants.INT_LITERAL);
        this.value = value;
    }

    print() {
        return this.value.toString();
    }
}

class TupleLit extends Literal {
    constructor(values) {
        super(constants.TUPLE_LITERAL);
        this.values = values;
    }

    print() {
        return '(' + this.values.map((v) => v.print()).join(',') + ')';
    }
}

class VarAccess {
    constructor(nodeKind) {
        this.nodeKind = nodeKind;
    }
}

class Identifier extends VarAccess {
    constructor(name) {
        super(constants.IDENTIFIER);
        this.name = name;
    }

    print() {
        return this.name;
    }
}

class ArrayAccess extends VarAccess {
    constructor(name, expr) {
        super(constants.ARRAY_ACCESS);
        this.name = name;
        this.index = expr;
    }

    print() {
        return this.name + '[' + this.index.print() + ']'
    }
}

class RecordAccess extends VarAccess {
    constructor(name, expr) {
        super(constants.RECORD_ACCESS);
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
        super(left, right, operator, constants.A_BINARY_EXPR);
    }
}

class BBinaryExpr extends BinaryExpr {
    constructor(left, right, operator) {
        super(left, right, operator, constants.B_BINARY_EXPR);
    }
}

class RBinaryExpr extends BinaryExpr {
    constructor(left, right, operator) {
        super(left, right, operator, constants.R_BINARY_EXPR);
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
        super(left, operator, constants.B_UNARY_EXPR)
    }
}

class Statement {
    constructor(nodeKind) {
        this.nodeKind = nodeKind;
    }
}

class StatementList extends Statement {
    constructor(statements) {
        super(constants.STMT_LIST)
        this.statements = statements;
    }
}

class AssignStmt extends Statement {
    constructor(identifier, expr) {
        super(constants.ASSIGN_STMT);
        this.identifier = identifier;
        this.value = expr;
    }

    print() {
        return this.identifier.print() + ' := ' + this.value.print();
    }
}

class IfStmt extends Statement{
    constructor(bexpr, body) {
        super(constants.IF_STMT);
        this.condition = bexpr;
        this.body = body;
    }

    print() {
        return 'if (' + this.condition.print() + ') {' + this.body.print() + '}';
    }
}

class IfElseStmt extends Statement {
    constructor(bexpr, ifbody, elsebody) {
        super(constants.IF_ELSE_STMT);
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
        super(constants.WHILE_STMT);
        this.condition = condition;
        this.body = body;
    }

    print() {
        return 'while (' + this.condition.print() + ') {' + this.body.print() + '}';
    }
}

class ReadStmt extends Statement {
    constructor(identifier) {
        super(constants.READ_STMT);
        this.name = identifier;
    }

    print() {
        return 'Read ' + this.identifier.print();
    }
}

class WriteStmt extends Statement {
    constructor(aexpr) {
        super(constants.WRITE_STMT);
        this.value = aexpr;
    }

    print() {
        return 'Write '+ this.identifier.print();
    }
}

module.exports = {
    IntDecl,
    ArrayDecl,
    RecordDecl,

    BoolLit,
    IntLit,
    TupleLit,
    Identifier,
    ArrayAccess,
    RecordAccess,
    ABinaryExpr,
    BBinaryExpr,
    RBinaryExpr,
    BUnaryExpr,

    StatementList,
    AssignStmt,
    IfStmt,
    IfElseStmt,
    WhileStmt,
    ReadStmt,
    WriteStmt,
};
