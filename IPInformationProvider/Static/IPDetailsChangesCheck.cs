using IPInformationProvider.API.Models;

namespace IPInformationProvider.API.Static
{
    public static class IPDetailsChangesCheck
    {
        public static bool CheckIPForChanges(IP? original, IP current)
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
