using UnityEngine;

[System.Serializable]
public class StoryOption 
{
    [Tooltip("The text shown on this option")]
    public string OptionName = "Click";

    [Tooltip("If this is logged, show this option")]
    public string IlogRequiredKey;

    public IStoryElement GetGoTo => Goto;
    [Tooltip("Selecting this option will move to this scene")]
    public StoryElement Goto;

    [Tooltip("Set something to true in log")]
    public string IlogSaveKey;

}
