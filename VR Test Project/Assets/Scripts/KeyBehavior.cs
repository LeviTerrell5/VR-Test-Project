using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    // The object that the key unlocks (e.g. a door)
    public GameObject unlockableObject;

    // The distance at which the key can be used to unlock the object
    public float unlockDistance = 1f;

    // Whether the key is currently being grabbed by the user
    private bool isGrabbed = false;

    // The controller object that is grabbing the key
    private GameObject grabbingController;

    void Update()
    {
        if (isGrabbed)
        {
            // If the key is within the unlock distance of the unlockable object, unlock it
            float distance = Vector3.Distance(transform.position, unlockableObject.transform.position);
            if (distance <= unlockDistance)
            {
                unlockableObject.SendMessage("Unlock");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If a controller enters the key's trigger area, set it as the grabbing controller
        if (other.gameObject.tag == "Controller")
        {
            grabbingController = other.gameObject;
            isGrabbed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the grabbing controller exits the key's trigger area, release the key
        if (other.gameObject.tag == "Controller" && other.gameObject == grabbingController)
        {
            grabbingController = null;
            isGrabbed = false;
        }
    }
}