using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 0.5f;
    public LayerMask pickupLayer;
    public Transform holdPoint;
    public float holdDistance = 0.118f;

    [Header("Delivery Settings")]
    public float interactRange = 0.5f;
    public LayerMask freezerLayer;

    private GameObject heldObject;
    private Collider2D heldCollider;
    private Vector2 lastMoveDir = Vector2.right;

    void Update()
    {
        UpdateMoveDirection();

        // Handle key input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                // Try to deliver; drop ONLY if no freezer nearby
                if (!TryDeliver())
                {
                    // Check if any freezer is nearby at all
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, freezerLayer);
                    if (hits.Length == 0)
                    {
                        Drop();
                    }
                    else
                    {
                        // Wrong freezer nearby, keep tub in hands
                        Debug.Log("Cannot deliver here! Wrong freezer.");
                    }
                }
            }
        }
    }

    // Move hold point based on player input
    void UpdateMoveDirection()
    {
        if (holdPoint == null) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y);
        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir.normalized;

        holdPoint.localPosition = lastMoveDir * holdDistance;
    }

    // Attempt to pick up an ice cream tub
    void TryPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickupLayer);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("IceCream")) continue;

            IceCreamTub tub = hit.GetComponent<IceCreamTub>();
            if (tub == null) continue;

            tub.OnPickedUp(); // Notify tub it was picked up

            heldObject = hit.gameObject;
            heldCollider = heldObject.GetComponent<Collider2D>();

            if (heldCollider != null)
                heldCollider.enabled = false;

            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;

            break; // only pick up one
        }
    }

    // Attempt to deliver held tub to a nearby freezer
    // Returns true if delivery attempted
    bool TryDeliver()
    {
        if (heldObject == null) return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, freezerLayer);

        bool freezerNearby = false;

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Freezer")) continue;

            freezerNearby = true; 

            IceCreamTub tub = heldObject.GetComponent<IceCreamTub>();
            Freezer freezer = hit.GetComponent<Freezer>();

            if (tub != null && freezer != null)
            {
                if (tub.flavor == freezer.freezerFlavor)
                {
                    GameManager.Instance.OnTubDelivery(tub.flavor);

                    if (freezer.deliveryEffect != null)
                        Instantiate(freezer.deliveryEffect, freezer.transform.position, Quaternion.identity);

                    Drop();
                    Destroy(tub.gameObject);

                }
                else
                {
                    GameManager.Instance.ResetStreak();
                    Debug.Log("Wrong freezer!");
                }

                return true; // attempted delivery
            }
        }
        return freezerNearby; // true if freezer nearby (wrong flavor), false if none
    }
    public void Drop()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);

            if (heldCollider != null)
                heldCollider.enabled = true;

            heldObject = null;
            heldCollider = null;
        }
    }
    // Optional: allow other scripts to check what player is holding
    public GameObject GetHeldObject()
    {
        return heldObject;
    }
}