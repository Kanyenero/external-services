namespace Navicon.Mdm.ExternalServices.Model.Entities;

public class Entity
{
    public PrimitiveEntity EntityData { get; set; }

    public ICollection<Entity> Children { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(EntityData.PrimitiveEntityName), EntityData?.PrimitiveEntityName, nameof(Children), Children?.Count ?? 0);
}
