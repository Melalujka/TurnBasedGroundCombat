using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] float movementPoints;

    private BattleManager manager;
    private Vector3 lastPosition;
    private float odometerDistance;
    private ICharConsts consts;
    private Vector3 destinationPoint;
    public NavMeshAgent agent;
    public bool isChoosenOne = false;
    private bool shouldIgnoreClick = false;
    [SerializeField] LineRenderer lineRenderer;

    public bool isAlive = true;

    private float castModifier = 1;
    private float sightModifier = 1;
    private float attackModifier = 1;
    private float speedModifier = 1;
    private float healthModifier = 1;

    private float currentHealth;
    public float SightRange => consts.SightRange;
    public float AttackRange => consts.AttackRange;
    public float CastRange => consts.CastRange;
    public float CastPower => consts.CastPower * castModifier;
    public float AttackPower => consts.AttackPower * attackModifier;
    //private GameObject[] visibleEnemies;
    //public void SetVisibleEnemy(GameObject[] enemies) { visibleEnemies = enemies; }

    public BattleUI battleUI;

    void Start()
    {
        manager = GameObject.Find(Constants.BattleManager).GetComponent<BattleManager>();
        consts = Constants.GetChar(gameObject.tag);
        currentHealth = consts.Health;

        agent = GetComponent<NavMeshAgent>();
        destinationPoint = transform.position;

        InitLineRenderer();

        point = Instantiate(point);
        point.SetActive(false);

        lastPosition = transform.position;

        RenewPoints();

        // TODO: remove later
        battleUI.turnButton.onClick.AddListener(RenewPoints);

        battleUI.shotButton.onClick.AddListener(ShotButtonAction);
        battleUI.goButton.onClick.AddListener(MoveToDestination);
    }

    private void FixedUpdate()
    {
        CheckIfAlive();
        MovementControl();
        RenderLine();
    }

    private void CheckIfAlive()
    {
        if (currentHealth <= 0 && isAlive)
        {
            Die();
            manager.CharacterOutOfHealth(this);
        }
    }

    void Die()
    {
        isAlive = false;
    }

    public void Resurect(float healthPoints)
    {
        isAlive = true;
        currentHealth = healthPoints;
        manager.CharacterAliveAgain(this);
    }

    private void OnMouseDown()
    {
        if (!isChoosenOne && !EventSystem.current.IsPointerOverGameObject())
        {
            SetPlayerAsChoosenAndUpdateCamera();
            shouldIgnoreClick = true;
        }
    }

    public void SetPlayerAsChoosenAndUpdateCamera()
    {
        manager.DeselectAllCharacters();
        battleUI.SetDefault();
        var cam = Camera.main.GetComponentInParent<CameraController>();
        cam.player = gameObject;
        SetChoosenOne();
    }

    void Update()
    {
        if (isChoosenOne)
        {
            battleUI.SetRange(CalculatePathLength(destinationPoint));
            battleUI.SetHealth(currentHealth);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                SetDestination();

        }
        CountMeters();
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
        if (!shouldIgnoreClick && isAlive)
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
        else
        {
            shouldIgnoreClick = false;
        }
    }

    Ray CastRay(Vector3 target)
    {
        Ray ray = new Ray();
        ray.origin = target;
        ray.direction = Vector3.up;
        return ray;
    }

    private void RenderLine()
    {
        if ((destinationPoint - transform.position).magnitude > 1)
        {
            point.SetActive(true);
            Vector3[] path = GetPath(destinationPoint);
            lineRenderer.positionCount = path.Length;
            lineRenderer.SetPositions(path);
        }
        else
        {
            point.SetActive(false);
            lineRenderer.positionCount = 0;
        }
    }

    void MoveToDestination()
    {
        if (isChoosenOne && isAlive)
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

    void ShotButtonAction()
    {
        if (isChoosenOne)
            Shot();
    }

    void Shot()
    {
        if (movementPoints > 25 && isAlive)
        {
            var succes = manager.Shot();
            if (succes)
            {
                movementPoints -= 25;
                battleUI.SetDefault();
            }
        }

    }

    void MovementControl()
    {
        if (isChoosenOne && !agent.isStopped && movementPoints <= 0 )
            Stop();
    }

    public void RenewPoints() { movementPoints = consts.MovementPoints; }

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

        //int pathPoints = path.corners.Length + 2;

        //Vector3[] resultPath = new Vector3[pathPoints];
        //resultPath[0] = transform.position;
        //path.corners.CopyTo(resultPath, 1);
        //resultPath[pathPoints - 1] = targetPosition;
        //List<Vector3> pathPoints;
        //for (int i = 0; i < path.corners.Length - 1; i++)
        //{
        //    if (path.corners[i].z != path.corners[i+1].z)
        //    {

        //    }
        //}

        return path.corners;
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

    public void SetChoosenOne(bool choosen = true) { isChoosenOne = choosen; }

    public void GotDamage(float damage) {
        Debug.Log("currentHealth: " + currentHealth);
        Debug.Log("GotDamage: " + damage);
        currentHealth -= damage;
        Debug.Log("health left: " + currentHealth);
    }
}
