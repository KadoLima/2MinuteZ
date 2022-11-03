using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawns;
    [Space(20)]
    [SerializeField] Transform spawnedParent;

    [Header("Rules")]
    [SerializeField]float spawnInterval = 3f;
    float lastSpawn;
    [SerializeField]float maxEnemies = 5;

    [SerializeField]GameObject[] enemiesPrefabs;
    bool canSpawn = true;

    public static EnemySpawner instance;

    public Transform SpawnedParent
    {
        get => spawnedParent;
    }


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        enemiesPrefabs = Resources.LoadAll("Zombies", typeof(GameObject)).Cast<GameObject>().ToArray();
    }

    private void Update()
    {
        if (!GameManager.instance.IsPlayable)
            return;

        if (GameManager.instance.RemainingTime >= 30)
            spawnInterval = GameManager.instance.RemainingTime / 60f;

        SpawnEnemy();
    }



    private void OnEnable()
    {
        EventsManager.OnPlayerDied += ForceStopEnemiesSpawn;
    }

    private void OnDisable()
    {
        EventsManager.OnPlayerDied -= ForceStopEnemiesSpawn;
    }

    bool CanSpawn()
    {
        return canSpawn && spawnedParent.childCount < maxEnemies && !GameManager.instance.IsMissionComplete();
    }

    private void SpawnEnemy()
    {
        //if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        //    return;

        if (Time.time - lastSpawn < spawnInterval)
            return;

        GameObject enemy = null;

        if (CanSpawn() && RandomSpawnPoint() != null)
        {
            //if (PhotonNetwork.InRoom)
            //{
            //    GameObject rndEnemy = RandomEnemy();
            //    enemy = PhotonNetwork.Instantiate("Zombies/"+rndEnemy.name, RandomSpawnPoint().position, Quaternion.identity);
            //}
            //else
            enemy = Instantiate(RandomEnemy(), RandomSpawnPoint().position, Quaternion.identity);

            enemy.transform.SetParent(spawnedParent);
            lastSpawn = Time.time;
        }
    }

    public void ForceStopEnemiesSpawn(int a=0)
    {
        canSpawn = false;
    }

    GameObject RandomEnemy()
    {
        int r = Random.Range(0, enemiesPrefabs.Length);
        return enemiesPrefabs[r];
    }

    Transform RandomSpawnPoint()
    {
        int r = -1;

        //bool allVisible = true;

        //for (int i = 0; i < enemySpawns.Length; i++)
        //{
        //    if (!enemySpawns[i].GetComponent<SpawnPoint>().IsVisible)
        //    {
        //        allVisible = false;
        //        break;
        //    }
        //}

        //if (allVisible)
        //{
        //    Debug.LogError("YOUR SCENE CAMERA IS LOOKING AT ALL SPAWN POINTS AT ONCE! ENEMIES WILL NOT SPAWN");
        //    return null;
        //}

#if UNITY_EDITOR
        r = Random.Range(0, enemySpawns.Length);
        return enemySpawns[r];
#endif
        do
        {
            r = Random.Range(0, enemySpawns.Length);
        } while (enemySpawns[r].GetComponent<SpawnPoint>().IsVisible);

        return enemySpawns[r];
    }

    public void KillAllEnemies()
    {
        StartCoroutine(KillAllEnemiesSequence());
    }


    IEnumerator KillAllEnemiesSequence()
    {
        foreach (Transform item in spawnedParent)
            item.gameObject.GetComponent<EnemyBehaviour>().KillByChopper();
        
        yield return new WaitForSeconds(0.35f);

        foreach (Transform item in spawnedParent)
            item.gameObject.GetComponent<EnemyBehaviour>().Die(item.gameObject.GetInstanceID());
    }



}
