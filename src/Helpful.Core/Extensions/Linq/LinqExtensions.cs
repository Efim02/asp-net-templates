namespace Helpful.Core.Extensions.Linq;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
    
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
    public static T Next<T>(this IEnumerable<T> enumerable, T element, bool isCycle = false)
    {
        return enumerable.ToList().Next(element, isCycle);
    }

    /// <summary>
    /// Объединяет строки через разделитель.
    /// </summary>
    public static string Join(this IEnumerable<string> source, string separator)
    {
        return string.Join(separator, source);
    }

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
        Func<T, IComparable>? getPropertyFunc = null)
        where T : class
    {
        var collection = collectionEnumerable.ToList();
        var sequence = sequenceEnumerable.ToList();

        // Если нет ни одного равного элемента, возвращаем тот же список.
        var firstItem = sequence.FirstOrDefault(i => collection
            .Any(ai => EqualsByFuncOrDefault(i, ai, getPropertyFunc)));
        if (firstItem == null)
            return collection.ToList();

        // Если нет второго равного элемента, возвращаем список с этого элемента.
        var secondItem = sequence.FirstOrDefault(item => collection
            .Any(anotherItem => anotherItem != firstItem
                                && EqualsByFuncOrDefault(item, anotherItem, getPropertyFunc)));
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
        Func<T, IComparable>? getPropertyFunc = null)
        where T : class
    {
        var countEquals = 0;
        var previousAnotherElementIndex = 0;

        foreach (var element in elements)
        {
            // Если нет такого элемента или индекс равен - пропускаем.
            var anotherElement = anotherElements
                .FirstOrDefault(ai => EqualsByFuncOrDefault(element, ai, getPropertyFunc));

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

    /// <summary>
    /// Получает только определенное количество элементов или все элементы если всех элементов не хватает.
    /// </summary>
    public static IEnumerable<T> TakeAllOrByCount<T>(this IEnumerable<T> source, int count)
    {
        using (var enumerator = source.GetEnumerator())
        {
            for (var index = 0; index < count; index++)
            {
                if (!enumerator.MoveNext())
                    break;

                yield return enumerator.Current;
            }
        }
    }

    /// <summary>
    /// Получает плоский список листьев дерева.
    /// </summary>
    /// <remarks> Проверяет есть ли в узле элементы и если нет, то возвращает этот элемент. </remarks>
    public static IEnumerable<T> FlattenLeaves<T>(
        this IEnumerable<T> elements,
        Func<T, IEnumerable<T>> getChildren)
    {
        foreach (var element in elements)
        {
            if (!getChildren(element).Any())
                yield return element;

            var subElements = FlattenLeaves(getChildren(element), getChildren);
            foreach (var subElelement in subElements)
            {
                yield return subElelement;
            }
        }
    }

    /// <summary>
    /// Агрегирование, через создание chain-ов между элементами.
    /// </summary>
    public static IEnumerable<TR> ChainAggregate<T, TR>(
        this IEnumerable<T> source,
        Func<T, T, TR> createChainFunc)
    {
        var hasFirst = false;
        T? first = default;

        foreach (var item in source)
        {
            if (!hasFirst)
            {
                first = item;
                hasFirst = true;
                continue;
            }

            yield return createChainFunc(first!, item);
            first = item;
        }
    }

    /// <summary>
    /// Начинает перечисление с указанного элемента, сравнивая объекты через указанную функцию или по-умолчанию.
    /// </summary>
    public static IEnumerable<T> StartWith<T>(
        this IEnumerable<T> elements,
        T firstElement,
        Func<T, IComparable>? getPropertyFunc = null)
    {
        // Элементы, которые окажутся позади.
        var backElements = new List<T>();
        var foundFirstElement = false;

        foreach (var element in elements)
        {
            if (EqualsByFuncOrDefault(element, firstElement) || foundFirstElement)
            {
                foundFirstElement = true;
                yield return element;
                continue;
            }

            backElements.Add(element);
        }

        if (!foundFirstElement)
            throw new InvalidOperationException($"В коллекции {elements.GetType().Name}, не найдет первый элемент");

        foreach (var backElement in backElements)
        {
            yield return backElement;
        }
    }

    /// <summary>
    /// Преобразует в стек.
    /// </summary>
    public static Stack<T> ToStack<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();

        // Иначе список будет выдаваться не в этом порядке.
        list.Reverse();
        return new Stack<T>(list);
    }

    /// <summary>
    /// Преобразует в очередь.
    /// </summary>
    public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
    {
        return new Queue<T>(source);
    }

    /// <summary>
    /// Получает элемент в последовательности по индексу или default.
    /// </summary>
    public static T? GetByIndexOrDefault<T>(this IEnumerable<T> source, int index)
    {
        var currentIndex = 0;
        foreach (var item in source)
        {
            if (currentIndex == index)
                return item;

            currentIndex++;
        }

        return default;
    }

    /// <summary>
    /// Сортирует, если условие выполняется.
    /// </summary>
    public static IEnumerable<T> OrderByIf<T, TR>(
        this IEnumerable<T> enumerable,
        bool condition,
        Func<T, TR> getPropertyFunc)
    {
        return condition ? enumerable.OrderBy(getPropertyFunc) : enumerable;
    }

    /// <summary>
    /// Получает перечисление элементов идущих до указанного элемента.
    /// </summary>
    public static IEnumerable<T> BeforeEnumerable<T>(this IEnumerable<T> enumerable, T obj)
    {
        foreach (var element in enumerable)
        {
            if (element!.Equals(obj)) yield break;

            yield return element;
        }

        throw new ArgumentException("Не найден переданный элемент");
    }

    /// <summary>
    /// Делит список элементов, на нужное кол-во делений, чтобы в каждом делении т.е. списки было
    /// не больше указанного кол-ва.
    /// </summary>
    public static IEnumerable<IReadOnlyList<T>> SplitByCount<T>(this IEnumerable<T> enumerable, int count)
    {
        var currentList = new List<T>();
        foreach (var element in enumerable)
        {
            currentList.Add(element);
            if (currentList.Count >= count)
            {
                yield return currentList;
                currentList = new List<T>();
            }
        }

        if (currentList.Count > 0)
            yield return currentList;
    }

    /// <summary>
    /// Соединяет коллекции если условие выполняется.
    /// </summary>
    public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> enumerable, bool condition, IEnumerable<T> other)
    {
        if (condition)
            return enumerable.Concat(other);

        return enumerable;
    }

    /// <summary>
    /// Добавить 1 элемент; isAfter - если нужно добавить в конец.
    /// </summary>
    public static IEnumerable<T> ConcatOne<T>(this IEnumerable<T> enumerable, T oneElement, bool isAfter = true)
    {
        if (!isAfter)
            yield return oneElement;

        foreach (var element in enumerable)
        {
            yield return element;
        }

        if (isAfter)
            yield return oneElement;
    }

    /// <summary>
    /// Сравнивает объекты по указанной функции, если функция указана.
    /// </summary>
    private static bool EqualsByFuncOrDefault<T>(T a, T b, Func<T, IComparable>? equalsFunc = null)
    {
        return equalsFunc == null ? a!.Equals(b) : equalsFunc(a).Equals(equalsFunc(b));
    }
}