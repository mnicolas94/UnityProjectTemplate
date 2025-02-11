// ----------------------------------------------------------------------------------------------
// <auto-generated>
// This code was auto-generated.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using SimpleDataEditor.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Audio;

namespace SimpleDataEditor.Generated
{
    public partial class SoundReferenceEditorWindow : DataTypeEditorWindow<SoundReference>
    {
        [MenuItem("Window/Data/Sound References")]
        private static void ShowWindow()
        {
            var window = GetWindow<SoundReferenceEditorWindow>();
            window.titleContent = new GUIContent("Sounds");
            window.Show();
        }
        
        // override in partial class for custom behaviour
        protected override List<SoundReference> LoadData()
        {
            return base.LoadData();
        }

        // override in partial class for custom behaviour
        protected override void BindDataToView(VisualElement element, int i)
        {
            base.BindDataToView(element, i);
        }

        // override in partial class for custom behaviour
        protected override VisualElement CreateDataElement()
        {
            return base.CreateDataElement();
        }
    }
}
