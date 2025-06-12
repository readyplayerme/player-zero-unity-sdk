using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PlayerZero.Editor
{
    public class EditableListElement : VisualElement
    {
        public IReadOnlyList<string> Items => _items;
        public Action OnUpdated; 

        private List<string> _items = new();
        private readonly string[] _choices;
        private readonly VisualElement _container;
        private readonly Button _addButton;

        public EditableListElement(string[] choices)
        {
            _choices = choices;

            style.flexDirection = FlexDirection.Column;

            _container = new VisualElement();
            _container.style.marginBottom = 4;
            _container.style.flexDirection = FlexDirection.Column;
            Add(_container);

            _addButton = new Button(() =>
            {
                AddItem(_choices[0]);
                OnUpdated?.Invoke();
            }) { text = "Add" };

            Add(_addButton);
        }

        public void SetInitialItems(List<string> items)
        {
            _items = new List<string>(items);
            _container.Clear();

            foreach (var item in _items.ToList())
                AddItemElement(item);
        }

        private void AddItem(string value)
        {
            _items.Add(value);
            AddItemElement(value);
        }

        private void AddItemElement(string currentValue)
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween
                }
            };

            var popup = new PopupField<string>(_choices.ToList(), currentValue);

            popup.RegisterValueChangedCallback(evt =>
            {
                var index = _container.IndexOf(row);
                if (index >= 0 && index < _items.Count)
                {
                    _items[index] = evt.newValue;
                    OnUpdated?.Invoke();
                }
            });

            var removeBtn = new Button(() =>
            {
                var index = _container.IndexOf(row);
                if (index >= 0 && index < _items.Count)
                {
                    _items.RemoveAt(index);
                    _container.Remove(row);
                    OnUpdated?.Invoke();
                }
            })
            { text = "X" };

            removeBtn.style.marginLeft = 4;
            removeBtn.style.minWidth = 20;

            row.Add(popup);
            row.Add(removeBtn);
            _container.Add(row);
        }
    }

}
