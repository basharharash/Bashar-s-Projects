using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone_script : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Invoke("Deactivate", 4f);
    }

    void Deactivate() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.player_tag) {
            collision.GetComponent<player_damage>().deal_damage();
            gameObject.SetActive(false);
        }
    }

}
