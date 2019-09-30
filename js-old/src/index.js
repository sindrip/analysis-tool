const parser = require('./parser').parser;

const p1 = parser.parse(`
{
    int x;
    int[10] A;

    x := 1;
    if (2 > 3) {
        x := 3;
    }
}
`);

console.log(JSON.stringify(p1, undefined,2));



