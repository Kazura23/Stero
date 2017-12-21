using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punch : MonoBehaviour {
    private Slider barMadness;
    public float addPointBarByPunchSimple = 3;
    public float addPointBarByPunchDouble = 5;
    public float puissanceOnde = 15;
    private PlayerController control;
	Transform getPlayer;
    private enum Technic
    {
        basic_punch,
        double_punch,
        onde_choc
    }
		
    private int numTechnic;
	[Tooltip ("X = force droite / gauche - Y = force haut / bas - Z = force Devant / derriere" )]
	public float projection_basic = 50;
	public float projection_double = 100;
	public float projection_dash = 150;

    public float facteurVitesseRenvoie = 1.5f;
	public bool RightPunch = false;

	bool canPunc = true;
	//float pourcPunch = 100;
    void Start()
    {
		getPlayer = GlobalManager.GameCont.Player.transform;
		control = getPlayer.GetComponent<PlayerController>();
        barMadness = control.BarMadness;
        barMadness.value = 0;
    }

    void OnTriggerEnter(Collider other)
    {
		//Debug.Log ( other.gameObject.name );
        if(numTechnic == (int)Technic.onde_choc)
        {
            switch (other.tag)
            {
					case Constants._EnnemisTag : case Constants._ElemDash :
                    Vector3 dir = Vector3.Normalize(other.transform.position - transform.position);
                    AbstractObject enn = other.GetComponentInChildren<AbstractObject>();
                    if (!enn)
                    {
                        return;
                    }
                    enn.Degat(dir * puissanceOnde, (int)Technic.onde_choc);
                    break;
                case Constants._ObsPropSafe:
                    GlobalManager.GameCont.MeshDest.SplitMesh(other.gameObject, control.transform, 100, 3);
                    break;
                //case tag bibli
            }
        }

		else if( canPunc && ( other.gameObject.tag == Constants._EnnemisTag || other.gameObject.tag == Constants._ObsPropSafe || other.gameObject.tag == Constants._ElemDash || other.gameObject.tag == Constants._ObjDeadTag  ))
        {
			AbstractObject tryGet = other.GetComponentInChildren<AbstractObject> ( );
			if ( !tryGet )
			{
				tryGet = other.gameObject.AddComponent<ProtoObs> ( );
			}

            if(other.gameObject.tag == Constants._EnnemisTag)
            {
                int rdmValue = UnityEngine.Random.Range(0, 5);
                GlobalManager.AudioMa.OpenAudio(AudioType.SteroKill, "MrStero_Kill_" + rdmValue, false, null, true);
                GlobalManager.Ui.BloodHit();
            }


            GlobalManager.AudioMa.OpenAudio(AudioType.Other, "PunchSuccess", false);
            // Debug.Log("song");
			Vector3 getProj = getPlayer.forward + getPlayer.right;
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
                MadnessMana("Simple");

                if ( RightPunch )
				{
					getProj += getPlayer.right * Random.Range ( 0.2f, 1f );
				}
				else
				{
					getProj -= getPlayer.right * Random.Range ( 0.2f, 1f );
				}

				if ( other.gameObject.tag != Constants._ObjDeadTag )
				{
					tryGet.Degat ( getProj * projection_basic, numTechnic );
				}
				else
				{
					other.GetComponentInChildren<Rigidbody>().AddForce ( getProj * projection_basic, ForceMode.VelocityChange );
				}

				break;
			case (int)Technic.double_punch:
                MadnessMana("Double");

                //Debug.Log ( pourcPunch );
                if ( other.gameObject.tag != Constants._ObjDeadTag )
				{
					tryGet.Degat ( projection_double * getPlayer.forward/* * pourcPunch*/, numTechnic );
				}
				else
				{
					other.GetComponentInChildren<Rigidbody>().AddForce ( projection_double * getPlayer.forward, ForceMode.VelocityChange );
				}
           	 	break;
            }
        }else if (other.gameObject.tag == Constants._MissileBazoo)
        {
            other.gameObject.GetComponent<MissileBazooka>().ActiveTir(-other.gameObject.GetComponent<MissileBazooka>().GetDirection(), facteurVitesseRenvoie, true);
        }
        
    }

	public void setTechnic(int typeTech/*, float pourc = 100*/ )
    {
		//pourcPunch = pourc;
        numTechnic = typeTech;
    }

	public void SetPunch ( bool canPush )
	{
		canPunc = canPush;
	}

    public void MadnessMana(string type)
    {
        Debug.Log("Madddddd");
        //if (!control.IsInMadness()) {
            if (/*barMadness.value + addPointBarByPunchSimple < barMadness.maxValue &&*/ type == "Simple")
            {
                //barMadness.value += addPointBarByPunchSimple;
                control.AddSmoothCurve(addPointBarByPunchSimple);
            } else if (/*barMadness.value + addPointBarByPunchDouble < barMadness.maxValue &&*/ type == "Double")
            {
                //barMadness.value += addPointBarByPunchDouble;
                control.AddSmoothCurve(addPointBarByPunchDouble);
            }
            /*else
            {
                barMadness.value = barMadness.maxValue;
                control.SetInMadness(true);
            }*/
        //}
    }
}
