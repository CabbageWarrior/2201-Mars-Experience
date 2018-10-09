using UnityEngine;
using DG.Tweening;
using VRTK;

public class LabComputer : MonoBehaviour
{
    public GameObject drawerA;
    public GameObject drawerB;
    public GameObject drawerC;
   // public GameObject testSphere;
	public GameObject c55;

	public GrabbableAtDistance Quarzo, Trimidite, Cristobalite;

    bool isDrawerAOpen = false;
    bool isDrawerBOpen = false;
    bool isDrawerCOpen = false;

    private void Start()
    {
       

        Quarzo.IsAllowed = Quarzo.InteractableObject.isGrabbable = false;
        Trimidite.IsAllowed = Trimidite.InteractableObject.isGrabbable = false;
        Cristobalite.IsAllowed = Cristobalite.InteractableObject.isGrabbable = false;

    }
	/*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isDrawerAOpen)
        {
            OpenDrawer(drawerA);
            isDrawerAOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && !isDrawerBOpen)
        {
            OpenDrawer(drawerB);
            isDrawerBOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.L) && !isDrawerCOpen)
        {
            OpenDrawer(drawerC);
            isDrawerCOpen = true;
        }
        if (isDrawerAOpen && isDrawerBOpen && isDrawerCOpen)
        {
            ChangeMonitorOutput();
        }
    }
	*/

	void Check()
	{
		if (isDrawerAOpen && isDrawerBOpen && isDrawerCOpen)
        {
            ChangeMonitorOutput();
        }
	}
    

	public	void	OpenDrawerA()
	{
		if (!isDrawerAOpen)
		{
			drawerA.transform.DOLocalMoveZ((drawerA.transform.localPosition.z - 0.15f), 1f);
			isDrawerAOpen = true;
            Quarzo.IsAllowed = Quarzo.InteractableObject.isGrabbable = true;
			Check();
		}
	}

	public	void	OpenDrawerB()
	{
		if (!isDrawerBOpen)
		{
			drawerB.transform.DOLocalMoveZ((drawerB.transform.localPosition.z - 0.15f), 1f);
			isDrawerBOpen = true;
            Trimidite.IsAllowed = Trimidite.InteractableObject.isGrabbable = true;
			Check();
		}
	}

	public	void	OpenDrawerC()
	{
		if (!isDrawerCOpen)
		{
			drawerC.transform.DOLocalMoveZ((drawerC.transform.localPosition.z - 0.15f), 1f);
			isDrawerCOpen = true;
            Cristobalite.IsAllowed = Cristobalite.InteractableObject.isGrabbable = true;
			Check();
		}
	}


    /// <summary>
    /// Slides drawer open using DOTween.
    /// </summary>
    /// <param name="drawer">Drawer to slide.</param>
   /* private void OpenDrawer(GameObject drawer)
    {
        drawer.transform.DOLocalMoveZ((drawer.transform.localPosition.z - 0.1f), 1f);
    }
	*/
    private void ChangeMonitorOutput()
    {
       // testSphere.SetActive(true);
		c55.SetActive(true);
    }
}