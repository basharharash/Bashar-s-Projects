using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird_script : MonoBehaviour {

    private Rigidbody2D b_body;
    private Animator b_anim;

    // for movement
    private Vector3 move_dir = Vector3.left;
    private Vector3 original_position;
    private Vector3 target_position;

    // attacking obj
    public GameObject bird_egg;

    // for collision
    public LayerMask player_layer;

    private bool attacked = false;

    // for changing dir
    private bool move_left;
    private bool can_move;


    private void Awake() {
        b_body = GetComponent<Rigidbody2D>();
        b_anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        can_move = true;
        // movement boundries, move the bird between 2 points
        original_position = transform.position;
        original_position.x += 6f;
        target_position = transform.position;
        target_position.x -= 6f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        move_bird();
        drop_egg();
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

    void drop_egg() {
        if (!attacked) {
            if (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, player_layer)) {
                Instantiate(bird_egg, new Vector3(transform.position.x,
                    transform.position.y - 1f, transform.position.z), Quaternion.identity);
                attacked = true;
                b_anim.Play("bird_fly");
            }
        }
    }

    IEnumerator bird_dead() {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.bullet) {
            // bullet hit the bird
            // play dead animation
            b_anim.Play("bird_dead");
            // turn on trigger so the bird dosnt get stuck on the ground
            GetComponent<BoxCollider2D>().isTrigger = true;
            // turn on gravity
            b_body.bodyType = RigidbodyType2D.Dynamic;
            // burd cant move
            can_move = false;
            // remove the object
            StartCoroutine(bird_dead());
        }
    }
}
