namespace Sitecore.Rocks.ContentTrees.Commands.Media
{
  using System;
  using System.Linq;
  using Microsoft.Win32;
  using Sitecore.VisualStudio.Commands;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Media;

  /// <summary>Defines the content tree command class.</summary>
  [Command]
  public class UploadMediaFiles : CommandBase
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadMediaFiles"/> class.
    /// </summary>
    public UploadMediaFiles()
    {
      this.Text = "Upload Media Files...";
      this.Group = "Media";
      this.SortingValue = 100;
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public override bool CanExecute(object parameter)
    {
      var context = parameter as IItemSelectionContext;
      if (context == null)
      {
        return false;
      }

      if (context.Items.Count() != 1)
      {
        return false;
      }

      var item = context.Items.First();
      if (string.Compare(item.Name, "Media Library", StringComparison.InvariantCultureIgnoreCase) != 0)
      {
        return false;
      }

      if (!item.ItemUri.Site.DataService.CanExecuteAsync("Media.Upload"))
      {
        return false;
      }

      return true;
    }

    /// <summary>Execute the command.</summary>
    /// <param name="parameter">The parameter.</param>
    public override void Execute(object parameter)
    {
      var context = parameter as IItemSelectionContext;
      if (context == null)
      {
        return;
      }

      var item = context.Items.First();

      var dialog = new OpenFileDialog
      {
        Title = "Upload files", 
        CheckFileExists = true, 
        Filter = @"All files|*.*", 
        Multiselect = true
      };

      if (dialog.ShowDialog() != true)
      {
        return;
      }

      GetValueCompleted<ItemHeader> uploadCompleted = delegate(ItemHeader value)
      {
        var itemVersionUri = new ItemVersionUri(value.ItemUri, LanguageManager.CurrentLanguage, VisualStudio.Data.Version.Latest);

        AppHost.OpenContentEditor(itemVersionUri);
      };

      MediaManager.Upload(item.ItemUri.DatabaseUri, @"/upload", dialog.FileNames, uploadCompleted);
    }

    #endregion
  }
}