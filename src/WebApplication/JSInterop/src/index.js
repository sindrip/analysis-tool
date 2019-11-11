import * as d3 from 'd3';
import Viz from "viz.js";
import { Module, render } from 'viz.js/full.render.js';

/*import * as CodeMirror from 'codemirror';*/
import CodeMirror from 'codemirror/lib/codemirror.js';
import 'codemirror/mode/clike/clike.js';
import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/material.css';

export function AddCircleSvg() {
    const svg = d3.select('#d3body')
        .append('svg')
        .attr('width', '100%')
        .call(d3.zoom().on("zoom", function () {
            svg.attr('transform', d3.event.transform)
        }))
        .append('g');

    svg.append('circle')
        .attr('cx', document.body.clientWidth / 2)
        .attr('cy', document.body.clientHeight / 2)
        .attr('r', 50)
        .style('fill', 'B8DEE6');
}

export function CreateFlowGraph(graph) {
    let viz = new Viz({ Module, render });
    console.log(graph);

    viz.renderSVGElement(graph)
        .then(function (element) {
            let imageDiv = document.getElementById('imageDiv');
            imageDiv.innerHTML = "";
            imageDiv.appendChild(element);
        })
        .catch(error => {
            viz = new Viz({ Module, render });

            // Possibly display the error
            console.error(error);
        });
};

// if  error, clear out current graph
export function ClearGraph() {
    let imageDiv = document.getElementById('imageDiv');
    imageDiv.innerHTML = "";
};

// Very ugly, but works.
export function InitCodeMirror() {
    let editor = document.getElementById("editor");
    let oldInstance = document.getElementsByClassName("CodeMirror");

    if (oldInstance.length === 0) {
        let cmEditor = CodeMirror.fromTextArea(editor, {
            lineNumbers: true,
            mode: "clike",
            theme: "material",
            autofocus: true,
        });
        cmEditor.on('change', x => {
            editor.value = x.getValue();
            editor.dispatchEvent(new Event('input', {
                bubbles: true,
                cancelable: true
            }));
        });
    } 

};