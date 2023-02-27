using IPInformationProvider.API.Interfaces;

namespace IPInformationProvider.API.Static
{
    public static class IPDetailsChangesCheck
    {
        public static bool CheckIPForChanges(IIPs? original,IIPs current)
        {
            if (original == null)
                return false;

            if (original.TwoLetterCode == current.TwoLetterCode
                && original.ThreeLetterCode == current.ThreeLetterCode)
            {
                return false;
            }
            return true;
        }
    }
}
