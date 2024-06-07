namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt Dto komentarzy
  /// </summary>
  public record CommentDto
  {
    /// <summary>
    /// Identyfikator komentarza
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Autor komentarza
    /// </summary>
    public string Author { get; init; }

    /// <summary>
    /// Treść komentarza
    /// </summary>
    public string Content { get; init; }

    /// <summary>
    /// Czas utworzenia komentarza
    /// </summary>
    public DateTime CreatedAt { get; init; }
  }
}
