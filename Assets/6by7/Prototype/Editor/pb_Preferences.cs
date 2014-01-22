#define PROTOTYPE
#define PROTOTYPE
using UnityEngine;
using UnityEditor;
using ProBuilder2.Common;
using ProBuilder2.EditorEnum;

public class pb_Preferences
{
	#if PROTOTYPE
	const string PRODUCT_NAME = "Prototype";
	#else
	const string PRODUCT_NAME = "ProBuilder";
	#endif

	private static bool prefsLoaded = false;

	static string defaultSelectionMode;
	static SelectMode _selectMode;

	static Color _faceColor;

	static bool defaultOpenInDockableWindow;

	static bool defaultHideFaceMask;

	static Material _defaultMaterial;
	
	static Vector2 settingsScroll = Vector2.zero;

	static int defaultColliderType = 2;
	static bool _showNotifications;

	static bool _forceConvex = false;
	
	static bool pbDragCheckLimit = false;

	static bool pbForceGridPivot = true;

	static pb_Shortcut[] defaultShortcuts;

	#if PROTOTYPE
	[PreferenceItem ("Prototype")]
	#else
	[PreferenceItem ("ProBuilder")]
	#endif
	public static void PreferencesGUI () {

		// Load the preferences
		if (!prefsLoaded) {
			LoadPrefs();
			prefsLoaded = true;
			OnWindowResize();
		}
		
		settingsScroll = EditorGUILayout.BeginScrollView(settingsScroll, GUILayout.MaxHeight(136));
		// Geometry Settings
		GUILayout.Label("Geometry Editing Settings", EditorStyles.boldLabel);

		_selectMode = (SelectMode)EditorGUILayout.EnumPopup("Default Selection Mode", _selectMode);
		
		_faceColor = EditorGUILayout.ColorField("Selected Face Color", _faceColor);

		_defaultMaterial = (Material) EditorGUILayout.ObjectField("Default Material", _defaultMaterial, typeof(Material), false);

		defaultOpenInDockableWindow = EditorGUILayout.Toggle("Open in Dockable Window", defaultOpenInDockableWindow);

		GUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Default Collider");
		defaultColliderType = (int)((ProBuilder.ColliderType)EditorGUILayout.EnumPopup( (ProBuilder.ColliderType)defaultColliderType ));
		GUILayout.EndHorizontal();

		if((ProBuilder.ColliderType)defaultColliderType == ProBuilder.ColliderType.MeshCollider)
			_forceConvex = EditorGUILayout.Toggle("Force Convex Mesh Collider", _forceConvex);

		_showNotifications = EditorGUILayout.Toggle("Show Editor Notifications", _showNotifications);

		pbDragCheckLimit = EditorGUILayout.Toggle(new GUIContent("Limit Drag Check to Selection", "If true, when drag selecting faces, only currently selected pb-Objects will be tested for matching faces.  If false, all pb_Objects in the scene will be checked.  The latter may be slower in large scenes."), pbDragCheckLimit);

		pbForceGridPivot = EditorGUILayout.Toggle(new GUIContent("Force Pivot to Grid", "If true, newly instantiated pb_Objects will have their pivot points set to the first available vertex, ensuring that the object's vertices are placed on-grid."), pbForceGridPivot);

		GUILayout.Space(4);

		GUILayout.Label("Texture Editing Settings", EditorStyles.boldLabel);

		defaultHideFaceMask = EditorGUILayout.Toggle("Hide face mask", defaultHideFaceMask);

		EditorGUILayout.EndScrollView();

		GUILayout.Space(4);

		GUILayout.Label("Shortcut Settings", EditorStyles.boldLabel);

		if(GUI.Button(resetRect, "Use defaults"))
			ResetToDefaults();

		ShortcutSelectPanel();
		ShortcutEditPanel();

		// Save the preferences
		if (GUI.changed)
			SetPrefs();
	}

	public static void OnWindowResize()
	{
		int pad = 10, buttonWidth = 100, buttonHeight = 20;
		resetRect = new Rect(Screen.width-pad-buttonWidth, Screen.height-pad-buttonHeight, buttonWidth, buttonHeight);
	}

	public static void ResetToDefaults()
	{
		if(EditorUtility.DisplayDialog("Delete ProBuilder editor preferences?", "Are you sure you want to delete these?, this action cannot be undone.", "Yes", "No")) {
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultEditMode);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultSelectionMode);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultFaceColor);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultOpenInDockableWindow);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultShortcuts);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultMaterial);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultHideFaceMask);
			EditorPrefs.DeleteKey(pb_Constant.pbDefaultCollider);
			EditorPrefs.DeleteKey(pb_Constant.pbForceConvex);
			EditorPrefs.DeleteKey(pb_Constant.pbShowEditorNotifications);
			EditorPrefs.DeleteKey(pb_Constant.pbDragCheckLimit);
			EditorPrefs.DeleteKey(pb_Constant.pbForceGridPivot);
		}

		LoadPrefs();
	}

	public static int shortcutIndex = 0;
	static Rect selectBox = new Rect(130, 207, 179, 185);

	static Rect resetRect = new Rect(0,0,0,0);
	static Vector2 shortcutScroll = Vector2.zero;
	static int CELL_HEIGHT = 20;
	public static void ShortcutSelectPanel()
	{
		GUILayout.Space(4);
		GUI.contentColor = Color.white;
		GUI.Box(selectBox, "");

		GUIStyle labelStyle = GUIStyle.none;

		if(EditorGUIUtility.isProSkin)
			labelStyle.normal.textColor = new Color(1f, 1f, 1f, .8f);

		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.contentOffset = new Vector2(4f, 0f);

		shortcutScroll = EditorGUILayout.BeginScrollView(shortcutScroll, false, true, GUILayout.MaxWidth(183), GUILayout.MaxHeight(186));

		for(int n = 1; n < defaultShortcuts.Length; n++)
		{
			if(n == shortcutIndex) {
				GUI.backgroundColor = new Color(0.23f, .49f, .89f, 1f);
					labelStyle.normal.background = EditorGUIUtility.whiteTexture;
					Color oc = labelStyle.normal.textColor;
					labelStyle.normal.textColor = Color.white;
					GUILayout.Box(defaultShortcuts[n].action, labelStyle, GUILayout.MinHeight(CELL_HEIGHT), GUILayout.MaxHeight(CELL_HEIGHT));
					labelStyle.normal.background = null;
					labelStyle.normal.textColor = oc;
				GUI.backgroundColor = Color.white;
	
				// if(GUILayout.Button(defaultShortcuts[n].action, EditorStyles.whiteLabel))
				// 	shortcutIndex = n;
			}
			else
			{

				if(GUILayout.Button(defaultShortcuts[n].action, labelStyle, GUILayout.MinHeight(CELL_HEIGHT), GUILayout.MaxHeight(CELL_HEIGHT)))
					shortcutIndex = n;
			}
		}

		EditorGUILayout.EndScrollView();

	}

	static Rect keyRect = new Rect(324, 210, 168, 18);
	static Rect keyInputRect = new Rect(356, 210, 133, 18);

	static Rect descriptionTitleRect = new Rect(324, 270, 168, 200);
	static Rect descriptionRect = new Rect(324, 290, 168, 200);

	static Rect modifiersRect = new Rect(324, 240, 168, 18);
	static Rect modifiersInputRect = new Rect(383, 240, 107, 18);

	public static void ShortcutEditPanel()
	{
		// descriptionTitleRect = EditorGUI.RectField(new Rect(240,150,200,50), descriptionTitleRect);

		string keyString = defaultShortcuts[shortcutIndex].key.ToString();
	
		GUI.Label(keyRect, "Key");
		keyString = EditorGUI.TextField(keyInputRect, keyString);
		defaultShortcuts[shortcutIndex].key = pbUtil.ParseEnum(keyString, KeyCode.None);

		GUI.Label(modifiersRect, "Modifiers");
		defaultShortcuts[shortcutIndex].eventModifiers = 
			(EventModifiers)EditorGUI.EnumMaskField(modifiersInputRect, defaultShortcuts[shortcutIndex].eventModifiers);

		GUI.Label(descriptionTitleRect, "Description", EditorStyles.boldLabel);

		GUI.Label(descriptionRect, defaultShortcuts[shortcutIndex].description, EditorStyles.wordWrappedLabel);
	}

	public static void LoadPrefs()
	{
		defaultSelectionMode = EditorPrefs.GetString(pb_Constant.pbDefaultSelectionMode);
		_selectMode = pbUtil.ParseEnum(defaultSelectionMode, _selectMode);

		_faceColor = pb_Preferences_Internal.GetColor( pb_Constant.pbDefaultFaceColor );

		if(!EditorPrefs.HasKey( pb_Constant.pbDefaultOpenInDockableWindow))
			EditorPrefs.SetBool(pb_Constant.pbDefaultOpenInDockableWindow, true);

		defaultHideFaceMask = (EditorPrefs.HasKey(pb_Constant.pbDefaultHideFaceMask)) ? EditorPrefs.GetBool(pb_Constant.pbDefaultHideFaceMask) : false;
			
		defaultOpenInDockableWindow = EditorPrefs.GetBool(pb_Constant.pbDefaultOpenInDockableWindow);

		defaultColliderType = EditorPrefs.HasKey(pb_Constant.pbDefaultCollider) ? EditorPrefs.GetInt(pb_Constant.pbDefaultCollider) : 2;
		
		_forceConvex = EditorPrefs.HasKey(pb_Constant.pbForceConvex) ? EditorPrefs.GetBool(pb_Constant.pbForceConvex) : false;
		
		pbDragCheckLimit = EditorPrefs.HasKey(pb_Constant.pbDragCheckLimit) ? EditorPrefs.GetBool(pb_Constant.pbDragCheckLimit) : true;

		pbForceGridPivot = EditorPrefs.HasKey(pb_Constant.pbForceGridPivot) ? EditorPrefs.GetBool(pb_Constant.pbForceGridPivot) : false;

		if(EditorPrefs.HasKey(pb_Constant.pbDefaultMaterial))
		{
			_defaultMaterial = (Material) Resources.LoadAssetAtPath(pb_Constant.pbDefaultMaterial, typeof(Material));
			if(_defaultMaterial == null)
				_defaultMaterial = ProBuilder.DefaultMaterial;
		}

		defaultShortcuts = EditorPrefs.HasKey(pb_Constant.pbDefaultShortcuts) ? 
			pb_Shortcut.ParseShortcuts(EditorPrefs.GetString(pb_Constant.pbDefaultShortcuts)) : 
			pb_Shortcut.DefaultShortcuts();

		_showNotifications = EditorPrefs.HasKey(pb_Constant.pbShowEditorNotifications) ?
			EditorPrefs.GetBool(pb_Constant.pbShowEditorNotifications) : true;
	}

	public static void SetPrefs()
	{
		EditorPrefs.SetString	(pb_Constant.pbDefaultSelectionMode, _selectMode.ToString().ToLower());
		EditorPrefs.SetString	(pb_Constant.pbDefaultFaceColor, _faceColor.ToString());
		EditorPrefs.SetBool  	(pb_Constant.pbDefaultOpenInDockableWindow, defaultOpenInDockableWindow);
		EditorPrefs.SetBool  	(pb_Constant.pbDefaultHideFaceMask, defaultHideFaceMask);
		EditorPrefs.SetString	(pb_Constant.pbDefaultShortcuts, pb_Shortcut.ShortcutsToString(defaultShortcuts));
		EditorPrefs.SetString	(pb_Constant.pbDefaultMaterial, AssetDatabase.GetAssetPath(_defaultMaterial));	
		EditorPrefs.SetInt 		(pb_Constant.pbDefaultCollider, defaultColliderType);	
		EditorPrefs.SetBool  	(pb_Constant.pbShowEditorNotifications, _showNotifications);
		EditorPrefs.SetBool  	(pb_Constant.pbForceConvex, _forceConvex);
		EditorPrefs.SetBool  	(pb_Constant.pbDragCheckLimit, pbDragCheckLimit);
		EditorPrefs.SetBool  	(pb_Constant.pbForceGridPivot, pbForceGridPivot);

		// pb_Editor.instance.LoadPrefs();
	}
}
