public static class ModuleInitializer
{
    #region Enable

    [ModuleInitializer]
    public static void Init()
    {
        VerifyWolverine.Enable();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}