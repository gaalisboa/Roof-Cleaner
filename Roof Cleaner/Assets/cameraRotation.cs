using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftAlt)) return;
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.up, horizontal, Space.World);
            transform.Rotate(Vector3.left, vertical);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            cameraTransform.Translate(Vector3.forward * scroll);
        }
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            float horizontal = Input.GetAxis("Mouse X") * moveSpeed;
            float vertical = Input.GetAxis("Mouse Y") * moveSpeed;
            Vector3 moveDirection = new Vector3(horizontal, vertical, 0);
            cameraTransform.Translate(moveDirection, Space.Self);
        }
    }
}
