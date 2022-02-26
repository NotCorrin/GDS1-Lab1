using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    #region Variables
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] SpriteRenderer spriteRenderer;
    Manager manager;
    Vector2 movement;
    #endregion

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        manager = GameObject.Find("Game Manager").GetComponent<Manager>();
        if (manager == null)
        {
            Debug.LogError("Manager is null");
        }
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            movement.x = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            movement.x = 0f;
        }

        if (Input.GetButton("Vertical"))
        {
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement.y = 0f;
        }

        Vector2 direction = movement.normalized;
        
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }

        if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected...");

        if (other.gameObject.CompareTag("Death"))
        {
            Debug.Log("Collided with Tree");
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Soldier"))
        {
            manager.HandleSoldierPickup(other.gameObject);
        }
    }
}
