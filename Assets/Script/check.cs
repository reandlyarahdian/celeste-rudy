using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    private Collider2D collider;
    public bool test;
    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        test = ClickSound.click;
        if (test) collider.isTrigger = true;
    }
}
