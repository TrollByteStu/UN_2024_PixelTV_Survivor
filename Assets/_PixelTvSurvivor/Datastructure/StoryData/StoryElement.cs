using System.Collections.Generic;
using UnityEngine;

public enum StoryElementType
{
    Message,
    MessageWithOptions,
    MessageWithImage,
    MessageWithImageAndOptions,
}

public interface IStoryElement
{
    StoryElementType Type { get; }
    List<StoryOption> Options { get; }
}

[CreateAssetMenu(fileName = "StoryElement", menuName = "ScriptableObjects/StoryElement", order = 1)]
public class StoryElement : ScriptableObject, IStoryElement
{
    [Tooltip("The Title at the top of the screen")]
    public string ElementTitle;

    [Tooltip("The breadtext to describe what is happening")]
    [TextArea(15, 20)]
    public string SceneBreadText;

    [Tooltip("The Image shown")]
    public Sprite Image;

    public List<StoryOption> Options => options;
    [Tooltip("The List of options you give the user")]
    [SerializeField] private List<StoryOption> options;

    public StoryElementType Type => type;
    [SerializeField] private StoryElementType type;
}
