using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassOffsetTracker : MonoBehaviour
{
    int val = 0;
    public Text tracker;
    // Start is called before the first frame update
    public void inc()
    {
        val++;
        tracker.text = "" + val;
    }
    public void dec()
    {
        val--;
        tracker.text = "" + val;
    }
  
}
