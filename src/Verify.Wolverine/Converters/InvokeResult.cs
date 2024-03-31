namespace VerifyTests.Wolverine;

public delegate T InvokeResult<out T>(object message)
    where T: notnull;