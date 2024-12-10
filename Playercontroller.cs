using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro; // Fyrir TextMeshPro
using UnityEngine.SceneManagement; // Fyrir senu stjórnun

public class Playercontroller : MonoBehaviour
{
    public float speed = 4.0f; // Hraði leikmanns
    public float sprintMultiplier = 1.5f; // Spretthraði leikmanns
    public float jumpForce = 10.0f; // Kraftur fyrir stökk
    public float gravity = -9.8f; // Þyngdarkraftur
    public float maxJumpTime = 1.5f; // Hámarks stökk tíma
    public int maxPoints = 5; // Hámarks stig leikmanns
    public TMP_Text pointsText; // TextMeshPro fyrir stigaskjá
    public Transform respawnPosition; // Endurræsistaðsetning
    public GameObject projectilePrefab; // Fyrirmynd fyrir skot
    public float projectileForce = 300.0f; // Kraftur fyrir skot
    public Tilemap tilemap; // Tilvísun í flísakort fyrir mörk
    public Camera mainCamera; // Tilvísun í myndavél
    public float invincibilityDuration = 1.0f; // Tími ósigrandi ástands eftir högg

    private int currentPoints; // Núverandi stig leikmanns
    private Rigidbody2D rigidbody2d; // Rigidbody2D leikmanns
    private Animator animator; // Animator fyrir hreyfimyndir
    private bool isGrounded = true; // Athugar hvort leikmaður sé á jörð
    private float jumpTimer = 0.0f; // Teljari fyrir stökk
    private bool isJumping = false; // Hvort leikmaður sé að stökkva
    private bool isInvincible = false; // Athugar hvort leikmaður sé ósigrandi
    private float invincibilityTimer = 0.0f; // Teljari fyrir ósigrandi ástand

    private Vector2 lookDirection = new Vector2(1, 0); // Sjálfgefin stefna sem leikmaður horfir í

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentPoints = maxPoints; // Upphafsstig
        UpdatePointsText(); // Uppfærir stigaskjá
    }

    void Update()
    {
        HandleMovement(); // Meðhöndlar hreyfingu
        HandleJump(); // Meðhöndlar stökk
        UpdateCameraPosition(); // Uppfærir myndavél
        HandleInvincibility(); // Meðhöndlar ósigrandi ástand

        if (Input.GetKeyDown(KeyCode.Space)) // Athugar hvort leikmaður skýtur
        {
            Shoot();
        }
    }

    void HandleMovement()
    {
        Vector2 inputDirection = new Vector2(
            Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0, // Vinstri og hægri
            0 // Aðeins lárétt hreyfing
        );

        if (inputDirection.magnitude > 1)
            inputDirection.Normalize(); // Normalíserar ef leikmaður hreyfist á ská

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) // Spretthraði
            currentSpeed *= sprintMultiplier;

        Vector2 velocity = inputDirection * currentSpeed * Time.deltaTime;

        // Leyfir hreyfingu í lofti
        rigidbody2d.velocity = new Vector2(velocity.x, rigidbody2d.velocity.y);

        // Uppfærir hreyfimyndir og horfustefnu
        if (inputDirection != Vector2.zero)
        {
            lookDirection = inputDirection;
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
        }

        animator.SetFloat("Speed", inputDirection.magnitude * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1));
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded) // Stökk hefst
        {
            isJumping = true;
            isGrounded = false;
            jumpTimer = 0.0f; // Endurstillir teljara fyrir stökk
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce); // Beitir stökkkrafti
        }

        if (isJumping && Input.GetKey(KeyCode.W)) // Heldur áfram að stökkva á meðan takki er haldið inni
        {
            if (jumpTimer < maxJumpTime) // Athugar hvort hámarks stökk tími er náð
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false; // Stökk lýkur
            }
        }

        if (Input.GetKeyUp(KeyCode.W)) // Stöðvar stökk þegar takki er sleppt
        {
            isJumping = false;
        }

        if (!isGrounded && !isJumping) // Beitir þyngdarkrafti þegar leikmaður er að falla
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y + gravity * Time.deltaTime);
        }
    }

    void HandleInvincibility()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    void UpdateCameraPosition()
    {
        if (mainCamera != null) // Athugar hvort myndavél sé til staðar
        {
            Vector3 newCameraPosition = transform.position; // Tekur stöðu leikmanns
            newCameraPosition.z = -10; // Stillir Z staðsetningu myndavélar
            mainCamera.transform.position = newCameraPosition; // Uppfærir stöðu myndavélar
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null) // Athugar hvort fyrirmynd fyrir skot sé til staðar
        {
            GameObject projectile = Instantiate(
                projectilePrefab,
                rigidbody2d.position + lookDirection * 0.5f,
                Quaternion.identity
            );

            projectile.tag = "skott";

            Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();
            if (projectileRigidBody != null)
            {
                projectileRigidBody.AddForce(lookDirection * projectileForce, ForceMode2D.Impulse);
            }

            Destroy(projectile, 3.0f); // Eyðir skoti eftir 3 sekúndur
        }
    }

    public void ChangePoints(int amount)
    {
        if (!isInvincible || amount > 0) // Athugar hvort leikmaður sé ósigrandi
        {
            currentPoints = Mathf.Clamp(currentPoints + amount, 0, maxPoints); // Uppfærir stig
            UpdatePointsText();

            if (amount < 0) // Ef leikmaður tekur högg
            {
                isInvincible = true; // Virkjar ósigrandi ástand
                invincibilityTimer = invincibilityDuration; // Stillir tímann
            }

            if (currentPoints == 0) // Ef stig ná núll, byrjar leikmaður í upphafssenu
            {
                RespawnInScene0();
            }
        }
    }

    void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "stig:" + currentPoints; // Uppfærir stigaskjá
        }
    }

    void RespawnInScene0()
    {
        SceneManager.LoadScene(0); // Byrjar upphafssenu
    }

    private bool IsPositionInsideTilemap(Vector2 position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(position);
        return tilemap.HasTile(tilePosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Leikmaður á jörð
            isJumping = false; // Endurstillir stökk ástand
        }

        if (collision.gameObject.CompareTag("ovinur"))
        {
            ChangePoints(-1); // Leikmaður missir stig
        }
    }
}
