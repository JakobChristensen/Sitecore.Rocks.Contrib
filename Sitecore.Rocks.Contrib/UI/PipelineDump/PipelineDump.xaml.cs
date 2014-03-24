// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PipelineDump.xaml.cs" company="Sitecore A/S">
//   Copyright (C) 2010 by Sitecore A/S
// </copyright>
// <summary>
//   Interaction logic for PipelineDump.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.VisualStudio.UI.PipelineDump
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Diagnostics;
  using Sitecore.VisualStudio.Extensibility.Pipelines;

  /// <summary>Interaction logic for PipelineDump.xaml</summary>
  public partial class PipelineDump
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="PipelineDump"/> class.</summary>
    public PipelineDump()
    {
      this.InitializeComponent();

      this.RenderPipelines();
    }

    #endregion

    #region Methods

    /// <summary>Renders the pipeline.</summary>
    /// <param name="output">The output.</param>
    /// <param name="pipeline">The pipeline.</param>
    /// <param name="value">The value.</param>
    private void RenderPipeline([NotNull] StringWriter output, [NotNull] Type pipeline, [NotNull] List<PipelineManager.Processor> value)
    {
      Debug.ArgumentNotNull(output, "output");
      Debug.ArgumentNotNull(pipeline, "pipeline");
      Debug.ArgumentNotNull(value, "value");

      output.WriteLine(pipeline.FullName + ", " + Path.GetFileName(pipeline.Assembly.Location));

      foreach (var processor in value)
      {
        var priority = processor.Priority.ToString("       0.##");

        priority = priority.Substring(priority.Length - 8);

        output.Write(" ");
        output.Write(priority);
        output.Write(" ");
        output.WriteLine(processor.Instance.GetType().FullName + ", " + Path.GetFileName(processor.Instance.GetType().Assembly.Location));
      }

      output.WriteLine();
      output.WriteLine();
    }

    /// <summary>Renders the pipelines.</summary>
    private void RenderPipelines()
    {
      var output = new StringWriter();

      foreach (var pipeline in PipelineManager.Pipelines)
      {
        this.RenderPipeline(output, pipeline.Key, pipeline.Value);
      }

      this.Dump.Text = output.ToString();
    }

    #endregion
  }
}