using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptedCards", order = 1)]
public class ScriptedCards : ScriptableObject
{
    public CardType _cardType;

    public GameObject _cardModel; 

    public int _amount; //Amount of coins it provides
    public int _cardID;

    [Space]
    [TextArea(10,14)] public string _description;
}

