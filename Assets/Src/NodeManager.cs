using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    Button m_SpawnButton;
    // Start is called before the first frame update
    void Start()
    {
        m_SpawnButton.onClick.AddListener(this.spawnAtSelected);
    }


    /// <summary>
    /// Finds selected node and spawns a child there
    /// </summary>
    private void spawnAtSelected()
    {
        Node[] nodes = GameObject.FindObjectsOfType<Node>();
        for(int i = 0; i < nodes.Length; i++)
        {
            if(nodes[i].IsSelected())
            {
                nodes[i].SpawnChild();
                break;
            }
        }
    }


}
