using UnityEngine;
public enum CardType
{
    ATTACK,
    STEAL,
    SHIELD,
    JOKER,
    ENERGY,
    COINS,
    FORTUNEWHEEL,
    SLOTMACHINE
}

public class Cards : MonoBehaviour
{
    public int _cardID;
    public CardType _cardType;
    public Vector3 _Position;
}
