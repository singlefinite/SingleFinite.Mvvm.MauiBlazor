// MIT License
// Copyright (c) 2026 Single Finite
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using SingleFinite.Mvvm.Services;

namespace SingleFinite.Mvvm.MauiBlazor.Internal.Services;

/// <summary>
/// Implementation of <see cref="IMainDispatcher"/> that uses the
/// <see cref="Dispatcher"/> from the main window to execute functions.
/// </summary>
internal partial class DispatcherMain : IMainDispatcher
{
    #region Methods

    /// <inheritdoc/>
    public Task<TResult> RunAsync<TResult>(
        Func<Task<TResult>> func,
        CancellationToken cancellationToken = default
    )
    {
        var dispatcher = Application.Current?.Dispatcher ??
            throw new InvalidOperationException("No dispatcher available.");

        var taskCompletionSource = new TaskCompletionSource<TResult>();

        dispatcher.DispatchAsync(async () =>
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                taskCompletionSource.SetResult(await func());
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
        });

        return taskCompletionSource.Task;
    }

    #endregion
}
