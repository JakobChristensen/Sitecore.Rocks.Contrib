// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportExportSettingsCommand.cs" company="Sitecore A/S">
//   Copyright (C) 2010 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the view command class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.VisualStudio.UI.ImportExportSettings
{
  using Sitecore.VisualStudio.Commands;
  using Sitecore.VisualStudio.Shell;

  /// <summary>Defines the view command class.</summary>
  [Command]
  [ShellMenuCommand(ShellMenuCommandPlacement.MainMenu, 1000)]
  [MenuCommand("Tools", "Plugins", 5100)]
  public class ImportExportSettingsCommand : CommandBase
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportExportSettingsCommand"/> class.
    /// </summary>
    public ImportExportSettingsCommand()
    {
      this.Text = "Import and Export Settings...";
    }

    #endregion

    #region Public Methods

    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public override bool CanExecute(object parameter)
    {
      return parameter is ShellContext;
    }

    /// <summary>The execute.</summary>
    /// <param name="parameter">The parameter.</param>
    public override void Execute(object parameter)
    {
      var d = new ImportExportWindow();
      d.ShowDialog();
    }

    #endregion
  }
}