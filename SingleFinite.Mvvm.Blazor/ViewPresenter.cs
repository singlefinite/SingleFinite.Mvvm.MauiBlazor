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
using Microsoft.AspNetCore.Components.Rendering;
using SingleFinite.Essentials;
using SingleFinite.Mvvm.Blazor.Internal;
using SingleFinite.Mvvm.Services.Presenters;

namespace SingleFinite.Mvvm.Blazor;

/// <summary>
/// A component that renders views provided through a presenter.
/// </summary>
public class ViewPresenter : IComponent, IDisposable
{
    #region Fields

    /// <summary>
    /// The render handle
    /// </summary>
    private RenderHandle? _renderHandle;

    /// <summary>
    /// The registration for the presenter change event.
    /// </summary>
    private IDisposable? _presenterChangedObserver;

    /// <summary>
    /// Set to true with the component has been disposed.
    /// </summary>
    private bool _isDisposed;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the type of the component to be rendered.
    /// </summary>
    [Parameter]
    public IPresenter? Presenter
    {
        get;
        set
        {
            if (field == value)
                return;

            _presenterChangedObserver?.Dispose();
            field = value;
            _presenterChangedObserver = value?.CurrentChanged
                ?.Observe()
                ?.OnEach(OnPresenterChanged);
        }
    }

    /// <summary>
    /// Gets or sets a dictionary of parameters to be passed to the component.
    /// </summary>
    [Parameter]
    public IDictionary<string, object>? Parameters { get; set; }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
    }

    /// <inheritdoc />
    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        _renderHandle?.Render(Render);
        return Task.CompletedTask;
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
                Presenter = null;
            }

            _isDisposed = true;
        }
    }

    /// <inheritdoc />
    void IDisposable.Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Render the current view.
    /// </summary>
    /// <param name="builder">The builder to render to.</param>
    private void Render(RenderTreeBuilder builder)
    {
        if (Presenter?.Current is not BlazorView view)
            return;

        builder.OpenComponent(
            sequence: 0,
            componentType: view.ComponentType
        );

        if (Parameters.Combine(view.Parameters) is var attributes)
        {
            builder.AddMultipleAttributes(
                sequence: 1,
                attributes: attributes
            );
        }

        builder.CloseComponent();
    }

    /// <summary>
    /// Render the current view when the presenter changes.
    /// </summary>
    /// <param name="args">The arguments with the new view to render.</param>
    private void OnPresenterChanged(IPresenter.CurrentChangedEventArgs args)
    {
        _renderHandle?.Render(Render);
    }

    #endregion
}
