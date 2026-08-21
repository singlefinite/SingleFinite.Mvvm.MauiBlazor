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

using SingleFinite.Mvvm;
using SingleFinite.Mvvm.Services.Presenters;

namespace Example.Models;

public partial class MainViewModel(
    IStackPresenter stack,
    IDialogPresenter dialogs
) : ViewModel, IMainViewModel
{
    public IStackPresenter Stack => stack;

    public IDialogPresenter Dialogs => dialogs;

    protected override void OnCreated()
    {
        Stack.Push<TestOneViewModel>();
    }

    public void Next()
    {
        if (Stack.Current?.ViewModel is TestOneViewModel)
            Stack.Push<TestTwoViewModel>();
    }

    public void Back()
    {
        if (Stack.Current?.ViewModel is TestTwoViewModel)
            Stack.Pop();
    }

    public void ShowDialog()
    {
        Dialogs.Show<DialogOneViewModel>();
    }
}
