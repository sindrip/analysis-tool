const { constants, nodes } = require('./../ast');

class Node {
    constructor(src, dest) {
        this.src = src;
        this.dest=  dest;
    }
}

// program : ASTNode
// blocks : []
const blocks = (program, blocksArray) => {
    switch(program.nodeKind) {
        case constants.STMT_LIST:
            program.statements.forEach((s) => {
                blocks(s, blocksArray);
            });
        break;
        case constants.IF_STMT:
        case constants.WHILE_STMT:
            blocksArray.push(program.condition);
            blocks(program.body, blocksArray);
        case constants.IF_ELSE_STMT:
            blocksArray.push(program.condition);
            blocks(program.ifbody, blocksArray);
            blocks(program.elsebody, blocksArray);
        break;
        case constants.ASSIGN_STMT:
        case constants.READ_STMT:
        case constants.WRITE_STMT:
        case constants.INT_DECL:
        case constants.ARRAY_DECL:
        case constants.RECORD_DECL:
            blocksArray.push(program);
        break;
    }
};

// blocks : []
// NOTE that the nodes of the blocks array are references to the nodes in the program
//      therefore this also mutates the nodes of the program
const labelBlocks = (blocks) => {
    let labelCount = 0; 
    blocks.forEach((b) => {
        b.label = ++labelCount;
    });
};

// program : ASTNode
const init = (program) => {
    switch(program.nodeKind) {
        case constants.STMT_LIST:
            // init(S1 S2) = init(S1), in our case we can safely take the first array entry
            const firstStatement = program.statements[0];
            return init(firstStatement);
        break;
        case constants.IF_STMT:
        case constants.IF_ELSE_STMT:
        case constants.WHILE_STMT:
            // The label is on the condition in the control flow statements
            return program.condition.label; 
        break;
        case constants.ASSIGN_STMT:
        case constants.READ_STMT:
        case constants.WRITE_STMT:
        case constants.INT_DECL:
        case constants.ARRAY_DECL:
        case constants.RECORD_DECL:
            return program.label;
        break;
    }
};

// program : ASTNode
// finalArray : []
const final = (program, finalArray) => {
    switch(program.nodeKind) {
        case constants.STMT_LIST:
            // We can safely take the last statement, according to the language grammar the empty program is not allowed
            // Every derivation of S must have at least one statement as \epsilon is not in first(S)
            const lastStatement = program.statements[program.statements.length -1];
            final(lastStatement, finalArray);
        break;
        case constants.IF_STMT:
            final(program.body, finalArray);
        break;
        case constants.IF_ELSE_STMT:
            final(program.ifbody, finalArray);
            final(program.elsebody, finalArray);
        break;
        case constants.WHILE_STMT:
            finalArray.push(program.condition.label); 
        break;
        case constants.ASSIGN_STMT:
        case constants.READ_STMT:
        case constants.WRITE_STMT:
        case constants.INT_DECL:
        case constants.ARRAY_DECL:
        case constants.RECORD_DECL:
            finalArray.push(program.label);
        break;
    }
};

// blocks : ASTNode
// flowArray : []
const flow = (blocks, flowArray) => {
    switch(blocks.nodeKind) {
        case constants.STMT_LIST:
            const s1 = blocks.statements[0];
            if (blocks.statements.length === 1) {
                flow(s1, flowArray);
                break;
            }

            const s2 = new nodes.StatementList(blocks.statements.slice(1));
            const init_s2 = init(s2);
            const final_s1 = [];
            final(s1, final_s1);

            flow(s1,flowArray);
            flow(s2, flowArray);
            final_s1.forEach((lab) => {
                flowArray.push(new Node(lab, init_s2))
            });
        break;
        case constants.IF_STMT:
            flow(blocks.body, flowArray);
            flowArray.push(new Node(blocks.condition.label, init(blocks.body)));
        break;
        case constants.IF_ELSE_STMT:
            flow(blocks.ifbody, flowArray);
            flow(blocks.elsebody, flowArray)
            flowArray.push(new Node(blocks.condition.label, init(blocks.ifbody)));
            flowArray.push(new Node(blocks.condition.label, init(blocks.elsebody)));
        break;
        case constants.WHILE_STMT:
            const init_s0 = init(blocks.body);
            const final_s0 = [];
            final(blocks.body, final_s0);

            flow(blocks.body, flowArray);
            flowArray.push(new Node(blocks.condition.label, init_s0))
            final_s0.forEach((lab) => {
                flowArray.push(new Node(lab, blocks.condition.label))
            });
        break;
        case constants.ASSIGN_STMT:
        case constants.READ_STMT:
        case constants.WRITE_STMT:
        case constants.INT_DECL:
        case constants.ARRAY_DECL:
        case constants.RECORD_DECL:
            // Do nothing
        break;
    }
};

module.exports = {
    blocks,
    labelBlocks,
    init,
    final,
    flow,
};
