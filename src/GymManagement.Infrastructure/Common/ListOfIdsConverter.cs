using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymManagement.Infrastructure.Common
{
    public class ListOfIdsConverter : ValueConverter<List<Guid>, string>
    {
        public ListOfIdsConverter(ConverterMappingHints? mappingHints = null) : base(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
            mappingHints)
        {
        }
    }

    public class ListOfIdsComparer : ValueComparer<List<Guid>>
    {
        public ListOfIdsComparer() : base(
          (t1, t2) => t1!.SequenceEqual(t2!),
          t => t.Select(x => x!.GetHashCode()).Aggregate((acc, x) => acc ^ x),
          t => t)
        {
        }
    }
}

