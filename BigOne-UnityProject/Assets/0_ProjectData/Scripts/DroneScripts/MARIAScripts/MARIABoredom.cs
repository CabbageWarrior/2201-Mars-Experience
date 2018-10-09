using UnityEngine;

public class MARIABoredom : MonoBehaviour
{
    private float viewAngle = 110f;
    public float timeBeforeBoredom = 10f;
    public float timeBeforeInterest = 5f;
    public float timeBeforeAnimations = 5f;
    [SerializeField]
    private float boredomCounter;
    [SerializeField]
    private float interestCounter;
    [SerializeField]
    private float waitForAnimationCounter;
    [SerializeField]
    private bool isBored;
    [SerializeField]
    private bool isVisibleByPlayer = false;

    public Animator animator;
    public Camera playerView;

    [SerializeField]
    private Transform mariaTransform;
    [SerializeField]
    private MARIAGrabAttach mariaGrabAttach;

    private void Update()
    {
        if (playerView == null)
        {
            playerView = Camera.main;
            return;
        }

        if (mariaGrabAttach.IsGrabbed)
        {
            isBored = false;
            boredomCounter = 0;
            interestCounter = 0;
            return;
        }

        FindMARIAPosition();

        if (isBored)
        {
            waitForAnimationCounter += Time.deltaTime;
            RandomizeAnimations();

            if (isVisibleByPlayer)
            {
                interestCounter += Time.deltaTime;

                if (interestCounter >= timeBeforeInterest)
                {
                    isBored = false;
                    animator.SetTrigger("BecomesSurprised");
                    interestCounter = 0;
                }
            }
            else
            {
                interestCounter = 0;
            }
        }
        else
        {
            waitForAnimationCounter = 0;
            animator.SetInteger("BoredAnimationNumber", 0);

            if (!isVisibleByPlayer)
            {

                boredomCounter += Time.deltaTime;

                if (boredomCounter >= timeBeforeBoredom)
                {
                    isBored = true;
                    animator.SetTrigger("BecomesBored");
                    boredomCounter = 0;
                }
            }
            else
            {
                boredomCounter = 0;
            }
        }
    }

    private void FindMARIAPosition()
    {
        Vector3 dirToMARIA = (mariaTransform.position - playerView.transform.position).normalized;
        float dstToMARIA = Vector3.Distance(playerView.transform.position, mariaTransform.position);

        if (Vector3.Angle(playerView.transform.forward, dirToMARIA) < viewAngle / 2)
        {
            if (Physics.Raycast(playerView.transform.position, dirToMARIA, dstToMARIA))
            {
                isVisibleByPlayer = true;
            }
        }
        else
        {
            isVisibleByPlayer = false;
        }
    }

    private void RandomizeAnimations()
    {
        if (waitForAnimationCounter >= timeBeforeAnimations)
        {
            animator.SetInteger("BoredAnimationNumber", Random.Range(1, 3));
            waitForAnimationCounter = 0;
        }
    }
}