const ast = require('./nodes');

/*
{
    int i;
    int[10] A;
    { int fst; int snd; } R;

    while (i < 10) {
        read A[i];
        i := i+1;
    }

    i := 0;

    while (i < 10) {
        if (A[i] >= 0) {
            R.fst := R.fst + A[i];
            i := i + 1;
        } else {
            i := i + 1;
        }
        R.snd := R.snd + 1;
    }
    write R.fst / R.snd;
}
*/
const program = [
    ast.intDeclaration('x'),
    ast.arrayDeclaration('A', 10),
    ast.recordDeclaration('R'),

    ast.whileNode(
        ast.booleanExpression(ast.relationalExpression(ast.arithmeticExpression(ast.varIdentifier('i')), '<', ast.arithmeticExpression(ast.integerLiteral(10)))),
        [
            ast.readNode(ast.arrayIdentifier('A', ast.arithmeticExpression(ast.varIdentifier('i')))),
            ast.assignmentNode(ast.varIdentifier('i'), ast.arithmeticExpression(ast.arithmeticBinopExpression(ast.arithmeticExpression(ast.varIdentifier('i')), '+', ast.arithmeticExpression(ast.integerLiteral(1))))),
        ]
    ),

    ast.assignmentNode(ast.varIdentifier('i'), ast.arithmeticExpression(ast.integerLiteral(0))),



    ast.writeNode(ast.arithmeticExpression(ast.arithmeticBinopExpression(
        ast.recordIdentifier('R', 'fst'),
        '/',
        ast.recordIdentifier('R', 'snd')))),

];


console.log(program);
console.log(JSON.stringify(program, undefined, 2));
