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

namespace SingleFinite.Mvvm.Blazor.Internal;

/// <summary>
/// A view displayed in a blazor app.
/// </summary>
internal class BlazorView : IView
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="componentType">The component type that is the View.</param>
    /// <param name="viewModel">The ViewModel for the component.</param>
    public BlazorView(Type componentType, IViewModel viewModel)
    {
        ComponentType = componentType;
        ViewModel = viewModel;
        ViewState = new Dictionary<string, object>();
        Parameters = new Dictionary<string, object>
        {
            { "ViewModel", ViewModel },
            { "ViewState", ViewState }
        };
    }

    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance of an empty BlazorView.
    /// </summary>
    public static BlazorView Empty = new(
        componentType: typeof(EmptyViewComponent),
        viewModel: new EmptyViewModel()
    );

    /// <summary>
    /// The component type that is the View.
    /// </summary>
    public Type ComponentType { get; }

    /// <inheritdoc />
    public IViewModel ViewModel { get; }

    /// <summary>
    /// The view state for the component.
    /// </summary>
    public IDictionary<string, object> ViewState { get; }

    /// <summary>
    /// The parameters for the component.
    /// </summary>
    public Dictionary<string, object> Parameters { get; }

    #endregion
}
