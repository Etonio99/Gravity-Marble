using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourceScript : MonoBehaviour
{
    public bool oneDirectionalGravity = false;
    [HideInInspector] public Vector3 gravityDirection;

    public bool useTransformUp = true;
    public Transform customTransformUp;
    public Vector3 customGravity;

    private void Start()
    {
        if (oneDirectionalGravity)
        {
            if (useTransformUp)
                gravityDirection = transform.up;
            else if (customTransformUp != null)
                gravityDirection = customTransformUp.up;
            else
                gravityDirection = customGravity.normalized;
        }
    }
}