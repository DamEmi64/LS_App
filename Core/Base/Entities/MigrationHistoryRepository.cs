using Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;

namespace Api.Setup
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
    public class MigrationHistoryRepository : SqlServerHistoryRepository
    {
        public const string ModuleColumnName = "Module";
        public const string InsDateColumnName = "InsDate";

        public MigrationHistoryRepository(HistoryRepositoryDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);
            history.Property<string>(ModuleColumnName).HasMaxLength(300).IsRequired();
            history.Property<string>(InsDateColumnName).IsRequired();
        }

        public override string GetInsertScript(HistoryRow row)
        {
            var moduleContext = Dependencies.CurrentContext.Context;

            var moduleName = "(default)";

            if (moduleContext is IDbContextBase contextBase)
            {
                moduleName = contextBase.ContextName;
            }

            var stringTypeMapping =
                Dependencies.TypeMappingSource.GetMapping(typeof(string));
            return new StringBuilder()
            .Append("INSERT INTO ")
                .Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
            .Append("(")
                .Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
                .Append(", ")
                .Append(SqlGenerationHelper.DelimitIdentifier(ProductVersionColumnName))
                .Append(", ")
                .Append(SqlGenerationHelper.DelimitIdentifier(ModuleColumnName))
                .Append(", ")
                .Append(SqlGenerationHelper.DelimitIdentifier(InsDateColumnName))
                .Append(") ")
                .Append("VALUES (")
                .Append(stringTypeMapping.GenerateSqlLiteral(row.MigrationId))
            .Append(", ")
                .Append(stringTypeMapping.GenerateSqlLiteral(row.ProductVersion))
                .Append(", ")
                .Append(stringTypeMapping.GenerateSqlLiteral(moduleName))
                .Append(", ")
                .Append("getDate()")
                .Append(")")
                .AppendLine(SqlGenerationHelper.StatementTerminator)
                .ToString();
        }
    }
}