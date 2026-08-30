using System;
using System.Collections.Generic;
using UnityEngine;

public class FireLighter : MonoBehaviour
{
    public Action<Fire> OnEnterFire;
    public Action<Fire> OnExitFire;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D " + collision.gameObject.name);
        if (collision.CompareTag("Source"))
        {
            Debug.Log("Enter fire" + collision.gameObject.name);
            Fire fire = collision.GetComponent<Fire>();
            OnEnterFire?.Invoke(fire);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Source"))
        {  
            Fire fire = collision.GetComponent<Fire>();
            OnExitFire?.Invoke(fire);
        }
    }
}
