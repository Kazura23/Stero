using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AudioManager : ManagerParent 
{
	#region Variable
	[Tooltip ("Create a new ScriptableAudio and drag and drop here (in project : create -> Scriptable -> Audio)")]
	public List<AudioScriptable> AllMF;
	Dictionary<AudioType, AudioSource> audioChild;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	public void OpenAudio ( AudioType thisType, string thisName, bool loopAudio = false )
	{
		AudioSource thisSource;

		List<AudioScriptable> getAMF = AllMF;
		List<AllAudio> getAllAudio;

		int a;
		int b;
		int c;

		for ( a = 0; a < getAMF.Count; a++ )
		{
			for ( b = 0; b < getAMF [ b ].AllMF.Count; b++ )
			{
				if ( getAMF [ a ].AllMF [ b ].ThisType == thisType )
				{
					getAllAudio = getAMF [ a ].AllMF [ b ].SetAudio;
					for ( c = 0; c < getAllAudio.Count; c++ )
					{
						if ( getAllAudio [ c ].AudioName == thisName )
						{
							if ( audioChild.TryGetValue ( thisType, out thisSource ) )
							{
                                if ( loopAudio )
								{
									thisSource.enabled = true;
									thisSource.volume = getAllAudio [ c ].Volume;
									thisSource.pitch = getAllAudio [ c ].Pitch;
									thisSource.clip = getAllAudio [ c ].Audio;
									thisSource.Play();

									thisSource.loop = true;
								}
								else
								{
									thisSource.loop = false;

									AudioSource getAud = thisSource.gameObject.AddComponent<AudioSource> ( );

									getAud.loop = false;
									getAud.volume = getAllAudio [ c ].Volume;
									getAud.pitch = getAllAudio [ c ].Pitch;
									getAud.enabled = true;
									getAud.clip = getAllAudio [ c ].Audio;
									getAud.Play ( );

									Destroy ( getAud, getAud.clip.length + 0.1f );
								}
								return;
							}
						}
					}
				}
			}
		}
	}

	public AudioSource GetSource ( AudioType thisType )
	{
		AudioSource thisSource;

		if ( audioChild.TryGetValue ( thisType, out thisSource ) )
		{
			return thisSource;
		}

		return null;
	}

	public void CloseAudio ( AudioType thisType )
	{
		AudioSource thisSource;

		if ( audioChild.TryGetValue ( thisType, out thisSource ) )
		{
			thisSource.enabled = false;
		}
	}
	#endregion
	
	#region Private
	protected override void InitializeManager ( )
	{
		Dictionary<AudioType, AudioSource> setDict = new Dictionary<AudioType, AudioSource> ( );

		Transform currT = transform;

		setDict.Add ( AudioType.FxSound, currT.Find ( "Fx" ).GetComponent<AudioSource> ( ) );
		setDict.Add ( AudioType.OtherMusic, currT.Find ( "OtherMusic" ).GetComponent<AudioSource> ( ) );
		setDict.Add ( AudioType.OtherSound, currT.Find ( "OtherFx" ).GetComponent<AudioSource> ( ) );
		setDict.Add ( AudioType.Other, currT.Find ( "Other" ).GetComponent<AudioSource> ( ) );
		setDict.Add ( AudioType.MusicBackGround, currT.Find ( "Music" ).GetComponent<AudioSource> ( ) );

        foreach ( KeyValuePair <AudioType, AudioSource> thisKV in setDict )
		{
			//thisKV.Value.enabled = false;
		}

		audioChild = setDict;
	}
	#endregion
}