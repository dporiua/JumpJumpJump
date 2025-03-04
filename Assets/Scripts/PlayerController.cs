using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject arrow;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    private bool isSwinging = false;
    private float angle = 90f; // Starting angle for arrow
    public float swingSpeed = 2f;
    public float semiCircleRadius = 2f;
    public float semiCircleHeight = 2f;
    private bool onCircularPlatform = false;
    private bool onNormalPlatform = false;
    private bool swingDirection = true; // true for swinging to the right, false for left
    public Animator slimeAnimation;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        arrow.SetActive(false);
        slimeAnimation = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && (onNormalPlatform || onCircularPlatform)) {
            StartArrowSwing();
        }

      if (Input.GetKeyUp(KeyCode.Space) && (onNormalPlatform || onCircularPlatform)) 
        {
           PerformJump();
        }

        if (isSwinging) {
            UpdateArrowSwing();
        }
    }

    void StartArrowSwing() {
        if (!onCircularPlatform) { // Only swing the arrow if not on a circular platform
            isSwinging =  true;
            arrow.SetActive(true);
            angle = 90f; // Reset to start from the middle
           
        }
        slimeAnimation.SetBool("Prepare", true);
    }

    void UpdateArrowSwing() {
        if (!onCircularPlatform) {
            float angleChange = swingSpeed * Time.deltaTime * (swingDirection ? 1 : -1);
            angle += angleChange;

            if (angle >= 180f || angle <= 0f) {
                swingDirection = !swingDirection; 
            }

            Vector3 semiCircleCenter = new Vector3(transform.position.x, transform.position.y + semiCircleHeight, 0);
            float radian = angle * Mathf.Deg2Rad;
            float x = semiCircleCenter.x + Mathf.Cos(radian) * semiCircleRadius;
            float y = semiCircleCenter.y + Mathf.Sin(radian) * semiCircleRadius;

            arrow.transform.position = new Vector3(x, y, 0);
        }
    }

    void PerformJump() {
        isSwinging = false;
        arrow.SetActive(false);
        Vector2 jumpDirection = Vector2.up;

        if (onCircularPlatform) {
            jumpDirection = transform.position - transform.parent.position;
            jumpDirection.Normalize();

            transform.SetParent(null, true);
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            onCircularPlatform = false;
        } else if (onNormalPlatform) {
            jumpDirection = (arrow.transform.position - transform.position).normalized;
        }
        if(arrow.transform.position.x > 0)
        {
            slimeAnimation.SetBool("Prepare", false);
            slimeAnimation.SetBool("Jumping Right", true);
        }
        if (arrow.transform.position.x < 0)
        {
            slimeAnimation.SetBool("Prepare", false);
            slimeAnimation.SetBool("Jumping Left", true);
        }


        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

       
        onNormalPlatform = false;
    }


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("CircularPlatform")) {
            onCircularPlatform = true;
            transform.SetParent(collision.transform, true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        } else if (collision.gameObject.CompareTag("NormalPlatform")) {
            onNormalPlatform = true;
            rb.rotation = 0;
        }
        slimeAnimation.SetBool("Jumping Right", false);
        slimeAnimation.SetBool("Jumping Left", false);
        slimeAnimation.SetTrigger("Landing");
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("CircularPlatform")) {
            onCircularPlatform = false;
            transform.SetParent(null, true); 
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        } else if (collision.gameObject.CompareTag("NormalPlatform")) {
            onNormalPlatform = false;
            rb.rotation = 0;
        }

    }
}
