#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 参考 : Unity ReorderableList
/// </summary>
[CustomEditor( typeof( BattleBehaviorGroup ) )]
public class BattleBehaviorGroupEditor : Editor
{
	private List<ReorderableList> m_ListFields;
	private bool m_IsShowingSort;

	private void OnEnable()
	{
		m_ListFields = InitSortableList( target.GetType(), serializedObject );
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField( "Expansion" );

		m_IsShowingSort = EditorGUILayout.Toggle( "Is Showing Sorting List", m_IsShowingSort );

		if( m_IsShowingSort )
		{
			foreach( var l in m_ListFields )
			{
				l.DoLayoutList();
			}
		}
	}

	/// <summary>
	/// 指定したクラスのフィールドからリストや配列を検出してReorderableListのリストを取得する。
	/// </summary>
	private static List<ReorderableList> InitSortableList( Type type, SerializedObject serializedObject )
	{
		List<Type> types = new List<Type>();
		types.Add( type );
		Type t = type.BaseType;

		while( t != null )
		{
			types.Add( t );
			t = t.BaseType;
		}

		List<FieldInfo> fieldList = new List<FieldInfo>();

		foreach( var tt in types )
		{
			var fields = tt.GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField );
			fieldList.AddRange( fields );
		}

		var collectionLists = new List<ReorderableList>();

		foreach( var f in fieldList )
		{
			var prop = serializedObject.FindProperty( f.Name );

			Type fType = f.FieldType;
			bool isCollection = false;

			if( fType.IsArray )
			{
				isCollection = true;
			}
			else if( fType.IsGenericType && typeof( List<> ).IsAssignableFrom( fType.GetGenericTypeDefinition() ) )
			{
				isCollection = true;
			}

			if( isCollection )
			{
				collectionLists.Add( new ReorderableList( serializedObject, prop ) );
			}
		}

		foreach( var l in collectionLists )
		{
			SetupCallback( l );
		}

		return collectionLists;
	}

	/// <summary>
	/// ReorderableListの初期化。
	/// </summary>
	private static void SetupCallback( ReorderableList list )
	{
		if( list == null )
		{
			return;
		}

		var prop = list.serializedProperty;

		list.drawElementCallback = ( rect, idx, isActive, isFocused ) =>
		{
			var element = prop.GetArrayElementAtIndex( idx );
			EditorGUI.PropertyField( rect, element );
		};
		list.drawHeaderCallback = ( rect ) =>
		{
			EditorGUI.LabelField( rect, prop.displayName );
		};
		// フッターは操作できないようにする
		list.drawFooterCallback = rect => {;};
	}
}

#endif