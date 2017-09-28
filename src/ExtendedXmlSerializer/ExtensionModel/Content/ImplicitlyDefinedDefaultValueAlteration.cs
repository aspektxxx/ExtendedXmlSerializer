﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;
using System;

namespace ExtendedXmlSerializer.ExtensionModel.Content
{
	sealed class ImplicitlyDefinedDefaultValueAlteration : IAlteration<IConverter>
	{
		public static ImplicitlyDefinedDefaultValueAlteration Default { get; } =
			new ImplicitlyDefinedDefaultValueAlteration();

		ImplicitlyDefinedDefaultValueAlteration() {}

		public IConverter Get(IConverter parameter)
		{
			var @default = Defaults.Instance.Get(parameter.Get())
			                       .Invoke()
			                       .Get();
			var parser = new Parser(parameter.Parse, @default);
			var result = new Converter<object>(parameter, parser.Get, parameter.Format);
			return result;
		}

		sealed class Defaults : Generic<ISource<object>>
		{
			public static Defaults Instance { get; } = new Defaults();
			public Defaults() : base(typeof(DefaultValues<>)) {}
		}

		sealed class Parser : IParser<object>
		{
			readonly Func<string, object> _parser;
			readonly object _defaultValue;

			public Parser(Func<string, object> parser, object defaultValue)
			{
				_parser = parser;
				_defaultValue = defaultValue;
			}

			public object Get(string parameter)
			{
				try
				{
					return _parser(parameter);
				}
				catch (FormatException)
				{
					return _defaultValue;
				}
			}
		}
	}
}