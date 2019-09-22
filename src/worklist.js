/*
W := flow(S_*)
for all l in labels(S_*) do
    if l = init(S_*) then RD_entry(l) := {(x,?) | x in FV(S_*)}
                     else RD_entry(l) := 0

    while W is not empty
        remove some (l, l') from from W
        if RD_exit(l) is not a subset of RD_entry(l')
        then RD_entry(l') := RD_entry(l') union RD_exit(l)
             for all l'' with (l', l'') in flow(S_*)
                do W := W union {(l', l'')}
        else do nothing


*/
class Worklist {

    constructor(func) {
        // 
        this.worklist = null;
        // func is the piece of code to be executed in each step
        this.func = func;
    }

    step() {
        if (!this.worklist) {
            return;
        }


    }

    // Run, steps through until the worklist is empty
    run() {
        while(this.worklist) {
            this.step();
        }
    }
}