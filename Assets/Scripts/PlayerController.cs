using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    enum WeaponType
    {
        Sword, Bow, Spear
    }
    enum ArmorType
    {
        Head, Chest, Legs, Feet, Hands, Shoulders, NumArmorSlots
    }
    enum Directions
    {
        Left, Up, Right, Down
    }

    Vector3 forward, side;
    float movementSpeed, walkSpeed, runSpeed;
    Animator animator;
    Directions currentDirection;
    string race;
    WeaponType equippedWeapon;
    int[] armor;
    bool isAiming;

    // Use this for initialization
    void Start()
    {
        forward = new Vector3(0, -1, 0);
        armor = new int[(int)(ArmorType.NumArmorSlots)];
        walkSpeed = 2.0f;
        runSpeed = 5.0f;
        movementSpeed = walkSpeed;
        animator = GetComponent<Animator>();
        PauseAnimation("tanHumanMWalkDown");
        currentDirection = Directions.Down;
        race = "tanHumanM";
        isAiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            Move(Directions.Up);
            PlayAnimation("tanHumanMWalkUp");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(Directions.Down);
            PlayAnimation("tanHumanMWalkDown");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(Directions.Left);
            PlayAnimation("tanHumanMWalkLeft");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Directions.Right);
            PlayAnimation("tanHumanMWalkRight");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = runSpeed;
            animator.speed = 1.5f;
        }

        //Attack
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            BeginBowAttack(currentDirection);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FireArrow(currentDirection);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            //if(animator.GetFloat("Exit Time") == 1.0f)
            //{
            //    Debug.Log("Animation End");
            //}
        }

        //Animation Reset
        if (Input.GetKeyUp(KeyCode.D))
            PauseAnimation("tanHumanMWalkRight");
        if (Input.GetKeyUp(KeyCode.A))
            PauseAnimation("tanHumanMWalkLeft");
        if (Input.GetKeyUp(KeyCode.S))
            PauseAnimation("tanHumanMWalkDown");
        if (Input.GetKeyUp(KeyCode.W))
            PauseAnimation("tanHumanMWalkUp");
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.speed = 1.0f;
            movementSpeed = walkSpeed;
        }

    }

    void PlayAnimation(string animation)
    {
        if (animator)
        {
            animator.speed = 1.0f;
            animator.Play(animation);
            //Debug.Log(animator.speed);
        }
    }

    void PauseAnimation(string animation, int keyFrame = -1, float time = 0)
    {
        if (animator)
        {
            animator.speed = 0.0f;
            animator.Play(animation, -1, 0);
        }
        //Debug.Log("Pausing");
    }

    void Move(Directions direction)
    {
        currentDirection = direction;
        switch (direction)
        {
            case Directions.Left:
                forward = new Vector3(-1, 0, 0);
                transform.position += forward * movementSpeed * Time.deltaTime;
                break;
            case Directions.Up:
                forward = new Vector3(0, 1, 0);
                transform.position += forward * movementSpeed * Time.deltaTime;
                break;
            case Directions.Right:
                forward = new Vector3(1, 0, 0);
                transform.position += forward * movementSpeed * Time.deltaTime;
                break;
            case Directions.Down:
                forward = new Vector3(0, -1, 0);
                transform.position += forward * movementSpeed * Time.deltaTime;
                break;
            default:
                Debug.Log("Huh...");
                break;
        }
    }

    void BeginBowAttack(Directions direction)
    {
        animator.SetBool("Fire", false);
        switch (direction)
        {
            case Directions.Left:
                PlayAnimation("tanHumanMBowDrawLeft");
                break;
            case Directions.Up:
                PlayAnimation("tanHumanBowDrawUp");
                break;
            case Directions.Right:
                PlayAnimation("tanHumanMBowDrawRight");
                break;
            case Directions.Down:
                PlayAnimation("tanHumanMBowDrawDown");
                break;
            default:
                break;
        }
    }

    void FireArrow(Directions direction)
    {
        animator.SetBool("Fire", true);
        //switch (direction)
        //{
        //    case Directions.Left:
        //        animator.SetBool("Fire", true);
        //        //PauseAnimation("tanHumanMBowDrawLeft");
        //        //PlayAnimation("tanHumanMBowReleaseLeft");
        //        break;
        //    case Directions.Up:
        //        PauseAnimation("tanHumanMBowDrawUp");
        //        PlayAnimation("tanHumanMBowReleaseUp");
        //        break;
        //    case Directions.Right:
        //        PauseAnimation("tanHumanMBowDrawRight");
        //        PlayAnimation("tanHumanMBowReleaseRight");
        //        break;
        //    case Directions.Down:
        //        PauseAnimation("tanHumanMBowDrawDown");
        //        PlayAnimation("tanHumanMBowReleaseDown");
        //        break;
        //    default:
        //        break;
        //}
    }

}//Class End
