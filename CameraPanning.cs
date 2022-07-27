using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    private float zoom = 12.0f;
    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GetComponent<Rigidbody2D>().MovePosition(transform.position + movement * Time.deltaTime * (5.0f * zoom));

        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel");
            if (zoom > 20)
            {
                zoom += Input.GetAxis("Mouse ScrollWheel");
            }
            else if ( zoom < 1.5f)
            {
                zoom += Input.GetAxis("Mouse ScrollWheel");
            }
            Camera.main.orthographicSize = zoom;
        }
    }
}
