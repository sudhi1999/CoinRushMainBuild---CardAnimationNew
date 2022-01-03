using UnityEngine;
using UnityEngine.UI;
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
    private float rotationZ;
    Sprite changeSprite;
    Vector2 prePos;

    public void PlayTwoCardMatchAnim()
    {
        Animation anim = GetComponent<Animation>();

        AnimationCurve CurvePosY;
        AnimationCurve CurvePosX;

        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
     

        Keyframe[] PosY;
        PosY = new Keyframe[3];
        PosY[0] = new Keyframe(0f, transform.localPosition.y);
        PosY[1] = new Keyframe(.5f, transform.localPosition.y+100);
        PosY[2] = new Keyframe(1f, transform.localPosition.y);
        CurvePosY = new AnimationCurve(PosY);


        Keyframe[] PosX;
        PosX = new Keyframe[3];
        PosX[0] = new Keyframe(0f, transform.localPosition.x);
        PosX[1] = new Keyframe(.5f, transform.localPosition.x);
        PosX[2] = new Keyframe(1f, transform.localPosition.x);
        CurvePosX = new AnimationCurve(PosX);

        clip.SetCurve("", typeof(Transform), "localPosition.y", CurvePosY);
        clip.SetCurve("", typeof(Transform), "localPosition.x", CurvePosX);
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);
    }
    void ChangeSprite()
    {
        gameObject.GetComponent<Image>().sprite = changeSprite;
    }

    public void PlayJokerSelectionPairAnim(bool isLeft,int index)
    {
        prePos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        float inPosX = index==1?312:372;
        inPosX = isLeft ? inPosX * -1 : inPosX;
        Animation anim = GetComponent<Animation>();

        AnimationCurve CurvePosY;
        AnimationCurve CurvePosX;
        AnimationClip clip = new AnimationClip();

        clip.legacy = true;
        Keyframe[] PosY;
        PosY = new Keyframe[2];
        PosY[0] = new Keyframe(0f, transform.localPosition.y);
        PosY[1] = new Keyframe(1f, transform.localPosition.y+300);

        CurvePosY = new AnimationCurve(PosY);


        Keyframe[] PosX;
        PosX = new Keyframe[2];
        PosX[0] = new Keyframe(0f, transform.localPosition.x);
        PosX[1] = new Keyframe(1f, inPosX);
      
        CurvePosX = new AnimationCurve(PosX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", CurvePosY);
        clip.SetCurve("", typeof(Transform), "localPosition.x", CurvePosX);
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);

    }
    public void PlayJokerSelectionPairGetBackAnim()
    {
        Animation anim = GetComponent<Animation>();

        AnimationCurve CurvePosY;
        AnimationCurve CurvePosX;
        AnimationClip clip = new AnimationClip();

        clip.legacy = true;
        Keyframe[] PosY;
        PosY = new Keyframe[2];
        PosY[0] = new Keyframe(0f, transform.localPosition.y);
        PosY[1] = new Keyframe(.4f, prePos.y);

        CurvePosY = new AnimationCurve(PosY);


        Keyframe[] PosX;
        PosX = new Keyframe[2];
        PosX[0] = new Keyframe(0f, transform.localPosition.x);
        PosX[1] = new Keyframe(.4f, prePos.x);

        CurvePosX = new AnimationCurve(PosX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", CurvePosY);
        clip.SetCurve("", typeof(Transform), "localPosition.x", CurvePosX);
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);
    }

    public void PlayThreeCardMatchAnim(float inPosX,Sprite s =null)
    {
       
        Animation anim = GetComponent<Animation>();

        AnimationCurve CurvePosY;
        AnimationCurve CurvePosX;
        AnimationCurve rotationCurvZ;
        AnimationCurve rotationCurvy;
        AnimationCurve scaleCurv;


        AnimationClip clip = new AnimationClip();

        clip.legacy = true;

        if (s != null)
        {
            changeSprite = s;
            Invoke("ChangeSprite", 1.2f);
            AnimationCurve rotationCurvX;
            Keyframe[] RotationX;
            RotationX = new Keyframe[2];
            RotationX[0] = new Keyframe(1f, 0);
            RotationX[1] = new Keyframe(1.5f, -360);
            rotationCurvX = new AnimationCurve(RotationX);
            clip.SetCurve("", typeof(Transform), "localEulerAngles.x", rotationCurvX);

        }

        Keyframe[] PosY;
        PosY = new Keyframe[2];
        PosY[0] = new Keyframe(0f, transform.localPosition.y);
        PosY[1] = new Keyframe(1f, 716);
      
        CurvePosY = new AnimationCurve(PosY);

        Keyframe[] PosX;
        PosX = new Keyframe[4];
        PosX[0] = new Keyframe(0f, transform.localPosition.x);
        PosX[1] = new Keyframe(1f, inPosX);
        PosX[2] = new Keyframe(1.75f, inPosX);
        PosX[3] = new Keyframe(2f, 0);
      
        CurvePosX = new AnimationCurve(PosX);

        rotationZ = (transform.localEulerAngles.z > 180) ? transform.localEulerAngles.z - 360 : transform.localEulerAngles.z;

        Keyframe[] RotationZ;
        RotationZ = new Keyframe[2];
        RotationZ[0] = new Keyframe(0f, rotationZ);
        RotationZ[1] = new Keyframe(1f, 0);

        Keyframe[] scale;
        scale = new Keyframe[5];
        scale[0] = new Keyframe(0f, .7f);
        scale[1] = new Keyframe(1f, 1f);
        scale[2] = new Keyframe(1.75f, 1f);
        scale[3] = new Keyframe(2.5f, .7f);
        scale[4] = new Keyframe(3f, 2);

        rotationCurvZ = new AnimationCurve(RotationZ);
        scaleCurv = new AnimationCurve(scale);



        Keyframe[] Rotationy;
        Rotationy = new Keyframe[2];
        Rotationy[0] = new Keyframe(2f, 0);
        Rotationy[1] = new Keyframe(3f, -1080);

        rotationCurvy = new AnimationCurve(Rotationy);
        clip.SetCurve("", typeof(Transform), "localEulerAngles.z", rotationCurvZ);
        clip.SetCurve("", typeof(Transform), "localEulerAngles.y", rotationCurvy);

        clip.SetCurve("", typeof(Transform), "localPosition.y", CurvePosY);
        clip.SetCurve("", typeof(Transform), "localPosition.x", CurvePosX);
        clip.SetCurve("", typeof(Transform), "localScale.x", scaleCurv);
        clip.SetCurve("", typeof(Transform), "localScale.y", scaleCurv);
        clip.SetCurve("", typeof(Transform), "localScale.z", scaleCurv);
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);


    }
}
