using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic gesture trigger class for the custom gestures
/// </summary>
public class GestureTrigger : MonoBehaviour {

    public enum GestureType
    {
        none,
        chop,
        stir
    }

    /// <summary>
    /// Checks for when the collidable body part is within the collider
    /// </summary>
    [HideInInspector] public bool isHere = false;
    /// <summary>
    /// The gesture type for this trigger
    /// </summary>
    public GestureType type;

    void OnTriggerEnter(Collider other)
    {
        isHere = true;
    }
    void LateUpdate()
    {
        isHere = false;
    }
}
