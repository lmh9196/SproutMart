using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBurrow : MonoBehaviour
{
    public GameObject[] signBtn;
    public string[] dirString;

    public Transform rightTarget;
    public Transform leftTarget;
    public Transform upTarget;
    public Transform downTarget;

    Player player;
    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
    private void Update()
    {
        if (player.burrow.currentBurrow == this && player.burrow.isBurrow) { player.burrow.RenewBtn(dirString); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player _player))
        {
            if (!player.burrow.isBurrow) { player.burrow.TriggerOn(this); }
        }
    }
    private void OnTriggerStay2D(Collider2D collision) { if (collision.TryGetComponent(out Player _player)) { player.burrow.currentBurrow = this; } }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player _player))
        {
            if (!player.burrow.isBurrow) { player.burrow.TriggerOff(); }
        }
    }
}
