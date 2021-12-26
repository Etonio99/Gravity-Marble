using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMarbleScript : MonoBehaviour
{
    public MarbleScript controller;

    private float floatTimer = 0f;
    private int collisions = 0;

    private void Update()
    {
        if (collisions <= 0)
            floatTimer += Time.deltaTime;

        if (floatTimer > 3f)
            controller.Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (floatTimer > 0f)
            floatTimer = 0f;

        collisions++;

        if (collision.transform.tag == "Death")
        {
            controller.Respawn();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
}
