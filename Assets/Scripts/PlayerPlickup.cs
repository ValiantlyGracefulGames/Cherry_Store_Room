using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlickup : MonoBehaviour
{
    public float pickupRange = 0.5f;
    public LayerMask pickupLayer;
    public Transform holdPoint;
    public float holdDistance = 0.118f; // distance from player

    private GameObject heldObject;
    private Collider2D heldCollider;

    private Vector2 lastMoveDir = Vector2.right;

    void Update()
    {
        UpdateMoveDirection();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                Drop();
        }

        // Make held object follow hold point
        if (heldObject != null)
            heldObject.transform.position = holdPoint.position;
    }

    void UpdateMoveDirection()
    {
        if (holdPoint == null) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y);
        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir.normalized;

        // Update hold point local position based on direction
        holdPoint.localPosition = lastMoveDir * holdDistance;
    }

    void TryPickup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickupLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("IceCream"))
            {
                heldObject = hit.gameObject;
                heldCollider = heldObject.GetComponent<Collider2D>();

                // Disable collider while held to prevent physics conflicts
                if (heldCollider != null)
                    heldCollider.enabled = false;

                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;

                break;
            }
        }
    }

    void Drop()
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}