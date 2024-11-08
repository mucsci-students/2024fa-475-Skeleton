using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(FloatingItem))]


public class PickUpController : MonoBehaviour
{
    //the weapon object this script is attached to. every weapon has
    // a pickup controller
    private Weapon weapon;
    private Player player; // Reference to the Player

    private FloatingItem floatingitem; //non equipped object logic
    public float pickUpRange = 2.0f; //radius to allow pickup
    public bool equipped = false; //is the weapon equipped

    private Transform player_pos; //players position

    private Vector3 distanceToPlayer; //weapon to player
    

    private void Start()
    {
        //Finds Game object tagged as player in the scene, THEN gets player script from it.
        GameObject p = GameObject.FindWithTag("Player");
        player = p.GetComponent<Player>(); 
        player_pos = player.transform;
        floatingitem = GetComponent<FloatingItem>(); // Get the FloatingItem component

    }

    private void Update()
    {
        if (equipped) return; // Skip if already equipped

        distanceToPlayer = player_pos.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if (player.weapon !=null)
            return;

        equipped = true;
        transform.SetParent(player_pos);

        // Adjust this offset based on your player model
        Vector3 weaponOffset = new Vector3(0.3f, -1.2f, 0f); // Example offset
        transform.localPosition = weaponOffset; // Adjust position
        transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,-60));
        transform.localScale = new Vector3(1.2f, 1.5f, 1f); // Adjust scale if necessary


        weapon.enabled = true; // Enable weapon functionality
        player.weapon = weapon; // Assign weapon to player

        if (floatingitem != null)
        {
            floatingitem.enabled = false; // Disable the floating effect
        }
    }


   public void Drop()
{
    if (equipped)
    {
        equipped = false;
        transform.SetParent(null); // Detach from player
        var sort = weapon.GetComponent<SpriteRenderer>();
        sort.sortingLayerName = "Interacable Objects";

        // Set weapon position to player's current position with an offset
        Vector3 dropPosition = player_pos.position;
        float dropOffsetX = (player_pos.localScale.x > 0) ? 1f : -1f; // 1 unit to the right if facing right, -1 if facing left
        dropPosition += new Vector3(dropOffsetX, -2f, 0); // Offset by -2 units in the Y direction
        transform.position = dropPosition;

        
        weapon.enabled = false; // Disable weapon functionality
        player.weapon = null; // Clear reference to dropped weapon

        if (floatingitem != null)
        {
            floatingitem.SetPosition(dropPosition); // Update floating effect position
            floatingitem.enabled = true; // Re-enable the floating effect
        }
    }
}


}
