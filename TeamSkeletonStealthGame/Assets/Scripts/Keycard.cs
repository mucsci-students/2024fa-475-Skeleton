using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    [SerializeField]
    private int accessLevel; 
    public int clearance;

    [SerializeField]
    public bool hasTruckKey;

    public void Start(){
        clearance = accessLevel; //set public variable initialized in editor
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player")
        {
            Player player = FindObjectOfType<Player>();
            player.GiveClearance(accessLevel);
            if(gameObject.name == "Truck Key") {
                player.hasTruckKey = hasTruckKey;
            }
            Destroy(gameObject);

        }

    }

    public bool TryOpenDoor(GameObject door)
        {
            if (door.GetComponent<Door>() && door.GetComponent<Door>().CanKeycardUnlock(this) && !door.GetComponent<Door>().IsOpened())
            {
                door.GetComponent<Door>().UnlockWithKeycard(clearance);
                Destroy(gameObject);
                return true;
            }
            return false;
        }
}
