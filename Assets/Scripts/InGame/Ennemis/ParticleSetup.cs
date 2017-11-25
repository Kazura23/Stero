using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSetup : MonoBehaviour {

    public AnimationCurve ShapeOverTime;
  
  
    private float animtime;
    public ParticleSystem ps;
   private float angle;
    int layer = 0;
   [ Range (1f,10f)]
    public float SpeedValue = 1f;
    private float normalizedTime;
    public Animator myAnimator;
    // Use this for initialization
    void Start () {

        myAnimator = transform.GetComponentInChildren<Animator>();
        //ps = transform.GetComponentInChildren<ParticleSystem>();
        //Debug.Log(GetComponentInChildren<ParticleSystem>());
        AnimatorStateInfo animationState = myAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = myAnimator.GetCurrentAnimatorClipInfo(0);
        float animtime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
    }

     void psSetup() {
   
        var main = ps.main;
         var sh = ps.shape;

       // animtime = attack["Vino_attack"].time;
        main.startSpeed = SpeedValue;
        sh.angle = (ShapeOverTime.Evaluate(animtime))*25; ;
    }
	// Update is called once per frame
	void Update () {
        psSetup();

    }
}
