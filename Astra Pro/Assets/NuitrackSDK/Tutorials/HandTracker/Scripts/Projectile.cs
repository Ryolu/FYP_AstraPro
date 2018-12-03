using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Vector3 dir;
    private float Speed = 3f;

    //// Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    private void Update()
    {
        transform.position += dir;
    }
}
