using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Anti hack giá trị của game
public struct SafeValues {
    private float offset;
    private float value;

    public SafeValues (float value = 0) {
        offset = Random.Range (-1000, +1000);
        this.value = value + offset;
    }

    public float GetValue () {
        return value - offset;
    }

    public void Dispose () {
        offset = 0;
        value = 0;
    }

    public override string ToString () {
        return GetValue ().ToString ();
    }

    public static SafeValues operator + (SafeValues f1, SafeValues f2) {
        return new SafeValues (f1.GetValue () + f2.GetValue ());
    }
}