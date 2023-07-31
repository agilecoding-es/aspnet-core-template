namespace Template.Domain.Entities.Shared
{
    public class Error : IEquatable<Error>
    {
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }
        public string Message { get; }

        public static implicit operator string(Error error) => error.Code;

        public static bool operator ==(Error left, Error right)
        {
            return left is not null && right is not null && left.Equals(right);
        }

        public static bool operator !=(Error left, Error right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != GetType()) return false;

            if (obj is not Error error) return false;

            return error.Code == Code;
        }

        public bool Equals(Error other)
        {
            if (other == null) return false;

            if (other.GetType() != GetType()) return false;

            return other.Code == Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
