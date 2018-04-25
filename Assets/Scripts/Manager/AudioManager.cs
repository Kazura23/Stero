using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public class AudioManager : ManagerParent
{
	#region Variable
	[Tooltip ("Create a new ScriptableAudio and drag and drop here (in project : create -> Scriptable -> Audio)")]
	public AudioScriptable [ ] AllMF;
	Dictionary<AudioType, AudioInfo> AllAudioInfo; 
	#endregion

	#region Mono
	#endregion

	#region Public
	public AudioSource OpenAudio (AudioType thisType, string thisName = "", bool loopAudio = false, System.Action thisAct = null, bool playIfEmpty = false)
	{
		AudioInfo thisInfo;
		AllAudio[] getAllAudio;
		
		if (AllAudioInfo.TryGetValue (thisType, out thisInfo))
		{
			getAllAudio = thisInfo.AllAudScript;

			if (getAllAudio == null || getAllAudio.Length == 0)
			{
				return null;
			}	
			if (thisName == "")
			{
				thisName = thisInfo.AllAudScript [Random.Range (0, thisInfo.AllAudScript.Length)].AudioName;
			}
		}
		else
		{
			return null;
		}

		List<AudioSource> getAC = thisInfo.audioChild;
		AudioSource thisSource = thisInfo.audioParent;
		AllAudio thisAud;

		int getVolume = thisInfo.VolumeAudio;
		int getLenght = getAllAudio.Length;
		int a;

		for (a = 0; a < getLenght; a++)
		{
			if (getAllAudio [a].AudioName == thisName)
			{
				thisAud = getAllAudio [a];
				if (loopAudio)
				{
					thisSource.enabled = true;
					thisSource.volume = thisAud.Volume * getVolume;
					thisSource.pitch = thisAud.Pitch;
					thisSource.clip = thisAud.Audio;
					thisSource.Play ( );

					thisSource.loop = true;
					return thisSource;
				}
				else
				{
					if (playIfEmpty)
					{
						if (getAC.Count > 0)
						{
							return null;
						}
					}

					thisSource.loop = false;

					AudioSource getAud = thisSource.gameObject.AddComponent<AudioSource> ( );

					getAud.loop = false;
					getAud.volume = thisAud.Volume * getVolume;
					getAud.pitch = thisAud.Pitch;
					getAud.enabled = true;
					getAud.clip = thisAud.Audio;
					getAud.Play ( );

					getAC.Add (getAud);

					StartCoroutine (waitEndAudio (getAud.clip.length - 0.1f, getAud, getAC, thisAct));
					return getAud;
				}
			}
			
		}

		return null;
	}

	public AudioSource GetSource (AudioType thisType)
	{
		AudioInfo thisInfo;

		if (AllAudioInfo.TryGetValue (thisType, out thisInfo))
		{
			return thisInfo.audioParent;
		}

		return null;
	}

	public void CloseAudio (AudioType thisType)
	{
		AudioInfo thisInfo;

		if (AllAudioInfo.TryGetValue (thisType, out thisInfo))
		{
			thisInfo.audioParent.enabled = false;
		}
	}

	public void CloseUnLoopAudio (AudioType thisType, bool allUnloop = false)
	{
		List<AudioSource> getAC;
		AudioInfo thisInfo;
		
		System.Array getArray = System.Enum.GetValues(typeof(AudioType));
		int getLengthA = getArray.Length;
		int getLengthB;
		int b;

		if( allUnloop )
		{
			for (int a = 0; a < getLengthA; a++)
			{
				if (!AllAudioInfo.TryGetValue ((AudioType)getArray.GetValue (a), out thisInfo))
				{
					continue;
				}
				else if ( thisInfo.audioChild == null )
				{
					continue;
				}

				getAC = thisInfo.audioChild;

				getLengthB = getAC.Count;
				for ( b = 0; b < getLengthB; b++)
				{
					Destroy (getAC [b]);
				}
				
				getAC.Clear ( );
			}
		}
		else
		{
			if (!AllAudioInfo.TryGetValue (thisType, out thisInfo))
			{
				return;
			}

			getAC = thisInfo.audioChild;

			getLengthB = getAC.Count;
			for ( b = 0; b < getLengthB; b++)
			{
				Destroy (getAC [b]);
			}
			
			getAC.Clear ( );
		}
	}

	public void CloseAllAudio ( )
	{
		CloseUnLoopAudio ( AudioType.Acceleration, true );
		AudioInfo thisInfo;
		System.Array getArray = System.Enum.GetValues(typeof(AudioType));
		int getLengthA = getArray.Length;
		
		for (int a = 0; a < getLengthA; a++)
		{
			if (AllAudioInfo.TryGetValue ((AudioType)getArray.GetValue (a), out thisInfo))
			{
				thisInfo.audioParent.clip = null;
			}
		}
	}
	#endregion

	#region Private
	protected override void InitializeManager ( )
	{	
		System.Array getArray = System.Enum.GetValues(typeof(AudioType));
		Dictionary<AudioType, List<AllAudio>> OrderAllAudio = new Dictionary<AudioType, List<AllAudio>>(getArray.Length); 
		AllAudioInfo = new Dictionary<AudioType, AudioInfo>(getArray.Length);

		AudioScriptable[] getAudScrip = AllMF;
		List<AllAudio> SetAllAudio;

		int getLength = getAudScrip.Length;

		{
			MusicFX[] currMFX;
			int getLengthB;
			int b;
			for (int a = 0; a < getLength; a++)
			{
				currMFX = getAudScrip[a].AllMF.ToArray();
				getLengthB = currMFX.Length;
				for ( b = 0; b < getLengthB; b++)
				{
					if(OrderAllAudio.TryGetValue(currMFX[b].ThisType, out SetAllAudio))
					{
						SetAllAudio.AddRange(currMFX[b].SetAudio);
					}	
					else
					{
						SetAllAudio = new List<AllAudio>(currMFX[b].SetAudio.ToArray());
						OrderAllAudio.Add(currMFX[b].ThisType, SetAllAudio);
					}				
				}
			}
		}
		
		AudioInfo currAudInfo;

		Transform getTrans = transform;
		Transform currT = transform;
		GameObject getObj;
		AudioType getAudType;

		getLength = getArray.Length;
		for (int a = 0; a < getLength; a++)
		{
			currAudInfo = new AudioInfo();
			getObj = new GameObject();	
			getObj.transform.SetParent(getTrans);
			
			getAudType = (AudioType)getArray.GetValue (a);
			getObj.name = getAudType.ToString();

			currAudInfo.audioParent = getObj.AddComponent<AudioSource>( );
			currAudInfo.audioChild = new List<AudioSource>();

			if(OrderAllAudio.TryGetValue(getAudType, out SetAllAudio))
			{
				currAudInfo.AllAudScript = SetAllAudio.ToArray();
			}
			
			AllAudioInfo.Add (getAudType, currAudInfo );
		}
	}

	IEnumerator waitEndAudio (float time, AudioSource thisSource, List<AudioSource> audioChild, System.Action thisAct = null)
	{
		yield return new WaitForSeconds (time);

		if (thisSource != null)
		{
			if (audioChild.Contains (thisSource))
			{
				audioChild.Remove (thisSource);
			}

			if (thisAct != null)
			{
				thisAct.Invoke ( );
			}

			Destroy (thisSource);
		}
	}
	#endregion
}

public class AudioInfo 
{
	public int VolumeAudio = 1;
	public AudioSource audioParent;
	public List<AudioSource> audioChild;
	public AllAudio[] AllAudScript;
}
