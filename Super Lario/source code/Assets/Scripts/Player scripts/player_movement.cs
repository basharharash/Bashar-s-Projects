using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_movement : MonoBehaviour {

    // used for movement
    public float speed = 5f;

    // used for movement/animation
    private Rigidbody2D my_body;
    private Animator anim;

    // used for jumping
    private bool on_ground;
    private bool jumping;
    private float jump_pow = 5f;

    private int coin_counter = 0;

    void Awake() {
        // initilaize rigid body and animator
        my_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        
        
    }

    // Update is called once per frame
    void Update() { 
    }

    void FixedUpdate() {
        player_walk();
        playr_jump();

    }

    void player_walk() {
        // GetAxisRaw stores either 1,-1,or 0 in the var h
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0) {
            // h = 1 direct the oject into +x
            change_diraction((int)h);
            // move the object towords +x
            my_body.velocity = new Vector2(speed, my_body.velocity.y);
        } else if(h < 0) {
            // h = -1 direct the oject into -x
            change_diraction((int)h);
            // move the object towords -x
            my_body.velocity = new Vector2(-speed, my_body.velocity.y);
        } else {
            // stop the object when releasing the key
            my_body.velocity = new Vector2(0f, my_body.velocity.y);
        }
        // activate the animation while moving
        anim.SetInteger(my_tags.speed_tag, Mathf.Abs((int)my_body.velocity.x));
    }

    void change_diraction(int dir) {
        // get the initial scale and store it in temp
        Vector3 temp_scale = transform.localScale;
        // export the x-component of the scal only
        temp_scale.x  = dir;
        transform.localScale = temp_scale;
    }

    // OnCollisionEnter2D execturtes whenever collision detected by unity
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == my_tags.ground_tag) {
            on_ground = true;  // player is touching the ground
            if (jumping) {
                jumping = false;  // if player already jumped, switch off the bollean jump
                anim.SetBool(my_tags.jump_tag, jumping);  // deactivate the jumping animation
            }
        }
    }

    void playr_jump() {
        if (on_ground) {
            if (Input.GetKey(KeyCode.Space)) {
                jumping = true;  // space key will switch jumping to true
                my_body.velocity = new Vector2(my_body.velocity.x, jump_pow);  // setting the vertical movement speed
                anim.SetBool(my_tags.jump_tag, jumping);  // activate the jumping animation
                on_ground = false;  // player is no longer on the ground
            }
        }
    }

    IEnumerator player_dead() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        score.score_count = 0;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.water) {
            StartCoroutine(player_dead());
        }
    }

}  // class
