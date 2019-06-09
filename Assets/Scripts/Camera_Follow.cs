using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public bool Horizontal = false;
    public float dampTime = .2f;
    public float screenEdgeBuffer = 4f;
    public float minSize = 6.5f;
    public Vector3 offset;
    public Transform target1;
    [HideInInspector] public Transform[] targets;
    //public Transform target2;
    private Camera camera1;
   // private Camera camera2;

    private const float Y_ANGLE_MIN = 0f;
    private const float Y_ANGLE_MAX = 25.0f;
    private float currentX = 0f;
    private float currentY = 0f;

    private float zoomSpeed;
    private float sensitivityX = 0f;
    private float sensitivityY = 3.0f;
    private float distance = 2.0f;
    private Transform tf1;
   // private Transform tf2;

    private Vector3 moveVelocity;
    private Vector3 desiredPosition;
    

    void Start()
    {

        tf1 = GetComponent<Transform>();
        camera1 = GetComponentInChildren<Camera>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeSplitScreen();
        }
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        tf1.position = target1.position + rotation * dir;
        tf1.LookAt(target1.position + offset);
       // tf2.LookAt(target2.position + offset);
    }

    public void ChangeSplitScreen()
    {
        Horizontal = !Horizontal;

        if (Horizontal)
        {
            camera1.rect = new Rect(0, 0, 1, 0.5f);
          //  camera2.rect = new Rect(0, 0.5f, 1, 0.5f);
        }
        else
        {
            camera1.rect = new Rect(0, 0, 0.5f, 1);
          //  camera2.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = desiredPosition;

        camera1.orthographicSize = FindRequiredSize();
    }

    public void SetPositionAndSize()
    {

    }

    public void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            averagePos += targets[i].position;
            numTargets++;
        }

        /*if (numTargets > 0)
        {
            averagePos /= numTargets;
        }*/

        averagePos.y = transform.position.y;

        desiredPosition = averagePos;
    }

    public void Move()
    {
       // FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }

    public void Zoom()
    {

    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

          //  size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

          //  size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / camera1.aspect);
        }

        //size += screenEdgeBuffer;

        size = Mathf.Max(size, minSize);

        return size;
    }
}
