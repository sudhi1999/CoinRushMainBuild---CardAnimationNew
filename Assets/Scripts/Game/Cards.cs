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

    public void PlayPopAnimation()
    {
        Animation anim = GetComponent<Animation>();

        AnimationCurve CurvePosY;

        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        Keyframe[] PosY;
        PosY = new Keyframe[3];
        PosY[0] = new Keyframe(0f, transform.localPosition.y);
        PosY[1] = new Keyframe(.5f, transform.localPosition.y+50);
        PosY[2] = new Keyframe(1f, transform.localPosition.y);
        CurvePosY = new AnimationCurve(PosY);

        clip.SetCurve("", typeof(Transform), "localPosition.y", CurvePosY);
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);
    }
}
