using UnityEngine;
using UnityEngine.UI;

public abstract class UiParent : MonoBehaviour
{
	#region Variables
	public abstract MenuType ThisMenu
	{
		get;
	}

	public Color BackGroundColor;
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	public void Initialize()
	{
		if ( BackGroundColor == new Color ( ) )
		{
			BackGroundColor = Color.white;
		}
		InitializeUi();
		CloseThis ( );
	}

	public virtual void OpenThis ( MenuTokenAbstract GetTok = null )
	{
		GlobalManager.Ui.GlobalBack.transform.GetChild ( 0 ).GetComponent<Image> ( ).color = BackGroundColor;
		gameObject.SetActive ( true );
	}

	public virtual void CloseThis ( )
	{
		gameObject.SetActive ( false );
	}
	#endregion

	#region Private Methods
	protected abstract void InitializeUi();
	#endregion
}
