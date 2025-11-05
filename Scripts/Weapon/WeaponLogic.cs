using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public Transform spawnPoint;

    public GameObject bullet;

    public float shotForce = 1500f;
    public float shotRate = 0.5f;

    private float shotRateTime = 0;

    private AudioSource audioSource;

    public AudioClip shotSound;

    public bool continueShooting = false;

    [SerializeField]
    private GameManager gameManager;

    private bool hasLoggedDegradedShot;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.InstanceOrNull();
        }

        if (gameManager == null)
        {
            Debug.LogWarning($"{nameof(WeaponLogic)}: GameManager reference missing. Weapon will operate in degraded mode.");
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
        {
            if (Time.time > shotRateTime && HasAmmoAvailable())
            {
                if (continueShooting)
                {
                    InvokeRepeating("Shoot",.001f,shotRate);
                }

                else
                {
                    Shoot();
                }
                        
            }
            
        }

        else if (Input.GetButtonUp("Fire1") && continueShooting && Time.timeScale != 0)
        {
            CancelInvoke("Shoot");
        }

    }


    public void Shoot()
    {
        if (!HasAmmoAvailable())
        {
            CancelInvoke("Shoot");
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        if (gameManager != null)
        {
            gameManager.gunAmmo--;
        }

        GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(spawnPoint.forward * shotForce);
        }

        shotRateTime = Time.time + shotRate;

        Destroy(newBullet, 5);
    }

    private bool HasAmmoAvailable()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.InstanceOrNull();
        }

        if (gameManager == null)
        {
            LogDegradedShotOnce();
            return true;
        }

        return gameManager.gunAmmo > 0;
    }

    private void LogDegradedShotOnce()
    {
        if (!hasLoggedDegradedShot)
        {
            Debug.Log($"{nameof(WeaponLogic)}: Shooting without GameManager. HUD and resource tracking will not update.");
            hasLoggedDegradedShot = true;
        }
    }
}
