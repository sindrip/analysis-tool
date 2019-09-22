const flow = require('./flow');
const { constants, nodes } = require('./ast');

class Program {
    constructor(program) {
        this.program = program;
        this.blocks = [];
        this.init = null;
        this.final = [];
        this.flow = [];
        this.reverseFlow = null;
        this.variables = [];

        this.calculateFlow();
        this.initVariables(this.program);
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
        this.reverseFlow = flow.reverseFlow(this.flow);
    }

    initVariables(program) {
        switch(program.nodeKind) {
            case constants.STMT_LIST:
                program.statements.forEach((s) => {
                    this.initVariables(s);
                });
            break;
            case constants.INT_DECL:
                this.variables.push({ name: program.name, type: 'int' });
            break;
            case constants.ARRAY_DECL:
                this.variables.push({ name: program.name, type: 'array' });
            break;
            case constants.RECORD_DECL:
                this.variables.push({ name: program.name, fields: program.fields, type: 'record' });
            break;
            case constants.ASSIGN_STMT:
            case constants.IF_STMT:
            case constants.WHILE_STMT:
            case constants.IF_ELSE_STMT:
            case constants.READ_STMT:
            case constants.WRITE_STMT:
            case constants.INT_DECL:
            case constants.ARRAY_DECL:
            case constants.RECORD_DECL:
            break;
        }
    }
}

const example = new nodes.StatementList([
    new nodes.IntDecl('x'),
    new nodes.ArrayDecl('A,', 10),
    new nodes.RecordDecl('R', [ { name: 'fst' }, { name: 'snd' } ]),
    new nodes.AssignStmt(new nodes.Identifier('x'), new nodes.IntLit(3)),
    new nodes.AssignStmt(new nodes.ArrayAccess('A', new nodes.IntLit(3)), new nodes.IntLit(3)),
    new nodes.AssignStmt(new nodes.Identifier('R'), [ new nodes.IntLit(4), new nodes.ArrayAccess('A', new nodes.IntLit(3))]),
]);

const p = new Program(example);

console.log(p);
//console.log(JSON.stringify(p, undefined,2));
