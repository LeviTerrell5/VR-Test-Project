using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHand : MonoBehaviour
{
    // The object that the VR hand is currently holding
    private GameObject heldObject;

    // The distance that the held object should be offset from the VR hand
    public Vector3 offset = new Vector3(0.1f, 0.1f, 0.1f);

    // The speed at which the held object should move toward the VR hand when it is picked up
    public float moveSpeed = 0.1f;

    // The maximum distance that the held object can be from the VR hand before it is "dropped"
    public float maxDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the VR hand is currently holding an object...
        if (heldObject != null)
        {
            // Calculate the direction from the held object to the VR hand
            Vector3 direction = transform.position - heldObject.transform.position;

            // If the distance between the held object and the VR hand is greater than the maximum allowed distance...
            if (direction.magnitude > maxDistance)
            {
                // "Drop" the held object by setting the heldObject variable to null
                heldObject = null;
            }
            else
            {
                // Otherwise, move the held object toward the VR hand
                heldObject.transform.position = Vector3.MoveTowards(heldObject.transform.position, transform.position + offset, moveSpeed);
            }
        }
    }

    // This function is called when the VR hand collides with another object
    void OnTriggerEnter(Collider other)
    {
        // If the VR hand is not currently holding an object and the collided object has a VRKey script attached to it...
        if (heldObject == null && other.gameObject.GetComponent<VRKey>() != null)
        {
            // Set the heldObject variable to the collided object
            heldObject = other.gameObject;

            // Call the PickUp function on the VRKey script attached to the collided object
            heldObject.GetComponent<VRKey>().PickUp();
        }
    }
}