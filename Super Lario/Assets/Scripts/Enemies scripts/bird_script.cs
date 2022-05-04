using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird_script : MonoBehaviour {

    private Rigidbody2D b_body;
    private Animator b_anim;

    private Vector3 move_dir = Vector3.left;
    private Vector3 original_position;
    private Vector3 target_position;

    public GameObject bird_egg;
    public LayerMask player_layer;
    private bool attacked;
    private bool move_left;
    private bool can_move;
    private Vector3 og_temp_scale;

    private void Awake() {
        b_body = GetComponent<Rigidbody2D>();
        b_anim = GetComponent<Animator>();
        Vector3 og_temp_scale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start() {
        original_position = transform.position;
        original_position.x += 1f;

        target_position = transform.position;
        target_position.x -= 1f;

        can_move = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        move_bird();
    }

    void move_bird() {
        if (can_move) {
            transform.Translate(move_dir * Time.smoothDeltaTime);
            if (transform.position.x >= original_position.x) {
                change_dir();
                move_dir = Vector3.left;
            } else if (transform.position.x <= target_position.x) {
                change_dir();
                move_dir = Vector3.right;
            }
        }
    }

    void change_dir() {
        move_left = !move_left;
        Vector3 temp_scale = transform.localScale;
        if (can_move) {
            if (move_left) {
                temp_scale.x = -Mathf.Abs(temp_scale.x);
            } else {
                temp_scale.x = Mathf.Abs(temp_scale.x);
            }
        }
        transform.localScale = temp_scale;
    }
}
