using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour
{
    // The minimum and maximum angles for the lever
    public float minAngle = 0f;
    public float maxAngle = 90f;

    // The speed at which the lever rotates
    public float rotationSpeed = 10f;

    // The current angle of the lever
    private float currentAngle = 0f;

    // Whether the lever is being grabbed by the user
    private bool isGrabbed = false;

    // The initial position and rotation of the lever
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // The controller object that is grabbing the lever
    private GameObject grabbingController;

    void Start()
    {
        // Store the initial position and rotation of the lever
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isGrabbed)
        {
            // Get the current position and rotation of the grabbing controller
            Vector3 controllerPosition = grabbingController.transform.position;
            Quaternion controllerRotation = grabbingController.transform.rotation;

            // Rotate the lever based on the movement of the controller
            currentAngle += rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            transform.rotation = initialRotation * Quaternion.AngleAxis(currentAngle, Vector3.forward);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If a controller enters the lever's trigger area, set it as the grabbing controller
        if (other.gameObject.tag == "Controller")
        {
            grabbingController = other.gameObject;
            isGrabbed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the grabbing controller exits the lever's trigger area, release the lever
        if (other.gameObject.tag == "Controller" && other.gameObject == grabbingController)
        {
            grabbingController = null;
            isGrabbed = false;
        }
    }
}