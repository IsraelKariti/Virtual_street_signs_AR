using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class CloseSave : MonoBehaviour
{
    public GameObject dialogPopup;
    public InputField heightInput;
    public InputField descInput;
    public double lat;
    public double lon;

    public void closeAndSave()
    {
        string height = heightInput.text;
        string desc = descInput.text;

        File.WriteAllText(Application.persistentDataPath + "/allPoints.txt", "lat: " + lat+" lon: "+lon+" height: "+height+" desc: "+desc);
        dialogPopup.SetActive(false);
    }
}
