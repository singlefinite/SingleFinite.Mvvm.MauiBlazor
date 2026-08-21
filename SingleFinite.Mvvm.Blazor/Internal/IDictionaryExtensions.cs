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
/// Extensions for the <see cref="IDictionary{TKey, TValue}"/> class.
/// </summary>
internal static class IDictionaryExtensions
{
    /// <summary>
    /// Extension members for dictionary types.
    /// </summary>
    /// <typeparam name="TKey">The type of key for the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of value for the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary being extended.</param>
    extension<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
        where TKey : notnull
    {
        /// <summary>
        /// Combine the other dictionary with this dictionary.
        /// </summary>
        /// <param name="other">The dictionary to combine with.</param>
        /// <returns>The combined dictionary.</returns>
        public IDictionary<TKey, TValue>? Combine(IDictionary<TKey, TValue> other)
        {
            if (dictionary is null && other is null)
                return null;
            if (dictionary is null)
                return other;
            if (other is null)
                return dictionary;

            var result = new Dictionary<TKey, TValue>(dictionary);
            foreach (var kvp in other)
                result[kvp.Key] = kvp.Value;

            return result;
        }
    }
}
