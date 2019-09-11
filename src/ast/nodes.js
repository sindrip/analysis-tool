const DECLARATION_NODE = 'DeclarationStatement'
const ASSIGNMENT_NODE = 'AssignmentStatement';
const WHILE_NODE = 'WhileStatement';
const IF_NODE = 'IfStatement';
const IF_ELSE_NODE = 'IfElseStatement';
const READ_NODE = 'ReadStatement';
const WRITE_NODE = 'WriteStatement';
const BOOLEAN_EXPRESSION = 'BooleanExpression';
const BOOLEAN_BINOP_EXPRESSION = 'BooleanBinopExpression';
const RELATIONAL_EXPRESSION = 'RelationalExpression';
const ARITHMETIC_EXPRESSION  = 'ArithmeticExpression';
const ARITHMETIC_BINOP_EXPRESSION = 'ArithmeticBinopExpression';
const BOOLEAN_LITERAL = 'BooleanLiteral';
const INTEGER_LITERAL = 'IntegerLiteral';

const VAR_IDENTIFIER = 'VarIdentifier';
const ARRAY_IDENTIFIER = 'ArrayIdentifier';
const RECORD_IDENTIFIER = 'RecordIdentifier';

const TYPE_INT = 'IntegerType';
const TYPE_BOOLEAN = 'BooleanType';
const TYPE_ARRAY = 'ArrayType';
const TYPE_RECORD = 'RecordType';


const intDeclaration = (name) => {
    return {
        nodeType: DECLARATION_NODE,
        type: TYPE_INT,
        name,
    }
};

const arrayDeclaration = (name, size) => {
    return {
        nodeType: DECLARATION_NODE,
        type: TYPE_ARRAY,
        name,
        size,
    };
};

const recordDeclaration = (name) => {
    return {
        nodeType: DECLARATION_NODE,
        type: TYPE_RECORD,
        name,
        fields: [
           { name: 'fst', type: TYPE_INT, },
           { name: 'snd', type: TYPE_INT, }, 
        ],
    };
};

const assignmentNode = (name, expr) => {
    return {
        type: ASSIGNMENT_NODE,
        name,
        expr,
    };
};

const whileNode = (condition, body) => {
    return {
        nodeType: WHILE_NODE,
        condition,
        body,
    };
};

const ifNode = (condition, body) => {
    return {
        nodeType: IF_NODE,
        condition,
        body,
    };
};

const ifElseNode = (condition, ifbody, elsebody) => {
    return {
        nodeType: IF_ELSE_NODE,
        condition,
        ifbody,
        elsebody,
    };
};

const readNode = (name) => {
    return {
        nodeType: READ_NODE,
        name,
    };
};

const writeNode = (expr) => {
    return {
        nodeType: WRITE_NODE,
        expr,
    };
};

const booleanExpression = (value) => {
    return {
        nodeType: BOOLEAN_EXPRESSION,
        value,
    };
};

const booleanBinopExpression = (left, operator, right) => {
    return {
        nodeType: BOOLEAN_BINOP_EXPRESSION,
        left,
        operator,
        right,
    };
};

const relationalExpression = (left, operator, right) => {
    return {
        nodeType: RELATIONAL_EXPRESSION,
        left,
        operator,
        right,
    };
};

const booleanLiteral = (value) => {
    return {
        nodeType: BOOLEAN_LITERAL,
        value,
    };
};

const arithmeticExpression = (value) => {
    return {
        nodeType: ARITHMETIC_EXPRESSION,
        value,
    };
};

const arithmeticBinopExpression = (left, operator, right) => {
    return {
        nodeType: ARITHMETIC_BINOP_EXPRESSION,
        left,
        operator,
        right,
    };
};

const integerLiteral = (value) => {
    return {
        nodeType: INTEGER_LITERAL,
        value,
    };
};

const varIdentifier = (name) => {
    return {
        nodeType: VAR_IDENTIFIER,
        name,
    };
};

const arrayIdentifier = (name, index) => {
    return {
        nodeType: ARRAY_IDENTIFIER,
        name,
        index,
    };
};

const recordIdentifier = (name, field) => {
    return {
        nodeType: RECORD_IDENTIFIER,
        name,
        field,
    };
};

module.exports = {
    constants: {
        DECLARATION_NODE,
        ASSIGNMENT_NODE,
        WHILE_NODE,
        IF_NODE,
        IF_ELSE_NODE,
        READ_NODE,
        WRITE_NODE,
        BOOLEAN_EXPRESSION,
        BOOLEAN_BINOP_EXPRESSION,
        RELATIONAL_EXPRESSION,
        ARITHMETIC_EXPRESSION,
        ARITHMETIC_BINOP_EXPRESSION,
        BOOLEAN_LITERAL,
        INTEGER_LITERAL,
        TYPE_INT,
        TYPE_BOOLEAN,
        TYPE_ARRAY,
        TYPE_RECORD,
        VAR_IDENTIFIER,
        ARRAY_IDENTIFIER,
        RECORD_IDENTIFIER,
    },

    intDeclaration,
    arrayDeclaration,
    recordDeclaration,

    varIdentifier,
    arrayIdentifier,
    recordIdentifier,
    integerLiteral,
    booleanLiteral,
    arithmeticExpression,
    arithmeticBinopExpression,
    booleanExpression,
    booleanBinopExpression,
    relationalExpression,

    assignmentNode,
    whileNode,
    ifNode,
    ifElseNode,
    readNode,
    writeNode,
}