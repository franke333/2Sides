using UnityEngine;

public interface IInteractable
{
    void HoverOver();
    void HoverOut();

    //value is true if left click, false if right click
    void InteractView(bool value);
    void Touch(bool value, Transform holdPos);
}
