using TMPro;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int curruntHealth;
    public int maxHealth;
    public TMP_Text healthText;


    private void Start()
    {
        healthText.text = "HP: " + curruntHealth + " / " + maxHealth;
    }
    public void ChangeHealth(int amount)
    {
        curruntHealth += amount;
        healthText.text = "HP: " + curruntHealth + " / " + maxHealth;
        if (curruntHealth <= 0)
        {
            gameObject.SetActive(false);
            healthText.text = "DEAD";
        }
    }
}
