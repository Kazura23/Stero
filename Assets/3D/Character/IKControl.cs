using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {

 Animator anim;


	public float ikWeight = 1f;
	public Transform RightHandTarget;
	
	

	public float IkValues;

	//Invoked when a submit button is clicked.

	void Start () 
	{
		anim = GetComponent<Animator>();
	}
    //a callback for calculating IK
    void weightupdate()
	{
        ikWeight = IkValues;
			}



	void OnAnimatorIK()
	{	
		
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand,ikWeight);
		anim.SetIKPosition(AvatarIKGoal.RightHand,RightHandTarget.position);

		}
    //Get IK Weight from animation parameter

    void Update () {
      
        IkValues = anim.GetFloat("IK") ;
        weightupdate();
    }
	}    
