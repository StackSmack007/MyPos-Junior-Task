namespace CommonLibrary
{
    public static class LockObjects
    {
        
        static LockObjects()
        {
            UserBalanceObject = new object();
        }

        public static object UserBalanceObject { get; }
    }
}