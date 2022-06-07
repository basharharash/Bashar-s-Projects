using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_bullet : MonoBehaviour {

    private float speed = 10f;
    private bool can_move;
    private Animator b_anim;
    
    private void Awake() {
        b_anim = GetComponent<Animator>();
    }
    void Start() {
        // bullets are disabled unless user clicked on 'K'
        StartCoroutine(disable_bullet(1f));
        can_move = true;
    }

    void FixedUpdate() {
        Move();
    }

    void Move() {
        if (can_move) {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;
            transform.position = temp;
        }
    }

    // setter/getter
    public float Speed {
        get {
            return speed;
        }
        set {
            speed = value;
        }
    }

    IEnumerator disable_bullet(float timer) {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        // disable the bullet with animation after hitting a target
        if (collision.gameObject.tag == my_tags.beetle_tag ||
            collision.gameObject.tag == my_tags.snail_tag ||
            collision.gameObject.tag == my_tags.spider_tag ||
            collision.gameObject.tag == "frog" ||
            collision.gameObject.tag == "boss") {
            b_anim.Play("firing");
            can_move = false;
            StartCoroutine(disable_bullet(0.3f));
        }
    }
}
