using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boss_hp_script : MonoBehaviour {

    private Animator anim;
    private int health = 100;
    private bool can_damage;

    private void Awake() {
        anim = GetComponent<Animator>();
        can_damage = true;
    }

    IEnumerator wait_for_damage() {
        yield return new WaitForSeconds(2f);
        can_damage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (can_damage) {
            if (collision.gameObject.tag == my_tags.bullet) {
                health--;
                print(health);
                if (health == 0) {
                    GetComponent<boss_script>().deactivate_boss_script();
                    anim.Play("dead");
                    StartCoroutine(remove());
                    can_damage = false;
                }
            }
        }
    }

    IEnumerator remove() {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
        score.score_count = 0;
        SceneManager.LoadScene("MainMenu");
    }
}
