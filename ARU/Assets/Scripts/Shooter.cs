using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Shooter : MonoBehaviour
{ public GameObject objectToSpawn;
    private ARRaycastManager raymanger;
    private GameObject visual;

    // Start is called before the first frame update
    void Start()
    {
        raymanger = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        visual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raymanger.Raycast(new Vector2(Screen.width/2, Screen.height/2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (visual.activeInHierarchy == false)
                visual.SetActive(true);
        }

        if(Input.touchCount> 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            GameObject go = Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
