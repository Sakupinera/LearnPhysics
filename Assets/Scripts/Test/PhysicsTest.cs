using DataStructures.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BVHTests test = new BVHTests();
        StartCoroutine(test.Test9RadialRetrival());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
