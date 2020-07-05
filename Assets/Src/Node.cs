using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building block of the force graph structure. Each node has an optional parent and can be assigned any number of children.
/// Each child is attached to the parent with a spring joint and repels anything in its FieldRadius that is not its parent.
/// The root node of any tree should have kinematic set to True so it stays anchored in space.
/// </summary>
public class Node : MonoBehaviour

{
    public Node m_Parent;
    private List<Node> m_Children = new List<Node>();
    private bool m_Selected = false;
    private float m_FieldRadius = 3.0f; // how close other Nodes have to be to be repelled by this one
    private float m_RepelStrength = 5f; // how strong the repelling force is
   

    // Start is called before the first frame update
    void Start()
    {
        if(m_Parent == null)
        {
            // root nodes are locked in place
            this.GetRigidBody().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        this.drawLines();
        if(m_Parent != null)
        {
            this.gameObject.transform.LookAt(m_Parent.GetTransform());
        }
    }

    void FixedUpdate()
    {
        // applied forces must happen in FixedUpdate, not regular Update
        this.repelCloseNodes();
    }

    void OnMouseDown()
    {
        // handle node selection
        Node[] nodes = GameObject.FindObjectsOfType<Node>();
        Debug.Log(nodes.Length);

        for(int i = 0; i< nodes.Length; i++)
        {
            nodes[i].Deselect();
        }
     
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        m_Selected = true;
    }


    public bool IsSelected()
    {
        return m_Selected;
    }

    public void Deselect() {
        m_Selected = false;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    } 

    public Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }

    public Rigidbody GetRigidBody()
    {
        return this.GetComponent<Rigidbody>();
    }

    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void SetParent(Node parent)
    {
        this.m_Parent = parent;
        // create a pulling force between this child and its parent
        SpringJoint spring = this.gameObject.AddComponent<SpringJoint>();
        spring.anchor = new Vector3();
        spring.connectedBody = parent.GetRigidBody();
        spring.damper = 1000;
        spring.tolerance = 2f;

        // parents with more children "weigh" more so they don't get pulled around as much
        this.GetRigidBody().mass += 10.0f;
        
    }

    


    public void SpawnChild()
    {
        Transform childHolder = transform.Find("Children");
        GameObject newChild = Instantiate(Resources.Load("Node")) as GameObject;
        newChild.transform.parent = childHolder.transform;

        // position for new child is random if this is a root node.
        // if it's not a root node, child spawns opposite from this Node's parent (child's grandparent)
        // this helps with the stability and aesthetics of the graph
        if(m_Parent != null)
        {

            newChild.transform.position = this.gameObject.transform.position + -(m_Parent.GetPosition() - this.GetPosition());
        }
        else
        {
            newChild.transform.position = this.gameObject.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
        }
        Node childNode = newChild.GetComponent<Node>();
        childNode.SetParent(this);
        m_Children.Add(newChild.GetComponent<Node>());

        

    }

    /// <summary>
    /// For aesthetics only, draw lines between parent/child nodes. Children 
    /// draw lines towards their parent so that the parent doesn't have a million LineRenderers.
    /// </summary>
    private void drawLines()
    {
        
        if(m_Parent)
        {
            LineRenderer line = gameObject.GetComponent<LineRenderer>();

            if(line == null)
            {
                line =this.gameObject.AddComponent<LineRenderer>();
            }

            line.startWidth = line.endWidth = 0.05f;
            line.SetPosition(0, m_Parent.GetPosition());
            line.SetPosition(1, this.gameObject.transform.position);

        }
    }

    private void repelCloseNodes()
    {
       for(int i = 0; i < m_Children.Count; i++)
        {
            // find all nodes within a certain radius
            Collider[] hitColliders = Physics.OverlapSphere(m_Children[i].GetPosition(), m_FieldRadius);
          
            for (int j = 0; j < hitColliders.Length; j++)
            {
             
                // children do not repel their parents, but do repel every other Node object in their radius
                if( hitColliders[j].gameObject != this.gameObject)
                {
                    Node closeNode = hitColliders[j].gameObject.GetComponent<Node>();
                    if(closeNode != null)
                    {
                        Vector3 repellingForce = closeNode.GetPosition() - m_Children[i].GetPosition() ;
                        closeNode.GetRigidBody().AddForce(repellingForce * m_RepelStrength, ForceMode.Acceleration); // acceleration here, not force - this is to attenuate the force over distance
                    }
                }
               
            }
        }
    }
   
    
}
