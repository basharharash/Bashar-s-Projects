using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog_script : MonoBehaviour {

    private Animator f_anim;
    private Rigidbody2D f_body;

    private bool animation_started;
    private bool animation_finished;
    private bool can_move;

    private int jumped_times;
    private bool jump_left = true;

    public LayerMask player_layer;

    private GameObject player;

    private string coroutine_name = "frog_jump";

    private void Awake() {
        f_anim = GetComponent<Animator>();
        f_body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(my_tags.player_tag);
    }

    void Start() {
        can_move = true;
        StartCoroutine(coroutine_name);
    }

    void Update() {
        if (can_move) {
            if (Physics2D.OverlapCircle(transform.position, 0.5f, player_layer)) {
                player.GetComponent<player_damage>().deal_damage();
            }
        }
        
    }


    void LateUpdate() {
        if (can_move) {
            if (animation_finished && animation_started) {
                animation_started = false;
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
            }
        }
        
    }

    IEnumerator frog_jump() {
        yield return new WaitForSeconds(Random.Range(1f, 4f));

        animation_started = true;
        animation_finished = false;
        jumped_times++;
        if (jump_left) {
            f_anim.Play("frog_jump_left");
        } else {
            f_anim.Play("frog_jump_right");
        }

        StartCoroutine(coroutine_name);
    }

    void anim_finished() {
        if (can_move) {
            animation_finished = true;
            if (jump_left) {
                f_anim.Play("frog_idle_left");
            } else {
                f_anim.Play("frog_idle_right");
            }
            if (jumped_times == 3) {
                jumped_times = 0;
                Vector3 temp_scale = transform.localScale;
                temp_scale.x *= -1;
                transform.localScale = temp_scale;
                jump_left = !jump_left;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.bullet) {
            can_move = false;
            f_anim.Play("dead");
            StartCoroutine(dead());
        }
    }

    IEnumerator dead() {
        // object is dead when called after 'timer' time
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);

    }
}
