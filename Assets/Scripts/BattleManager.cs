using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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

    void Start()
    {
        //battleUI.shotButton.onClick.AddListener(ChangeShotState);
    }

    public void Configure()
    {
        TopAliveCharacters = characters.Take(4).Select(person => person.GetComponent<CharacterController>()).ToList();
        BottomAliveCharacters = characters.Skip(4).Take(4).Select(person => person.GetComponent<CharacterController>()).ToList();
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
        SetCastAvailable();

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

    public void Shot(CharacterController character = null, CharacterController to = null)
    {
        CharacterController activeChar = (character != null) ? character : GetActiveChar();
        if (activeChar == null)
            return;
        
        var aimChar = to;
        if (aimChar == null)
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
                aimChar = InactiveCharacters[index];
            }
            else
                return;
        }
        else
            return;
        Cast(character: activeChar, to: aimChar);
    }

    private void Cast(CharacterController character, CharacterController to )
    {
        if (CheckClose(character.transform.position, to.transform.position, character.GetComponent<CharacterController>().CastRange))
            to.GotDamage(character.CastPower);
    }

    public void CharacterOutOfHealth(CharacterController character)
    {

        //var id = character.GetInstanceID();

        TopAliveCharacters.Remove(character);
        BottomAliveCharacters.Remove(character);

    }
}