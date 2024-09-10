using HackathonEveryone.Model;
using HackathonEveryone.Model.Employee;

namespace HackathonEveryone.ServiceContract;

/// <summary>
/// Интерфейс для генерации списков пожеланий (wishlists) участников хакатона.
/// </summary>
public interface IWishlistGenerator
{
    /// <summary>
    /// Генерирует списки пожеланий для заданных сотрудников.
    /// </summary>
    /// <param name="requestingEmployees">Список сотрудников, которые создают wishlists и для которых эти списки будут сгенерированы.</param>
    /// <param name="availableEmployees">Список сотрудников, которые могут быть включены в wishlists. Это те сотрудники, которые будут перечислены в списках пожеланий.</param>
    /// <returns>Список объектов <see cref="Wishlist"/>, где каждый элемент представляет собой список желаемых сотрудников для конкретного сотрудника из <paramref name="requestingEmployees"/>.</returns>
    IEnumerable<Wishlist> GenerateWishlists(IEnumerable<Employee> requestingEmployees,
        IEnumerable<Employee> availableEmployees);
}