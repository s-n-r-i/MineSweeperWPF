using Microsoft.Xaml.Behaviors;
using MineSweeperWPF.Models;
using MineSweeperWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MineSweeperWPF.Views;

public class OpenAction : TriggerAction<FrameworkElement>
{
    protected override void Invoke(object parameter)
    {
        var panel = AssociatedObject as StackPanel ?? throw new Exception();

        if (((DependencyPropertyChangedEventArgs)parameter).NewValue is not OpenRequest request)
        {
            return;
        }

        // セルオープンリクエストで指定されたセルに対応するTggleButtonの状態を変更する
        foreach (var cell in request.Cells)
        {
            if (GetTargetToggleButton(panel, cell) is { } toggleButton)
            {
                toggleButton.Content = cell.IsBomb ? "*" : cell.NeighborBombCount.ToString();
                toggleButton.IsEnabled = false;
                toggleButton.IsChecked = true;
            }
        }
    }

    /// <summary>
    /// StackPanelからCell情報に合致するToggleButtonを取得する
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    private static ToggleButton? GetTargetToggleButton(StackPanel panel, Cell cell)
    {
        var rowPanels = LogicalTreeHelper.GetChildren(panel).OfType<StackPanel>().ToArray();
        foreach (var rowPanel in rowPanels)
        {
            var toggleButtons = LogicalTreeHelper.GetChildren(rowPanel).OfType<ToggleButton>().ToArray();
            if (toggleButtons.SingleOrDefault(x => x.CommandParameter.ToString() == cell.Index.ToString()) is { } toggleButton)
            {
                return toggleButton;
            }
        }

        return null;
    }
}
