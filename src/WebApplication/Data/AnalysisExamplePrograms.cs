using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class AnalysisExamplePrograms
    {
        public static Dictionary<string, string> GetPrograms()
        {
            return new Dictionary<string, string> ()
            {
                { "Factorial function", @"{
    int x;
    int y;
    read x;
    y := 1;
    while (x > 0) { 
        y := x * y;
        x := x -1; 
    } 
}" },
                {"Modulo function", @"{
    int x;
    int y;
    int q;
    int r;
    if (x >= 0 & y > 0) {
        q := 0;
        r := x;
        if (r >= y) {
            r := r - y;
            q := q + 1;
        }
    }   
}"},
                { "Insertion sort", @"{
    int[10] A;
    int i;
    int j;
    int t;

    while (i < 10) {
        j := i;
        while (j > 0 & (A[j-1] > A[j])) {
            t := A[j];
            A[j] := A[j-1];
            A[j-1] := t;
            j := j - 1;
        }
        i := i + 1;
    }
}"},
                { "Matrix Transpose", @"{
    int i; int j; int n;
    int m; int u; int t;
    int[10] B; int[10] A;

    i := 0;
    while (i < n) {
        j := 0;
        while ( j < m) {
            u := (i * m) + j;
            t := (j * n) + i;
            B[t] := A[u];
            j := j + 1;
        }
        i := i + 1;
    }
}" },
            };
        }
    }
}
