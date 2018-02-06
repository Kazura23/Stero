using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punch : MonoBehaviour {
    
    public float puissanceOnde = 15;
    private PlayerController control;
	Transform getPlayer;
   
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
    }

    void OnTriggerEnter(Collider other)
    {
		//Debug.Log ( other.gameObject.name );
		Rigidbody getRid = other.GetComponentInChildren<Rigidbody> ( );
		AbstractObject tryGet = other.GetComponentInChildren<AbstractObject> ( );
        if(numTechnic == (int)Technic.onde_choc)
        {
            switch (other.tag)
            {
			case Constants._EnnemisTag:
			case Constants._ElemDash:
				Vector3 dir = Vector3.Normalize ( other.transform.position - transform.position );
				if ( !tryGet )
				{
					return;
				}
				tryGet.Degat ( dir * puissanceOnde, ( int ) Technic.onde_choc );
                    break;
                case Constants._ObsPropSafe:
				GlobalManager.GameCont.MeshDest.SplitMesh(other.gameObject, control.transform, 100, 3 );
                    break;
                //case tag bibli
            }
        }
		else if( canPunc && ( other.gameObject.tag == Constants._EnnemisTag || other.gameObject.tag == Constants._ObsPropSafe || other.gameObject.tag == Constants._ElemDash || other.gameObject.tag == Constants._ObjDeadTag  ))
        {
			if ( !tryGet )
			{
				other.gameObject.tag = Constants._UnTagg;
				if ( getRid != null )
				{
					getRid.AddForce ( projection_double * getPlayer.forward, ForceMode.VelocityChange );
				}
			}

            if(other.gameObject.tag == Constants._EnnemisTag)
            {
                GlobalManager.AudioMa.OpenAudio(AudioType.SteroKill, "", false, null, true);
                GlobalManager.Ui.BloodHit();
            }

            GlobalManager.AudioMa.OpenAudio(AudioType.Other, "PunchSuccess", false);
			Vector3 getProj = Vector3.zero;
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
                //MadnessMana("Simple");

                if ( RightPunch )
				{
					getProj -= getPlayer.right;
				}
				else
				{
					getProj += getPlayer.right;
				}

				if ( other.gameObject.tag != Constants._ObjDeadTag && tryGet)
				{
					tryGet.Degat ( getProj * projection_basic, numTechnic );
				}
				else
				{
					if ( getRid != null )
					{
						getRid.AddForce ( getProj * projection_basic, ForceMode.VelocityChange );
					}
				}

                break;
			case (int)Technic.double_punch:
                //MadnessMgetProj = getPlayer.forward;ana("Double");
                getProj = getPlayer.forward;

				//Debug.Log ( pourcPunch );
				if ( other.gameObject.tag != Constants._ObjDeadTag && tryGet)
				{
					control.StopCDPunch ( );
					tryGet.Degat ( projection_double * getProj/* * pourcPunch*/, numTechnic );
				}
				else
				{
					if ( getRid != null )
					{
						getRid.AddForce ( projection_double * getProj + Vector3.up * 5, ForceMode.VelocityChange );
					}
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

    
}
