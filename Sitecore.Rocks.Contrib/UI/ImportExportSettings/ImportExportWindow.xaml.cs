// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportExportWindow.xaml.cs" company="Sitecore A/S">
//   Copyright (C) 2010 by Sitecore A/S
// </copyright>
// <summary>
//   Interaction logic for ImportExportWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.VisualStudio.UI.ImportExportSettings
{
  using System;
  using System.Text;
  using System.Windows;
  using System.Xml;
  using System.Xml.Linq;
  using Microsoft.Win32;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Applications;
  using Sitecore.VisualStudio.Applications.Storages;
  using Sitecore.VisualStudio.Diagnostics;
  using Sitecore.VisualStudio.Extensions.XElementExtensions;

  /// <summary>Interaction logic for ImportExportWindow.xaml</summary>
  public partial class ImportExportWindow
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ImportExportWindow"/> class.</summary>
    public ImportExportWindow()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    /// <summary>Exports the specified file name.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void Export([NotNull] string fileName)
    {
      Debug.ArgumentNotNull(fileName, "fileName");

      var output = new XmlTextWriter(fileName, Encoding.UTF8)
      {
        Indentation = 2, 
        IndentChar = ' ', 
        Formatting = Formatting.Indented
      };

      output.WriteStartElement("settings");

      this.ExportOptions(output);
      this.ExportStorage(output);

      output.WriteEndElement();

      output.Flush();

      output.Close();
    }

    /// <summary>Exports the click.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void ExportClick([NotNull] object sender, [NotNull] RoutedEventArgs e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      var fileName = Storage.Read("ImportExportSettings", "Path", string.Empty) as string ?? string.Empty;
      if (string.IsNullOrEmpty(fileName))
      {
        fileName = "Settings.xml";
      }

      var dialog = new SaveFileDialog
      {
        Title = "Export Settings", 
        CheckPathExists = true, 
        OverwritePrompt = true, 
        FileName = fileName, 
        DefaultExt = ".xml", 
        Filter = "Xml documents (.xml)|*.xml|All files|*.*"
      };

      if (dialog.ShowDialog() != true)
      {
        return;
      }

      fileName = dialog.FileName;
      Storage.Write("ImportExportSettings", "Path", fileName);

      this.Export(fileName);

      this.Close();
    }

    /// <summary>Exports the options.</summary>
    /// <param name="output">The output.</param>
    private void ExportOptions([NotNull] XmlTextWriter output)
    {
      Debug.ArgumentNotNull(output, "output");

      output.WriteStartElement("options");

      var options = AppHost.Settings.Options;

      var properties = options.GetType().GetProperties();

      foreach (var property in properties)
      {
        if (!property.PropertyType.Equals(typeof(bool)) && !property.PropertyType.Equals(typeof(string)))
        {
          continue;
        }

        output.WriteStartElement("option");
        output.WriteAttributeString("name", property.Name);
        output.WriteAttributeString("type", property.PropertyType.Name);
        output.WriteValue(property.GetValue(options, null));
        output.WriteEndElement();
      }

      output.WriteEndElement();
    }

    /// <summary>Exports the registry.</summary>
    /// <param name="output">The output.</param>
    private void ExportStorage([NotNull] XmlTextWriter output)
    {
      Debug.ArgumentNotNull(output, "output");

      output.WriteStartElement("storage");

      this.ExportStorage(output, string.Empty);

      output.WriteEndElement();
    }

    /// <summary>Exports the storage.</summary>
    /// <param name="output">The output.</param>
    /// <param name="path">The path.</param>
    private void ExportStorage([NotNull] XmlTextWriter output, [NotNull] string path)
    {
      Debug.ArgumentNotNull(output, "output");
      Debug.ArgumentNotNull(path, "path");

      foreach (var key in Storage.GetKeys(path))
      {
        output.WriteStartElement("key");
        output.WriteAttributeString("name", key);

        var obj = Storage.Read(path, key, null);
        if (obj == null)
        {
          output.WriteAttributeString("null", "true");
        }
        else
        {
          output.WriteValue(obj.ToString());
        }

        output.WriteEndElement();
      }

      foreach (var subpath in Storage.GetSubPaths(path))
      {
        if (subpath == "System")
        {
          continue;
        }

        output.WriteStartElement("path");
        output.WriteAttributeString("name", subpath);

        this.ExportStorage(output, path + "\\" + subpath);

        output.WriteEndElement();
      }
    }

    /// <summary>Imports the specified file name.</summary>
    /// <param name="fileName">Name of the file.</param>
    private void Import([NotNull] string fileName)
    {
      Debug.ArgumentNotNull(fileName, "fileName");

      XDocument doc;
      try
      {
        doc = XDocument.Load(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      var root = doc.Root;
      if (root == null)
      {
        return;
      }

      var optionsElement = root.Element("options");
      if (optionsElement != null)
      {
        this.ImportOptions(optionsElement);
      }

      var storageElement = root.Element("storage");
      if (storageElement != null)
      {
        this.ImportStorage(storageElement, string.Empty);
      }
    }

    /// <summary>
    /// Imports the storage.
    /// </summary>
    /// <param name="storageElement">The storage element.</param>
    /// <param name="path">The path.</param>
    private void ImportStorage([NotNull] XElement storageElement, [NotNull] string path)
    {
      Debug.ArgumentNotNull(storageElement, "storageElement");
      Debug.ArgumentNotNull(path, "path");

      foreach (var element in storageElement.Elements("key"))
      {
        var key = element.GetAttributeValue("name");
        var value = element.Value;

        Storage.Write(path, key, value);
      }

      foreach (var element in storageElement.Elements("path"))
      {
        var name = element.GetAttributeValue("name");
        this.ImportStorage(element, path + "\\" + name);
      }
    }

    /// <summary>Imports the click.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void ImportClick([NotNull] object sender, [NotNull] RoutedEventArgs e)
    {
      Debug.ArgumentNotNull(sender, "sender");
      Debug.ArgumentNotNull(e, "e");

      var fileName = Storage.Read("ImportExportSettings", "Path", string.Empty) as string ?? string.Empty;
      if (string.IsNullOrEmpty(fileName))
      {
        fileName = "Settings.xml";
      }

      var dialog = new OpenFileDialog
      {
        Title = "Import Settings", 
        CheckFileExists = true, 
        DefaultExt = ".xml", 
        FileName = fileName, 
        Filter = "Xml documents|*.xml|All files|*.*"
      };

      if (dialog.ShowDialog() != true)
      {
        return;
      }

      fileName = dialog.FileName;
    
      Storage.Write("ImportExportSettings", "Path", fileName);

      this.Import(fileName);

      AppHost.Settings.Options.Save();
      var contentTree = ActiveContext.ActiveContentTree;
      if (contentTree != null)
      {
        contentTree.Initialize();
      }

      this.Close();
    }

    /// <summary>Imports the options.</summary>
    /// <param name="optionsElement">The options element.</param>
    private void ImportOptions([NotNull] XElement optionsElement)
    {
      Debug.ArgumentNotNull(optionsElement, "optionsElement");

      var options = AppHost.Settings.Options;

      foreach (var element in optionsElement.Elements())
      {
        var name = element.GetAttributeValue("name");
        var type = element.GetAttributeValue("type");
        var value = element.Value;

        var property = options.GetType().GetProperty(name);
        if (type == "Boolean")
        {
          property.SetValue(options, value == "true", null);
        }
        else if (type == "String")
        {
          property.SetValue(options, value, null);
        }
      }
    }

    #endregion
  }
}