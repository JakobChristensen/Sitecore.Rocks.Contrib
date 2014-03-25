namespace Sitecore.Rocks.ContentTrees.Commands.Editing
{
  using System.Linq;
  using Sitecore.VisualStudio.Commands;
  using Sitecore.VisualStudio.ContentTrees.Commands.Editing;
  using Sitecore.VisualStudio.Data;

  /// <summary>
  /// Class CopyShortIdToClipboard.
  /// </summary>
  [Command(Submenu = EditSubmenu.Name)]
  public class CopyShortIdToClipboard : CommandBase
  {
    #region Constructors and Destructors

    /// <summary>
    /// </summary>
    public CopyShortIdToClipboard()
    {
      this.Text = "Copy Item Short ID";
      this.Group = "clipboard";
      this.SortingValue = 2011;
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public override bool CanExecute(object parameter)
    {
      var selection = parameter as IItemSelectionContext;
      if (selection == null)
      {
        return false;
      }

      return selection.Items.Count() == 1;
    }

    /// <summary>The execute.</summary>
    /// <param name="parameter">The parameter.</param>
    public override void Execute(object parameter)
    {
      var selection = parameter as IItemSelectionContext;
      if (selection == null)
      {
        return;
      }

      var item = selection.Items.FirstOrDefault();
      if (item == null)
      {
        return;
      }

      var itemId = item.ItemUri.ItemId.ToString();

      itemId = itemId.Substring(1, 8) + itemId.Substring(10, 4) + itemId.Substring(15, 4) + itemId.Substring(20, 4) + itemId.Substring(25, 12);

      AppHost.Clipboard.SetText(itemId);
    }

    #endregion
  }
}