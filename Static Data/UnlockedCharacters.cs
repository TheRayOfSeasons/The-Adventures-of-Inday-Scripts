using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Unlocked Characters", order = 3)]
public class UnlockedCharacters : ScriptableObject
{
    public bool b, k, d, g, h, l, m, n, ng, p, s, t, w, y, vowels;

    public void ResetConsonants()
    {
        b = false;
        k = false;
        d = false;
        g = false;
        h = false;
        l = false;
        m = false;
        n = false;
        ng = false;
        p = false;
        s = false;
        t = false;
        w = false;
        y = false;
    }

    public void UnlockAllConsonants()
    {
        b = true;
        k = true;
        d = true;
        g = true;
        h = true;
        l = true;
        m = true;
        n = true;
        ng = true;
        p = true;
        s = true;
        t = true;
        w = true;
        y = true;
    }

    public bool[] ToArray()
    {
        return new bool[] {
            b,
            k,
            d,
            g,
            h,
            l,
            m,
            n,
            ng,
            p,
            s,
            t,
            w,
            y
        };
    }
}
