using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    public int level; 
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player")
        {
            Player player = FindObjectOfType<Player>();
            player.GiveClearance(level);
            Destroy(gameObject);
        }

    }
}
