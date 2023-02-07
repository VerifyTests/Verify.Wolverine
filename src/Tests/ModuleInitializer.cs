public static class ModuleInitializer
{
    #region Enable

    [ModuleInitializer]
    public static void Init() =>
        VerifyWolverine.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitOther() =>
        VerifierSettings.InitializePlugins();
}