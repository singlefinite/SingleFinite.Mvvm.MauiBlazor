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

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using SingleFinite.Essentials;
using SingleFinite.Mvvm.Blazor.Services;

namespace SingleFinite.Mvvm.Blazor.Internal.Services;

/// <summary>
/// Implementation of <see cref="IScrollManager"/>.
/// </summary>
/// <param name="serviceProvider">Used to lookup IJSRuntime.</param>
internal class ScrollManager(
    IServiceProvider serviceProvider
) : IScrollManager
{
    #region Fields

    /// <summary>
    /// Caches the javascript module used.
    /// </summary>
    private IJSObjectReference? _jsModule;

    #endregion

    #region Methods

    /// <inheritdoc/>
    public async Task<double> GetScrollPositionAsync(ElementReference element)
    {
        var jsModule = await GetJSModuleAsync();
        return await jsModule.InvokeAsync<double>(
            identifier: "getScrollPosition",
            element
        );
    }

    /// <inheritdoc/>
    public async Task SetScrollPositionAsync(ElementReference element, double position)
    {
        var jsModule = await GetJSModuleAsync();
        await jsModule.InvokeVoidAsync(
            identifier: "setScrollPosition",
            element,
            position
        );
    }

    /// <inheritdoc/>
    public async Task<IEventObserver<ScrollPositionChangedArgs>> ObserveScrollPositionAsync(
        ElementReference element,
        string key = ""
    )
    {
        var observer = new ScrollPositionObserver(key);
        var jsModule = await GetJSModuleAsync();
        await jsModule.InvokeVoidAsync(
            identifier: "addScrollEventListener",
            element,
            observer.Reference,
            nameof(ScrollPositionObserver.OnScrollPositionChanged)
        );

        return observer;
    }

    /// <summary>
    /// Gets the javascript module.  If this is the first call it will load and
    /// cache the module for future use.
    /// </summary>
    /// <returns>The javascript module.</returns>
    private async Task<IJSObjectReference> GetJSModuleAsync()
    {
        _jsModule ??= await serviceProvider
            .GetRequiredService<IJSRuntime>()
            .InvokeAsync<IJSObjectReference>(
                identifier: "import",
                "./_content/SingleFinite.Mvvm.Blazor/SingleFinite.Mvvm.Blazor.js"
            );
        return _jsModule;
    }

    #endregion
}
