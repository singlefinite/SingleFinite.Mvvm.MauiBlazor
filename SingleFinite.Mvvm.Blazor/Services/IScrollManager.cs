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
using SingleFinite.Essentials;

namespace SingleFinite.Mvvm.Blazor.Services;

/// <summary>
/// Service that supports management of a scrollable element.
/// </summary>
public interface IScrollManager
{
    /// <summary>
    /// Get the current scroll position for the given element.
    /// </summary>
    /// <param name="element">A scrollable element.</param>
    /// <returns>The current scroll position for the given element.</returns>
    Task<double> GetScrollPositionAsync(ElementReference element);

    /// <summary>
    /// Set the current scroll position for the given element.
    /// </summary>
    /// <param name="element">A scrollable element.</param>
    /// <param name="position">The position value to set.</param>
    /// <returns>The running task for the method.</returns>
    Task SetScrollPositionAsync(ElementReference element, double position);

    /// <summary>
    /// Create an observer that emits whenever the scroll position changes on
    /// the given element.
    /// </summary>
    /// <param name="element">A scrollable element.</param>
    /// <param name="key">A key to include with the event arguments.</param>
    /// <returns>
    /// An observer that emits when the scroll position changes.
    /// </returns>
    Task<IEventObserver<ScrollPositionChangedArgs>> ObserveScrollPositionAsync(
        ElementReference element,
        string key = ""
    );
}
