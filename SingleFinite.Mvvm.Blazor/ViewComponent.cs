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
using SingleFinite.Mvvm.Blazor.Services;

namespace SingleFinite.Mvvm.Blazor;

/// <summary>
/// The base class for components that implement the IView interface.
/// </summary>
/// <typeparam name="TViewModel">
/// The type of view model the view displays.
/// </typeparam>
public class ViewComponent<TViewModel> :
    ComponentBase, IView<TViewModel>, IDisposable
    where TViewModel : IViewModel
{
    #region Fields

    /// <summary>
    /// Set to true with this component is disposed.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Prefix used when generating ViewState keys for remembering scroll
    /// positions.
    /// </summary>
    private const string ScrollKeyPrefix =
        "SingleFinite.Mvvm.Blazor.ViewComponent.ScrollPosition";

    #endregion

    #region Properties

    /// <summary>
    /// The view model being displayed.
    /// </summary>
    [Parameter, EditorRequired]
    public TViewModel ViewModel { get; set; }

    /// <summary>
    /// The view state for this component.
    /// </summary>
    [Parameter, EditorRequired]
    public IDictionary<string, object> ViewState { get; set; }

    /// <summary>
    /// A task scope that is canceled when the view is disposed.
    /// </summary>
    protected ITaskScope ViewScope { get; } = new TaskScope();

    /// <summary>
    /// ScrollManager used to remember scroll positions.
    /// </summary>
    [Inject]
    protected IScrollManager ScrollManager { get; set; } = default!;

    #endregion

    #region Methods

    /// <summary>
    /// Remember the scroll position for the given element and restore it if
    /// this view gets destroyed and recreated.  This method must be called
    /// from the OnAfterRenderAsync method.
    /// </summary>
    /// <param name="firstRender">
    /// The value provided by the OnAfterRenderAsync method.
    /// </param>
    /// <param name="element">
    /// The scrollable element to remember the scroll position of.
    /// </param>
    /// <param name="key">
    /// An optional key.  If there is more than one scrollable element in the
    /// view that needs to have the scroll position remembered, use a unique
    /// key value for each element.
    /// </param>
    /// <returns>A running task for this method.</returns>
    protected async Task RememberScrollPositionAsync(
        bool firstRender,
        ElementReference element,
        string key = ""
    )
    {
        if (!firstRender)
            return;

        (await ScrollManager.ObserveScrollPositionAsync(element, key))
            .Debounce(TimeSpan.FromMilliseconds(250))
            .OnEach(args =>
            {
                ViewState[CreateRememberScrollPositionKey(args.Key)] =
                    args.Position;
            })
            .Until(ViewScope.CancellationToken);

        var scrollKey = CreateRememberScrollPositionKey(key);
        if (
            ViewState.TryGetValue(scrollKey, out var position) &&
            position is double positionAsDouble
        )
        {
            await ScrollManager.SetScrollPositionAsync(
                element: element,
                position: positionAsDouble
            );
        }
    }

    /// <summary>
    /// Setup observer for view model changes.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ViewModel is IChangeable changeable)
        {
            changeable.Changed
                .Observe()
                .OnEach(OnViewModelChanged)
                .Until(ViewScope.CancellationToken);
        }
    }

    /// <summary>
    /// Notify system that the state has changed.
    /// </summary>
    protected virtual void OnViewModelChanged()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Called when this view has been disposed.
    /// </summary>
    protected virtual void OnDisposed()
    {
    }

    /// <summary>
    /// Dispose of this component.
    /// </summary>
    /// <param name="disposing">
    /// True if the component is being disposed, false otherwise.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                ViewScope.Cancel();
            }

            _isDisposed = true;
            OnDisposed();
        }
    }

    /// <inheritdoc />
    void IDisposable.Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates a ViewState key used to store remembered scroll positions.
    /// </summary>
    /// <param name="key">A key that identifies the element.</param>
    /// <returns>The ViewState key.</returns>
    private static string CreateRememberScrollPositionKey(string key) =>
        $"{ScrollKeyPrefix}:{key}";

    #endregion
}
