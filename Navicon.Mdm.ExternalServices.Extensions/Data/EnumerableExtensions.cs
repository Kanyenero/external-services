namespace Navicon.Mdm.ExternalServices.Extensions.Data;

/// <summary>
/// Предоставляет методы расширения для <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Позволяет осуществить приведение коллекции к типу <see cref="List{T}"/>, если возможно.
    /// Иначе, создание нового <see cref="List{T}"/> на основе коллекции не являющейся <see langword="null"/>.
    /// </summary>
    /// <param name="enumerable">Коллекция для приведения к <see cref="List{T}"/>.</param>
    /// <param name="createEmptyListIfCastingFailed">Определяет, нужно ли вернуть пустую коллекцию <see cref="List{T}"/> в случае неудачного приведения.</param>
    /// <returns>Коллекцию <see cref="List{T}"/>; пустую коллекцию <see cref="List{T}"/> или <see langword="null"/>.</returns>
    public static List<T> AsList<T>(this IEnumerable<T> enumerable, bool createEmptyListIfCastingFailed = false) =>
        enumerable as List<T> ?? enumerable?.ToList() ?? (createEmptyListIfCastingFailed ? Enumerable.Empty<T>().ToList() : null);
}
