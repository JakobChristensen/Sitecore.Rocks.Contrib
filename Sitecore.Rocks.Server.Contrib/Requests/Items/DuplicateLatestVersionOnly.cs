namespace Sitecore.Rocks.Server.Requests.Items
{
  using Sitecore.Configuration;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Class DuplicateLatestVersionOnly.
  /// </summary>
  public class DuplicateLatestVersionOnly
  {
    #region Public Methods and Operators

    /// <summary>
    /// Executes the specified database name.
    /// </summary>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="languageName">Name of the language.</param>
    /// <param name="newName">The new name.</param>
    /// <returns>System.String.</returns>
    public string Execute(string databaseName, string itemId, string languageName, string newName)
    {
      var database = Factory.GetDatabase(databaseName);
      Assert.IsNotNull(database, "Database not found");

      var language = LanguageManager.GetLanguage(languageName);
      Assert.IsNotNull(language, "Language not found");

      var item = database.GetItem(itemId, language);
      Assert.IsNotNull(database, "Item not found");

      var newItem = item.Duplicate(newName);

      foreach (var version in newItem.Versions.GetVersions(true))
      {
        if (version.Language != language || !version.Versions.IsLatestVersion())
        {
          version.Versions.RemoveVersion();
        }
      }

      return newItem.ID.ToString();
    }

    #endregion
  }
}