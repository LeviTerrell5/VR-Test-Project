using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKey : MonoBehaviour
{
    // The object that represents the VR hand
    public GameObject VRHand;

    // The transform component of the VR hand
    private Transform VRHandTransform;

    // The distance that the key should be offset from the VR hand
    public Vector3 offset = new Vector3(0.1f, 0.1f, 0.1f);

    // The speed at which the key should move toward the VR hand when it is picked up
    public float moveSpeed = 0.1f;

    // The maximum distance that the key can be from the VR hand before it is "dropped"
    public float maxDistance = 0.5f;

    // A flag that indicates whether the key is currently being held by the VR hand
    private bool isHeld = false;

    // A reference to the rigidbody component of the key
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the transform component of the VR hand
        VRHandTransform = VRHand.transform;

        // Get a reference to the rigidbody component of the key
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the key is being held...
        if (isHeld)
        {
            // Calculate the direction from the key to the VR hand
            Vector3 direction = VRHandTransform.position - transform.position;

            // If the distance between the key and the VR hand is greater than the maximum allowed distance...
            if (direction.magnitude > maxDistance)
            {
                // "Drop" the key by setting the isHeld flag to false
                isHeld = false;
            }
            else
            {
                // Otherwise, move the key toward the VR hand
                transform.position = Vector3.MoveTowards(transform.position, VRHandTransform.position + offset, moveSpeed);
            }
        }
    }

    // This function is called when the key is picked up by the VR hand
    public void PickUp()
    {
        // Set the isHeld flag to true
        isHeld = true;

        // Disable the rigidbody component so that the key is "fixed" in place
        rb.isKinematic = true;
    }

    // This function is called when the key is dropped by the VR hand
    public void Drop()
    {
        // Set the isHeld flag to false
        isHeld = false;

        // Re-enable the rigidbody component so that the key can move freely again
        rb.isKinematic = false;
    }
}