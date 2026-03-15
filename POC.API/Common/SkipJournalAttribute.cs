namespace POC.API.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipJournalAttribute : Attribute, ISkipJournal
    {
    }
}
