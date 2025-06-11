using System;
using System.Collections.Generic;
using PlayerZero.Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerZero.Editor
{
    public class EditableList
    {
        private const string REMOVE_BUTTON_TEXT = "X";
        private VisualElement elementContainer;
        private List<Label> choiceLabels;
        private Dictionary<VisualElement, string> elementToValueMap;

        public EditableList(string[] choices, VisualElement externalContainer = null)
        {
            Initialize(choices);
        }
        
        public void Initialize(string[] choices, VisualElement externalContainer = null)
        {
            elementContainer = externalContainer ?? new VisualElement();

            choiceLabels = new List<Label>();
            foreach (var choice in choices)
            {
                choiceLabels.Add(new Label(choice));
            }

            elementToValueMap = new Dictionary<VisualElement, string>();
        }
        
        public VisualElement GetRoot()
        {
            return elementContainer;
        }
        
        public VisualElement CreateNewElement(int popFieldDefaultIndex, Func<Label, string> formatSelectedValueCallback = null, Action<string> onItemRemoved = null)
        {
            var newElement = new VisualElement();
            newElement.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            newElement.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
            elementToValueMap.Add(newElement, AvatarMorphTargets.MorphTargetNames[popFieldDefaultIndex]);
            newElement.Add(CreatePopupField(popFieldDefaultIndex, newElement, formatSelectedValueCallback));
            newElement.Add(CreateRemoveButton(newElement, onItemRemoved));
            elementContainer.Add(newElement);
            elementContainer.MarkDirtyRepaint();
            return newElement;
        }

        private PopupField<Label> CreatePopupField(int defaultIndex, VisualElement parent,  Func<Label,string> formatSelectedValueCallback = null)
        {
            return new PopupField<Label>(string.Empty,
                choiceLabels,
                defaultIndex,
                x =>
                {
                    if (formatSelectedValueCallback != null)
                    {
                        formatSelectedValueCallback(x);
                    }
                    elementToValueMap[parent] = x.text;
                    return x.text;
                },
                x => x.text);
        }

        private VisualElement CreateRemoveButton(VisualElement parent, Action<string> onItemRemoved = null)
        {
            var removeButton = new Button(() =>
            {
                if(onItemRemoved != null)
                {
                    onItemRemoved.Invoke(elementToValueMap[parent]);
                }
                elementToValueMap.Remove(parent);
                elementContainer.Remove(parent);
            });
            removeButton.text = REMOVE_BUTTON_TEXT;
            elementContainer.MarkDirtyRepaint();
            return removeButton;
        }
    }
}
