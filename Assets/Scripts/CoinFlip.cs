using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlip : MonoBehaviour {
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject GameManagerHolder;

    private Rigidbody rb;
    private bool beingFlipped = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        //Randomly choose heads or tails at beginning
        if (Random.Range(0, 2) == 0) {
            transform.Rotate(180, 0, 0);
        }
    }

    private void Update() {
        if (rb.velocity == Vector3.zero && beingFlipped) {
            var GameManager = GameManagerHolder.GetComponent<GameManager>();
            bool sawGround = Physics.Raycast(transform.position + Vector3.up * 3, transform.up, Mathf.Infinity, layerMask);

            if (sawGround) {
                GameManager.numTails += 1;
                GameManager.UpdateNumbers("Tails");
            } else {
                GameManager.numHeads += 1;
                GameManager.UpdateNumbers("Heads");
            }

            if (rb.velocity == Vector3.zero && beingFlipped) {
                beingFlipped = false;
            }
        }

        //Failsafe if coin glitches out
        if (transform.position.y < -100f) {
            beingFlipped = false;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, 3, 0);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var obj = collision.gameObject;

        if (obj.name == "Wall") {
            if (obj.transform.rotation.x > 0) {
                //Multiply to reverse the direction of coin and make it bounce off wall
                Vector3 bounce = Vector3.Scale(rb.velocity, obj.transform.forward * 3);
                rb.velocity = bounce;
            }
        }
    }

    public void FlipButtonPressed() {
        if (beingFlipped) {
            return;
        }

        beingFlipped = true;

        float upForce = Random.Range(80f, 200f);
        float xRot;
        float yRot = Random.Range(-1000000f, 1000000f);
        float zRot = Random.Range(-1000000f, 1000000f);

        //Make sure the coin will always flip mid-air at least once
        if (Random.Range(0, 2) == 0) {
            xRot = Random.Range(-1000000f, -300000f);
        } else {
            xRot = Random.Range(300000f, 1000000f);
        }

        //Make coin go up
        rb.velocity = Vector3.up * upForce;
        //Make coin spin
        rb.AddTorque(xRot, yRot, zRot);
    }
}
