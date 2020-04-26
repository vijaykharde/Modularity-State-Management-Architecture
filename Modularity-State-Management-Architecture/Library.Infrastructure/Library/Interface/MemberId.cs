namespace StateBuilder.Library.Interface
{
    public class MemberId
    {
        public MemberId(string loginName, long oracleId)
        {
            LoginName = loginName;
            OracleId = oracleId;
        }

        public string LoginName { get; }
        public long OracleId { get; }

        public override bool Equals(object obj)
        {
            var other = obj as MemberId;
            if (other == null)
            {
                return false;
            }
            return string.Equals(LoginName, other.LoginName) && OracleId == other.OracleId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((LoginName != null ? LoginName.GetHashCode() : 0) * 397) ^ OracleId.GetHashCode();
            }
        }
    }
}
