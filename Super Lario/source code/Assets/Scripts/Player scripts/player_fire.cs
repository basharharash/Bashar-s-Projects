using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_fire : MonoBehaviour {

    public GameObject bullet;

    void Update() {
        fire();
    }

    void fire() {
        if (Input.GetKeyDown(KeyCode.K)) {
            // make copies of the bullet
            GameObject bullet_clone = Instantiate(bullet, transform.position, Quaternion.identity);
            bullet_clone.GetComponent<fire_bullet>().Speed *= transform.localScale.x;
        }
    }
}
