using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShot :  MonoBehaviour, IPowerUp
{
    public GameObject particle;
    public int shotsToAdd = 1;

    public void PowerUp() {
        FindObjectOfType<AudioManager>().PlaySound("oneUp");
        FindObjectOfType<PlayerScript>().shots.AddShots(shotsToAdd);
        Instantiate(particle, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(this.gameObject);
    }
}
