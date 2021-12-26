using UnityEngine;
using System.Collections;

public class SmoothFollow3D : MonoBehaviour
{
    public Transform target;
    public float upDistance = 7.0f;
    private float savedUpDistance;
    public float backDistance = 10.0f;
    private float savedBackDistance;
    public float trackingSpeed = 3.0f;
    public float rotationSpeed = 9.0f;

    private Vector3 v3To;
    private Quaternion qTo;

    public bool lockPos = false;

    private Vector2 rightStickInput;
    private bool movingRightStick = false;

    private void Start()
    {
        savedUpDistance = upDistance;
        savedBackDistance = backDistance;
    }

    private void Update()
    {
        //rightStickInput = new Vector2(Input.GetAxis("RHor1"), Input.GetAxis("RVer1"));
        //rightStickInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (rightStickInput.magnitude > 0.2f)
        {
            movingRightStick = true;

            upDistance += rightStickInput.y;
            backDistance -= rightStickInput.x / 1.5f;

            upDistance = Mathf.Clamp(upDistance, 1f, 14f);
            backDistance = Mathf.Clamp(upDistance, 4f, 10f);

            //target.transform.RotateAround(target.transform.position, -transform.right, Input.GetAxis("Mouse Y"));
            target.transform.Rotate(0f, rightStickInput.x * 1.5f, 0f);
        }
        if (rightStickInput.magnitude <= 0.2f && movingRightStick)
        {
            movingRightStick = false;
            upDistance = savedUpDistance;
            backDistance = savedBackDistance;
        }
    }

    void LateUpdate()
    {
        if (!lockPos)
        {
            v3To = target.position - target.forward * backDistance + target.up * upDistance;
            transform.position = Vector3.Lerp(transform.position, v3To, trackingSpeed * Time.deltaTime);
        }
        
        qTo = Quaternion.LookRotation(target.position - transform.position, target.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
    }
}