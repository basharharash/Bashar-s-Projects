using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class egg_script : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == my_tags.player_tag) {
            // apply damage
            collision.gameObject.GetComponent<player_damage>().deal_damage();
            
        }
        gameObject.SetActive(false);
    }

}
