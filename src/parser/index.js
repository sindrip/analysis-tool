const parser = require("./parser").parser;

function exec (input) {
    return parser.parse(input);
}

const p1 = exec(`
{ 
    int test;
    int[10] tester;
    { int fst; int snd} testaroo;

    test := 1 + 1 * 10;

    if (true) {
        test := 1+1;
    }

    while (true | false) {
        test[2 * 3 + x] := 2;
    }

    read A[x];
    write R.fst;

    R.fst := 2 + 3 * 7 + x + R.snd + A[x];
}
`);

console.log(JSON.stringify( p1, undefined, 2));
console.log(p1);


const p2 = exec(`
{ 
    int i;
    {int fst; int snd} R;
    int[10] A;

    while (i<10) {
        read A[i]; i:=i+1;
    }

    i:=0;
    while (i<10) {
        if (A[i] >= 0) {
            R.fst := R.fst+A[i]; 
            i := i+1;
        } else {
            i := i+1;
        }
        R.snd := R.snd+1;
    }
    write R.fst/R.snd;
} 
`);

console.log(p2);
console.log(JSON.stringify(p2, undefined, 2));
