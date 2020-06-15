using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject[] characters = new GameObject[8];

    //public GameObject[] aliveCharacters = new GameObject[8];

    public List<CharacterController> TopAliveCharacters;

    public List<CharacterController> BottomAliveCharacters;

    public List<CharacterController> ActiveCharacters => GetCharactersRange(isTopTurn);

    public List<CharacterController> InactiveCharacters => GetCharactersRange(!isTopTurn);

    public bool isTopTurn = true;

    //private bool shotState = false;

    [SerializeField] BattleUI battleUI;
    private LineRenderer lineRenderer;

    void Start()
    {
        InitLineRenderer();
        //battleUI.shotButton.onClick.AddListener(ChangeShotState);

    }

    void InitLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void EndTurnAction()
    {
        foreach (CharacterController character in InactiveCharacters)
            character.RenewPoints();
        isTopTurn = !isTopTurn;
        if (ActiveCharacters.Count > 0)
            ActiveCharacters[0].SetPlayerAsChoosenAndUpdateCamera();
    }

    private void NextCharAction()
    {
        var currentActiveChar = GetActiveChar();
        for (int i = 0; i < ActiveCharacters.Count; ++i)
        {
            Debug.Log("(int i = 0; i < ActiveCharacters.Count; ++i): " + i);
            if (ActiveCharacters[i] == currentActiveChar)
            {
                Debug.Log("ActiveCharacters[i] == currentActiveChar " + i);
                var nextIndex = (i == ActiveCharacters.Count - 1) ? 0 : i + 1;
                ActiveCharacters[nextIndex].SetPlayerAsChoosenAndUpdateCamera();
                return;
            }
        }
        Debug.Log("ActiveCharacters[0].SetPlayerAsChoosenAndUpdateCamera();");
        ActiveCharacters[0].SetPlayerAsChoosenAndUpdateCamera();
    }

    private void CheckWinConditions()
    {
        var aliveActive = ActiveCharacters.Where(character => character.isAlive);
        var aliveInactive = InactiveCharacters.Where(character => character.isAlive);
        if (aliveActive == null || aliveActive.Count() <= 0)
        {
            var text = isTopTurn ? "Bottom team won!" : "Top team won!";
            Debug.Log(text);
            GameEnd();
        }
        else if (aliveInactive == null || aliveInactive.Count() <= 0)
        {
            var text = isTopTurn ? "Top team won!" : "Bottom team won!";
            Debug.Log(text);
            GameEnd();
        }
    }

    public void GameEnd()
    {
        Debug.Log("The End");
        SceneManager.LoadScene(SceneTag.Menu.ToString());
    }

    public void Configure()
    {
        TopAliveCharacters = characters.Take(4).Select(person => person.GetComponent<CharacterController>()).ToList();
        BottomAliveCharacters = characters.Skip(4).Take(4).Select(person => person.GetComponent<CharacterController>()).ToList();
        battleUI.endTurnButton.onClick.AddListener(EndTurnAction);
        battleUI.nextCharButton.onClick.AddListener(NextCharAction);
    }

    //void ChangeShotState()
    //{
    //    shotState = !shotState;
    //    battleUI.ShotOrMove(shotState);
    //}

    //CharacterController[] GetCharacters()
    //{
    //    return characters.Select(person => person.GetComponent<CharacterController>()).ToArray();
    //}

    List<CharacterController> GetCharactersRange(bool isTopTurn)
    {
        return isTopTurn ? TopAliveCharacters : BottomAliveCharacters;
    }

    public void DeselectAllCharacters()
    {
        foreach (GameObject characterObj in characters)
        {
            characterObj.GetComponent<CharacterController>().SetChoosenOne(false);
        }
    }

    bool CheckClose(Vector3 character, Vector3 against, float range)
    {
        return (Vector3.Distance(character, against) <= range);
    }

    bool CheckCastClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().CastRange);
    }

    bool CheckAttackClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().AttackRange);
    }

    bool CheckSightClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().SightRange);
    }

    List<GameObject> GetAvailableToCast(GameObject character, GameObject[] against)
    {
        List<GameObject> available = new List<GameObject>();
        for (int i = 0; i < against.Length; ++i)
            if (CheckCastClose(character, against[i]))
                available.Add(against[i]);

        return available;
    }

    void ShowAvailableToCast(GameObject character, GameObject[] against)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < against.Length; ++i)
        {
            if (CheckCastClose(character, against[i]))
                available.Add(i);
        }

        battleUI.ShowEnemyButtons(available.ToArray());
    }

    void Update()
    {
        CheckWinConditions();
        SetCastAvailable();
        
    }
    private void LateUpdate()
    {
        ShowAttackLine();
    }

    private void ShowAttackLine()
    {
        if (GetAimChar() != null)
        {
            lineRenderer.positionCount = 2;
            var attackPoint = GetAttackPoint();
            lineRenderer.SetPositions(new Vector3[] { GetActiveChar().transform.position, GetAttackPoint() });
        }
        else
            lineRenderer.positionCount = 0;
    }

    Vector3 GetAttackPoint()
    {
        Ray ray = new Ray();
        var from = GetActiveChar();
        var to = GetAimChar();
        ray.origin = from.transform.position;
        ray.direction = to.transform.position - from.transform.position;
        //Debug.DrawRay(ray.origin, ray.direction * 300, Color.blue);
        RaycastHit hit;
        Physics.SyncTransforms();
        if (Physics.Raycast(ray, out hit, 300)) // TODO change distance
            return hit.point;
        else
            return to.transform.position;
        //Debug.LogFormat("{0} {1}", hit.point, ActiveCharacters.Aggregate(false, (acc, p) => acc || !p.agent.isStopped));
        //Debug.DrawRay(ray.origin, ray.direction * 300, Color.blue);
        //Debug.Log(hit.collider.name);
        //return hit.point;
    }

    void SetCastAvailable()
    {
        var activeChar = GetActiveChar();
        if (activeChar != null) //.Aggregate((_, x) => x.isChoosenOne ? x : _).gameObject;
        {
            var arr = InactiveCharacters.Select(person => person.gameObject).ToArray();
            //List<GameObject> availableToCast = GetAvailableToCast(activeChar, arr);
            ShowAvailableToCast(activeChar.gameObject, arr);
            //if (availableToCast.Count > 0)
            //{
            //    var colors = battleUI.shotButton.colors;
            //    colors.normalColor = Color.red;
            //    battleUI.shotButton.colors = colors;
            //}
            //else
            //{
            //    var colors = battleUI.shotButton.colors;
            //    colors.normalColor = Color.white;
            //    battleUI.shotButton.colors = colors;
            //}
        }
    }

    private CharacterController GetActiveChar()
    {
        return ActiveCharacters.SingleOrDefault(x => x.isChoosenOne);
    }

    public bool Shot(CharacterController character = null, CharacterController to = null)
    {
        CharacterController activeChar = (character != null) ? character : GetActiveChar();
        if (activeChar == null)
            return false;

        var aimChar = (to != null) ? to : GetAimChar();

        if (aimChar == null)
            return false;

        return Cast(character: activeChar, to: aimChar);
    }

    private CharacterController GetAimChar()
    {
        int index = int.MaxValue;
        for (int i = 0; i < battleUI.enemyButtons.Length; ++i)
            if (battleUI.enemyButtons[i].colors.normalColor == Color.red)
            {
                index = i;
                break;
            }

        //var redBottuns = battleUI.enemyButtons.Where(button => button.colors.normalColor == Color.red).ToArray();
        if (index < InactiveCharacters.Count)
        {
            return InactiveCharacters[index];
        }
        else
            return null;
    }

    private bool Cast(CharacterController character, CharacterController to)
    {
        if (CheckClose(character.transform.position, to.transform.position, character.GetComponent<CharacterController>().CastRange))
        {
            to.GotDamage(character.CastPower);
            return true;
        }
        else
            return false;
    }

    public void CharacterOutOfHealth(CharacterController character)
    {
        TopAliveCharacters.Remove(character);
        BottomAliveCharacters.Remove(character);
    }

    public void CharacterAliveAgain(CharacterController character)
    {
        for (int i = 0; i < characters.Length; ++i)
            if (characters[i] == character)
                if (i < 4)
                    TopAliveCharacters.Insert(i, character);
                else
                    BottomAliveCharacters.Insert(i - 4, character);
    }
}