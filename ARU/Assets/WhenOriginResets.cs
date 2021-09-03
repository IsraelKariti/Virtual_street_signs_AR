using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class WhenOriginResets : MonoBehaviour
{
    ARSessionOrigin sess;
    // Start is called before the first frame update
    void Start()
    {
        sess = GetComponent<ARSessionOrigin>();

        sess.trackablesParentTransformChanged += func;
    }

    public void func(ARTrackablesParentTransformChangedEventArgs a)
    {
        Debug.Log("talikariti");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
