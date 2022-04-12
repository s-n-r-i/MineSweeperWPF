using Microsoft.Xaml.Behaviors;
using MineSweeperWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace MineSweeperWPF.Views;

/// <summary>
/// 盤面作成アクション
/// </summary>
public class StageCreateAction : TriggerAction<FrameworkElement>
{
    protected override void Invoke(object parameter)
    {
        var panel = AssociatedObject as StackPanel ?? throw new Exception();
        
        // StackPanelの現在の子アイテムを消去
        panel.Children.Clear();

        if (((DependencyPropertyChangedEventArgs)parameter).NewValue is not StartRequest request)
        {
            return;
        }

        // StackPanel配下に行に相当する子StackPanelを作成し、その配下に列に相当するToggleButtonを作成する
        var index = 0;
        for (var rowNum = 0; rowNum < request.RowCount; rowNum++)
        {
            var row = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Height = 20,
            };

            for (var columnNum = 0; columnNum < request.ColumnCount; columnNum++)
            {
                var button = new ToggleButton
                {
                    Width = 20,
                    Height = 20,
                    CommandParameter = $"{index++}",
                };
                button.SetBinding(ToggleButton.CommandProperty, new Binding("OpenCommand"));
                row.Children.Add(button);
            }

            panel.Children.Add(row);
        }
    }
}
