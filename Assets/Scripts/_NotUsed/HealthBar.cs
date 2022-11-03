//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class HealthBar : MonoBehaviour
//{
//    public static HealthBar instance;
//    [SerializeField] Image healthBarImage;
//    [SerializeField] Color greenC;
//    [SerializeField] Color orangeC;
//    [SerializeField] Color redC;
//    Animator anim;


//    private void Awake()
//    {
//        instance = this;
//        anim = GetComponent<Animator>();
//    }

//    public void UpdateHealthBar(float maxHealth, float damageTaken)
//    {
//        float r = damageTaken / maxHealth;

//        healthBarImage.fillAmount -= r;

//        UpdateColor();

//        anim.SetTrigger("update");

//        if (healthBarImage.fillAmount < 0) healthBarImage.fillAmount = 0;
//    }

//    void UpdateColor()
//    {
//        if (healthBarImage.fillAmount <= 0.4f)
//            healthBarImage.color = redC;
//        else if (healthBarImage.fillAmount > 0.4f && healthBarImage.fillAmount <= 0.7f)
//            healthBarImage.color = orangeC;
//        else healthBarImage.color = greenC;
//    }

//}
