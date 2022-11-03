using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;


public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletForce = 20f;
    [SerializeField] Color hitColor;
    [SerializeField] Color hitZombieColor;
    [SerializeField] ParticleSystem particles;
    MeshRenderer meshRend;
    float dmg;
    PlayerBehaviour player;
    Rigidbody bulletRb;

    IDamageable target;

     void OnEnable()
    {
        particles.startColor = hitZombieColor;
        meshRend = GetComponent<MeshRenderer>();
        bulletRb = GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);

        Destroy(this.gameObject, 3f);
    }

    public void Init(int damage, PlayerBehaviour player)
    {
        dmg = damage;
        this.player = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var interactable = collision.gameObject.GetComponent<IDamageable>();

        if (interactable != null)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            particles.startColor = hitZombieColor;
            target = collision.gameObject.GetComponent<EnemyBehaviour>();
            target.TakeDamage(1);
            bulletRb.isKinematic = true;
        }
        else
        {
            particles.startColor = hitColor;
        }


        meshRend.enabled = false;
        particles.Play();

        Destroy(this.gameObject, .5f);
    }

}
