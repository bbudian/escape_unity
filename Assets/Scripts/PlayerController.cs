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
    float movementSpeed, walkSpeed, runSpeed, currentHealth, maxHealth, currentMana, maxMana, currentStamina, maxStamina;
    Animator animator;
    Directions currentDirection;
    string race;
    WeaponType equippedWeapon;
    int[] armor;
    bool isAiming;
    string levelName;
    string debugTileLayer;
    BoxCollider2D environmentCollision, attackCollision;

    // Use this for initialization
    void Start()
    {
        forward = new Vector3(0, -1, 0);
        armor = new int[(int)(ArmorType.NumArmorSlots)];
        walkSpeed = 2.0f;
        runSpeed = 3.5f;
        movementSpeed = walkSpeed;
        animator = GetComponent<Animator>();
        PauseAnimation("tanHumanMWalkDown");
        currentDirection = Directions.Down;
        race = "tanHumanM";
        isAiming = false;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Environment";
        levelName = "TestMap";
        debugTileLayer = "EMPTY";
        environmentCollision = gameObject.AddComponent<BoxCollider2D>();
        environmentCollision.size = new Vector2(0.32f, 0.2f);
        environmentCollision.center = new Vector2(0.0f, -0.21f);
        environmentCollision.name = "playerEnvCol";
        transform.Translate(1f, -1f, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 prevPosition = transform.position;
        HandleInput();
        SetCameraPos();
        CheckTile();
    }

    void SetCameraPos()
    {
        Vector3 cameraPosition = GameObject.Find("Main Camera").transform.position;
        cameraPosition.x = transform.position.x;
        cameraPosition.y = transform.position.y;
        GameObject.Find("Main Camera").transform.position = cameraPosition;
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
            //Based on Currently equipped weapon
            //Change to 
            //Attack(currentDirection);
            BeginBowAttack(currentDirection); // Use this in Attack(currentDirection)
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FireArrow(currentDirection);
        }
        if (Input.GetKey(KeyCode.Space))
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

        //Debug Keys
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject.Find("TestMap").GetComponent<TileMap>().DisplayMapLayers();
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
    }
    bool CheckTile()
    {
        float yOffset = -0.12f;
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + yOffset);
        Tile.TileLayers layer = GameObject.Find(levelName).GetComponent<TileMap>().CheckPositionOnMap(position);
        debugTileLayer = layer.ToString();
        switch (layer)
        {
            case Tile.TileLayers.Passable:
                break;
            case Tile.TileLayers.DOT:
                TakeDamage(4); //Damage will be based on tile.Damage;
                break;
            case Tile.TileLayers.Kill:
                TakeDamage(float.MaxValue);
                break;
            case Tile.TileLayers.Impassable:
                return false;
            case Tile.TileLayers.OffMap:
                break;
            default:
                break;
        }
        return true;
    }

    void TakeDamage(float damageAmount)
    {
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2 - debugTileLayer.Length,Screen.height/2 + 30, 100, 100), debugTileLayer);
        
        // Make a background box
        GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
        {
           // Application.LoadLevel(1);
        }

        // Make the second button.
        if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2"))
        {
            // Application.LoadLevel(2);
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision with " + collision.ToString());
    //}

}//Class End
