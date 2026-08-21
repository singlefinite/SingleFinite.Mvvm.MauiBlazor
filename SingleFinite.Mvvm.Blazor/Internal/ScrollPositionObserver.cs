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

using Microsoft.JSInterop;
using SingleFinite.Essentials;

namespace SingleFinite.Mvvm.Blazor.Internal;

/// <summary>
/// An observer that emits when an associated scrollable elements scroll
/// position changes.
/// </summary>
internal class ScrollPositionObserver : IEventObserver<ScrollPositionChangedArgs>
{
    #region Fields

    /// <summary>
    /// Holds the key associated with the scrollable element.
    /// </summary>
    private readonly string _key;

    /// <summary>
    /// The observable source used to emit events.
    /// </summary>
    private readonly EventObservableSource<ScrollPositionChangedArgs> _source;

    /// <summary>
    /// The underlying observer that supports this class.
    /// </summary>
    private readonly IEventObserver<ScrollPositionChangedArgs> _observer;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="key">
    /// The key associated with the scrollable element.
    /// </param>
    public ScrollPositionObserver(string key)
    {
        _key = key;
        _source = new EventObservableSource<ScrollPositionChangedArgs>();
        _observer = _source.Observable.Observe();
        Reference = DotNetObjectReference.Create(this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// A reference to this object that can be provided to a javascript module.
    /// </summary>
    public DotNetObjectReference<ScrollPositionObserver> Reference { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Emit a scroll position changed event.  This method can be called from a
    /// javascript module.
    /// </summary>
    /// <param name="position">The new scroll position.</param>
    [JSInvokable]
    public void OnScrollPositionChanged(double position)
    {
        _source.Emit(
            new(
                Key: _key,
                Position: position
            )
        );
    }

    /// <inheritdoc/>
    public event Action<ScrollPositionChangedArgs> NextWithArgs
    {
        add => _observer.NextWithArgs += value;
        remove => _observer.NextWithArgs -= value;
    }

    /// <inheritdoc/>
    public event Action Next
    {
        add => _observer.Next += value;
        remove => _observer.Next -= value;
    }

    /// <inheritdoc/>
    public event Action Disposed
    {
        add => _observer.Disposed += value;
        remove => _observer.Disposed -= value;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _observer.Dispose();
        Reference.Dispose();
    }

    #endregion
}
