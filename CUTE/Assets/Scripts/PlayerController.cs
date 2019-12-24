using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    public GameObject bullet, explosion, waveText, confetti, hint;
    public float moveSpeed, timeBetweenShoot = 0.5f;
    private float saved, addToAngle, savedAtWin, savedAtDeath, savedAtSwarm, savedAtFlash, savedAtHint;
    private Vector2 velocity;
    private GameObject gameOver, main, bankText;
    private readonly ArmyMode[] modes = { ArmyMode.FOLLOWING, ArmyMode.SHEILD, ArmyMode.FROZEN };
    public ArmyMode mode = ArmyMode.FOLLOWING;
    private int currMode, counter, savedLength, flashCount;
    private GameObject[] texts;
    public int family = 5, town = 10, city = 20, nation = 50, civilization = 100;
    public bool win, dead, swarmStarted, showHint;
    public CameraFollow follow;
    private int minSpawn = 3, maxSpawn = 6;
    public Spawner[] spawnerScripts;
    public Image redPanel;

    // Start is called before the first frame update
    void Start()
    {
        texts = new GameObject[] { GameObject.Find("FollowText"), GameObject.Find("ShieldText"), GameObject.Find("FreezeText") };
        currMode = (int)mode;
        saved = Time.time;
        body = GetComponent<Rigidbody2D>();
        gameOver = GameObject.FindWithTag("GameOverCanvas");
        main = GameObject.FindWithTag("MainCanvas");
        gameOver.SetActive(false);
        bankText = GameObject.Find("BankText");
    }

    // Update is called once per frame
    void Update()
    {
        if (dead && Time.time - savedAtDeath >= 0.2)
        {
            Destroy(gameObject);
            gameOver.SetActive(true);
            Time.timeScale = 1;
        }
        if (win && Time.time - savedAtWin >= 3.5)
            SceneManager.LoadScene("WinScene");
        if (flashCount > 0 && Time.time - savedAtFlash >= 0.25)
        {
            if (redPanel.color.a == 0)
                redPanel.color = new Color(1, 0, 0, 0.5f);
            else
                redPanel.color = new Color(1, 0, 0, 0);
            flashCount--;
            savedAtFlash = Time.time;
            if (flashCount == 0)
            {
                waveText.GetComponent<Text>().color = new Color(1, 1, 1, 0);
                waveText.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0);
            }
        }
        if (swarmStarted && Time.time - savedAtSwarm >= 10 - (minSpawn + maxSpawn) / 2)
        {
            swarmStarted = false;
            foreach (Spawner spawner in spawnerScripts)
            {
                spawner.minSecondsBetweenSpawn = minSpawn;
                spawner.maxSecondsBetweenSpawn = maxSpawn;
            }
        }
        if (!gameObject.GetComponent<Renderer>().enabled)
        {
            velocity = new Vector2(0, 0);
            return;
        }
        if (showHint && Time.time - savedAtHint >= 2)
        {
            hint.SetActive(false);
            showHint = false;
        }
        ChangeArmyMode();

        if (counter % 40 == 0)
            TimeArmyModes();
        counter++;

        if (!dead)
            Shoot();

        if (!dead)
        {
            velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg - 90);
        }
        GameObject[] friends = GameObject.FindGameObjectsWithTag("Friend");
        BankMice(friends);

        if (mode != ArmyMode.SHEILD)
        {
            addToAngle = 0;
            return;
        }

        SetSheildPositions(friends);
    }

    public void BankMice(GameObject[] friends)
    {
        if (!Input.GetKeyDown(KeyCode.R))
            return;
        if (friends.Length < 5)
        {
            hint.SetActive(true);
            showHint = true;
            savedAtHint = Time.time;
            return;
        }
        int count = System.Convert.ToInt32(bankText.GetComponent<Text>().text) + friends.Length;
        if (friends.Length >= 1)
            SoundManager.PlaySound("bankSound");
        bankText.GetComponent<Text>().text = count + "    ";
        foreach (GameObject friend in friends)
            Destroy(friend);
        int numMove = 0;
        if (savedLength < family && count >= family)
        {
            numMove += 10;
            DecreaseSpawn();
        }
        if (savedLength < town && count >= town)
        {
            numMove += 10;
            DecreaseSpawn();
        }
        if (savedLength < city && count >= city)
        {
            numMove += 10;
            DecreaseSpawn();
        }
        if (savedLength < nation && count >= nation)
        {
            numMove += 10;
            DecreaseSpawn();
        }
        if (savedLength < civilization && count >= civilization)
        {
            win = true;
            savedAtWin = Time.time;
            confetti.SetActive(true);
        }
        if ((savedLength < family && count >= family || savedLength < town && count >= town
            || savedLength < city && count >= city || savedLength < nation && count >= nation)
            && !(savedLength < civilization && count >= civilization))
        {
            StartCoroutine(follow.Shake(0.3f, 1f));
            savedAtSwarm = Time.time;
            swarmStarted = true;
            foreach (Spawner spawner in spawnerScripts)
            {
                spawner.minSecondsBetweenSpawn = 0.5f;
                spawner.maxSecondsBetweenSpawn = 0.5f;
            }
            flashCount = 6;
            savedAtFlash = Time.time;
            waveText.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            waveText.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
        }
        MoveBoundaries(numMove);
        GetComponent<TimelineUpdater>().UpdateTimeline(count);
        savedLength = count;
    }

    private void DecreaseSpawn()
    {
        minSpawn -= 1;
        maxSpawn -= 1;
    }

    private void Normalize(int inMode)
    {
        texts[currMode].GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0);
        currMode = (inMode < 0 ? 2 : inMode) % 3;
        mode = modes[currMode];
        texts[currMode].GetComponent<Outline>().effectColor = new Color(255, 255, 255, 255);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeArmyMode()
    {
        if (Input.GetKeyUp(KeyCode.E))
            Normalize(currMode + 1);
        else if (Input.GetKeyUp(KeyCode.Q))
            Normalize(currMode - 1);
    }

    private void TimeArmyModes()
    {
        for (int i = 0; i < texts.Length - 1; i++)
            texts[i].GetComponent<Text>().fontSize -= currMode == i ? 1 : (texts[i].GetComponent<Text>().fontSize >= 27 ? 0 : -1);
        if (texts[currMode].GetComponent<Text>().fontSize == 6)
            Normalize(currMode + 1);
    }

    private void Shoot()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time - saved >= timeBetweenShoot)
        {
            Instantiate(bullet);
            saved = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            saved = 0;
    }

    private void MoveBoundaries(int distance)
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");
        foreach (GameObject boundary in boundaries)
            boundary.GetComponent<BoundaryMover>().MoveUntil(distance);

        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");
        foreach (GameObject grass in grasses)
            grass.GetComponent<GrassMover>().MoveUntil(distance);

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in spawners)
            spawner.GetComponent<SpawnerMover>().MoveUntil(distance);
    }

    private void SetSheildPositions(GameObject[] friends)
    {
        addToAngle += 2 * Mathf.PI / 360;
        for (int i = 0; i < friends.Length; i++)
        {
            float angle = 2 * Mathf.PI / friends.Length * i + addToAngle;
            FriendChaser script = friends[i].GetComponent<FriendChaser>();
            Transform pos = friends[i].GetComponent<Transform>();
            float x = script.hitBoundaryVert ? 0 : 2 * Mathf.Cos(angle);
            float y = script.hitBoundaryHor ? 0 : -2 * Mathf.Sin(angle);
            script.Dest = gameObject.transform.position + new Vector3(x, y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
            return;
        if (!win)
        {
            savedAtDeath = Time.time;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            Time.timeScale = 0.1f;
            dead = true;
            main.SetActive(false);
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            SoundManager.PlaySound("deathSound");
        }
    }
}
