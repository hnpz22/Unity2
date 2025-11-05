using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text ammoText;

    public static GameManager Instance { get; private set; }

    public static GameManager InstanceOrNull() => Instance;

    public int gunAmmo = 10;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private int currentHealth = 100;

    [SerializeField]
    private int maxHealth = 100;

    private int lastDisplayedAmmo = int.MinValue;
    private int lastDisplayedHealth = int.MinValue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
    }

    public void LoseHealth(int healthToReduce)
    {
        currentHealth = Mathf.Clamp(currentHealth - healthToReduce, 0, maxHealth);
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Has Muerto");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public int CurrentHealth => currentHealth;

    private void UpdateAmmoUI()
    {
        if (ammoText == null)
        {
            return;
        }

        if (lastDisplayedAmmo != gunAmmo)
        {
            ammoText.text = gunAmmo.ToString();
            lastDisplayedAmmo = gunAmmo;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText == null)
        {
            return;
        }

        if (lastDisplayedHealth != currentHealth)
        {
            healthText.text = currentHealth.ToString();
            lastDisplayedHealth = currentHealth;
        }
    }
}
