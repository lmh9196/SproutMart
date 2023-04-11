using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public CharacterAnimDir animDir = new();
    MoveCrop moveCrop = new();
    FloatingText goldFloatingText = new();
    FloatingText gemFloatingText = new();
    TimerImageAct boostTimerAct = new();
    TimerImageAct trashTimerAct = new();

    public CharData charData;
    [Space(10f)]
    public BoxData boxData;
    public Transform itemBox;
    SpriteRenderer boxSpriteRenderer;
    public Transform itemBoxCover;
    SpriteRenderer coverSpriteRenderer;
    public GameObject fullSign;
    public ParticleSystem buffeffect;


    [Space(10f)]
    public PlayerUI playerUI;
    public Hat hat;
    public Burrow burrow;
    public PlayerMove playerMove;
    public Boost boost;
    public Attack attack;
    [Space(10f)]

    bool isTrashTimerOn;

    [HideInInspector] public CapsuleCollider2D capsuleCollider;
    [HideInInspector] public Rigidbody2D rigid;
    void Awake()
    {
        if (instance == null) { instance = this; }

        boxSpriteRenderer = itemBox.GetComponent<SpriteRenderer>();
        coverSpriteRenderer = itemBoxCover.GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animDir.anim = GetComponent<Animator>();

        charData.Init();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        burrow.Init(this);
        playerMove.Init(this);
        boost.Init(this);
        attack.Init(this);
    }

    private void FixedUpdate()
    {
        playerMove.Movement();
        boxData.UpdateFullSign(fullSign, itemBox, charData.maxHandsCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { Booster(); }
      
        charData.SetHandsCount();

        trashTimerAct.FillTimerImage(playerUI.trashTimerImage.transform.parent.gameObject, playerUI.trashTimerImage, isTrashTimerOn, Time.deltaTime, ItemBoxClear);
        boostTimerAct.FillTimerImage(playerUI.boostTimerImage.transform.parent.gameObject, playerUI.boostTimerImage, true, Time.deltaTime * boost.boostCooltime, null);
        

        hat.UpdateHat(animDir);

        playerUI.FloatingTextImage(goldFloatingText, "+ " + goldFloatingText.FloatingString(GameManager.instance.data.gold).ToString(), playerUI.goldText, playerUI.goldTextLine, playerUI.goldCoinImage);
        playerUI.FloatingTextImage(gemFloatingText, "+ " + gemFloatingText.FloatingString(GameManager.instance.data.gem).ToString(), playerUI.gemText, playerUI.gemTextLine, playerUI.gemCoinImage);

        SetAnim();

        boxData.UpdateBoxSprite(animDir.lookDir, boxSpriteRenderer, coverSpriteRenderer);
        boxData.UpdateCropsPos(itemBox);

        GameManager.instance.checkList.BuffEvent(GameManager.instance.checkList.isCharSPeedBuff, buffeffect);

        if (GameManager.instance.checkList.IsTutorialEnd &&  !GameManager.instance.checkList.IsTutorial_Full && itemBox.childCount == charData.maxHandsCount) 
        {
            MenuManager.instance.CallNotice(MenuManager.instance.trash.trashBtn.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Drop"))
        {
            if (itemBox.childCount < charData.maxHandsCount) { collision.transform.SetParent(transform.GetChild(0).transform); }
        }

        if (collision.TryGetComponent(out TableArea area))
        {
            moveCrop.isTouch = true;
            StartCoroutine(moveCrop.MoveCrops(area.tag, area, itemBox, charData.maxHandsCount, MoveCropsFeedBack));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TableArea area)) { moveCrop.isTouch = false; }
    }
    void SetAnim()
    {

        animDir.SetDir(playerMove.joyDir, playerMove.joyDir.Equals(Vector3.zero));

        if (itemBox.childCount > 0) { animDir.CheckHaveItem(true); }
        else { animDir.CheckHaveItem(false); }

        if (GameManager.instance.isItemBoxOff || !animDir.anim.GetBool("isHave"))
        {
            itemBox.gameObject.SetActive(false);
            itemBoxCover.gameObject.SetActive(false);
        }
        else
        {
            itemBox.gameObject.SetActive(true);
            itemBoxCover.gameObject.SetActive(true);
        }

        animDir.SetAnim();
    }


    public void Booster()
    {
        if (playerUI.boostTimerImage.fillAmount >= 1)
        {
            boost.StartBooster(boost.SetDir(), boost.boosterPower, BoosterAct);
            playerUI.boostTimerImage.fillAmount = 0;
        }
    }
    void BoosterAct() { StartCoroutine(boost.BoosterDelay()); }
    public void EndAttackAnim() { attack.EndAttack(); }
    public void AttackAct() { attack.AttackAct(); }
    

    public void BurrowUp()
    {
        GameManager.instance.ClickVib();
        StartCoroutine(burrow.BurrowUpAct());
    }
    public void BurrowDown()
    {
        GameManager.instance.ClickVib();
        GameManager.instance.isItemBoxOff = true;
        StartCoroutine(burrow.BurrowDownAct());
    }
    public void BurrowMove(string dir) { burrow.BurrowMoveBtn(dir); }

    // Trash
    public void TrashOn() 
    { 
        if (itemBox.childCount > 0) 
        {
            if (playerMove.joyDir == Vector3.zero) { animDir.lookDir = Vector3.down; }
            isTrashTimerOn = true;
        }
    }
    public void TrashOff() { isTrashTimerOn = false; }
    void ItemBoxClear()
    {
        SoundManager.instance.PlaySfx("Trash");
        GameManager.instance.ClickVib();

        for (int i = 0; i < itemBox.childCount; i++) 
        { 
            Destroy(itemBox.GetChild(i).gameObject);

            if (!GameManager.instance.checkList.IsTutorial_Full)
            {
                MenuManager.instance.FinishNotice();
                GameManager.instance.checkList.IsTutorial_Full = true;
            }
        }
        isTrashTimerOn = false;
    }

    //MoveCrop
    void MoveCropsFeedBack()
    {
        SoundManager.instance.PlaySfx("Harvest");
        GameManager.instance.ClickVib(25);
    }
}


[Serializable]
public class PlayerUI
{
    public Text goldText;
    public Outline goldTextLine;
    public Image goldCoinImage;

    [Space(10f)]
    public Text gemText;
    public Outline gemTextLine;
    public Image gemCoinImage;

    [Space(10f)]
    public Image trashTimerImage;
    public Image boostTimerImage;

    public Color AlphaColorController(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha / 255f);
    }
    public void FloatingTextImage(FloatingText floatText, string floatContents, Text text, Outline outLine, Image image = null)
    {
        text.text = floatContents;
        text.color = AlphaColorController(text.color, floatText.alphaCount);
        outLine.effectColor = AlphaColorController(outLine.effectColor, floatText.alphaCount);

        if (image != null) { image.color = AlphaColorController(image.color, floatText.alphaCount); }
    }
}

[Serializable]
public class Hat
{
    HatData hatData;
    public HatData _HatData
    {
        get { return hatData; }
        set
        {
            hatData = value;
            ES3.Save("HatName", hatData.name);
        }
    }
    public HatData[] hatDatas;

    public HatData unwearHat;

    [SerializeField] SpriteRenderer hatSpriteRenderer;

    CharacterAnimDir animDir;
    public void UpdateHat(CharacterAnimDir _animDir)
    {
        if(animDir == null) { animDir = _animDir; }

        if(hatData != null)
        {
            animDir.CheckHat(!_HatData.isShowHead);
            animDir.HatState(_HatData, hatSpriteRenderer);
        }
    }
}

[Serializable]
public class Burrow
{
    public bool isBurrow;

    [SerializeField] Button burrowDownBtn;
    [SerializeField] Button burrowUpBtn;
    [SerializeField] Transform moveBtnParent;
    [HideInInspector] public RabbitBurrow currentBurrow;
    Player player;
    public void Init(Player _player)
    {
        player = _player;
    }

    public void RenewBtn(string[] dirString)
    {
        for (int i = 0; i < moveBtnParent.childCount; i++)
        {
            for (int j = 0; j < dirString.Length; j++)
            {
                if (moveBtnParent.GetChild(i).name.Contains(dirString[j])) { moveBtnParent.GetChild(i).gameObject.SetActive(true); break; }

                if (j == dirString.Length - 1) { moveBtnParent.GetChild(i).gameObject.SetActive(false); }
            }
        }
    }
    public void TriggerOn(RabbitBurrow _currentBurrow) 
    {
        currentBurrow = _currentBurrow;
        burrowDownBtn.gameObject.SetActive(true);
    }
    public void TriggerOff() { burrowDownBtn.gameObject.SetActive(false); }
    public void BurrowMoveBtn(string dirStr)
    {
        Transform moveTarget = null;

        switch (dirStr)
        {
            case "Up":
                moveTarget = currentBurrow.upTarget; break;
            case "Down":
                moveTarget = currentBurrow.downTarget; break;
            case "Right":
                moveTarget = currentBurrow.rightTarget; break;
            case "Left":
                moveTarget = currentBurrow.leftTarget; break;
        }

        if (moveTarget != null)
        {
            if (moveTarget.transform.parent.gameObject.activeSelf) { player.transform.position = moveTarget.position; }
        }

        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();
    }

    public IEnumerator BurrowDownAct()
    {
        GameManager.instance.canvasList.SetBurrowCanvas(false);
        player.playerMove.isStop = true;
        player.animDir.lookDir = Vector3.down;
        burrowDownBtn.gameObject.SetActive(false);
        player.transform.position = currentBurrow.transform.position + new Vector3(0, -0.329f, 0);

        currentBurrow.GetComponent<Animator>().SetBool("isOpen", true);
        SoundManager.instance.PlaySfx("OpenBurrow");

        yield return new WaitForSeconds(0.3f);

        player.animDir.anim.SetBool("isBurrow", true);
        SoundManager.instance.PlaySfx("BurrowDown");

        yield return new WaitForSeconds(0.7f);
        burrowUpBtn.gameObject.SetActive(true);
        isBurrow = true;

        SoundManager.instance.PlaySfx("CloseBurrow");
        currentBurrow.GetComponent<Animator>().SetBool("isOpen", false);
    }

    public IEnumerator BurrowUpAct()
    {
        isBurrow = false;

        for (int i = 0; i < moveBtnParent.childCount; i++) { moveBtnParent.GetChild(i).gameObject.SetActive(false); }
      

        Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();

        burrowUpBtn.gameObject.SetActive(false);
        currentBurrow.GetComponent<Animator>().SetBool("isOpen", true);
        SoundManager.instance.PlaySfx("OpenBurrow");

        player.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.3f);
        player.animDir.anim.SetBool("isBurrow", false);
        SoundManager.instance.PlaySfx("BurrowUp");

        rigid.gravityScale = 1;
        rigid.AddForce((Vector3.up * 150f));
        player.animDir.lookDir = Vector3.down;

        yield return new WaitForSeconds(0.5f);
        GameManager.instance.isItemBoxOff = false;
        currentBurrow.GetComponent<Animator>().SetBool("isOpen", false);

        SoundManager.instance.PlaySfx("CloseBurrow");

        yield return new WaitForSeconds(0.3f);
        player.playerMove.isStop = false;
        GameManager.instance.canvasList.SetBurrowCanvas(true);

        player.GetComponent<Collider2D>().enabled = true;
        rigid.gravityScale = 0;
        rigid.velocity = Vector3.zero;
    }
}

[Serializable]
public class PlayerMove
{
    public bool isStop;
    public VariableJoystick joystick;
    [SerializeField] GameObject walkEffectPrefabs;

    [HideInInspector] public Vector3 joyDir;

    Player player;
    public void Init(Player _player) { player = _player; }

    public void Movement()
    {
        if (MoveCheck())
        {
            joyDir = Vector3.up * joystick.Vertical + Vector3.right * joystick.Horizontal;

            float joyDis = Vector3.Distance(joyDir, Vector3.zero);

            float weight = 0;

            if (joyDis >= 0) weight = 1;

            if (joyDis > 0.5f) weight = 1.2f;

            float joyMoveSpeed = joyDis * player.charData.SetMoveSpeed() * weight;

            player.transform.Translate(joyDir.normalized * joyMoveSpeed * Time.deltaTime);
        }
        else { joyDir = Vector3.zero; }

        WalkEffect();
    }

    bool MoveCheck()
    {
        if (!isStop ) { return true; }
        else { return false; }
    }
   
    float walkCheckTimer = 0.33f;
    void WalkEffect()
    {
        if (joyDir != Vector3.zero)
        {
            walkCheckTimer -= Time.fixedDeltaTime;

            if (walkCheckTimer <= 0)
            {
                UnityEngine.Object.Instantiate(walkEffectPrefabs, player.transform.position, Quaternion.identity);
                SoundManager.instance.PlaySfx("Walk");
                walkCheckTimer = 0.33f;
            }
        }
        else { walkCheckTimer = 0.33f; }
    }
}
[Serializable]
public class Boost
{
    [SerializeField] GameObject boostEffect;
    public float boosterPower;
    public float boostCooltime;
    Player player;

    public void Init(Player _player)
    {
        player = _player;
    }
    public Vector3 SetDir()
    {
        if (player.playerMove.joyDir == Vector3.zero) { return player.animDir.lookDir; }
        else { return player.playerMove.joyDir; }
    }
    public void StartBooster(Vector3 dir, float speed, Action BoosterAct)
    {
        player.rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        player.rigid.drag = 5;
        BoosterAct?.Invoke();
    }
    public void EndBooster()
    {
        player.rigid.drag = 0;
        player.rigid.velocity = Vector3.zero;
    }
    public IEnumerator BoosterDelay()
    {
        GameManager.instance.ClickVib();
        SoundManager.instance.PlaySfx("Boost");
        UnityEngine.Object.Instantiate(boostEffect, player.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);
        EndBooster();
    }

}

[Serializable]
public class Attack
{
    public GameObject attackEffect;
    Action attackAction;
    Player player;

    Vector3 targetPos;
    public void Init(Player _player)
    {
        player = _player;
    }
    public void StartAttack(Vector3 _targetPos, Action attackAct = null)
    {
        targetPos = _targetPos;

        player.capsuleCollider.enabled = false;
        GameManager.instance.isItemBoxOff = true;
        player.playerMove.isStop = true;

        player.rigid.velocity = Vector3.zero;
        player.playerMove.joyDir = Vector3.zero;

        player.animDir.lookDir = player.animDir.CalDir((targetPos - player.transform.position).normalized);
        player.animDir.anim.SetBool("isAttack", true);

        if (attackAct != null) { attackAction = attackAct; }
    }
    public void EndAttack()
    {
        player.rigid.velocity = Vector3.zero;
        player.capsuleCollider.enabled = true;
        GameManager.instance.isItemBoxOff = false;
        player.playerMove.isStop = false;

        player.animDir.anim.SetBool("isAttack", false);
    }
    public void AttackAct()
    {
        SoundManager.instance.PlaySfx("Pop");
        SoundManager.instance.PlaySfx("Attack");
        GameManager.instance.ClickVib();

        UnityEngine.Object.Instantiate(attackEffect, targetPos, Quaternion.identity);

        attackAction?.Invoke();
    }
}

