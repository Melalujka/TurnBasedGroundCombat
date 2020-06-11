using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] float movementPoints;

    private Vector3 lastPosition;
    private float odometerDistance;
    private ICharConsts consts;
    private Vector3 destinationPoint;
    public NavMeshAgent agent;
    public bool isChoosenOne = false;
    [SerializeField] LineRenderer lineRenderer;

    public BattleUI battleUI;

    void Start()
    {
        consts = Constants.GetChar(gameObject.tag);

        agent = GetComponent<NavMeshAgent>();
        destinationPoint = transform.position;

        InitLineRenderer();

        point = Instantiate(point);
        point.SetActive(false);

        lastPosition = transform.position;

        RenewPoints();

        // TODO: remove later
        battleUI.turnButton.onClick.AddListener(RenewPoints);

        battleUI.shotButton.onClick.AddListener(Shot);
        battleUI.goButton.onClick.AddListener(MoveToDestination);
    }

    void Update()
    {
        if (isChoosenOne)
            battleUI.SetRange(CalculatePathLength(destinationPoint));
            

        CountMeters();
        MovementControl();
        RenderLine();
    }

    private void OnMouseDown()
    {
            if (Input.GetMouseButtonDown(0))
                SetDestination();
        
    }

    void InitLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = new Color(255, 255, 0, 0);
    }

    void SetDestination()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
            destinationPoint = hit.point;

        //RenderLine();

        // TODO: maybe no need to remove the listener
        //battleUI.goButton.onClick.RemoveListener(MoveToDestination);

        point.SetActive(true);
        point.transform.position = destinationPoint;
        //battleUI.goButton.onClick.AddListener(MoveToDestination);
    }

    private void RenderLine()
    {
        Vector3[] path = GetPath(destinationPoint);
        lineRenderer.positionCount = path.Length;
        lineRenderer.SetPositions(path);
    }

    void MoveToDestination()
    {
        if (isChoosenOne)
        {
            agent.isStopped = false;
            agent.destination = destinationPoint;
          //  battleUI.goButton.onClick.RemoveListener(MoveToDestination);
            battleUI.stopButton.onClick.AddListener(Stop);
        }
    }

    void Stop()
    {
        if (isChoosenOne)
        {
            agent.isStopped = true;
            battleUI.stopButton.onClick.RemoveListener(Stop);
          //  battleUI.goButton.onClick.AddListener(MoveToDestination);
        }
    }

    void Shot()
    {
        // TODO: implement shot functionality
        if (isChoosenOne && movementPoints > 25)
            movementPoints -= 25;
    }

    void MovementControl()
    {
        if (isChoosenOne && !agent.isStopped && movementPoints <= 0 )
            Stop();
    }

    public void RenewPoints()
    {
        movementPoints = consts.MovementPoints;
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        Vector3[] path = GetPath(targetPosition);

        float pathLength = 0;

        for (int i = 0; i < path.Length - 1; i++)
            pathLength += Vector3.Distance(path[i], path[i + 1]);

        return pathLength;
    }

    Vector3[] GetPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.enabled)
            agent.CalculatePath(targetPosition, path);

        int pathPoints = path.corners.Length + 2;

        Vector3[] resultPath = new Vector3[pathPoints];
        resultPath[0] = transform.position;
        path.corners.CopyTo(resultPath, 1);
        resultPath[pathPoints - 1] = targetPosition;

        return resultPath;
    }

    void CountMeters()
    {
        if (!isChoosenOne)
            return;

        Vector3 currentPosition = transform.position;    // just make a copy for clarity
        float distance = Vector3.Distance(currentPosition, lastPosition);    // how far?
        odometerDistance += distance;        // accumulate
        movementPoints -= distance;         // calculate
        lastPosition = currentPosition;    // save your last position for next frame

        battleUI.SetSteps(movementPoints);
    }
}
