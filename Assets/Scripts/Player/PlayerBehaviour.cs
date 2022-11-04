using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;


public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    Camera mainCam;
    Vector3 movement;
    Vector3 rotation;
    Vector3 lookPos;
    float score;

    Animator playerAnim;
    Rigidbody playerRb;
    [Header("Player Info")]
    [SerializeField] float health = 5;
    float maxHealth;
    [SerializeField] float speed = 5;
    float shootWalkingSpeed;
    float defaultSpeed;
    [Header("Combat Info")]
    [SerializeField] float shootWalkPenalty = 0.3f;
    [SerializeField] int damage = 1;
    [SerializeField] float fireCD = 0.05f;
    [Header("References")]
    [SerializeField] GameObject gun;
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LineRenderer laserSight;
    [SerializeField] Transform firingPosition;
    [SerializeField] ParticleSystem muzzleParticles;
    [SerializeField] SpriteRenderer circleIndicator;
    [SerializeField] LayerMask groundLayer;
    Vector3 currentDirection;
    float lastShot;

    //FixedJoystick moveJoystick;
    //FixedJoystick rotationFireJoystick;

    //PhotonView photonView;


    public Vector3 CurrentDirection
    {
        get => currentDirection;
        set => currentDirection = value;
    }
    public float Health
    {
        get => health;
        set => health = value;
    }

    public float Damage => damage;

    public float Score => score;

    // Start is called before the first frame update
    void Start()
    {
        InitVariables();

        //SetLocalPlayer();

        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        EventsManager.OnPlayerDied += Die;

        //if (PhotonNetwork.InRoom)
        //    MultiplayerManager.instance.CoopPlayers(this.transform);
    }

    private void OnDisable()
    {
        EventsManager.OnPlayerDied -= Die;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove())
        {
            movement.z = Input.GetAxisRaw("Horizontal");
            movement.x = Input.GetAxisRaw("Vertical")*-1;
            Rotate();
            MoveAnimations();

            if (Input.GetMouseButton(0))
                FireCommand(true);


            if (Input.GetMouseButtonUp(0))
                FireCommand(false);

        }
    }

    private void FixedUpdate()
    {
        if (CanMove())
        {
            Move();
        }
    }

    private void Move()
    {
        if (movement.z != 0 && movement.x != 0)
            playerRb.MovePosition(playerRb.position + movement * speed * .707f * Time.fixedDeltaTime);
        else playerRb.MovePosition(playerRb.position + movement * speed * Time.fixedDeltaTime);

        
    }

    public void MoveAnimations()
    {
        if (movement.x != 0 || movement.z != 0)
        {
            PlayAnim_Run();
        }
        else if (movement.x == 0 && movement.z == 0)
        {
            PlayAnim_Idle();
        }
    }

    private void Rotate()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    #region Initialize Variables and References
    private void InitVariables()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();

        laserSight.enabled = false;
        gun.SetActive(false);

        maxHealth = health;

        defaultSpeed = speed;
        shootWalkingSpeed = speed * (1 - shootWalkPenalty);

        //moveJoystick = GameManager.instance.GetJoystick(0);
        //rotationFireJoystick = GameManager.instance.GetJoystick(1);

        //photonView = GetComponent<PhotonView>();
    }
    #endregion

    #region Movement
    //public void Move()
    //{
        //movement.x = moveJoystick.Vertical*-1;
        //movement.z = moveJoystick.Horizontal;

        //float joystickValue;
        //if (Mathf.Abs(moveJoystick.Direction.x) >= Mathf.Abs(moveJoystick.Direction.y))
        //    joystickValue = moveJoystick.Direction.x;
        //else joystickValue = moveJoystick.Direction.y;

        //if (Mathf.Approximately(Mathf.Abs(moveJoystick.Direction.x), Mathf.Abs(moveJoystick.Direction.y)))
        //    joystickValue = 0f;

        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        

        //playerAnim.SetFloat("joystick", Mathf.Abs(joystickValue));

        //currentDirection = new Vector3(movement.x, 0, movement.z);

        //if (movement.x != 0 || movement.z!=0)
        //{
        //    PlayAnim_Run();
        //}
        //else if (movement.x == 0 && movement.z == 0)
        //{
        //    PlayAnim_Idle();
        //}
    //}

    //private void Rotate()
    //{
    //    //float h = rotationFireJoystick.Horizontal;
    //    //float v = rotationFireJoystick.Vertical * -1;

    //    mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);


    //    Vector3 direction = new Vector3(v, 0, h);
    //    Vector3 lookAtPos = transform.position + direction;
    //    transform.LookAt(lookAtPos);

    //    if (rotationFireJoystick.Direction != Vector2.zero)
    //    {
    //        FireCommand(true);
    //    }
    //    else
    //    {
    //        FireCommand(false);
    //    }
    //}

    bool CanMove()
    {
        //if (photonView != null)
            //return !IsDead() && GameManager.instance.IsPlayable && photonView.IsMine;

        return !IsDead() && GameManager.instance.IsPlayable;
    }
    #endregion

    #region Visuals
    public void PlayAnim_Run()
    {
        string b = "isRunning";
        playerAnim.SetBool(b, true);
    }
    public void PlayAnim_Idle()
    {
        string b = "isRunning";
        playerAnim.SetBool(b, false);
    }

    public void PlayAnim_Dead()
    {
        string b = "isDead";
        playerAnim.SetBool(b, true);
    }

    public void SetIndicatorColor(Color c)
    {
        circleIndicator.color = c;
    }
    #endregion

    #region Combat
    void FireCommand(bool state)
    {
        if (state)
        {
            speed = shootWalkingSpeed;

            ShootBullet();

        }
        else speed = defaultSpeed;

        playerAnim.SetBool("isFiring", state);
        laserSight.enabled = state;
        gun.SetActive(state);
    }

    //GameObject RPCBullet;
    void ShootBullet()
    {
        if (Time.time - lastShot < fireCD)
            return;

        SFXManager.instance.PlayFiringGun(gunAudioSource);
        GameObject bullet = null;
        bullet = Instantiate(bulletPrefab, firingPosition.position, firingPosition.rotation);
        //if (PhotonNetwork.InRoom)
        //    bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firingPosition.position, firingPosition.rotation);
        //else bullet = Instantiate(bulletPrefab, firingPosition.position, firingPosition.rotation);

        bullet.GetComponent<Bullet>().Init(1, this);
        muzzleParticles.Play();
        lastShot = Time.time;

        gun.GetComponent<Animator>().SetTrigger("fire");
    }

    public void TakeDamage(float amount)
    {
        if (IsDead())
            return;

        //if (photonView!=null && !photonView.IsMine)
        //    return;

        Health -= amount;
        HUDManager.instance.UpdateHealthBar(maxHealth, amount);
        SFXManager.instance.PlayRandomPlayerTakeDamageSound(playerAudioSource);

        if (health <= 0)
        {
            SFXManager.instance.PlayPlayerDiedSound(playerAudioSource);
            EventsManager.OnPlayerDied?.Invoke(this.GetInstanceID());
        }
    }

    public void Die(int id)
    {
        if (id == this.GetInstanceID())
            PlayAnim_Dead();

        this.playerRb.constraints = RigidbodyConstraints.FreezeAll;
        transform.Find("BodyCollider").gameObject.SetActive(false);

        //if (photonView!=null && PhotonNetwork.LocalPlayer == this.photonView.Owner)
        //    MultiplayerManager.instance.SetRPC_AddToDeadPlayersCount();
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    #endregion

    #region Multiplayer

    //public bool isRoomHost()
    //{
    //    return photonView.Owner.IsMasterClient;
    //}

    //private void SetLocalPlayer()
    //{
    //    if (PhotonNetwork.InRoom)
    //    {
    //        if (PhotonNetwork.LocalPlayer == this.photonView.Owner)
    //        {
    //            GameManager.instance.Player = GetComponent<PlayerBehaviour>();
    //            VirtualCameraController.instance.SetCameraToFollow(this.transform);
    //        }

    //        if (isRoomHost())
    //            SetIndicatorColor(Color.green);
    //        else
    //            SetIndicatorColor(Color.blue);
    //    }
    //    else
    //    {
    //        GameManager.instance.Player = GetComponent<PlayerBehaviour>();
    //        VirtualCameraController.instance.SetCameraToFollow(this.transform);
    //        SetIndicatorColor(Color.green);
    //    }
    //}
    #endregion
}
