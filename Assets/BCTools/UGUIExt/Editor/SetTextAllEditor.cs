using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SetTextAllEditor : EditorWindow
{
	private bool m_OnlyThisName = false;
	public string OnlyUseThisName = "";

	private bool m_ChangeHorizon = false;
	HorizontalWrapMode m_HorizonMode = HorizontalWrapMode.Wrap;

	private bool m_ChangeVertical = false;
	VerticalWrapMode m_VerticalMode = VerticalWrapMode.Truncate;

	private bool m_ChangeSize = false;
	int m_FontSize = 20;

	private bool m_ChangeAlighment = false;
	TextAnchor m_Anchor = TextAnchor.MiddleCenter;

	private bool m_ChangeStyle = false;
	FontStyle m_Style = FontStyle.Normal;

	private bool m_ChangeFont = false;
	Font m_Font = null;

	private bool m_setOutlineColor = false;
	Color m_outlineColor = Color.black;
	private bool m_setOutlineSize = false;
	Vector2 m_outlineSize = new Vector2(1, -1);

	Font m_FontDefault = null;//if font is none,use me


    [MenuItem("BCTool/UGUI Set Text All")]
    public static void ShowWindow()
    {
        SetTextAllEditor pEditor = (SetTextAllEditor)EditorWindow.GetWindow(typeof(SetTextAllEditor));
    }

	void OnEnable()
	{
	}

	void OnGUI()
	{
		EditorGUILayout.HelpBox("该工具用于批量修改UGUI的Text组件，选中一个游戏物体，然后点击下面按钮即"+
		"可批量修改该物体的所有含有Text组件的子物体。OnlyThisName如果被设置，就只改对应名的子物体。" + 
		"当发现某个Text字体为None时，会使用Default默认字体。更多详情欢迎查看源码SetTextAllEditor.cs", MessageType.Info);

		m_OnlyThisName = EditorGUILayout.Toggle("OnlyThisName", m_OnlyThisName);
		if(m_OnlyThisName)
		{
			OnlyUseThisName = EditorGUILayout.TextField("", OnlyUseThisName);
		}

		m_ChangeHorizon = EditorGUILayout.Toggle("Change Horizon", m_ChangeHorizon);
		if (m_ChangeHorizon)
		{
			m_HorizonMode = (HorizontalWrapMode)EditorGUILayout.EnumPopup("Horizon", m_HorizonMode);
		}

		m_ChangeVertical = EditorGUILayout.Toggle("Change Vertical", m_ChangeVertical);
		if (m_ChangeVertical)
		{
			m_VerticalMode = (VerticalWrapMode)EditorGUILayout.EnumPopup("Vertical", m_VerticalMode);
		}

		m_ChangeSize = EditorGUILayout.Toggle("Change Size", m_ChangeSize);
		if (m_ChangeSize)
		{
			m_FontSize = EditorGUILayout.IntField("Font size", m_FontSize);
		}

		m_ChangeAlighment = EditorGUILayout.Toggle("Change Alighment", m_ChangeAlighment);
		if (m_ChangeAlighment)
		{
			m_Anchor = (TextAnchor)EditorGUILayout.EnumPopup("Anchor", m_Anchor);
		}

		m_ChangeStyle = EditorGUILayout.Toggle("Change Style", m_ChangeStyle);
		if (m_ChangeStyle)
		{
			m_Style = (FontStyle)EditorGUILayout.EnumPopup("Font style", m_Style);
		}


		m_ChangeFont = EditorGUILayout.Toggle("Change Font", m_ChangeFont);
		if (m_ChangeFont)
		{
			m_Font = (Font)EditorGUILayout.ObjectField("Font", m_Font, typeof(Font));
		}

		m_setOutlineColor = EditorGUILayout.Toggle("Set Outline Color", m_setOutlineColor);
		if (m_setOutlineColor)
		{
			m_outlineColor = (Color)EditorGUILayout.ColorField("OutlineColor", m_outlineColor);
		}

		m_setOutlineSize = EditorGUILayout.Toggle("Set Outline Size", m_setOutlineSize);
		if (m_setOutlineSize)
		{
			m_outlineSize = EditorGUILayout.Vector2Field("OutlineSize", m_outlineSize);
		}

		m_FontDefault = (Font)EditorGUILayout.ObjectField("Default Font", m_FontDefault, typeof(Font));

		if (GUILayout.Button("Set all child Text"))
		{
			DoSet();
		}
	}

	void DoSet()
	{
		Transform[] trans = UnityEditor.Selection.transforms;
		if (trans == null || trans.Length != 1 || trans[0] == null)
		{
			if (!EditorUtility.DisplayDialog("Error...", "Please select one(only one) transform in Hierarchy before you do this!", "Got it", "Ask BC"))
			{
				EditorUtility.DisplayDialog("Important Thing say 3 times", "Select one transform in Hierarchy\nselect One transform in Hierarchy\nselect one Transform in Hierarchy", "OK");
			}
			return;
		}
		Text[] txtArr = trans[0].GetComponentsInChildren<Text>(true);
		for (int i = 0; i < txtArr.Length; i++)
		{
			if (m_OnlyThisName)
			{
				if (!txtArr[i].name.Equals(OnlyUseThisName))
				{
					continue;
				}
			}
			if (m_ChangeHorizon && !txtArr[i].resizeTextForBestFit)
			{
				txtArr[i].horizontalOverflow = m_HorizonMode;
			}
			if (m_ChangeVertical && !txtArr[i].resizeTextForBestFit)
			{
				txtArr[i].verticalOverflow = m_VerticalMode;
			}
			if (m_ChangeSize && !txtArr[i].resizeTextForBestFit)
			{
				txtArr[i].fontSize = m_FontSize;
			}
			if (m_ChangeAlighment)
			{
				txtArr[i].alignment = m_Anchor;
			}
			if (m_ChangeStyle)
			{
				txtArr[i].fontStyle = m_Style;
			}
			if (m_setOutlineColor || m_setOutlineSize)
			{
				Outline outline = txtArr[i].GetComponent<Outline>();
				if (outline != null)
				{
					if (m_setOutlineColor)
					{
						outline.effectColor = m_outlineColor;
					}
					if (m_setOutlineSize)
					{
						outline.effectDistance = m_outlineSize;
					}
				}
			}

			if (m_ChangeFont)
			{
				txtArr[i].font = m_Font;
			}
			else
			{
				if (txtArr[i].font == null)
				{
					txtArr[i].font = m_FontDefault;
				}
			}
		}
		EditorUtility.DisplayDialog("Done", "Now all Text in the transform whith you select had been changed!\n( ^_^ )", "Thank BC");
	}
}
