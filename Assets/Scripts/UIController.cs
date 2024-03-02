using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    public void SetHealth(float health)
    {
        _healthSlider.value = health;
    }
}
