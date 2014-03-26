namespace Sitecore.Rocks.ContentTrees.Commands.Editing
{
  using System;
  using System.Linq;
  using Sitecore.VisualStudio;
  using Sitecore.VisualStudio.Commands;
  using Sitecore.VisualStudio.ContentTrees;
  using Sitecore.VisualStudio.ContentTrees.Items;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Sites;

  /// <summary>
  /// Class CopyShortIdToClipboard.
  /// </summary>
  [Command]
  public class DuplicateLatestVersionOnly : CommandBase
  {
    #region Constructors and Destructors

    /// <summary>
    /// </summary>
    public DuplicateLatestVersionOnly()
    {
      this.Text = "Duplicate Latest Version Only";
      this.Group = "Edit";
      this.SortingValue = 2001;
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public override bool CanExecute(object parameter)
    {
      var context = parameter as ContentTreeContext;
      if (context == null)
      {
        return false;
      }

      if (context.SelectedItems.Count() != 1)
      {
        return false;
      }

      var item = context.SelectedItems.FirstOrDefault() as ItemTreeViewItem;
      if (item == null)
      {
        return false;
      }

      return true;
    }

    /// <summary>The execute.</summary>
    /// <param name="parameter">The parameter.</param>
    public override void Execute(object parameter)
    {
      var context = parameter as ContentTreeContext;
      if (context == null)
      {
        return;
      }

      var item = context.SelectedItems.FirstOrDefault() as ItemTreeViewItem;
      if (item == null)
      {
        return;
      }

      var newName = string.Format(Resources.SetName_Process_Copy_of__0_, item.Item.Name);
      newName = AppHost.Prompt(Resources.SetName_Process_Enter_the_Name_of_the_New_Item_, Resources.SetName_Process_Duplicate, newName);
      if (newName == null)
      {
        return;
      }

      Site.RequestCompleted completed = delegate(string response)
      {
        var newItemUri = new ItemUri(item.ItemUri.DatabaseUri, new ItemId(new Guid(response)));
        var itemVersionUri = new ItemVersionUri(newItemUri, LanguageManager.CurrentLanguage, VisualStudio.Data.Version.Latest);

        Notifications.RaiseItemDuplicated(this, newItemUri, item.ItemUri);

        if (AppHost.CurrentContentTree != null)
        {
          AppHost.CurrentContentTree.Locate(newItemUri);
        }

        AppHost.OpenContentEditor(itemVersionUri);
      };

      item.ItemUri.Site.Execute("Items.DuplicateLatestVersionOnly", completed, item.ItemUri.DatabaseName.ToString(), item.ItemUri.ItemId.ToString(), Language.Current.ToString(), newName);
    }

    #endregion
  }
}