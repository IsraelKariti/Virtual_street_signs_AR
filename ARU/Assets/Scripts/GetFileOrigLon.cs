using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GetFileOrigLon : MonoBehaviour
{
    public Text t;
    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = File.ReadAllLines(Application.persistentDataPath + "/origin.txt");
        string[] parts = lines[0].Split(' ');
        t.text = parts[1];
    }
}
