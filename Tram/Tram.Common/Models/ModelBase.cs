namespace Tram.Common.Models
{
    public abstract class ModelBase
    {
        public string Id { get; set; }

        public override bool Equals(object obj) => obj != null && (obj as ModelBase).Id == Id;

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Id + " " + base.ToString();
    }
}
