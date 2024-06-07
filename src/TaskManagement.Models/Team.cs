namespace Models
{
  /// <summary>
  /// Model zespołu
  /// </summary>
  public class Team
  {
    /// <summary>
    /// Identyfikator zespołu
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nazwa zespołu
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Identyfikator lidera zespołu
    /// </summary>
    public Guid TeamLeaderId { get; set; }

    /// <summary>
    /// Lista członków zespołu
    /// </summary>
    public List<Guid> UserIds { get; set; }
  }
}
