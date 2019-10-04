window.CreateFlowGraph = function (flow, blocks) {
    const viz = new Viz();

    let result = `{ node [shape=box] `;
    let dict = {};

    const blocksSplit = blocks.split("\n");
    blocksSplit.forEach(function (val) {
        let number = val.split("[")[0];
        dict[number] = `"${val}"`;
        result += `"${val}"`;
    })

    result += `} \n`; // close style bracket

    const edges = flow.replace(/[{()}]/g, "").split("\n");
    edges.forEach(function (val, index, array) {
        array[index] = val.split(`, `);
        var relation = `${dict[array[index][0]]} -> ${dict[array[index][1]]}`;
        result += `${relation}\n`;
    });

    viz.renderSVGElement(`digraph { ${result} }`)
        .then(function (element) {
            let imageDiv = document.getElementById('imageDiv');
            imageDiv.innerHTML = "";
            imageDiv.appendChild(element);
        })
        .catch(error => {
            viz = new Viz();

            // Possibly display the error
            console.error(error);
        });
}

// if  error, clear out current graph
window.ClearGraph = function () {
    let imageDiv = document.getElementById('imageDiv');
    imageDiv.innerHTML = "";
}