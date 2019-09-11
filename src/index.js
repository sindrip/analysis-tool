/* cool */

const flow = require('./flow');

const parser = require("./parser/parser.js").parser;


function exec (input) {
    return parser.parse(input);
}

const p1 = exec(`
{
    x := 1;
    if (2 > 3) {
        x := 3;
    }
}
`);

console.log(JSON.stringify(p1, undefined,2));
//console.log(flow(p1.statements));


