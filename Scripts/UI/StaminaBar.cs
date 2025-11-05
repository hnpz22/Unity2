using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField]
    private Slider staminaSlider;

    [SerializeField]
    private float maxStamina = 100f;

    [SerializeField]
    private float regenRate = 20f;

    [SerializeField]
    private float spendRate = 25f;

    private float currentStamina;

    private Coroutine regenCo;
    private Coroutine spendCo;

    [SerializeField]
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (staminaSlider == null)
        {
            staminaSlider = GetComponent<Slider>();
        }

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
    }

    private void Start()
    {
        maxStamina = Mathf.Max(0f, maxStamina);
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
    }

    public void SpendStart(float rate)
    {
        spendRate = Mathf.Max(0f, rate);

        if (regenCo != null)
        {
            StopCoroutine(regenCo);
            regenCo = null;
        }

        if (spendCo == null && spendRate > 0f)
        {
            spendCo = StartCoroutine(SpendRoutine());
        }
    }

    public void SpendStop()
    {
        if (spendCo != null)
        {
            StopCoroutine(spendCo);
            spendCo = null;
        }

        if (!Mathf.Approximately(currentStamina, maxStamina) && regenCo == null && regenRate > 0f)
        {
            regenCo = StartCoroutine(RegenerateRoutine());
        }
    }

    private IEnumerator SpendRoutine()
    {
        while (true)
        {
            float previousValue = currentStamina;
            currentStamina = Mathf.Clamp(currentStamina - spendRate * Time.deltaTime, 0f, maxStamina);
            UpdateSlider(previousValue);

            if (Mathf.Approximately(currentStamina, 0f))
            {
                if (playerMovement != null)
                {
                    playerMovement.ForceStopSprinting();
                }
                break;
            }

            yield return null;
        }

        spendCo = null;

        if (!Mathf.Approximately(currentStamina, maxStamina) && regenCo == null && regenRate > 0f)
        {
            regenCo = StartCoroutine(RegenerateRoutine());
        }
    }

    private IEnumerator RegenerateRoutine()
    {
        while (!Mathf.Approximately(currentStamina, maxStamina))
        {
            float previousValue = currentStamina;
            currentStamina = Mathf.Clamp(currentStamina + regenRate * Time.deltaTime, 0f, maxStamina);
            UpdateSlider(previousValue);

            if (Mathf.Approximately(currentStamina, maxStamina))
            {
                break;
            }

            yield return null;
        }

        regenCo = null;
    }

    private void UpdateSlider(float previousValue)
    {
        if (staminaSlider == null)
        {
            return;
        }

        if (!Mathf.Approximately(previousValue, currentStamina) && !Mathf.Approximately(staminaSlider.value, currentStamina))
        {
            staminaSlider.value = currentStamina;
        }
    }

    public bool IsDepleted => Mathf.Approximately(currentStamina, 0f);
}
