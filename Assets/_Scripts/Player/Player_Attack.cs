using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player_Attack : MonoBehaviour {

    public static Player_Attack main;

    //private Player_Stats m_stats;
    //private Player_ClickToMove m_clickToMove;
    private Animator m_Animator;
    private List<Transform> enemiesInRange = new List<Transform>();

    [Header("Stats TEMP")]
    public int strength; //{ get; private set; }
    public int endurance { get; private set; }
    public int agility; //{ get; private set; }
    public int wisdom { get; private set; }
    public int intelligence { get; private set; }

    [Header("Health TEMP")]
    public int health;
    public int currentHealth { get; private set; }
    public int totalHealth { get; private set; }
    public Text currentHealthText;
    public Text totalHealthText;
    public Transform healthBar;

    [Header("Combat TEMP")]
    public bool attacking;  // time it takes the attack animation to complete
    public float attackSpeed = 1; //TEMPORARY STAT! It has to be attached to the weapon!
    public float attackRange;
    public float attackDamage;
    public float minWeaponDamage;  //later this have to go to a weapon class
    public float maxWeaponDamage;
    public float bonusDamage;

    [Header("LvlUP TEMP")]
    public int level = 1;
    public Text levelText;
    public float totalExperience { get; private set; }
    public float currentExperience { get; private set; }
    public Transform experienceBar;

    [Header("Misc TEMP")]
    public GameObject bloodSplat;

    //[HideInInspector]public AudioSource source;
    //public AudioClip swooshClip;

    //private GameObject m_lastAttacked;
    //private float m_lastAttack = 0;    

    //// OBJECT POOLING    
    //public GameObject arrow;
    //public Transform arrowPos;
    //public int pooledAmount = 3;         // max amount of arrows that will be able to be shoot at once
    //public List<GameObject> arrows;            // create a list containing all the arrows that will be allocated to memory once it starts

    void Awake()
    {
        SetInitialReferences();
    }

    void SetInitialReferences()
    {
    //    m_stats = GetComponent<Player_Stats>();
        m_Animator = GetComponent<Animator>();
        //    m_clickToMove = GetComponent<Player_ClickToMove>();

        //    source = GetComponent<AudioSource>();
        //    source.clip = swooshClip;
        //    source.playOnAwake = false;

        //    //arrow pool
        //    arrows = new List<GameObject>();
        //    for (int i = 0; i < pooledAmount; i++)
        //    {
        //        GameObject obj = (GameObject)Instantiate(arrow);
        //        obj.SetActive(false);
        //        arrows.Add(obj); //CREATES THE POOL
        //    
    }

    void Start()
    {
        if (main == null) main = this;
        Player_Animations.OnSlashAnimationHit += DealDamage;

        //Experience
        experienceBar = UIController.instance.canvas.Find("PlayerUI_BG/Experience");
        levelText = UIController.instance.canvas.Find("PlayerUI_BG/Level_Text").GetComponent<Text>();
        SetExperience(0);

        //Health
        healthBar = UIController.instance.canvas.Find("PlayerUI_BG/Health_Points");
        currentHealthText = UIController.instance.canvas.Find("PlayerUI_BG/Health_Points/Text").GetComponent<Text>();
        totalHealth = 200;
        currentHealth = totalHealth;
        GetHit(0);

        //Attack
        SetAttackDamage();
    }


    // This is being called by an event in the attack animation
    void DealDamage()
    {
        print("Deal Damage!");
        GetEnemiesInRange();
        foreach (Transform enemy in enemiesInRange)
        {
            Enemy_Controller ec = enemy.GetComponent<Enemy_Controller>();
            if (ec == null) continue;
            ec.GetHit(attackDamage);
            Instantiate(bloodSplat, ec.transform.position, ec.transform.rotation);
        }
    }

    //public void Attack(GameObject attacked)
    public void Attack()
    {
        // ---- TEMP -----
        if (!attacking)
        {
            m_Animator.SetTrigger("Attack");          
            StartCoroutine(AttackRoutine());  // make so the character waits for an amount of time before attacks again           
        }
        // ---- TEMP -----


        //    if(!m_stats.isBowEquipped)
        //    {
        //        // check if the player can attack again
        //        if (Time.time - m_lastAttack >= m_stats.currentAttackSpeed)
        //        {
        //            // attack!
        //            m_animator.SetTrigger("Attack");
        //            source.PlayOneShot(swooshClip, .10f); //plays the clip SwooshClip at a volume of 10%

        //            // update the time of the last attack
        //            m_lastAttack = Time.time;

        //            // register the gameobject the player attacked
        //            m_lastAttacked = attacked;
        //        }
        //    }
        //    else
        //    {
        //        // check if the player can attack again
        //        if (Time.time - m_lastAttack >= m_stats.currentAttackSpeed)
        //        {
        //            GetComponent<Projectile>();
        //            // loops through the list
        //            for (int i = 0; i < arrows.Count; i++)
        //            {   // if the arrow is not currently active (looking for inactive arrows)
        //                if (!arrows[i].activeInHierarchy)
        //                {
        //                    // check again for a valid target
        //                    if(m_clickToMove.target != null)
        //                    {
        //                        arrows[i].transform.position = arrowPos.position;
        //                        //arrows[i].transform.rotation = Quaternion.identity;
        //                        arrows[i].SetActive(true);

        //                        // attack!
        //                        m_animator.SetTrigger("BowAttack");
        //                        source.PlayOneShot(swooshClip, .25f); //plays the clip SwooshClip at a volume of 25%

        //                        // update the time of the last attack
        //                        m_lastAttack = Time.time;

        //                        // register the gameobject the player attacked
        //                        m_lastAttacked = attacked;

        //                        arrows[i].GetComponent<Projectile>().target = m_clickToMove.targetTransf;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
    }
    
    IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(1/attackSpeed);
        attacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
         //Use the same vars you use to draw your Overlapsphere to draw your Wire Sphere.
        Gizmos.DrawWireSphere((transform.position + transform.forward * 0.75f), 0.75f);
    }

    void GetEnemiesInRange()
    {
        enemiesInRange.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * 0.75f), 0.75f))
        {
            if ( c.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("Hit Enemy ColliderSphere");
                enemiesInRange.Add(c.transform);
            }
        }
    }

    // called everytime we change weapons, or something in the equip is changed, etc...
    void SetAttackDamage()
    {
        attackDamage = GameLogic.CalculatePlayerBaseAttackDamage(this) + Random.Range(minWeaponDamage, maxWeaponDamage) + bonusDamage;
    }

    public void GetHit(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) print("You are DEAD!!!");
        healthBar.Find("Fill_bar").GetComponent<Image>().fillAmount = (float)(currentHealth) / (float)(totalHealth);
        currentHealthText.text = "HP: "+currentHealth+"/"+totalHealth;
    }

    #region EXPERIENCE
    public void SetExperience(float exp)
    {
        totalExperience += exp;
        currentExperience += exp;
        float experienceNeeded = GameLogic.ExperienceForNextLevel(level);
        float previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        // LEVEL UP
        while (currentExperience >= experienceNeeded)
        {
            LevelUp();
            experienceNeeded = GameLogic.ExperienceForNextLevel(level);
            previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        }
        experienceBar.Find("Fill_bar").GetComponent<Image>().fillAmount = (currentExperience) / (experienceNeeded);
    }

    void LevelUp()
    {
        currentExperience = 0;
        level++;
        levelText.text = "Lv. " + level.ToString("00");
    }
    #endregion

    ///// <summary>
    ///// CALLED BY THE ATTACK ANIMATION EVENT, look at: https://unity3d.com/learn/tutorials/topics/animation/animation-curves-and-events
    ///// Each attack animation needs an event placed at the point where damage should be dealt.
    ///// The event will call this function.
    ///// </summary>
    //public void DealDamage()
    //{
    //    // Consider Weapon Damage + Player Strength (still missing Dodge, ac...)
    //    int temp = (Random.Range(m_stats.minWeaponDamage, m_stats.maxWeaponDamage) + (int)(m_stats.playerStrength / 5));
    //    // damage the enemy ( SendMessageOptions.DontRequireReceiver ensures that no errors are thrown if the gameobject cannot be damaged )
    //    m_lastAttacked.SendMessage("ReceiveDamage", temp, SendMessageOptions.DontRequireReceiver);
    //    Debug.Log("DMG BY THE PLAYER: "+temp);
    //}
}
