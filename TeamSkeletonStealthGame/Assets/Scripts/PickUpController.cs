using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Weapon weaponScript;
    private Player playerScript; // Reference to the Player
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Transform playerTransform;

    private FloatingItem floatingitem;

    public float pickUpRange = 2.0f; // Distance for pickup
    public bool equipped = false;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>(); // Correctly assign playerScript

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        playerTransform = playerScript.transform;

        floatingitem = GetComponent<FloatingItem>(); // Get the FloatingItem component

    }

    private void Update()
    {
        if (equipped) return; // Skip if already equipped

        Vector3 distanceToPlayer = playerTransform.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if (playerScript.weapon !=null)
            return;

        equipped = true;
        transform.SetParent(playerTransform);

        // Adjust this offset based on your player model
        Vector3 weaponOffset = new Vector3(0.3f, -1.2f, 0f); // Example offset
        transform.localPosition = weaponOffset; // Adjust position
        transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,-60));
        transform.localScale = new Vector3(1.2f, 1.5f, 1f); // Adjust scale if necessary

        rb.isKinematic = true; // Disable physics
        coll.isTrigger = true; // Enable trigger

        weaponScript.enabled = true; // Enable weapon functionality
        playerScript.weapon = weaponScript; // Assign weapon to player

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
        var sort = weaponScript.GetComponent<SpriteRenderer>();
        sort.sortingLayerName = "Interacable Objects";

        // Set weapon position to player's current position with an offset
        Vector3 dropPosition = playerTransform.position;
        float dropOffsetX = (playerTransform.localScale.x > 0) ? 1f : -1f; // 1 unit to the right if facing right, -1 if facing left
        dropPosition += new Vector3(dropOffsetX, -2f, 0); // Offset by -2 units in the Y direction
        transform.position = dropPosition;

        rb.isKinematic = true; // Keep it kinematic since we're not using physics
        coll.isTrigger = false; // Enable collision
        weaponScript.enabled = false; // Disable weapon functionality
        playerScript.weapon = null; // Clear reference to dropped weapon

        if (floatingitem != null)
        {
            floatingitem.SetPosition(dropPosition); // Update floating effect position
            floatingitem.enabled = true; // Re-enable the floating effect
        }
    }
}


}
