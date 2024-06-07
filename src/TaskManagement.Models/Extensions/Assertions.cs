namespace Models.Extensions
{
  /// <summary>
  /// Klasa zawiera metody wspomagające kontrolę wartości parametrów, własności,
  /// zmiennych i wyników zwracanych przez funkcje.
  /// </summary>
  public static class Assertions
  {
    /// <summary>
    /// Sprawdzenie, czy podana wartość nie jest równa null.
    /// </summary>
    /// <typeparam name="T">Typ parametru.</typeparam>
    /// <param name="target">Wartość do sprawdzenia.</param>
    /// <param name="name">Nazwa sprawdzanego parametru.</param>
    /// <returns>Wartość do przypisania po sprawdzeniu.</returns>
    public static T AssertNotNull<T>(this T target, string name = null) where T : class
    {
      if (target == null)
      {
        if (name == null)
          throw new ArgumentNullException(String.Format("Obiekt ma wartość null", typeof(T).FullName));
        else
          throw new ArgumentNullException(String.Format("Parametr ma wartość null", name));
      }
      return target;
    }
  }
}
