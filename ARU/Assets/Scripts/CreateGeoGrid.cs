using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGeoGrid : MonoBehaviour
{
    public GameObject parent;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        // create 100X100 GEO GRID dynamically
        for (int z = -100; z <= 100; z += 2)
        {
            for (int x = -100; x <= 100; x += 2)
            {
                GameObject clone = Instantiate(prefab, new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0), parent.transform);
                clone.GetComponent<TextMesh>().text = "(" + x + "," + z + ")";
                clone.transform.RotateAround(parent.transform.position, new Vector3(0, 1, 0), parent.transform.rotation.eulerAngles.y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
