using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour
{
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [SerializeField] private float maxZ;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if(GameManager.Instance.IsPaused)
            return;

        transform.localScale = new Vector3(
            x && transform.localScale.x < maxX ? transform.localScale.x + 0.1f : transform.localScale.x,
            y && transform.localScale.y < maxY ? transform.localScale.y + 0.1f : transform.localScale.y,
            z && transform.localScale.z < maxZ ? transform.localScale.z + 0.1f : transform.localScale.z
        );

        if(Check())
            Reset();
    }

    bool Check()
    {
        List<float> values = new List<float>();
        List<float> maxValues = new List<float>();

        if(x)
        {
            values.Add(transform.localScale.x);
            maxValues.Add(maxX);
        }
        if(y)
        {
            values.Add(transform.localScale.y);
            maxValues.Add(maxY);
        }
        if(z)
        {
            values.Add(transform.localScale.z);
            maxValues.Add(maxZ);
        }

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] < maxValues[i])
                return false;
        }

        return true;
    }

    public void Initiate()
    {
        transform.localScale = originalScale;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        transform.localScale = originalScale;
        gameObject.SetActive(false);
    }
}
