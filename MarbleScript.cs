using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleScript : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    [SerializeField] private int controllerNum = 1;

    [SerializeField] private Transform marble;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 2f;

    private Transform curGravitySource = null;
    [SerializeField] private float gravity = 9.81f;
    private Vector3 groundNormal;
    [SerializeField] private float pullDistance = 50f;

    [SerializeField] private LayerMask whatIsPlanets;

    private Vector2 leftStickInput;
    private Vector2 rightStickInput;

    private Vector3 respawnPoint;
    private Vector3 respawnAngle;

    private void Awake()
    {
        m_Rigidbody = marble.GetComponent<Rigidbody>();
        m_Rigidbody.maxAngularVelocity = 500f;
    }

    private void Start()
    {
        respawnPoint = transform.position;
    }

    private void Update()
    {
        transform.position = marble.position;

        leftStickInput = new Vector2(Input.GetAxisRaw("Hor" + controllerNum), Input.GetAxisRaw("Ver" + controllerNum));
        rightStickInput = new Vector2(Input.GetAxis("RHor" + controllerNum), Input.GetAxis("RVer" + controllerNum));

        if (Input.GetKeyDown(KeyCode.R))
            Respawn();

        /*if (curGravitySource != null)
        {
            if (curGravitySource.oneDirectionalGravity)
            {
                groundNormal = curGravitySource.gravityDirection;
            }
            else
            {
                RaycastHit _closestPoint;
                Physics.Raycast(transform.position, curGravitySource.transform.position - transform.position, out _closestPoint, Vector3.Distance(transform.position, curGravitySource.transform.position), whatIsPlanets);

                groundNormal = _closestPoint.normal;
            }
        }*/

        Collider[] _planets = Physics.OverlapSphere(transform.position, pullDistance, whatIsPlanets);
        float _dist = 100f;
        for (int i = 0; i < _planets.Length; i++)
        {
            if (_planets.Length == 0)
                break;

            RaycastHit _closestPoint;
            Physics.Raycast(transform.position, _planets[i].transform.position - transform.position, out _closestPoint, pullDistance, whatIsPlanets);
            if (_closestPoint.transform == null || _closestPoint.transform.gameObject != _planets[i].gameObject)
                continue;
            float _tempDist = Vector3.Distance(transform.position, _closestPoint.point);
            if (_tempDist < _dist)
            {
                _dist = _tempDist;
                curGravitySource = _planets[i].transform;
                GravitySourceScript _gss = _planets[i].GetComponent<GravitySourceScript>();
                if (_gss.oneDirectionalGravity)
                {
                    groundNormal = _gss.gravityDirection;
                }
                else
                {
                    groundNormal = _closestPoint.normal;
                }
            }
        }

        Quaternion _toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, 5f * Time.deltaTime);

        //if (rightStickInput.magnitude > 0.2f)
        //{
            transform.Rotate(groundNormal * rightStickInput.x * 200f * Time.deltaTime);
        //}
    }

    private void FixedUpdate()
    {
        if (curGravitySource != null)
        {
            Vector3 _gravityForce = -groundNormal * gravity;
            m_Rigidbody.AddForce(_gravityForce);
        }

        /*if (Input.GetKey(KeyCode.W))
            m_Rigidbody.AddForce(transform.forward * moveSpeed);
        if (Input.GetKey(KeyCode.A))
            m_Rigidbody.AddForce(-transform.right * moveSpeed);
        if (Input.GetKey(KeyCode.D))
            m_Rigidbody.AddForce(transform.right * moveSpeed);
        if (Input.GetKey(KeyCode.S))
            m_Rigidbody.AddForce(-transform.forward * moveSpeed);*/

        if (leftStickInput.magnitude > 0.1f)
        {
            //m_Rigidbody.AddForce(transform.forward * leftStickInput.y * moveSpeed);
            //m_Rigidbody.AddForce(transform.right * leftStickInput.x * moveSpeed);

            m_Rigidbody.AddTorque(transform.right * leftStickInput.y * moveSpeed / 3f);
            m_Rigidbody.AddTorque(-transform.forward * leftStickInput.x * moveSpeed / 3f);
        }

        if (m_Rigidbody.velocity.magnitude > maxSpeed)
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * maxSpeed;
        }
    }

    public void SetRespawnPoint(Vector3 _pos)
    {
        respawnPoint = _pos;
    }

    public void SetRespawnAngle(Vector3 _angle)
    {
        respawnAngle = _angle;
    }

    public void Respawn()
    {
        marble.position = respawnPoint;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        marble.rotation = Quaternion.identity;

        transform.eulerAngles = respawnAngle;
    }

    /*void AlignCamera()
    {
        //inspired from anser by Cherno: .https://answers.unity.com/questions/851335/how-do-i-lock-to-a-45-degree-angle-when-projecting.html

        Vector3 _angleOfRotation = transform.up;
        bool useUp = true;
        if (groundNormal.x > groundNormal.y || groundNormal.z > groundNormal.y)
        {
            _angleOfRotation = transform.forward;
            Debug.Log("Not Using Up.");
            useUp = false;
        }
            

        Vector3 _aimDir = transform.forward;
        Quaternion _qTo;
        float _angle;
        if (useUp)
            _angle = -Mathf.Atan2(_aimDir.z, _aimDir.x) * Mathf.Rad2Deg + 90f;
        else
            _angle = -Mathf.Atan2(_aimDir.y, _aimDir.z) * Mathf.Rad2Deg + 90f;
        _angle = Mathf.Round(_angle / 45f) * 45f;
        if (useUp)
            _qTo = Quaternion.Euler(transform.eulerAngles.x, _angle, transform.eulerAngles.z);
        else
            _qTo = Quaternion.Euler(180f, transform.eulerAngles.y, transform.eulerAngles.z);
        //_qTo = Quaternion.AngleAxis(_angle, _angleOfRotation);
        transform.rotation = _qTo;
    }*/
}