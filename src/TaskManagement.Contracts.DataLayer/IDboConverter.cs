namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji związanych z konwersją danych.
  /// </summary>
  public interface IDboConverter
  {
    /// <summary>
    /// Konwersja jednego obiektu na inny typ za pomocą mappera.
    /// </summary>
    /// <typeparam name="TResult">Typ obiektu wynikowego.</typeparam>
    /// <returns>Utworzony obiekt wynikowy.</returns>
    TResult Convert<TResult>(object source);

    /// <summary>
    /// Aktualizacja danych w obiekcie z przekazanego innego obiektu.
    /// </summary>
    /// <typeparam name="TSource">Typ obiektu źródłowego.</typeparam>
    /// <typeparam name="TResult">Typ obiektu wynikowego.</typeparam>
    /// <param name="source">Obiekt z którego czytane są dane do aktualizacji.</param>
    /// <param name="target">Obiekt do zaktualizowania danych.</param>
    /// <returns>Referencja do zaktualizowanego obiektu.</returns>
    TResult Update<TSource, TResult>(TSource source, TResult target);
  }
}
