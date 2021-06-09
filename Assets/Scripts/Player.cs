using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player player;

    // Variables to make character go foward
    private Rigidbody rb;
    public static float speed;

    // Variables to make character go to sides
    // Number of lanes: 0 - left, 1 - middle, 2 = right
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    public float laneSpeed;

    // Add animator
    private Animator anim;

    // Variables to make character jump
    private bool jumping = false;
    private float jumpStart;
    public float jumpLength; // total distance of jump
    public float jumpHeight; // total height of jump

    // Add box collider, decrease its size in slide
    private BoxCollider boxCollider;

    // Variables to make character slide
    // private bool sliding = false;
    // private float slideStart;
    // public float slideLength;
    private Vector3 boxColliderSize;

    // Variables to make character collide and lose life
    public int maxLife = 3;
    private int currentLife;
    private bool isHit = false;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    private bool invencible = false;
    static int blinkingValue;
    public float invencibleTime;
    public GameObject model;
    private UIManager uiManager;

    // Variable to count coins collected
    [HideInInspector] // does not allow it to be edited by unity
    public int coins;

    // Variable to count points made in one game
    [HideInInspector] // does not allow it to be edited by unity
    public float score;

    // Variable to show attention during the game
    [HideInInspector] // does not allow it to be edited by unity
    public float attention;

    // Mindwave controller
    private int m_BlinkStrength = 0;
    private MindwaveDataModel m_MindwaveData;
    private MindwaveDataModel _Data;

    // Sound effects
    private AudioSource soundEffect;
    public AudioClip impactSound;
    public AudioClip coinSound;
    public AudioClip jumpSound;
    private bool allowSound = true;
    public Sprite[] soundSprites;
    public Image soundButton;
    public Text soundDescription;

    // Variables of the pause menu
    public GameObject panelPause;
    public Button pauseButton;
    public Text[] missionDescription;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        GameManager.isPaused = false;
        
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;

        // Pause menu elements
        //soundButton = GameObject.Find("Sound Effects Button").GetComponent<Image>();
        panelPause.SetActive(false);
        SetMission(); // Load missions to pause menu

        // Change animation
        anim.Play("Run");

        // Add life and speed to character
        currentLife = maxLife;
        speed = minSpeed;
        uiManager = FindObjectOfType<UIManager>();

        // Add missions
        GameManager.gm.StartMissions();

        // Audio source
        soundEffect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mindwave controller blink;
        // MindwaveManager.Instance.Controller.OnUpdateBlink += OnUpdateBlink;

        MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
        score += Time.deltaTime * speed;

        // Update in screen values
        uiManager.UpdateScore((int)score);
        uiManager.UpdateAttention((int)attention);
        uiManager.UpdateSpeed((int)speed);

        player.SpeedControl();

        // If the game isn't in pause mode, the player can move
        if(!GameManager.isPaused) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                ChangeLane(-1);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                ChangeLane(1);
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                Jump();
            }
        } 

        // Functions to pause and resume game
        if (Input.GetKeyDown(KeyCode.Space)){
            if(GameManager.isPaused) {
                GameManager.gm.ResumeGame();
                panelPause.SetActive(false);
                pauseButton.gameObject.SetActive(true);
            } else {
                GameManager.gm.PauseGame();
                panelPause.SetActive(true);
                pauseButton.gameObject.SetActive(false);
            }
        }
        // else if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     Slide();
        // }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1) {
                jumping = false; // jump is over
                anim.SetBool("Jumping", false);
            } else {
                // unity's formula for update position in jump
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        } else {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        // if (sliding)
        // {
        //     float ratio = (transform.position.z - slideStart) / slideLength;
        //     if (ratio >= 1)
        //     {
        //         sliding = false; // slide is over
        //         anim.SetBool("Sliding", false);
        //         boxCollider.size = boxColliderSize;
        //     }
        // }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    public void OnUpdateBlink(int _BlinkStrength) {
		m_BlinkStrength = _BlinkStrength;
	}

    // Update attention data
    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {
        m_MindwaveData = _Data;
        attention = m_MindwaveData.eSense.attention;
    }

    // Update is called in fixed time (default: 0.2s)
    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }

    // Move character to sides
    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 2) return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);
    }

    // Make character jump
    public void Jump()
    {
        if (!jumping)
        {
            // distance that character start jump
            jumpStart = transform.position.z;
            // call animator
            //anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            anim.Play("Jump");
            soundEffect.clip = jumpSound;
            soundEffect.Play();
            jumping = true;
        }
    }

    // Make character slide
    // void Slide()
    // {
    //     if (!jumping && !sliding)
    //     {
    //         slideStart = transform.position.z;
    //         anim.SetFloat("JumpSpeed", speed / slideLength);
    //         anim.SetBool("Sliding", true);
    //         // decrease size of box collider
    //         Vector3 newSize = boxCollider.size;
    //         newSize.y = newSize.y / 2;
    //         boxCollider.size = newSize; // update all values of vector
    //         sliding = true;
    //     }
    // }

    // Make character collide with obstacles
    private void OnTriggerEnter(Collider other) {
        // Compare if object collided is a coin
        if (other.CompareTag("Coin")) {
            if(attention >= 80) {
                coins += 2; // add 2 coins to the sum, bonus if user have elevated attention!
                Debug.Log("Você conseguiu um bônus!");
            } else {
                coins++; // add one coin to the sum
            }
            soundEffect.clip = coinSound;
            soundEffect.Play();
            uiManager.UpdateCoins(coins);
            other.transform.parent.gameObject.SetActive(false);
        }

        if (invencible) return;

        if (other.CompareTag("Obstacle")) {
            soundEffect.clip = impactSound;
            soundEffect.Play();
            currentLife--;
            uiManager.UpdateLives(currentLife);
            anim.SetBool("Hit", true);
            anim.Play("Hit");
            speed = 0; 
            if (currentLife <= 0) {
                speed = 0;
                anim.SetBool("Dead", true);
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 2f);
            } else {
                isHit = true;
                StartCoroutine(Blinking(invencibleTime));
                anim.SetBool("Hit", false);
            }
        }
    }

    // Visual blink when character collide
    IEnumerator Blinking(float time) {
        invencible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        speed = minSpeed;
        isHit = false;
        while (timer < time && invencible) {
            model.SetActive(enabled);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if (blinkPeriod < lastBlink) {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        model.SetActive(true);
        invencible = false;
    }

    // Update speed
    public void SpeedControl() {
        if (speed >= maxSpeed) speed = maxSpeed;
        if (speed <= minSpeed) speed = minSpeed;

        if(currentLife <= 0) speed = 0;

        if(isHit) {
            speed = 0;
        } else {
            if(attention > 80) {
                speed *= 1.0005f;
            } else if(attention > 60 && attention < 80) {
                speed *= 1.0001f;
            } else {
                speed /= 1.0005f;
            }
        }
    }

    public void CallMenu() {
        GameManager.gm.EndRun();
    }

    public void SetMission() {
        for (int i = 0; i < 2; i++) {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
        }
        GameManager.gm.Save();
    }

    public void ControlSound() {
        if(allowSound) {
            soundEffect.volume = 0;
            soundButton.sprite = soundSprites[0];
            soundDescription.text = "Efeitos sonoros desativados";
            allowSound = false;
        } else {
            soundEffect.volume = 0.5f;
            soundButton.sprite = soundSprites[1];
            soundDescription.text = "Efeitos sonoros ativados";
            allowSound = true;
        }
    }

    public void ResumeGame() {
        if(GameManager.isPaused) {
            GameManager.gm.ResumeGame();
            panelPause.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        } else {
            GameManager.gm.PauseGame();
            panelPause.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }
    }
}
