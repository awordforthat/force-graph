using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    private Vector3 m_CameraAngle;

    // Start is called before the first frame update
    void Start()
    {
        m_CameraAngle = Vector3.Normalize(new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        // zoom
        float zoomDirection = Input.GetKey(KeyCode.DownArrow) ? 1.0f : Input.GetKey(KeyCode.UpArrow) ? -1.0f : 0.0f;
        this.gameObject.transform.position += m_CameraAngle * 0.2f * zoomDirection;

        // rotation
        float rotationDirection = Input.GetKey(KeyCode.LeftArrow) ? -1.0f : Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f;
        this.transform.RotateAround(new Vector3(), Vector3.up, 1.0f * rotationDirection);


    }
}
