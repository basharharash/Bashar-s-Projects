using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonus_block : MonoBehaviour {

    public Transform bottom_collision;

    private Animator anim;

    public LayerMask player_layer;

    private Vector3 move_dir = Vector3.up;
    private Vector3 og_pos;
    private Vector3 anim_pos;

    private bool start_anim;
    private bool can_animate = true;

    private AudioSource audio_block;
    void Awake() {
        anim = GetComponent<Animator>();
        audio_block = GetComponent<AudioSource>();
    }
    void Start() {
        og_pos = transform.position;
        anim_pos = transform.position;
        anim_pos.y += 0.15f;
    }

    void FixedUpdate() {
        check_for_collision();
        animate_up_down();
    }

    void check_for_collision() {
        if (can_animate) {
            Collider2D hit = Physics2D.OverlapCircle(bottom_collision.position,0.17f, player_layer);
            if (hit != null) {
                // increase score
                anim.Play("idle");
                start_anim = true;
                can_animate = false;
                audio_block.Play();
            }
        }
    }

    void animate_up_down() {
        if (start_anim) {
            transform.Translate(move_dir * Time.smoothDeltaTime);
            if (transform.position.y >= anim_pos.y) {
                move_dir = Vector3.down;
            } else if (transform.position.y <= og_pos.y) {
                start_anim = false;
            }
        }
        
    }
}
