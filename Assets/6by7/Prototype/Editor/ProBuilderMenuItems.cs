#define PROTOTYPE
#define PROTOTYPE
using UnityEngine;
using UnityEditor;
using System.Collections;
using ProBuilder2.EditorEnum;

public class ProBuilderMenuItems : EditorWindow
{
	const int SECTION = 15;

	// todo - Consolidate PRODUCT_NAME to pb_Constant
	#if PROTOTYPE
	const string PRODUCT_NAME = "Prototype";
	#else
	const string PRODUCT_NAME = "ProBuilder";
	#endif

#region WINDOW

	[MenuItem("Tools/" + PRODUCT_NAME + "/" + PRODUCT_NAME + " Window", false, SECTION + 0)]
	public static pb_Editor OpenEditorWindow()
	{
		if(EditorPrefs.HasKey(pb_Constant.pbDefaultOpenInDockableWindow) && !EditorPrefs.GetBool(pb_Constant.pbDefaultOpenInDockableWindow))
			return (pb_Editor)EditorWindow.GetWindow(typeof(pb_Editor), true, PRODUCT_NAME, true);			// open as floating window
		else
			return (pb_Editor)EditorWindow.GetWindow(typeof(pb_Editor), false, PRODUCT_NAME, true);			// open as dockable window
	}

	#if !PROTOTYPE
	[MenuItem("Tools/ProBuilder/Texture Window", false, SECTION + 1)]
	public static void OpenTextureWindow()
	{
		pb_Editor.instance.SetEditLevel(EditLevel.Texture);
	}
	#endif

	[MenuItem("Tools/" + PRODUCT_NAME + "/Open Shape Menu %#k", false, SECTION + 2)]
	public static void ShapeMenu()
	{
		EditorWindow.GetWindow(typeof(pb_Geometry_Interface), true, "Shape Menu", true);
	}

	public static void ForceCloseEditor()
	{
		EditorWindow.GetWindow<pb_Editor>().Close();
	}
#endregion

#region ProBuilder/Edit

	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Edit Level")]
	public static void ToggleEditLevel()
	{
		pb_Editor.instance.ToggleEditLevel();
	}

	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Selection Mode")]
	public static void ToggleSelectMode()
	{
		pb_Editor.instance.ToggleSelectionMode();
	}

	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Lightmap Settings Window", false, SECTION + 2)]
	public static void LightmapWindowInit()
	{
		pb_Lightmap_Editor.Init(pb_Editor.instance);
	}

#endregion	
}
