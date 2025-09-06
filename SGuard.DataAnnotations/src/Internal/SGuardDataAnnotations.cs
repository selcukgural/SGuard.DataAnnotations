using System.Diagnostics.CodeAnalysis;

namespace SGuard.DataAnnotations.Internal;

/// <summary>
/// Provides utility methods for guarding conditions and safely invoking callbacks.
/// </summary>
internal static class SGuardDataAnnotations
{
    /// <summary>
    /// Guards a condition and executes a callback safely after attempting to throw an exception if the condition is true.
    /// </summary>
    /// <param name="condition">The condition to evaluate. If true, the <paramref name="throwAction"/> is executed.</param>
    /// <param name="throwAction">The action to execute if the condition is true.</param>
    /// <param name="callback">An optional callback to invoke after the guard logic.</param>
    public static void Guard([DoesNotReturnIf(true)] bool condition, Action throwAction, SGuardCallback? callback)
    {
        try
        {
            if (condition)
            {
                throwAction();
            }
        }
        finally
        {
            // Safely invoke the callback with the appropriate outcomes.
            InvokeCallbackSafely(condition, callback, GuardOutcome.Failure, GuardOutcome.Success);
        }
    }

    /// <summary>
    /// Safely invokes a callback with default success and failure outcomes.
    /// </summary>
    /// <param name="condition">The condition to evaluate for determining the outcome.</param>
    /// <param name="callback">The callback to invoke.</param>
    public static void InvokeCallbackSafely(bool condition, SGuardCallback? callback) 
        => InvokeCallbackSafely(condition, callback, GuardOutcome.Success, GuardOutcome.Failure);

    /// <summary>
    /// Safely invokes a callback with specified success and failure outcomes.
    /// </summary>
    /// <param name="condition">The condition to evaluate for determining the outcome.</param>
    /// <param name="callback">The callback to invoke.</param>
    /// <param name="successOutcome">The outcome to pass to the callback if the condition is false.</param>
    /// <param name="failureOutcome">The outcome to pass to the callback if the condition is true.</param>
    private static void InvokeCallbackSafely(bool condition, SGuardCallback? callback, GuardOutcome successOutcome, GuardOutcome failureOutcome)
    {
        try
        {
            // Invoke the callback with the appropriate outcome based on the condition.
            callback?.Invoke(condition ? successOutcome : failureOutcome);
        }
        catch
        {
            // Ignore any exceptions thrown by the callback.
        }
    }
}