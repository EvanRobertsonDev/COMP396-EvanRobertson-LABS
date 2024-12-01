using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private int maxStamina;
    [SerializeField] private FloatEventChannel staminaChanel;
    private int currentStamina;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    void Start()
    {
        PublishStaminaPercentage();
    }

    public void UpdateStamina(int value)
    {
        currentStamina = value;
        PublishStaminaPercentage();
    }

    void PublishStaminaPercentage()
    {
        if (staminaChanel != null)
        {
            staminaChanel.Invoke(currentStamina / (float)maxStamina);
        }
    }

    
}
