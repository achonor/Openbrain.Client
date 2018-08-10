using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


public class AnimClipCopyEditor : EditorWindow
{
	[MenuItem("BCTool/GetClipFromAnimation")]
	static void ShowWindow()
	{
		GetWindow<AnimClipCopyEditor>();
	}

	void OnEnable()
	{
		savePath = Application.dataPath;
	}
	Animation anim;
	string savePath;
	void OnGUI()
	{
		anim = (Animation)EditorGUILayout.ObjectField(anim, typeof(Animation));
		if (anim != null)
		{
			if(anim.clip != null)
			{
				EditorGUILayout.LabelField("当前的Clip是："+ anim.clip.name);
				if (FileOperation.showIOPath("复制动画clip到这里：", ref savePath, false, true, anim.clip.name ,"anim"))
				{
					if (!savePath.Contains(Application.dataPath))
					{
						EditorUtility.DisplayDialog("警告", "目前不允许使用这个路径，请先将文件保存到项目的Assets目录下面（任意位置任意深度），然后复制到电脑任意位置就随你的便", "OK");
						return;
					}
					DoExport();
				}
			}
			else
			{
				EditorGUILayout.LabelField("当前的Clip为空，请先到Animation组件上设置");
			}
		}
	}
	void DoExport()
	{
		string temp = savePath;
		temp = temp.Replace(Application.dataPath, "");
		temp = "Assets" + temp;
		AnimationClip newClip = new AnimationClip();
		EditorUtility.CopySerialized(anim.clip, newClip);
		AssetDatabase.CreateAsset(newClip, temp);
	}
}