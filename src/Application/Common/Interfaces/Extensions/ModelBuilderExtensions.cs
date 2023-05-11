using System.Linq.Expressions;
using System.Reflection;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces.Extensions;
public static class ModelBuilderExtensions
{
    public static void RegisterSoftDeleteQueryFilter(this ModelBuilder modelBuilder, ICollection<Type> excludedTypes = null)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes()
                                                                  .Where(t => !(excludedTypes ?? new List<Type>()).Contains(t.ClrType))
                                                                  .ToList();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            // Create the query filter
            ParameterExpression parameter = Expression.Parameter(entityType.ClrType);

            // EF.Property<bool>(post, "IsDeleted")
            MethodInfo propertyMethodInfo = typeof(Microsoft.EntityFrameworkCore.EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
            MethodCallExpression isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(nameof(ISoftDeletable.IsDeleted)));

            // EF.Property<bool>(post, "IsDeleted") == false
            BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

            // post => EF.Property<bool>(post, "IsDeleted") == false
            LambdaExpression lambda = Expression.Lambda(compareExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    public static void RemoveCascadeDeleteBehavior(this ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
                                                                 .SelectMany(t => t.GetForeignKeys())
                                                                 .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (IMutableForeignKey fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    /// <summary>
    /// Configures all decimal properties in the provided model to have the specified number of digits and precision.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model.</param>
    /// <param name="precision">The precision that will be used for decimals (number of digits after the decimal sign).</param>
    /// <param name="numberOfDigitsBeforeDecimalSign">The number of digits before the decimal sign.</param>
    /*public static void SetDecimalPrecision(this ModelBuilder modelBuilder, ushort precision, ushort numberOfDigitsBeforeDecimalSign = 18)
    {
        foreach (IMutableProperty property in modelBuilder.Model.GetEntityTypes()
                                                          .SelectMany(t => t.GetProperties())
                                                          .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            string columnType = property.Relational().ColumnType;

            if (columnType == null)
            {
                property.Relational().ColumnType = $"decimal({numberOfDigitsBeforeDecimalSign},{precision})";
            }
        }
    }*/
}