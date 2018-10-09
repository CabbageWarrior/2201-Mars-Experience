// Scripted by Roberto Leogrande

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace HintSystem {

	using System.Collections.Generic;

	public static class EditorInfo {

		public	const	string	ResPath				= "Assets/0_ProjectData/Resources/HintSys/";
		public	const	string	ResPathCycles		= "Assets/0_ProjectData/Resources/HintSys/Cycles/";
		public	const	string	ResPathTransitions	= "Assets/0_ProjectData/Resources/HintSys/Transitions/";
		public	const	string	ResPathOthers		= "Assets/0_ProjectData/Resources/HintSys/Others/";

	}

	public class HintTreeCreatorMgrWindow : EditorWindow {

		public static	HintTreeCreatorMgrWindow		pWindow				= null;

		private	HintCycleCollection						m_CycleCollection	= null;
		public	HintCycleCollection						CycleCollection {
			get { return m_CycleCollection; }
			set { m_CycleCollection = value; }
		}
		private Vector2									m_ScrollPosition	= Vector2.zero;
		private	string									m_FilterString		= "";

		[MenuItem ("Window/Hint System Editor")]
		private static void  Init () {

			if ( pWindow != null ) {
				pWindow.Close();
				pWindow = null;
			}
			
			if ( pWindow == null ) {
				pWindow = EditorWindow.GetWindow<HintTreeCreatorMgrWindow>( true, "Hint Sys Editor" );
				pWindow.minSize = new Vector2( 1000f, 400f );
			}
			pWindow.Show();

			if ( !System.IO.Directory.Exists( EditorInfo.ResPath ) )
				System.IO.Directory.CreateDirectory( EditorInfo.ResPath );

			if ( !System.IO.Directory.Exists( EditorInfo.ResPathCycles ) )
				System.IO.Directory.CreateDirectory( EditorInfo.ResPathCycles );

			if ( !System.IO.Directory.Exists( EditorInfo.ResPathTransitions ) )
				System.IO.Directory.CreateDirectory( EditorInfo.ResPathTransitions );

			if ( !System.IO.Directory.Exists( EditorInfo.ResPathOthers ) )
				System.IO.Directory.CreateDirectory( EditorInfo.ResPathOthers );

			AssetDatabase.Refresh();

			Setup();

		}


		//////////////////////////////////////////////////////////////////////////
		// Setup
		private static void Setup() {

			// Create asset if not exists
			if ( !System.IO.File.Exists( EditorInfo.ResPath + "HintsData.asset" ) ) {

				pWindow.m_CycleCollection = ScriptableObject.CreateInstance<HintCycleCollection>();
				AssetDatabase.CreateAsset( pWindow.m_CycleCollection, EditorInfo.ResPath + "HintsData.asset" );
				AssetDatabase.SaveAssets();

			}
			// else load data
			else {
				
				pWindow.m_CycleCollection = AssetDatabase.LoadAssetAtPath<HintCycleCollection>( EditorInfo.ResPath + "HintsData.asset" );

			}

			pWindow.m_CycleCollection.Create();

			EditorUtility.SetDirty( pWindow.m_CycleCollection );

		}


		//////////////////////////////////////////////////////////////////////////
		// DRAW TREE
		private	void	OnGUI () {

			// SANITY CHECK
			if ( m_CycleCollection == null )
			{
				Setup();
				return;
			}

			 
			// NEW HINT ROOM BUTTON
			if ( GUILayout.Button( "Create new Hint Cycle" ) )
			{
				NewHintCycleWindow.Init();
			}

			if ( m_CycleCollection.Count == 0 )
				return;
			{
				GUIStyle thisStyle = new GUIStyle();
				thisStyle.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label( "Name filter", thisStyle );
			}
			m_FilterString = GUILayout.TextField( m_FilterString );

			{
				GUIStyle thisStyle = new GUIStyle();
				thisStyle.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label( "Hint Cylces", thisStyle );
			}
			m_ScrollPosition = GUILayout.BeginScrollView( m_ScrollPosition );
			{
				GUILayout.BeginVertical();
				{
					// DRAW CYCLES
					for ( int i = 0; i < m_CycleCollection.Count; i++ )
					{
						HintCycle hintCycle = m_CycleCollection[ i ];

						if ( m_FilterString.Length > 0 && !hintCycle.Name.ToLower().Contains(m_FilterString.ToLower()) )
						{
							continue;
						}

						// remove null references
						if ( hintCycle == null )
						{
							m_CycleCollection.RemoveAt(i);
							GUILayout.EndHorizontal();
							GUILayout.EndScrollView();
							return;
						}

						GUILayout.BeginVertical();
						{
							// CYCLE NAME LABEL
							{
								GUIStyle thisStyle = new GUIStyle();
								thisStyle.alignment = TextAnchor.MiddleCenter;
								string cycleName = hintCycle.name + ( hintCycle.IsTransition ? "     TRANSITION" : "");
								GUILayout.Label( cycleName, thisStyle, GUILayout.Width( 400f ), GUILayout.MaxWidth( 400f ), GUILayout.ExpandWidth(false) );
							}

							// CYCLE TRANSITION REMOVE BUTTON
							if ( hintCycle.IsTransition )
							{
								GUILayout.BeginHorizontal();
								{
									if ( GUILayout.Button( "Edit Hint Group", GUILayout.MaxWidth( 100f ), GUILayout.ExpandWidth(false) ) )
									{
										EditHintGroupWindow.Init( ( hintCycle.Childs[0] as HintPuzzle ).Childs[0] as HintGroup );
									}
									if ( GUILayout.Button( "Remove", GUILayout.MaxWidth( 300f ), GUILayout.ExpandWidth(false) ) )
									{
										ClearHintCycle( hintCycle );

										string assetPath = hintCycle.Path;
										m_CycleCollection.RemoveAt( i );
										AssetDatabase.DeleteAsset( assetPath );
									}
								}
								GUILayout.EndHorizontal();

								GUILayout.EndVertical();
								continue;
							}

							// ROOM BUTTONS
							GUILayout.BeginHorizontal();
							{

								// NEW HINT CYCLE BUTTON
								GUILayout.Space( 100f );
								if ( GUILayout.Button( "New Hint Puzzle", GUILayout.MaxWidth( 100f ), GUILayout.ExpandWidth(false) ) )
								{	
									NewHintPuzzleWindow.Init( hintCycle );
								}

								// REMOVE HINT ROOM BUTTON
								if ( GUILayout.Button( "Remove", GUILayout.MaxWidth( 60f ), GUILayout.ExpandWidth(false) ) )
								{
									ClearHintCycle( hintCycle );

									string assetPath = hintCycle.Path;
									m_CycleCollection.RemoveAt( i );
									AssetDatabase.DeleteAsset( assetPath );
									GUILayout.EndHorizontal();
									break;
								}


							}
							// CYCLES BUTTONS END
							GUILayout.EndHorizontal();

							// HINT PUZZLE LABEL
							if ( hintCycle.Childs.Count > 0 ) {
								GUIStyle thisStyle = new GUIStyle();
								thisStyle.alignment = TextAnchor.MiddleCenter;
								GUILayout.Label( "Hint puzzles", thisStyle, GUILayout.Width( 400f ), GUILayout.MaxWidth( 400f ), GUILayout.ExpandWidth(false) );
							}


							// DRAW HINT PUZZLE
							for ( int j = 0; j < hintCycle.Childs.Count; j++ )
							{
								HintPuzzle hintPuzzle = hintCycle.Childs[ j ] as HintPuzzle;

								// HINT PUZZLE BUTTONS
								GUILayout.BeginHorizontal();
								{
									GUILayout.Space( 20f );
									{	// NAME LABEL
										GUILayout.Label( hintPuzzle.Name, GUILayout.Width( 200f ), GUILayout.MaxWidth( 200f ), GUILayout.ExpandWidth(false) );
									}

									// NEW HINT PUZZLE BUTTON
									if ( GUILayout.Button( "New hint Group", GUILayout.MaxWidth( 100f ), GUILayout.ExpandWidth(false) ) )
									{
										NewHintGroupWindow.Init( hintPuzzle );
									}

									// REMOVE HINT PUZZLE BUTTON
									if ( GUILayout.Button( "Remove", GUILayout.MaxWidth( 60f ), GUILayout.ExpandWidth(false) ) )
									{
										ClearHintPuzzle( hintPuzzle );

										string assetPath = hintPuzzle.Path;
										hintCycle.Childs.RemoveAt( j );
										AssetDatabase.DeleteAsset( assetPath );

										GUILayout.EndHorizontal();
										break;
									}

									{
										GUIStyle thisStyle = new GUIStyle();
										thisStyle.alignment = TextAnchor.MiddleRight;
										GUILayout.Label ( "Father", thisStyle );
									}
									EditorGUILayout.ObjectField( hintPuzzle.Father, typeof( HintCommon ), true, GUILayout.MaxWidth( 400f ) );

								}
								// HINT CYCLE BUTTONS END
								GUILayout.EndHorizontal();


								// HINT PUZZLE BUTTONS
								GUILayout.BeginVertical();
								{
									// DRAW HINT GROUPS
									for ( int k = 0; k < hintPuzzle.Childs.Count; k++ )
									{
										HintGroup hintGroup = hintPuzzle.Childs[ k ] as HintGroup;

										GUILayout.BeginHorizontal();
										{
											GUILayout.Space( 100f );
											GUILayout.Label( hintGroup.Name, GUILayout.Width( 140f ), GUILayout.MaxWidth( 140f ), GUILayout.ExpandWidth(false) );
										
											// EDIT HINT GROUP
											if ( GUILayout.Button( "Edit Hint Group", GUILayout.MaxWidth( 100f ), GUILayout.ExpandWidth(false) ) )
											{
												EditHintGroupWindow.Init( hintGroup );
											}

											// REMOVE HINT PUZZLE
											if ( GUILayout.Button( "Remove", GUILayout.MaxWidth( 60f ), GUILayout.ExpandWidth(false) ) )
											{
												ClearHintGroup( hintGroup );

												string assetPath = hintGroup.Path;
												hintPuzzle.Childs.RemoveAt( k );
												AssetDatabase.DeleteAsset( assetPath );

												GUILayout.EndHorizontal();
												break;
											}

											{
												GUIStyle thisStyle = new GUIStyle();
												thisStyle.alignment = TextAnchor.MiddleRight;
												GUILayout.Label ( "Father", thisStyle );
											}
											EditorGUILayout.ObjectField( hintGroup.Father, typeof( HintCommon ), true, GUILayout.MaxWidth( 400f ) );

										}
										GUILayout.EndHorizontal();

									}
								}
								GUILayout.EndVertical();
								// GROUP BUTTONS END


								GUILayout.Label( "------------------------------------------------------------" );
							}


						}
						GUILayout.EndVertical();

					}

				}
				GUILayout.EndVertical();

			}
			GUILayout.EndScrollView();


		}
		

		//////////////////////////////////////////////////////////////////////////
		// ClearHintGroup
		private void	ClearHintGroup( HintGroup hintGroup ) {

			for ( int i = 0; i < hintGroup.Childs.Count; i++ ) {

				Hint hint = hintGroup.Childs[ i ] as Hint;
				AssetDatabase.DeleteAsset( hint.Path );
				hintGroup.Childs[ i ] = null;
			}

			hintGroup.Childs = null;

		}


		//////////////////////////////////////////////////////////////////////////
		// ClearHintPuzzle
		private	void	ClearHintPuzzle( HintPuzzle hintPuzzle ) {

			foreach ( HintGroup g in hintPuzzle.Childs ) {

				ClearHintGroup( g );
				AssetDatabase.DeleteAsset( g.Path );

			}

			hintPuzzle.Childs.Clear();

		}


		//////////////////////////////////////////////////////////////////////////
		// ClearHintCycle
		private void	ClearHintCycle( HintCycle hintCycle ) {

			foreach ( HintPuzzle p in hintCycle.Childs ) {

				ClearHintPuzzle( p );
				p.Childs.Clear();
				AssetDatabase.DeleteAsset( p.Path );

			}

			hintCycle.Childs.Clear();

		}


		private void OnDestroy() {

			if (EditHintGroupWindow.pWindow)	EditHintGroupWindow.pWindow.Close();

			if ( NewHintGroupWindow.pWindow )	NewHintGroupWindow.pWindow.Close();
			if ( NewHintCycleWindow.pWindow )	NewHintCycleWindow.pWindow.Close();
			if ( NewHintPuzzleWindow.pWindow )	NewHintPuzzleWindow.pWindow.Close();
			

			foreach( var cycle in m_CycleCollection ) {

				foreach( var puzzle in cycle.Childs ) {

					foreach( var group in puzzle.Childs ) {

						foreach( var hint in group.Childs ) {

							EditorUtility.SetDirty( hint );

						}
						EditorUtility.SetDirty( group );

					}
					EditorUtility.SetDirty( puzzle );

				}
				EditorUtility.SetDirty( cycle );
			}


			// force data saving
			EditorUtility.SetDirty( m_CycleCollection );
			AssetDatabase.SaveAssets();

		}

	}



}

#endif