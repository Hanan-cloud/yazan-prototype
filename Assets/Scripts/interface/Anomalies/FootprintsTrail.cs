using DG.Tweening;
using UnityEngine;
using System.Collections;

public class FootprintsTrail : AnomalyBase
{
    [SerializeField] GameObject anomalyObject;

    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;
    [SerializeField] float step;
    
    
    [SerializeField] GameObject leftFoot;
    [SerializeField] GameObject rightFoot;

    SpriteRenderer leftSR;
    SpriteRenderer rightSR;
    private int direction = 1;
    [SerializeField] private int moveSpeed = 2;

    WaitForSeconds betweenSteps = new WaitForSeconds(1);
    WaitForSeconds stepToDisappar = new WaitForSeconds(0.5f);

    Coroutine StepsCreater;
    private bool canFlip=true;

    float timeToFlip=6;
    float TimeCounter=0;

    private void Awake()
    {
        canFlip = true;
        leftSR = leftFoot.GetComponent<SpriteRenderer>();
        rightSR = rightFoot.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        TimeCounter = 0;
        // anomalyObject.gameObject.SetActive(false);

        SetAnomalyName();

     

        //leftFoot.SetActive(false);
        //rightFoot.SetActive(false);

    }

    private void OnEnable()
    {
 

        StepsCreater = StartCoroutine(Steps());
    }

    IEnumerator Steps()
    {
        GetInitialPos();
        SetDir();

     

        while (true)

        {

            RightFootStep();
            yield return stepToDisappar;
            leftFoot.SetActive(false);


            yield return betweenSteps;


            LeftFootStep();
            yield return stepToDisappar;
            rightFoot.SetActive(false);
            yield return betweenSteps;


            CheckDir();

        }
    }

    void GetInitialPos()
    { 
        float temp = (rightEdge.position.x - leftEdge.position.x) / 2;
        leftFoot.transform.position = new Vector3(temp, leftFoot.transform.position.y, leftFoot.transform.position.z);
        rightFoot.transform.position = new Vector3(temp, rightFoot.transform.position.y, rightFoot.transform.position.z);

    }

    void SetDir()
    {

        if (UnityEngine.Random.Range(1, 7) % 2 == 0)
        {
            direction = 1;
            leftSR.flipX = true;
            rightSR.flipX = true;


        }
        else 
        { 
            direction = -1;
            leftSR.flipX = false;
            rightSR.flipX = false;


        }

    }


    void RightFootStep()
    {
        
        rightFoot.transform.position = new Vector3(leftFoot.transform.position.x + (step * direction), rightFoot.transform.position.y);
        rightFoot.SetActive(true);


    }


    void LeftFootStep()
    {

        leftFoot.transform.position = new Vector3(rightFoot.transform.position.x + (step * direction), leftFoot.transform.position.y);
        leftFoot.SetActive(true);


    }

    void CheckDir()
    {
        if((rightFoot.transform.position.x > rightEdge.position.x || rightFoot.transform.position.x < leftEdge.position.x ) && canFlip) 
        {
            
            canFlip = false;
            TimeCounter = 0;
            direction *= -1;
            if (direction < 0)
            {
                leftSR.flipX = false;
                rightSR.flipX = false;
            }

            else
            {
                leftSR.flipX = true;
                rightSR.flipX = true;

            }

        }
    }

    public override void SetAnomaly()
    {
        anomalyObject.gameObject.SetActive(true);

    }


    public override void ResetAnomaly()
    {
        anomalyObject.gameObject.SetActive(false);


    }



    private void Update()
    {
        if (canFlip == false) 
        {
            TimeCounter += 0.1f * Time.deltaTime;
            if(TimeCounter> timeToFlip )
                canFlip = true;
        
        }
    }

    private void OnDisable()
    {
        StopCoroutine(StepsCreater);
    }







}
