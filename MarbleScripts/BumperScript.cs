using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    public float bounceAmount = 5f;

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.tag == "Player")
        {
            Vector3 _dir = _collision.transform.position - transform.position;
            _collision.collider.GetComponent<Rigidbody>().AddForce(_dir * bounceAmount);
        }
    }
}