using System;
using System.Windows;
using System.Collections;
using System.Windows.Controls;

namespace Jump
{
    public class ListBoxSelectedItemsBehavior
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(ListBoxSelectedItemsBehavior),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBox_SelectionChanged;

                if (e.NewValue != null)
                {
                    listBox.SelectionChanged += ListBox_SelectionChanged;
                }
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var boundList = GetSelectedItems(listBox);

            if (boundList == null)
                return;

            boundList.Clear();
            foreach (var item in listBox.SelectedItems)
            {
                boundList.Add(item);
            }
        }
    }
}
