using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Vector3 destinationPoint;
    [SerializeField] GameObject point;

    public NavMeshAgent agent;
    public bool isChoosenOne = false;
    [SerializeField] LineRenderer lineRenderer;

    public BattleUI battleUI;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destinationPoint = transform.position;

        InitLineRenderer();

        point = Instantiate(point);
        point.SetActive(false);
    }

    void Update()
    {
        if (isChoosenOne && Input.GetMouseButtonDown(1))
        {
            SetDestination();
            battleUI.SetSteps(CalculatePathLength(destinationPoint));
        }
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500))
            destinationPoint = hit.point;

        Vector3[] path = GetPath(destinationPoint);

        // TODO
        battleUI.goButton.onClick.RemoveListener(MoveToDestination);

        lineRenderer.positionCount = path.Length;
        lineRenderer.SetPositions(path);

        point.SetActive(true);
        point.transform.position = destinationPoint;
        battleUI.goButton.onClick.AddListener(MoveToDestination);
    }

    void MoveToDestination()
    {
        agent.destination = destinationPoint;
        battleUI.goButton.onClick.RemoveListener(MoveToDestination);
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
}
