using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity;
    private GameObject myBullet;
	private Animator myAnimator;

	public void Start()
	{
		myAnimator = GetComponent<Animator>();
	}

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        Shoot();
    //    }
    //}

    public void Shoot()
    {
        myBullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        myBullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * bulletVelocity;
		Destroy(myBullet, 3.0f);

		if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
		{
			myAnimator.Play("Default");
			myAnimator.Play("Shooting");
		}

		else
		{
			myAnimator.Play("Shooting");
		}

		//Debug.Log("Bang!");
		//Debug.Log("The rotation angle is"+myBullet.transform.rotation);
	}
}