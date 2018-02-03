using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScreen : UiParent 
{
	#region Variables
	public override MenuType ThisMenu
	{
		get
		{
			return MenuType.Title;
		}
	}
	#endregion
	
	
	#region Mono
	void Update()
	{
		if(Input.anyKeyDown)
		{
			GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete( () =>
			{
				GlobalManager.Ui.CloseThisMenu ( );
			});
		}
	}
	#endregion
	
	
	#region Public

	#endregion
	
	
	#region Private
	protected override void InitializeUi()
	{
	}

	public override void OpenThis(MenuTokenAbstract GetTok = null)
	{
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOKill ( );
		GlobalManager.Ui.MenuParent.GetComponent<CanvasGroup>().DOFade(1, 0);

		base.OpenThis ( GetTok );
	}
	public override void CloseThis ( )
	{
		base.CloseThis();
	}
	#endregion
}
