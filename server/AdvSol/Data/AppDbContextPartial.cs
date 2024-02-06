using AdvSol.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AdvSol.Utils;

namespace AdvSol.Data
{
    public partial class AppDbContext
    {
        public override int SaveChanges()
        {
            PerformAudit();

            int result;

            try
            {
                result = base.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        #region Audit Helper
        private void PerformAudit()
        {
            IEnumerable<EntityEntry> modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            DateTime currentTime = DateTime.UtcNow;

            foreach (var entry in modifiedEntries)
            {
                var entityName = entry.Metadata.Name.GetWordAfterLastDot();
                var entityId = int.Parse(entry.CurrentValues["Id"].ToString());

                var originalValues = entry.OriginalValues;
                var currentValues = entry.CurrentValues;

                foreach (var property in entry.OriginalValues.Properties)
                {
                    var originalValue = (originalValues[property] ?? "").ToString();
                    var currentValue = (currentValues[property] ?? "").ToString();

                    if (!object.Equals(originalValue, currentValue))
                    {
                        var audit = new Audit
                        {
                            Entity = entityName,
                            EntityId = (int)entityId,
                            Field = property.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            Operation = entry.State == EntityState.Added ? "Record Created" : "Record Updated",
                            UpdatedTime = currentTime
                        };

                        Add<Audit>(audit);
                    }
                }
            }

            base.SaveChanges();
        }

        #endregion

        public void CreateAuditEntryForAdd(EntityEntry entry)
        {
            var entityName = entry.Metadata.Name.GetWordAfterLastDot();
            var entityId = int.Parse(entry.CurrentValues["Id"].ToString());

            var currentValues = entry.CurrentValues;

            DateTime currentTime = DateTime.UtcNow;

            foreach (var property in entry.OriginalValues.Properties)
            {
                var currentValue = (currentValues[property] ?? "").ToString();

                var audit = new Audit
                {
                    Entity = entityName,
                    EntityId = (int)entityId,
                    Field = property.Name,
                    OldValue = "",
                    NewValue = currentValue,
                    Operation = "Record Created",
                    UpdatedTime = currentTime
                };

                Add<Audit>(audit);
            }
        }
    }
}
