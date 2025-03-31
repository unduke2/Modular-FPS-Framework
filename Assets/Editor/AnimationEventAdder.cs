using UnityEngine;
using UnityEditor;

public class AnimationEventAdder
{
    [MenuItem("Tools/Add ReloadComplete Event")]
    static void AddReloadCompleteEvent()
    {
        AnimationClip clip = Selection.activeObject as AnimationClip;
        if (clip == null)
        {
            Debug.Log("Please select an animation clip.");
            return;
        }

        // Create a new event
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "ReloadComplete";
        // Set the event time – for example, near the end of the clip:
        evt.time = clip.length - 0.1f;

        // Get any existing events, add our new event, and set them back.
        AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);
        UnityEditor.ArrayUtility.Add(ref events, evt);
        AnimationUtility.SetAnimationEvents(clip, events);

        Debug.Log("ReloadComplete event added to clip: " + clip.name);
    }
}
