using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertPOIManually : MonoBehaviour
{
    public GameObject insertDialog;


    public void popupDialog()
    {
        insertDialog.SetActive(true);
        insertDialog.GetComponent<InsertDialogWrapper>().fileName = "allPoints";
    }
}
