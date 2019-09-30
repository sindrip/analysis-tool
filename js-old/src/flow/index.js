const { constants, nodes } = require('./../ast');

class Node {
    constructor(src, dest) {
        this.src = src;
        this.dest=  dest;
    }
}

// program : ASTNode
// blocksArray : []
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

// program : ASTNode
// flowArray : []
const flow = (program, flowArray) => {
    switch(program.nodeKind) {
        case constants.STMT_LIST:
            const s1 = program.statements[0];
            if (program.statements.length === 1) {
                flow(s1, flowArray);
                break;
            }

            const s2 = new nodes.StatementList(program.statements.slice(1));
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
            flow(program.body, flowArray);
            flowArray.push(new Node(program.condition.label, init(program.body)));
        break;
        case constants.IF_ELSE_STMT:
            flow(program.ifbody, flowArray);
            flow(program.elsebody, flowArray)
            flowArray.push(new Node(program.condition.label, init(program.ifbody)));
            flowArray.push(new Node(program.condition.label, init(program.elsebody)));
        break;
        case constants.WHILE_STMT:
            const init_s0 = init(program.body);
            const final_s0 = [];
            final(program.body, final_s0);

            flow(program.body, flowArray);
            flowArray.push(new Node(program.condition.label, init_s0))
            final_s0.forEach((lab) => {
                flowArray.push(new Node(lab, program.condition.label))
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

// reverseFlow takes a flow array of the form [ { src: s_1, dest: d_1 }, ... { src: s_n, dest d_n} ]
// and returns [ { src: d_1, dest: s_1 }, ... { src: d_n, dest s_n} ]
const reverseFlow = flowArray => flowArray.map(({src, dest}) => {
    return { src: dest, dest: src}; 
});

module.exports = {
    blocks,
    labelBlocks,
    init,
    final,
    flow,
    reverseFlow,
};
