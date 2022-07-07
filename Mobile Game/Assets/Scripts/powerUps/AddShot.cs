using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShot :  MonoBehaviour, IPowerUp
{
    public int shotsToAdd = 1;

    public void PowerUp() {
        FindObjectOfType<GameManager>().shots.AddAmount(shotsToAdd);
        Destroy(this.gameObject);
    }
}
