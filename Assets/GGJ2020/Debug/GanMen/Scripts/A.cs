using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    int x = 0;
    // Start is called before the first frame update
    void Start()
    {
        var b = new B(1);
        x = b.b;
        Debug.Log(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct B
{
    public int b;
    public B(int num)
    {
        b = num;
    }
}
