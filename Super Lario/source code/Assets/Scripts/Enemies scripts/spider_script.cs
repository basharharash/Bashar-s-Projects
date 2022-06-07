using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spider_script : MonoBehaviour {

    private Rigidbody2D s_body;
    private Animator s_anim;
    private Vector3 move_dir = Vector3.down;
    private string coroutine_name = "change_movement";

    private void Awake() {
        s_anim = GetComponent<Animator>();
        s_body = GetComponent<Rigidbody2D>();
    }

    void Start() {
        StartCoroutine(coroutine_name);
    }

    void FixedUpdate() {
        move_spider();
    }

    void move_spider() {
        transform.Translate(move_dir * Time.smoothDeltaTime);
    }

    IEnumerator change_movement() {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        if (move_dir == Vector3.down) {
            move_dir = Vector3.up;
        } else {
            move_dir = Vector3.down;
        }
        StartCoroutine(coroutine_name);
    }

    IEnumerator spider_dead() {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.bullet) {
            s_anim.Play("spider_dead");
            s_body.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(spider_dead());
            StopCoroutine(coroutine_name);
        }

        if ( collision.tag == my_tags.player_tag) {
            collision.GetComponent<player_damage>().deal_damage();
        }
    }
}
