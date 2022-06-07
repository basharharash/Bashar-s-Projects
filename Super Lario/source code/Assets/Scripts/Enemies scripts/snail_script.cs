using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snail_script : MonoBehaviour {

    // for snail movement
    public float s_speed = 1f;
    private Rigidbody2D s_body;
    private Animator s_anim;

    // for reversing diraction/detecting collisions
    private bool move_left;
    private bool can_move;
    private bool stunned;

    public Transform down_collision, up_collision,
        left_collision, right_collision;

    public LayerMask player_layer;
    public LayerMask ground_layer;

    private Vector3 left_collision_position, right_collision_position;

    void Awake() {
        // initialize game components
        s_body = GetComponent<Rigidbody2D>();
        s_anim = GetComponent<Animator>();

        // store the collision objects in temp vars, so we can swap the objects 
        // when the snail reverse its movement diraction
        left_collision_position = left_collision.localPosition;
        right_collision_position = right_collision.localPosition;

    }
    // Start is called before the first frame update
    void Start() {
        move_left = true;
        can_move = true;
        stunned = false;
    }

    // called every 0.0001 seconds for better fps
    void FixedUpdate() {
        if (can_move) {
            if (move_left) {
                s_body.velocity = new Vector2(-s_speed, s_body.velocity.y);  // provide speed for the snail to move left
            } else {
                s_body.velocity = new Vector2(s_speed, s_body.velocity.y);  // provide speed for the snail to move left
            }
        }
        check_collision();
    }

    void check_collision() {
        RaycastHit2D left_hit = Physics2D.Raycast(left_collision.position,
            Vector3.left, 0.1f, player_layer);

        RaycastHit2D right_hit = Physics2D.Raycast(right_collision.position,
            Vector3.right, 0.1f, player_layer);

        // Physics2D.OverlapCircle places a tracing circle with center up_collision.position
        // and radius r (in this case 0.2f), and will detect if the layer specified is 
        // within the circle (in this case the player_layer)
        Collider2D top_hit = Physics2D.OverlapCircle(up_collision.position, 0.2f, player_layer);

        // Physics2D.Raycast(down_collision.position, Vector2.down, 0.1f)
        // projects a ray (in this case downwords) from the object down_collision
        // with a length of 0.1 pixle, if the tip of the ray is touching any
        // other rigid body, the function Physics2D.Raycast will return true
        // in our case, as soon as the function Physics2D.Raycast returns false
        // that means our object reached an edge and about to fall down, to 
        // prevent that we flipped the bollean move_left
        if (!Physics2D.Raycast(down_collision.position, Vector2.down, 0.1f, ground_layer)) {
            change_diraction();
        }
        
        // check if the snail got hit from the top
        if (top_hit != null) {
            // was it hit by the player?
            if (top_hit.gameObject.tag == my_tags.player_tag) {
                // is it already stunned?
                if (!stunned) {
                    // bounce the player off the snail with an increased velocity
                    top_hit.gameObject.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(top_hit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);
                    // the snail cant move now
                    can_move = false;
                    s_body.velocity = new Vector2(0, 0);
                    // actavite the stunned animation
                    s_anim.Play(my_tags.Stunned);
                    stunned = true;

                    // Beetle code
                    if (tag == my_tags.beetle_tag) {
                        //s_anim.Play(my_tags.Stunned);
                        StartCoroutine(dead(0.75f));
                    }
                }
            }
        }

        if (left_hit) {
            if (left_hit.collider.gameObject.tag == my_tags.player_tag) {
                if (!stunned) {
                    // apply damgae to player
                    left_hit.collider.gameObject.GetComponent<player_damage>().deal_damage();
                } else {
                    // only the snail have the functionality of bouncing after hitting it when its stunned
                    if (tag != my_tags.beetle_tag) {
                        s_body.velocity = new Vector2(15f, s_body.velocity.y);
                        StartCoroutine(dead(3f));
                    }
                }
            }
        }

        if (right_hit) {
            if (right_hit.collider.gameObject.tag == my_tags.player_tag) {
                if (!stunned) {
                    // apply damgae to player
                    right_hit.collider.gameObject.GetComponent<player_damage>().deal_damage();
                } else {
                    // only the snail have the functionality of bouncing after hitting it when its stunned
                    if (tag != my_tags.beetle_tag) {
                        s_body.velocity = new Vector2(-15f, s_body.velocity.y);
                        StartCoroutine(dead(3f));
                    }
                }
            }
        }
    }

    void change_diraction() {
        move_left = !move_left;  // reverse the diraction of the snail
        Vector3 temp_scale = transform.localScale;  // import the current scale info (we are only intersted in the x component in our case)
        if (move_left) {
            // move_left = true , then we dont need to mirror the object
            temp_scale.x = Mathf.Abs(temp_scale.x);
            // the default orientation to the snail
            left_collision.localPosition = left_collision_position;
            right_collision.localPosition = right_collision_position;
        } else {
            // move_left = false , then we dont need to mirror the object
            temp_scale.x = -Mathf.Abs(temp_scale.x);
            // mirrored orientation of the snail around the x axis
            left_collision.localPosition = right_collision_position;
            right_collision.localPosition = left_collision_position;
        }
        transform.localScale = temp_scale;  // export the updated scale info
    }

    IEnumerator dead(float timer) {
        // object is dead when called after 'timer' time
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.bullet) {
            if (tag == my_tags.beetle_tag) {
                // if a bullet hits the bettle, disable it after 0.75 seconds
                can_move = false;
                s_body.velocity = new Vector2(0f, 0f);
                s_anim.Play(my_tags.Stunned);
                StartCoroutine(dead(0.75f));
            }
            if (tag == my_tags.snail_tag) {
                // if a bullet hits a snail stun it and disable it 
                // after second hit
                if (!stunned) {
                    s_anim.Play(my_tags.Stunned);
                    stunned = true;
                    can_move = false;
                    s_body.velocity = new Vector2(0f, 0f);
                } else {
                    gameObject.SetActive(false);
                }
                
            }
        }
    }
}
