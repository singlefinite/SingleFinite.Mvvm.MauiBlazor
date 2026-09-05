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
using SingleFinite.Mvvm.Blazor.Internal;
using SingleFinite.Mvvm.Blazor.Internal.Services;
using SingleFinite.Mvvm.Blazor.Services;
using SingleFinite.Mvvm.Services;

namespace SingleFinite.Mvvm.Blazor;

/// <summary>
/// Extensions for the <see cref="AppHostBuilder"/> class.
/// </summary>
public static class AppHostBuilderExtensions
{
    #region Methods

    /// <summary>
    /// Add MVVM blazor services to the app.
    /// </summary>
    /// <typeparam name="TMainViewModelInterface">
    /// The interface that will be used to register the view model with for
    /// dependency injection.
    /// </typeparam>
    /// <typeparam name="TMainViewModelImplementation">
    /// The type of view model that will be built as the entry point for the
    /// app.
    /// </typeparam>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>The builder that was passed in.</returns>
    public static AppHostBuilder AddSingleFiniteMvvmBlazor<TMainViewModelInterface, TMainViewModelImplementation>(
        this AppHostBuilder builder
    )
        where TMainViewModelInterface : class
        where TMainViewModelImplementation : class, TMainViewModelInterface, IViewModel => builder
            .AddServices(
                services =>
                {
                    services.AddSingleton(
                        serviceProvider => (TMainViewModelInterface)serviceProvider.GetRequiredService<IBlazorAppHost>().View.ViewModel
                    );
                }
            )
            .AddSingleFiniteMvvmBlazor<TMainViewModelImplementation>();

    /// <summary>
    /// Add MVVM blazor services to the app.
    /// </summary>
    /// <typeparam name="TMainViewModel">
    /// The type of view model that will be built as the entry point for the
    /// app.
    /// </typeparam>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>The builder that was passed in.</returns>
    public static AppHostBuilder AddSingleFiniteMvvmBlazor<TMainViewModel>(
        this AppHostBuilder builder
    )
        where TMainViewModel : IViewModel => builder
            .AddServices(
                services =>
                {
                    services
                        .AddSingleton<IBlazorAppHost>(serviceProvider =>
                        {
                            var appHost = (BlazorAppHost<TMainViewModel>)serviceProvider.GetRequiredService<AppHost>();
                            appHost.ServiceProvider = serviceProvider;
                            return appHost;
                        })
                        .AddScoped<IScrollManager, ScrollManager>()
                        .AddScoped<IViewBuilder, ComponentViewBuilder>();
                }
            );

    #endregion
}
