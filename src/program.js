const flow = require('./flow');
const { nodes } = require('./ast');

class Program {
    constructor(program) {
        this.program = program;
        this.blocks = [];
        this.init = null;
        this.final = [];
        this.flow = [];

        this.calculateFlow();
    }

    calculateFlow() {
        if (this.blocks.length !== 0) {
            return;
        }

        flow.blocks(this.program, this.blocks);
        flow.labelBlocks(this.blocks);
        // Taking init of the program works because the blocks are references to nodes in the program
        // This means that the program blocks now have the labels
        this.init = flow.init(this.program);
        flow.final(this.program, this.final);

        flow.flow(this.program, this.flow);
    }
}

const example = new nodes.StatementList([
    new nodes.IntDecl('x'),
    new nodes.ArrayDecl('A,', 10),
    new nodes.RecordDecl('R', ['fst', 'snd']),
    new nodes.AssignStmt(new nodes.Identifier('x'), new nodes.IntLit(3)),
    new nodes.AssignStmt(new nodes.ArrayAccess('A', new nodes.IntLit(3)), new nodes.IntLit(3)),
    new nodes.AssignStmt(new nodes.Identifier('R'), [ new nodes.IntLit(4), new nodes.ArrayAccess('A', new nodes.IntLit(3))]),
]);

const p = new Program(example);

console.log(p);
console.log(JSON.stringify(p, undefined,2));
