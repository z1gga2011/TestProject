using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PlayerMovement : MonoBehaviour
{
    [Header("«вук")]

    public AudioClip[] Concrete;
    public AudioClip[] Dirt;
    public AudioClip[] Grass;

    public AudioClip Cable;

    public surface SurfaceType;
    public bool InDoor; // в помещении ли игрок?

    public enum surface
    {
        Concrete, Dirt, Grass
    }



    [Header("ѕередвижение")]

 
    public float SlopeDistance; // дистанци€ обнаружени€ "ступенек"
    public float speed = 1.5f; // скорость ходьбы
    public float LadderSpeed; // скорость по вертикальной лестнице
    public float CableSpeed; // скорость при спуске по тросу

    public bool facingRight = true; // в какую сторону смотрит персонаж на старте?
    public bool coolDown; // отключение управлени€ на N-секунд

    private Vector3 direction; // направление движени€
    private float vertical, horizontal; // управление по горизонтали и вертикали


    [Header("Ћестницы")]

    public AudioClip[] LadderSound;
    public bool isLadder; // можно ли передвигатьс€ по лестнице?
    private Vector3 upLadder, downLadder, ladderPos, ladderUpOutPos, ladderUpInPos; // позиции дл€ лестницы


    [Header("“рос")]

    public float CurveRatio; // насколько сильно трос прогибаетс€ под игроком

    public bool isCable; // можно ли передвигатьс€ по тросу?

    private Vector3 cableEndPos; // точка конца троса
    private Cable cable; // экземпл€р класса тросса, к которому будем обращатьс€

    [Header("ползанье")]
    [SerializeField] private AudioClip[] crawlSound;
    [SerializeField] private CapsuleCollider2D characterCollider;
    [SerializeField] private float crawlSpeed;
    private Transform crawlObstaclePosition;
    private Transform CrawlingExitPos1;
    private Transform CrawlingExitPos2;
    private bool isCrawling;


    [Header("кпк")]

    public Kpk mainKpk;
    private bool isKpk;
    [SerializeField] private bool isKpkActive;



    private Rigidbody2D body;
    private Animator _animator;
    private int layerMask;


    private void Awake()
    {
        _animator = GetComponent<Animator>();

        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;

        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
    }
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dirt") SurfaceType = surface.Dirt;
        else if(collision.tag == "Grass") SurfaceType = surface.Grass;
        else if(collision.tag == "Concrete") SurfaceType = surface.Concrete;

        if (collision.tag == "Room") InDoor = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!coolDown)
        {
            if (collision.tag == "Ladder" && !isLadder)
            {
                Ladder ladder = collision.GetComponent<Ladder>();
                ladderUpInPos = ladder.UpIn.position;
                ladderUpOutPos = ladder.outPos.position;
                upLadder = ladder.up.position;
                downLadder = ladder.down.position;
                ladderPos = collision.transform.position;


                if (transform.position.y < downLadder.y && vertical > 0 || transform.position.y > upLadder.y && vertical < 0)
                {
                    SwitchLadderMode();
                }
            }
            if (collision.tag == "Cable" && Input.GetKeyDown(KeyCode.E))
            {
                cable = collision.GetComponent<Cable>();
                SwitchCableMode();
            }

            if (collision.tag == "CrawlObstacle" && Input.GetKeyDown(KeyCode.E) && !isCrawling)
            {
                crawlObstaclePosition = collision.transform;
                CrawlingExitPos1 = collision.transform.GetChild(0);
                CrawlingExitPos2 = collision.transform.GetChild(1);

                SwitchCrawlMode();
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder" && isLadder) SwitchLadderMode();

        if (collision.tag == "Room") InDoor = false;
    }

    void FixedUpdate()
    {
        Move();
    }
    void Update()
    {
        if(facingRight )
        Debug.DrawRay(transform.position - Vector3.down * -1.6f, Vector3.right * SlopeDistance, Color.red); // подсветка, дл€ визуальной настройки jumpDistance
        else
            Debug.DrawRay(transform.position - Vector3.down * -1.6f, Vector3.right * -SlopeDistance, Color.red);

        MovementInput();
        Animation();

        if (isLadder) LadderMode(vertical);
        if (isCable) SlideOnCable();

        if(!isLadder && !isCrawling && Input.GetKeyDown(KeyCode.P))
        {
            if(isKpkActive)
            {
                if (!isKpk && !coolDown && !isCable) { _animator.SetTrigger("OpenKpk"); coolDown = true; }
                else { _animator.SetTrigger("CloseKpk"); }
            }    
        }
    }
    private void Move()
    {
        if (!isCrawling)
        {
            body.AddForce(direction * body.mass * speed * 100f);
        }
        else
        {
            CrawlingMode();
        }


        if (SlopeCheck())
        {
            if (Mathf.Abs(body.velocity.x) > speed)
            {
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed / 2, body.velocity.y);
            }
            if (Mathf.Abs(body.velocity.y) > speed)
            {
                body.velocity = new Vector2(Mathf.Sign(body.velocity.y) * speed, body.velocity.x);
            }
        }
        else
        {
            if (Mathf.Abs(body.velocity.x) > speed)
            {
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);
            }
        }
    }
    private void MovementInput()
    {
        if(!coolDown)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }
        

        if (SlopeCheck()) { direction = new Vector2(horizontal, Mathf.Abs(horizontal)); } else { direction = new Vector2(horizontal, 0); }


        if(!isCrawling)
        {
            if (!isLadder)
            {
                if (horizontal > 0 && !facingRight) Flip(); else if (horizontal < 0 && facingRight) Flip();
            }
            else
            {
                if (!facingRight) Flip();
            }
        }
    }
    #region
    public void SwitchLadderMode()
    {
        body.velocity = Vector2.zero;
        coolDown = true;

        if (!isLadder)
        {
            if (transform.position.y < upLadder.y) _animator.SetBool("LadderDown", true);
            else { _animator.SetBool("LadderUp", true); }

            isLadder = true;
            body.isKinematic = true;
        }
        else
        {
            if (transform.position.y < upLadder.y) _animator.SetBool("LadderOutDown", true);
            else { _animator.SetBool("LadderOutUp", true); transform.position = ladderUpOutPos; }

            isLadder = false;
            body.isKinematic = false;
        }
    }
    public void SwitchToLadderMode()
    {
        coolDown = false;
        if (transform.position.y > upLadder.y) { transform.position = ladderUpInPos; }
    }
    public void SwitchToNormalMode()
    {
        _animator.SetBool("LadderUp", false);
        _animator.SetBool("LadderDown", false);
        _animator.SetBool("LadderOutUp", false);
        _animator.SetBool("LadderOutDown", false);

        coolDown = false;
    }
    private void LadderMode(float verticalInput)
    {
        if (body.isKinematic)
        {
            transform.Translate(new Vector2(0, LadderSpeed * vertical * Time.fixedDeltaTime)); // движение по лестнице
            float xPos = Mathf.Lerp(transform.position.x, ladderPos.x + 0.05f, 10 * Time.fixedDeltaTime);
            transform.position = new Vector2(xPos, transform.position.y); // плавное выравнивание по центру лестницы
        }

        if (transform.position.y < upLadder.y && vertical > 0)
        {
            body.isKinematic = true;
        }
        else if (transform.position.y > downLadder.y && vertical < 0 && transform.position.y > upLadder.y)
        {
            body.isKinematic = true;
        }
        else if (vertical < 0 && transform.position.y < upLadder.y && transform.position.y < downLadder.y)
        {
            body.isKinematic = false;
        }
        
        if(transform.position.y > upLadder.y && vertical > 0 || transform.position.y < downLadder.y && vertical < 0 && isLadder)
        {
            SwitchLadderMode();
        }
    }
    #endregion 
    #region 
    private void SwitchCableMode()
    {
        coolDown = true;

        if (!isCable) { _animator.SetBool("Cable", true); body.isKinematic = true; body.velocity = Vector2.zero; }
        else { _animator.SetBool("Cable", false); cable.AudioSource.Stop(); }
    }
    private void SwitchToCableMode()
    {
        cable.AudioSource.Play();

        isCable = true;
        coolDown = false;

        transform.position = cable.StartPos.position;
    }
    private void SwitchCableToNormalMode()
    {
        isCable = false;
        coolDown = false;

        body.isKinematic = false;

        cable.ResetMovePointPosition();
    }
    private void SlideOnCable()
    {
        transform.position = Vector2.MoveTowards(transform.position, cable.EndPos.position, CableSpeed * Time.deltaTime);
        cable.PlayerFollow(transform.position);

        Vector2 centerPosition = (cable.StartPos.position + cable.EndPos.position) / 2;
        Vector2 myXPosition = new Vector2(transform.position.x, 0);
        
        if(Vector2.Distance(myXPosition, centerPosition) < Vector2.Distance(myXPosition, cable.EndPos.position))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - CurveRatio, transform.position.z);
        }

        if(Vector2.Distance(transform.position, cable.EndPos.position) <= 0.1f)
        {
            SwitchCableMode();
        }
    }
    #endregion
    #region
    public void GiveKpk()
    {
        isKpkActive = true;
    }
    public void DepriveKpk()
    {
        isKpkActive = false;
    }
    private void OpenKpk()
    {
        _animator.ResetTrigger("CloseKpk");

        mainKpk.gameObject.SetActive(true);
        isKpk = true;
    }
    private void CloseKpk()
    {
        mainKpk.gameObject.SetActive(false);
        coolDown = false;
        isKpk = false;
    }
    #endregion
    #region
    private void CrawlingMode()
    {
        if (facingRight)
        {
            body.AddForce(new Vector2(Mathf.Clamp(direction.x, 0, direction.x), 0) * body.mass * crawlSpeed * 100f);
        }
        else
        {
            body.AddForce(new Vector2(Mathf.Clamp(direction.x, direction.x, 0), 0) * body.mass * crawlSpeed * 100f);
        }

        if (transform.position.x < CrawlingExitPos1.position.x) { coolDown = true; _animator.SetTrigger("OutCrawl"); }
        if (transform.position.x > CrawlingExitPos2.position.x) { coolDown = true; _animator.SetTrigger("OutCrawl"); }
    }
    private void SwitchCrawlMode()
    {
        if(!isCrawling) { coolDown = true; _animator.SetTrigger("ToCrawl"); }
        else { coolDown = true; _animator.SetTrigger("OutCrawl"); }

        if (transform.position.x < crawlObstaclePosition.position.x && !facingRight) Flip();
        if (transform.position.x > crawlObstaclePosition.position.x && facingRight) Flip();
    }
    private void EnterCrawlMode()
    {
        _animator.ResetTrigger("OutCrawl");

        isCrawling = true;

        coolDown = false;
        characterCollider.enabled = false;
    }
    private void OutCrawlMode()
    {
        _animator.ResetTrigger("ToCrawl");

        isCrawling = false;

        coolDown = false;
        characterCollider.enabled = true;
    }
    #endregion
    private bool SlopeCheck()
    {
        bool result = false;

        float dir;
        if (facingRight) dir = 1;
        else dir = -1;

        RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.down * -1.55f, Vector3.right, SlopeDistance * dir, layerMask);


        if (hit.collider)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.gameObject.tag == "Slope") result = true;
        }

        return result;
    }
    private void Animation()
    {
        if (horizontal != 0 && body.velocity.magnitude > 0.1f) { _animator.SetFloat("Movement", 1); }
        else { _animator.SetFloat("Movement", 0); }

        if (isLadder && !coolDown)
        {
            if (vertical > 0)
            {
                _animator.enabled = true;
                _animator.SetFloat("LadderDirection", 0);
            }
            else if (vertical < 0)
            {
                _animator.enabled = true;
                _animator.SetFloat("LadderDirection", 1);
            }
            else
            {
                _animator.enabled = false;
            }
        }
        else
        {
            _animator.enabled = true;
        }

        if(isCrawling && !coolDown)
        {
            if(horizontal > 0 && facingRight || horizontal < 0 && !facingRight)
            {
                _animator.enabled = true;
            }
            else
            {
                _animator.enabled = false;
            }
        }
    }
    public void Flip() // отражение по горизонтали
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #region
    public void PlayFootStepFX()
    {
        switch (SurfaceType)
        {
            case surface.Concrete:
                AudioManager.instance.PlayFX(Concrete[Random.Range(0, Concrete.Length - 1)], false);
                break;
            case surface.Dirt:
                AudioManager.instance.PlayFX(Dirt[Random.Range(0, Dirt.Length - 1)], false);
                break;
            case surface.Grass:
                AudioManager.instance.PlayFX(Grass[Random.Range(0, Grass.Length - 1)], false);
                break;
        }
    }
    public void PlayLadderFX()
    {
        AudioManager.instance.PlayFX(LadderSound[Random.Range(0, LadderSound.Length - 1)], false);
    }
    public void PlayCrawlFX()
    {
        AudioManager.instance.PlayFX(crawlSound[Random.Range(0, crawlSound.Length - 1)], false);
    }
    #endregion 
}
