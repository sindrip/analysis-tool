import Viz from "viz.js";
import { Module, render } from 'viz.js/full.render.js';

/*import * as CodeMirror from 'codemirror';*/
import CodeMirror from 'codemirror/lib/codemirror.js';
import 'codemirror/mode/clike/clike.js';
import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/material.css';

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
        cmEditor.on('change', (x, change) => {
            if (change.origin !== 'setValue') {
                editor.value = x.getValue();
                console.log(x.getValue());
                editor.dispatchEvent(new Event('input', {
                    bubbles: true,
                    cancelable: true
                }));
            }
        });
    }

};

export function UpdateCodeMirror(source) {
    let cmEditor = document.querySelector('.CodeMirror').CodeMirror;
    let existingValue = cmEditor.getDoc().getValue();
    if (existingValue !== source) {
        cmEditor.getDoc().setValue(source);
    }
}