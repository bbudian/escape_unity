using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{
    public enum AnimationSlot
    {
        walk_L = 0, walk_R, walk_U, walk_D,
        bowDraw_L, bowDraw_R, bowDraw_U, bowDraw_D,
        bowRelease_L, bowRelease_R, bowRelease_U, bowRelease_D,
        spearThrust_L, spearThrust_R, spearThrust_U, spearThrust_D,
        swordSlash_L, swordSlash_R, swordSlash_U, swordSlash_D, death
    }

    //FIELDS
    string[] walkDirections = { "WalkDown", "WalkUp", "WalkLeft", "WalkRight" };
    public AnimationClip[] animations;


    //FUNCTIONS

    // Use this for initialization
    void Start()
    {
        animations = new AnimationClip[21];
    }

    public void LoadAnimations(string animName)
    {
        //Load Walk Animations
        int walkStart = 0;
        foreach (string direction in walkDirections)
        {
            string filePath = "animations/" + animName + direction;
            AnimationClip animClip;
            animClip = Resources.Load<AnimationClip>(filePath);
            if (animClip == null)
            {
                Debug.Log("Failed to load animation: " + filePath + "Check Filename or location.");
            }
            //animations[walkStart++] = animClip;
            gameObject.animation.AddClip(animClip, direction);
        }

        //Load Bow Draw Animations

        //Load Bow Release Animations

        //Load Spear Thrust Animations

        //Load Sword Slash Animations


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
        }
    }

    public AnimationClip GetAnimation(AnimationSlot animSlot)
    {
        return animations[(uint)animSlot];
    }
}
