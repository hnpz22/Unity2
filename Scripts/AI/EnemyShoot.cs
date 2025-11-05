using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;

    public Transform spawnBulletPoint;

    private Transform playerPosition;

    [SerializeField]
    private float bulletSpeed = 25f;



    void Start()
    {
        playerPosition = FindObjectOfType<PlayerMovement>().transform;

        Invoke("ShootPlayer",3);
    }


    void Update()
    {

    }

    void ShootPlayer()
    {
        Vector3 direction = (playerPosition.position - spawnBulletPoint.position).normalized;

        GameObject newBullet;

        newBullet = Instantiate(enemyBullet,spawnBulletPoint.position,spawnBulletPoint.rotation);

        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * bulletSpeed;
        }

        Invoke("ShootPlayer", 3);

    }


}
