using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeSchema 
{
    public string id;
    public string title;
    public List<NodeSchema> children;
    public List<string> cousins;
}
