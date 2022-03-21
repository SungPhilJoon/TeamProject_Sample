using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float delayTimeToDestroy = 1.0f;
    private int damage;
    private Text text;

    public int Damage
    {
        get => damage;
        set
        {
            damage = value;
            text.text = damage.ToString();
        }
    }

    void Start()
    {
        Destroy(gameObject, delayTimeToDestroy);
        text = GetComponent<Text>();
    }
}
