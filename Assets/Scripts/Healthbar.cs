using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private Slider slider;
    public Transform targetTransform;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void UpdateHealthBar(float newHealth, float maxHealth)
    {
        slider.value = newHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = targetTransform.position;
    }
}
