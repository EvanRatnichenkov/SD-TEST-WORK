

using UnityEditor;            

[CustomEditor(typeof(FMODStudioMaterialSetter))]                                                               
public class FMODStudioFootstepsEditor : Editor                                                               
{
    public override void OnInspectorGUI()                                                                      
    {
        var MS = target as FMODStudioMaterialSetter;                                                           
        var FPF = FindObjectOfType<FMODStudioFirstPersonFootsteps>();                                          

        MS.MaterialValue = EditorGUILayout.Popup("Set Material As", MS.MaterialValue, FPF.MaterialTypes);      
    }
}



[CustomEditor(typeof(FMODStudioFirstPersonFootsteps))] 
public class FMODStudioFootstepsEditorTwo : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();                                                                                                       

        var FPF = target as FMODStudioFirstPersonFootsteps;                                                                           

        FPF.DefulatMaterialValue = EditorGUILayout.Popup("Set Default Material As", FPF.DefulatMaterialValue, FPF.MaterialTypes);     
    }
}
