using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Setup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 0);

        // load "CMS" data
       // Debug.Log(Application.streamingAssetsPath + "/content.json");
       // StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/content.json");
  

       //NodeSchema data = JsonUtility.FromJson<NodeSchema>(reader.ReadToEnd());

       // reader.Close();
    }

   
}
