using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Baybayin Data List", order = 9)]
public class BaybayinDataList : ScriptableObject
{
    [Header("Vowels")]
    public BaybayinData a;
    public BaybayinData e;
    public BaybayinData i;

    [Header("Modifiers")]
    public BaybayinData e_i;
    public BaybayinData o_u;
    public BaybayinData noConsonant;

    [Header("Consonants")]
    public BaybayinData b;
    public BaybayinData k;
    public BaybayinData d;
    public BaybayinData g; 
    public BaybayinData h; 
    public BaybayinData l; 
    public BaybayinData m;
    public BaybayinData n;
    public BaybayinData ng;
    public BaybayinData p; 
    public BaybayinData s; 
    public BaybayinData t; 
    public BaybayinData w;
    public BaybayinData y;
}
