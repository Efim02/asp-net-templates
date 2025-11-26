namespace Helpful.Core.Extensions.Linq;

using Helpful.Core.Extensions.Collections;

public static class LinqExtensions
{
    /// <summary>
    /// Пустое ли перечисление.
    /// </summary>
    public static bool Empty<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.Any();
    }

    public static bool EqualsEnumerable<T>(this IEnumerable<T> enumerable, ICollection<T> other)
    {
        foreach (var item in enumerable)
        {
            if (other.All(o => o!.Equals(item)))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Имеются ли элементы в перечисление.
    /// </summary>
    public static bool Has<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Any();
    }

    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> query,
        Func<T, bool> whereQueryFunc,
        bool condition)
    {
        return condition ? query.Where(whereQueryFunc) : query;
    }

    /// <summary>
    /// Получает индекс элемента в последовательности. -1 - значит не содержится.
    /// </summary>
    public static int IndexOf<T>(this IEnumerable<T> source, T element)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (item!.Equals(element))
                return index;
            index++;
        }

        return -1;
    }

    /// <summary>
    /// Получает номер элемента в последовательности. 0 - значит не содержится.
    /// </summary>
    public static int NumberOf<T>(this IEnumerable<T> source, T element)
    {
        return source.IndexOf(element) + 1;
    }
    
    /// <summary>
    /// Получает по указанному элементу, следующее элемент или null если это последнее элемент.
    /// </summary>
    /// <param name="enumerable"> Список. </param>
    /// <param name="element"> Элемент относительно, которого ищется. </param>
    /// <param name="isCycle"> Зацикленный ли список. </param>
    public static T? Next<T>(this IEnumerable<T> enumerable, T element, bool isCycle = false)
    {
        return CollectionExtensions.Next(enumerable.ToList(), element, isCycle);
    }
    
    /// <summary>
    /// Объединяет строки через разделитель.
    /// </summary>
    public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);

    
        /// <summary>
        /// Выполняет пересечение меньшего списка из большего списка.
        /// </summary>
        /// <param name="enumerable"> Список. </param>
        /// <param name="anotherEnumerable"> Другой список. </param>
        /// <param name="getPropertyFunc"> Функция для сравнения. </param>
        public static IEnumerable<T> IntersectByBigList<T, TP>(
            this IEnumerable<T> enumerable,
            IEnumerable<T> anotherEnumerable,
            Func<T, TP> getPropertyFunc)
        {
            var list = enumerable.ToList();
            var anotherList = anotherEnumerable.ToList();

            var bigList = list.Count > anotherList.Count ? list : anotherList;
            var smallList = list.Count > anotherList.Count ? anotherList : list;

            return bigList.Intersect(smallList, new ByPropertyComparer<T, TP>(getPropertyFunc));
        }

        /// <summary>
        /// Выполняет исключение меньшего списка из большего списка.
        /// </summary>
        /// <param name="enumerable"> Список. </param>
        /// <param name="anotherEnumerable"> Другой список. </param>
        /// <param name="getPropertyFunc"> Функция для сравнения. </param>
        public static IEnumerable<T> ExceptByBigList<T, TP>(
            this IEnumerable<T> enumerable,
            IEnumerable<T> anotherEnumerable,
            Func<T, TP> getPropertyFunc)
        {
            var list = enumerable.ToList();
            var anotherList = anotherEnumerable.ToList();

            var bigList = list.Count > anotherList.Count ? list : anotherList;
            var smallList = list.Count > anotherList.Count ? anotherList : list;

            return bigList.Except(smallList, new ByPropertyComparer<T, TP>(getPropertyFunc));
        }

        /// <summary>
        /// Получает список, который должен иметь последовательность элементов как в указанной последовательности.
        /// В случае если не сработает, ошибки не возвращает.
        /// </summary>
        public static List<T> GetListLikeSequence<T>(
            this IEnumerable<T> collectionEnumerable,
            IEnumerable<T> sequenceEnumerable,
            Func<T, IComparable> getPropertyFunc = null)
            where T : class
        {
            var collection = collectionEnumerable.ToList();
            var sequence = sequenceEnumerable.ToList();

            // Если нет ни одного равного элемента, возвращаем тот же список.
            var firstItem = sequence.FirstOrDefault(i => collection
                .Any(ai => EqualsByFuncOrDefault(getPropertyFunc, i, ai)));
            if (firstItem == null)
                return collection.ToList();

            // Если нет второго равного элемента, возвращаем список с этого элемента.
            var secondItem = sequence.FirstOrDefault(item => collection
                .Any(anotherItem => anotherItem != firstItem
                                    && EqualsByFuncOrDefault(getPropertyFunc, item, anotherItem)));
            if (secondItem == null)
                return collection.StartWith(firstItem, getPropertyFunc).ToList();

            var list = collection.StartWith(firstItem, getPropertyFunc).ToList();

            // Список с обратной последовательностью, и начинающийся с того же первого элемента; для проверки ниже.
            var reversedList = list.ToList();
            reversedList.Reverse();
            reversedList = reversedList.StartWith(firstItem, getPropertyFunc).ToList();

            // Для случая равно, может отработать криво, как отработает:
            // возможно нужно будет переделать на условие через 1-2 элемент (был такой вариант проверять).
            var resultList = CountSequentiallyEquals(list, sequence) >= CountSequentiallyEquals(reversedList, sequence)
                ? list
                : reversedList;

            if (collection.Count != resultList.Count)
                throw new Exception("Ошибка метода GetListLikeSequence, ошибочное кол-во элементов на выходе");

            return resultList;
        }

        /// <summary>
        /// Получает количество последовательно расположенных равных элементов.
        /// </summary>
        public static int CountSequentiallyEquals<T>(
            this IEnumerable<T> elements,
            List<T> anotherElements,
            Func<T, IComparable> getPropertyFunc = null)
            where T : class
        {
            var countEquals = 0;
            var previousAnotherElementIndex = 0;

            foreach (var element in elements)
            {
                // Если нет такого элемента или индекс равен - пропускаем.
                var anotherElement = anotherElements
                    .FirstOrDefault(ai => EqualsByFuncOrDefault(getPropertyFunc, element, ai));

                // Проверка на null, IndexOf вдруг исключение выкинет.
                var anotherElementIndex = anotherElement == null ? -1 : anotherElements.IndexOf(anotherElement);
                if (anotherElementIndex == -1 || anotherElementIndex == previousAnotherElementIndex)
                    continue;

                // Если предыдущий индекс больше, значит уже не последовательно расположены.
                if (previousAnotherElementIndex > anotherElementIndex)
                    break;

                previousAnotherElementIndex = anotherElementIndex;
                countEquals++;
            }

            return countEquals;
        }

}
