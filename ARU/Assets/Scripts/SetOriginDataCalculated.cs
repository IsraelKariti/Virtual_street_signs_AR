using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOriginDataCalculated : MonoBehaviour
{
    public GameObject btnSetPoi;
    public GameObject dialog;
    public void sendPoi()
    {
        btnSetPoi.GetComponent<SetPoi>().setLat(SpinCity.avgGPSOrigin.Item1);
        btnSetPoi.GetComponent<SetPoi>().setLon(SpinCity.avgGPSOrigin.Item2);
        btnSetPoi.GetComponent<SetPoi>().setPois();

        dialog.SetActive(false);
    }
}
