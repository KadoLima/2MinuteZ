using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Photon.Pun;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    NavMeshAgent agent;
    [Header("Combat Info")]
    [SerializeField] float minSpeed = 1.2f;
    [SerializeField] float maxSpeed = 2.5f;
    [SerializeField] float maxFinalSpeed = 4f;
    float speed = 0f;
    [SerializeField] float shotsToDie = 4;
    [SerializeField] float attackCD = 0.5f;
    [SerializeField] float damage = 1f;
    [SerializeField] float searchTargetInterval = 0.3f;
    [SerializeField] Collider hitterArm;
    [SerializeField] Collider bodyBulletCollider;
    float lastAttack;
    [Header("References")]
    [SerializeField] SkinnedMeshRenderer meshRend;
    [SerializeField] Material transparentMat;
    [SerializeField] GameObject[] positionIndicators;
    [SerializeField] ParticleSystem chopperKillParticles;
    Animator enemyAnim;
    float pointsWorth = 10;
    int uniqueID;

    public Animator EnemyAnim => enemyAnim;
    public float Damage => damage;



    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        pointsWorth *= (shotsToDie+Mathf.RoundToInt(speed));
        enemyAnim = GetComponent<Animator>();
        hitterArm.enabled = false;

    }

    private void OnEnable()
    {
        uniqueID = this.gameObject.GetInstanceID();

        InvokeRepeating("CheckAttack", 0.1f, 0.2f);

        EventsManager.OnEnemyDied += Die;
        EventsManager.OnEnemyDied += GivePoints;

        InvokeRepeating(nameof(SearchForPlayer), 0.15f, searchTargetInterval);
    }

    private void OnDisable()
    {
        EventsManager.OnEnemyDied -= Die;
        EventsManager.OnEnemyDied -= GivePoints;
    }

    public float PointsWorth => pointsWorth;

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }


    #region Navigation
    private void EnemyMovement()
    {
        //SarchForPlayer();
        EnemyAnim_Walk(agent.remainingDistance > agent.stoppingDistance);
    }

    private void SearchForPlayer() //Invoked
    {
        if (isDead())
            return;

        if (GameManager.instance.RemainingTime > 0)
        {
            agent.speed = speed + (5f / GameManager.instance.RemainingTime);
            agent.speed = Mathf.Clamp(agent.speed, minSpeed, maxFinalSpeed);
        }

        agent.destination = GameManager.instance.Player.transform.position;

        //if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    agent.destination = GameManager.instance.Player.transform.position;
        //    return;
        //}


        //float minDist = float.MaxValue;
        //int nearestIndex = -1;
        //for (int i = 0; i < MultiplayerManager.instance.GetCoopPlayers().Count; i++)
        //{
        //    if (MultiplayerManager.instance.GetCoopPlayers()[i].GetComponent<PlayerBehaviour>().IsDead())
        //        continue;

        //    var dist = Vector3.Distance(transform.position, MultiplayerManager.instance.GetCoopPlayers()[i].transform.position);
        //    if (dist < minDist)
        //    {
        //        nearestIndex = i;
        //        minDist = dist;
        //    }
        //}
        //agent.destination = MultiplayerManager.instance.GetCoopPlayers()[nearestIndex].position;
    }


    #endregion

    #region Combat

    public bool CanAttack()
    {
        return Time.time - lastAttack >= attackCD;
    }

    void CheckAttack()
    {
        if (isDead())
            return;

        if (!agent.pathPending &&
            agent.remainingDistance <= (agent.stoppingDistance) &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            if (!CanAttack())
            {
                EnemyAnim_Attack(false);
                return;
            }
            EnemyAnim_Attack(true);
            StartCoroutine(ArmCollider());
            lastAttack = Time.time;
        }
        else EnemyAnim_Attack(false);
    }

    IEnumerator ArmCollider()
    {
        yield return new WaitForSeconds(0.4f);
        hitterArm.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitterArm.enabled = false;
    }


    public void TakeDamage(float amount)
    {
        VirtualCameraController.instance.ShakeCamera();
        shotsToDie-=amount;
        StartCoroutine(FlashRed());

        if (shotsToDie <= 0)
        {
            EventsManager.OnEnemyDied?.Invoke(uniqueID);
        }
            
    }

    public void KillByChopper()
    {
        StartCoroutine(FlashRed());
        chopperKillParticles.Play();

    }

    IEnumerator FlashRed()
    {
        meshRend.material.color = Color.red;
        yield return new WaitForSeconds(0.09f);
        meshRend.material.color = Color.white;
    }

    public void Die(int ID)
    {
        if (uniqueID != ID)
            return;
        agent.isStopped = true;
        this.bodyBulletCollider.enabled = false;

        foreach ( GameObject item in positionIndicators)
        {
            item.SetActive(false);
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    bool isDead()
    {
        return shotsToDie <= 0;
    }

    public void GivePoints(int ID)
    {
        if (uniqueID != ID)
            return;

        GameManager.instance.AddPoints(pointsWorth);
    }

    IEnumerator FadeOutAndDestroy()
    {
        float alpha = 1;
        meshRend.material = transparentMat;
        while (alpha>0)
        {
            alpha -= Time.deltaTime*3;
            meshRend.material.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        //if (PhotonNetwork.InRoom)
        //    PhotonNetwork.Destroy(this.gameObject);
        //else
        Destroy(this.gameObject);
    }
    #endregion

    #region Animation
    void EnemyAnim_Walk(bool s)
    {
        enemyAnim.SetBool("isWalking", s);
    }

    void EnemyAnim_Attack(bool s)
    {
        enemyAnim.SetBool("isAttacking", s);
    }
    #endregion
}
