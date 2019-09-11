const blocks = (statements) => {
    const first = statements[0];    
    statements.shift();
    console.log(statements);
    console.log(first);
    console.log(statements);
    if (statements.length > 1) {
        return blocks(rest).concat(blocks(first));
    }
    console.log(statements.length);

    switch (first.token) {
        case 'ASSIGN':
            return [{block: 'ASSIGN'}];
        break;
        case 'RECORD_ASSIGN':
            return [{block: 'RECORD_ASSIGN'}];
        break;
        case 'IF':
            return [{block: first.condiditon}].concat(blocks(first.body))

        break;
        
        default:

    }
}

module.exports = blocks;