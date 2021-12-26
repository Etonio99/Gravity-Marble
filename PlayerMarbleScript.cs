using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarbleScript : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    [SerializeField] private Transform marble;
    private Transform playerModel;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 2f;

    [SerializeField] private Transform directionControl;

    private Transform curGravitySource = null;
    [SerializeField] private float gravity = 9.81f;
    private Vector3 groundNormal;
    [SerializeField] private float pullDistance = 50f;

    [SerializeField] private LayerMask whatIsPlanets;

    private Vector2 leftStickInput;

    private void Awake()
    {
        m_Rigidbody = marble.GetComponent<Rigidbody>();
        playerModel = transform.Find("Body");
    }

    private void Update()
    {
        transform.position = marble.position;

        leftStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Collider[] _planets = Physics.OverlapSphere(transform.position, pullDistance, whatIsPlanets);
        float _dist = 100f;
        for (int i = 0; i < _planets.Length; i++)
        {
            RaycastHit _closestPoint;
            Physics.Raycast(transform.position, _planets[i].transform.position - transform.position, out _closestPoint, pullDistance, whatIsPlanets);
            float _tempDist = Vector3.Distance(transform.position, _closestPoint.point);
            if (_tempDist < _dist)
            {
                _dist = _tempDist;
                curGravitySource = _planets[i].transform;
                groundNormal = _closestPoint.normal;
            }
        }

        Quaternion _toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, 5f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (curGravitySource != null)
        {
            Vector3 _gravityForce = -groundNormal * gravity;
            m_Rigidbody.AddForce(_gravityForce);
        }

        if (Input.GetKey(KeyCode.W))
            m_Rigidbody.AddForce(directionControl.forward * moveSpeed);
        if (Input.GetKey(KeyCode.A))
            m_Rigidbody.AddForce(-directionControl.right * moveSpeed);
        if (Input.GetKey(KeyCode.D))
            m_Rigidbody.AddForce(directionControl.right * moveSpeed);
        if (Input.GetKey(KeyCode.S))
            m_Rigidbody.AddForce(-directionControl.forward * moveSpeed);
        if (Input.GetKeyDown(KeyCode.Space))
            m_Rigidbody.AddForce(directionControl.up * gravity * 40f);

        if (m_Rigidbody.velocity.magnitude > maxSpeed)
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * maxSpeed;
        }

        directionControl.position = playerModel.position;
        directionControl.rotation = playerModel.rotation;
    }
}