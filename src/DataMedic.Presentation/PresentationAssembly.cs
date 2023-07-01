using System.Reflection;

namespace DataMedic.Presentation;

/// <summary>
/// Assembly marker for Presentation Layer
/// </summary>
public static class PresentationAssembly
{
    /// <summary>
    /// Presentation Layer Assembly
    /// </summary>
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}