namespace Models
{
  /// <summary>
  /// Model komentarzy
  /// </summary>
  public class Comment
  {
    /// <summary>
    /// Identyfikator komentarza
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Autor komentarza
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Treść komentarza
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Czas utworzenia komentarza
    /// </summary>
    public DateTime CreatedAt { get; set; }
  }
}
