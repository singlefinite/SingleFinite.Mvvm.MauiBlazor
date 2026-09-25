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

using Microsoft.Extensions.DependencyInjection;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Mvvm.Blazor.Internal;

/// <summary>
/// Custom AppHost for Blazor.
/// </summary>
/// <typeparam name="TMainViewModel">
/// The type of view model to build for the main window.
/// </typeparam>
/// <param name="initializers">Initializers for the app host.</param>
internal partial class BlazorAppHost<TMainViewModel>(
    IInitializerCollection initializers
) : AppHost(initializers), IBlazorAppHost
    where TMainViewModel : IViewModel
{
    #region Properties

    /// <summary>
    /// The service provider used when the app host is started.
    /// </summary>
    public IServiceProvider? ServiceProvider { get; set; }

    /// <inheritdoc/>
    public IView? View { get; private set; }

    #endregion

    #region Methods

    /// <inheritdoc/>
    public override async Task<IView> StartAsync(
        IServiceProvider serviceProvider
    )
    {
        ServiceProvider = serviceProvider;
        return await StartAsync();
    }

    /// <inheritdoc/>
    public async Task<IView> StartAsync()
    {
        if (View is not null)
            return View;

        var serviceProvider = ServiceProvider ??
            throw new InvalidOperationException(
                message: "ServiceProvider has not been set."
            );

        var viewAssembler = serviceProvider.GetRequiredService<IViewAssembler>();
        var assembleResult = viewAssembler.AssembleFromDescriptor(
            viewModelDescriptor: new ViewModelDescriptor(
                ViewModelType: typeof(TMainViewModel),
                ViewModelParameters: []
            )
        );
        View = assembleResult.View;
        assembleResult.Start();

        await base.StartAsync(serviceProvider);

        return View;
    }

    #endregion
}
