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
using SingleFinite.Mvvm.Blazor.Internal;

namespace SingleFinite.Mvvm.Blazor;

/// <summary>
/// Displays the root view from the injected AppHost.
/// </summary>
public class ViewComponentRoot : IComponent
{
    #region Fields

    /// <summary>
    /// The render handle
    /// </summary>
    private RenderHandle? _renderHandle;

    /// <summary>
    /// The root view.
    /// </summary>
    private IView? _view;

    #endregion

    #region Properties

    /// <summary>
    /// Injected with the app host.
    /// </summary>
    [Inject]
    public IBlazorAppHost? AppHost { get; set; }

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
    public async Task SetParametersAsync(ParameterView parameters)
    {
        if (AppHost is null)
            return;

        _view = await AppHost.StartAsync();
        parameters.SetParameterProperties(this);
        _renderHandle?.Render(Render);
    }

    /// <summary>
    /// Render the current view.
    /// </summary>
    /// <param name="builder">The builder to render to.</param>
    private void Render(RenderTreeBuilder builder)
    {
        if (_view is not BlazorView view)
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

    #endregion
}
